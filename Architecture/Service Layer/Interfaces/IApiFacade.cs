using Azure_Connection_Sample.Architecture.Domain_Layer.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Connection_Sample.Architecture.Service_Layer
{
    public interface IApiFacade : IDisposable
    {
        TokenRecord GenerateToken(string uri, string policy, string key);

        void Send<TEntity>(Uri endpoint, TEntity entity, TokenRecord token);

        void SendBatch<TCollection>(Uri endpoint, TCollection collection, TokenRecord token);

    }
}
