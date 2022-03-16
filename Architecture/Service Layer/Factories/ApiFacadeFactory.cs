using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Connection_Sample.Architecture.Service_Layer
{
    internal class ApiFacadeFactory : IApiFacadeFactory
    {
        private readonly HttpClient client;
        private readonly ILogger logger;

        #region Constructor:

        public ApiFacadeFactory(HttpClient client, ILogger logger)
        {
            this.client = client;
            this.logger = logger;
        }

        #endregion

        public IApiFacade Connect() => new ApiFacade(client, logger);
    }
}
