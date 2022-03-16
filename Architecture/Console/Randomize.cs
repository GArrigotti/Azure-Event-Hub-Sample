using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Connection_Sample.Architecture.Console
{
    internal static class Randomize
    {
        public static string GetRandom(int length, int offset)
        {
            var builder = new StringBuilder(length);

            for (var index = 0; index < length; index++)
                builder.Append((char)RandomNumberGenerator.GetInt32(offset, offset + length));            

            return builder.ToString();
        }

        public static int GetRandom(int maximum) => RandomNumberGenerator.GetInt32(maximum);

        public static Guid GetRandom() => Guid.NewGuid();
    }
}
