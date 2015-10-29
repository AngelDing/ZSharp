using System;
using System.Collections.Generic;
using System.Text;

namespace ZSharp.Framework.Logging.Simple
{
    public class ConsoleOutLogger : BaseSimpleLogger
    {
        private static readonly Dictionary<LogLevelType, ConsoleColor> colors =
            new Dictionary<LogLevelType, ConsoleColor>
            {
                { LogLevelType.Fatal, ConsoleColor.Red },
                { LogLevelType.Error, ConsoleColor.Yellow },
                { LogLevelType.Warn, ConsoleColor.Magenta },
                { LogLevelType.Info, ConsoleColor.White },
                { LogLevelType.Debug, ConsoleColor.Gray },
                { LogLevelType.Trace, ConsoleColor.DarkGray },
            };

        private readonly bool useColor;

        public ConsoleOutLogger(LogArgumentEntity argEntity)
            : base(argEntity)
        {
        }

        public ConsoleOutLogger(LogArgumentEntity argEntity, bool useColor)
            : this(argEntity)
        {
            this.useColor = useColor;
        }

        protected override void Write(LogLevelType level, object message, Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            FormatOutput(sb, level, message, ex);

            ConsoleColor color;
            if (this.useColor && colors.TryGetValue(level, out color))
            {
                var originalColor = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = color;
                    Console.Out.WriteLine(sb.ToString());
                    return;
                }
                finally
                {
                    Console.ForegroundColor = originalColor;
                }
            }

            Console.Out.WriteLine(sb.ToString());
        } 
    }
}
