using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Connection_Sample.Architecture.Service_Layer
{
    internal interface IApiFacadeFactory
    {
        IApiFacade Connect();
    }
}
