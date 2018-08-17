using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiChain.Models
{
    public class Admin: ServiceBase
    {

        public string GetInfo()
        {
            return jsonRpcClient.JsonRpcRequest("getinfo");
        }

        public string GetPeerInfo()
        {
            throw new NotImplementedException();
        }

        public string GetBlock(string height)
        {
            throw new NotImplementedException();
        }

        public string GetBlockchainParams()
        {
            throw new NotImplementedException();
        }

        public Admin(JsonRpcClient c):base(c)
        {

        }
    }
}
