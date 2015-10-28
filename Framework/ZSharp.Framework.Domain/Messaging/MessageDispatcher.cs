using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ZSharp.Framework.Domain
{
    public class MessageDispatcher
    {
        private Dictionary<Type, List<Tuple<Type, Action<Envelope>>>> handlersByEventType;
        private Dictionary<Type, Action<IMessage, string>> dispatchersByEventType;

        public MessageDispatcher()
        {
            this.handlersByEventType = new Dictionary<Type, List<Tuple<Type, Action<Envelope>>>>();
            this.dispatchersByEventType = new Dictionary<Type, Action<IMessage, string>>();
        }

        public MessageDispatcher(IEnumerable<IHandler> handlers)
            : this()
        {
            foreach (var handler in handlers)
            {
                this.Register(handler);
            }
        }

        public void Register(IHandler handler)
        {
            var handlerType = handler.GetType();

            foreach (var invocationTuple in this.BuildHandlerInvocations(handler))
            {
                var envelopeType = typeof(Envelope<>).MakeGenericType(invocationTuple.Item1);

                List<Tuple<Type, Action<Envelope>>> invocations;
                if (!this.handlersByEventType.TryGetValue(invocationTuple.Item1, out invocations))
                {
                    invocations = new List<Tuple<Type, Action<Envelope>>>();
                    this.handlersByEventType[invocationTuple.Item1] = invocations;
                }
                invocations.Add(new Tuple<Type, Action<Envelope>>(handlerType, invocationTuple.Item2));

                if (!this.dispatchersByEventType.ContainsKey(invocationTuple.Item1))
                {
                    this.dispatchersByEventType[invocationTuple.Item1] = this.BuildDispatchInvocation(invocationTuple.Item1);
                }
            }
        }

        public void DispatchMessage(IMessage message, string correlationId)
        {
            Type messageType = message.GetType();
            Action<IMessage, string> dispatch;
            if (this.dispatchersByEventType.TryGetValue(messageType, out dispatch))
            {
                dispatch(message, correlationId);
            }
            // Invoke also the generic handlers that have registered to handle IMessage directly.
            if (this.dispatchersByEventType.TryGetValue(typeof(IMessage), out dispatch))
            {
                dispatch(message, correlationId);
            }
        }

        private void DoDispatchMessage<T>(T msg, string correlationId) where T : IMessage
        {
            var envelope = Envelope.Create(msg);
            envelope.CorrelationId = correlationId;

            List<Tuple<Type, Action<Envelope>>> handlers;
            var msgType = typeof(T);
            if (this.handlersByEventType.TryGetValue(msgType, out handlers))
            {
                foreach (var handler in handlers)
                {
                    handler.Item2(envelope);
                }
            }
        }

        private IEnumerable<Tuple<Type, Action<Envelope>>> BuildHandlerInvocations(IHandler handler)
        {
            var interfaces = handler.GetType().GetInterfaces();

            var eventHandlerInvocations =
                interfaces
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler<>))
                    .Select(i => new { HandlerInterface = i, EventType = i.GetGenericArguments()[0] })
                    .Select(e => new Tuple<Type, Action<Envelope>>(e.EventType, this.BuildHandlerInvocation(handler, e.HandlerInterface, e.EventType)));

            var envelopedEventHandlerInvocations =
                interfaces
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnvelopedEventHandler<>))
                    .Select(i => new { HandlerInterface = i, EventType = i.GetGenericArguments()[0] })
                    .Select(e => new Tuple<Type, Action<Envelope>>(e.EventType, this.BuildEnvelopeHandlerInvocation(handler, e.HandlerInterface, e.EventType)));

            return eventHandlerInvocations.Union(envelopedEventHandlerInvocations);
        }

        private Action<Envelope> BuildHandlerInvocation(IHandler handler, Type handlerType, Type messageType)
        {
            var envelopeType = typeof(Envelope<>).MakeGenericType(messageType);

            var parameter = Expression.Parameter(typeof(Envelope));
            var invocationExpression =
                Expression.Lambda(
                    Expression.Block(
                        Expression.Call(
                            Expression.Convert(Expression.Constant(handler), handlerType),
                            handlerType.GetMethod("Handle"),
                            Expression.Property(Expression.Convert(parameter, envelopeType), "Body"))),
                    parameter);

            return (Action<Envelope>)invocationExpression.Compile();
        }

        private Action<Envelope> BuildEnvelopeHandlerInvocation(IHandler handler, Type handlerType, Type messageType)
        {
            var envelopeType = typeof(Envelope<>).MakeGenericType(messageType);

            var parameter = Expression.Parameter(typeof(Envelope));
            var invocationExpression =
                Expression.Lambda(
                    Expression.Block(
                        Expression.Call(
                            Expression.Convert(Expression.Constant(handler), handlerType),
                            handlerType.GetMethod("Handle"),
                            Expression.Convert(parameter, envelopeType))),
                    parameter);

            return (Action<Envelope>)invocationExpression.Compile();
        }

        private Action<IMessage, string> BuildDispatchInvocation(Type msgType)
        {
            var msgParameter = Expression.Parameter(typeof(IMessage));
            var correlationIdParameter = Expression.Parameter(typeof(string));

            var dispatchExpression =
                Expression.Lambda(
                    Expression.Block(
                        Expression.Call(
                            Expression.Constant(this),
                            this.GetType().GetMethod("DoDispatchMessage", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(msgType),
                            Expression.Convert(msgParameter, msgType),
                            correlationIdParameter)),
                    msgParameter,                    
                    correlationIdParameter);

            return (Action<IMessage, string>)dispatchExpression.Compile();
        }
    }
}
