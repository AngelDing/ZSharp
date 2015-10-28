using System;

namespace ZSharp.Framework.Domain
{
    public abstract class Envelope
    {
        public static Envelope<T> Create<T>(T body)
        {
            return new Envelope<T>(body);
        }
    }

    public class Envelope<T> : Envelope
    {
        public Envelope(T body)
        {
            this.Body = body;
        }

        public T Body { get; private set; }

        public TimeSpan Delay { get; set; }

        public TimeSpan TimeToLive { get; set; }

        public string CorrelationId { get; set; }

        public string SysCode { get; set; }

        public string Topic { get; set; }

        public static implicit operator Envelope<T>(T body)
        {
            return Envelope.Create(body);
        }
    }
}
