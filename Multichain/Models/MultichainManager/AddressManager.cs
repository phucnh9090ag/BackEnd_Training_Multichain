using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiChain.Models
{ 
    public class AddressManager : ServiceBase
    {
        public AddressManager(JsonRpcClient c) : base(c)
        {

        }

        public string GetAddresses()
        {
            return jsonRpcClient.JsonRpcRequest("getaddresses", true);
        }

        public string GetNewAddress()
        {
            throw new NotImplementedException();
        }

        public string ImportAddress(string address)
        {
            return jsonRpcClient.JsonRpcRequest("importaddress", address);
        }

        public string ImportPrivateKey(string privatekey)
        {
            return jsonRpcClient.JsonRpcRequest("importprivkey", privatekey);
        }

        public string AddMultisigAddress(int nrequired, params string[] pubkeys)
        {
            throw new NotImplementedException();
        }

        public string CreateKeypairs()
        {
            return jsonRpcClient.JsonRpcRequest("createkeypairs");
        }
    }
}
