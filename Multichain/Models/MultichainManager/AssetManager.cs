using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiChain.Models
{
    public class AssetManager : ServiceBase
    {
        public AssetManager(JsonRpcClient c) : base(c)
        {

        }

        public string GetAddressBalances(string address)
        {
            throw new NotImplementedException();
        }


        public string IssueAsset(string address, string assetName, int qty, double unit, string note)
        {
            return jsonRpcClient.JsonRpcRequest("issue", address, new { name = assetName, open = true }, qty, unit, 0, new { type = "testasset", note = note });
        }

        public string ListAssets(string assetName, bool verbose)
        {
            throw new NotImplementedException();
        }

        public string ListAssets()
        {
            throw new NotImplementedException();
        }

        public string SendAsset(string addressTo, string assetName, double qty)
        {
            throw new NotImplementedException();
        }

        public string IssueMore(string addressTo, string assetName, double qty)
        {
            throw new NotImplementedException();
        }

        public string PrepareLockUnspent(string assetname, int qty)
        {
            throw new NotImplementedException();
        }


    }
}
