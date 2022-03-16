using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Connection_Sample.Architecture.Domain_Layer.Records
{
    public record TokenRecord(string signature, DateTime expiration);
}
