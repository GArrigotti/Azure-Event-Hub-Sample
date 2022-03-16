using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Connection_Sample.Architecture.Domain_Layer.Models
{
    internal class EventHubModel
    {
        public string Endpoint { get; set; }

        public string Authorization { get; set; }

        public string Key { get; set; }

    }
}
