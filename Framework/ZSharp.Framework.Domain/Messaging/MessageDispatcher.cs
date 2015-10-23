using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace ZSharp.Framework.Domain
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly Dictionary<Type, List<object>> handlers = new Dictionary<Type, List<object>>();

        public virtual void Clear()
        {
            handlers.Clear();
        }

        public virtual void DispatchMessage<T>(T message) where T : IMessage
        {
            Type messageType = typeof(T);
            if (handlers.ContainsKey(messageType))
            {
                var messageHandlers = handlers[messageType];
                foreach (var messageHandler in messageHandlers)
                {
                    var dynMessageHandler = (IHandler<T>)messageHandler;
                   
                    try
                    {
                        dynMessageHandler.Handle(message);
                    }
                    catch
                    {
                    }
                }
            }
        }

        #region IHandlerRegistry

        public virtual void Register<T>(IHandler<T> handler)
        {
            Type keyType = typeof(T);

            if (handlers.ContainsKey(keyType))
            {
                List<object> registeredHandlers = handlers[keyType];
                if (registeredHandlers != null)
                {
                    if (!registeredHandlers.Contains(handler))
                    {
                        registeredHandlers.Add(handler);
                    }
                }
                else
                {
                    registeredHandlers = new List<object>();
                    registeredHandlers.Add(handler);
                    handlers.Add(keyType, registeredHandlers);
                }
            }
            else
            {
                List<object> registeredHandlers = new List<object>();
                registeredHandlers.Add(handler);
                handlers.Add(keyType, registeredHandlers);
            }
        }

        public virtual void UnRegister<T>(IHandler<T> handler)
        {
            var keyType = typeof(T);
            if (handlers.ContainsKey(keyType) &&
                handlers[keyType] != null &&
                handlers[keyType].Count > 0 &&
                handlers[keyType].Contains(handler))
            {
                handlers[keyType].Remove(handler);
            }
        }

        #endregion      
    }
}
