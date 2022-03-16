using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Connection_Sample.Architecture.Domain_Layer.Models
{
    internal class AzureModel
    {
        public EventHubModel EventHub { get; set; }

        public KeyVaultModel KeyVault { get; set; }
    }
}
