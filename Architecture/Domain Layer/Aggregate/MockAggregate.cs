using Azure_Connection_Sample.Architecture.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Connection_Sample.Architecture.Domain_Layer.Aggregate
{
    internal class MockAggregate
    {
        public Guid Id => Randomize.GetRandom();

        public DateTime Timestamp => DateTime.UtcNow;

        public int Measurement => Randomize.GetRandom(100);
    }
}
