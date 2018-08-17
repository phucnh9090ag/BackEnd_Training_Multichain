using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiChain.Models
{
    public abstract class ServiceBase
    {
        protected JsonRpcClient jsonRpcClient;
        public ServiceBase(JsonRpcClient c)
        {
            jsonRpcClient = c;
        }
    }
}
