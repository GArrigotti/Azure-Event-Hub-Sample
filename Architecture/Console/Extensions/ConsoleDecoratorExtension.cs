using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Connection_Sample.Architecture.Console
{
    internal static class ConsoleDecoratorExtension
    {
        public static void Decorate(this Exception exception, ILogger logger)
        {
            logger.Error($"┌{new string('─', 100)}┐");
            logger.Error($"│{"Exception:".Center()}│");
            logger.Error($"│{exception.Message.Center()}│");
            logger.Error($"└{new string('─', 100)}┘");
        }

        public static string Center(this string content, int window = 100)
        {
            int left = (window - content.Length) / 2;
            int right = window - (left + content.Length);

            return $"{new String(' ', left)}{content}{new String(' ', right)}";
        }

        public static string Left(this string content, int window = 100)
        {
            int right = window - content.Length;
            return $"{content}{new String(' ', right)}";
        }

        private static string Right(this string content, int window = 100)
        {
            int left = window - content.Length;
            return $"{new String(' ', left)}{content}";
        }
    }
}
