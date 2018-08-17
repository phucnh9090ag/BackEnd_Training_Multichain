using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MultiChain.Models.MultichainManager
{
    public static class MultichainHelper
    {
        public static string RpcName = "";
        public static string RpcPassword = "";
        public static string NodeIp = "";
        public static int NodePort = 0;
        public static string ChainName = "";

        private static void AssertReady()
        {
            if (String.IsNullOrEmpty(RpcName))
                throw new Exception("RpcName not specified");
            if (String.IsNullOrEmpty(RpcPassword))
                throw new Exception("RpcPassword not specified");
            if (String.IsNullOrEmpty(NodeIp))
                throw new Exception("NodeIp not specified");
            if (NodePort==0)
                throw new Exception("NodePort not specified");
            if (String.IsNullOrEmpty(ChainName))
                throw new Exception("ChainName not specified");
        }
        //private static void Check(string json)
        //{
        //    dynamic result = JsonConvert.DeserializeObject(json);
        //    if (result.error != null)
        //        throw new Exception(result.error.message.ToString());
        //}

        public static void Init(string rpcName, string rpcPassword, string nodeIp, int nodePort, string chainName)
        {
            RpcName = rpcName;
            RpcPassword = rpcPassword;
            NodeIp = nodeIp;
            NodePort = nodePort;
            ChainName = chainName;
        }

        public static string JsonRpcRequest(string method, params object[] args)
        {
            var client = new JsonRpcClient(RpcName, RpcPassword, NodeIp, NodePort, ChainName);
            return client.JsonRpcRequest(method, args);


            //Dictionary<string, object> mcArgs = new Dictionary<string, object>()
            //{
            //    { "method",method },
            //    { "chain_name",ChainName },
            //    { "params", args }
            //};

            //try
            //{
            //    string jsonRpcRequest = JsonConvert.SerializeObject(mcArgs);
            //    string jsonRpcResponse = "";
            //    using (var handler = new HttpClientHandler() { Credentials = new NetworkCredential(RpcName, RpcPassword) })
            //    using (var client = new HttpClient(handler))
            //    using (var response = client.PostAsync(string.Format("http://{0}:{1}/", NodeIp, NodePort), new StringContent(jsonRpcRequest, Encoding.UTF8, "text/plain")).Result)
            //    using (var content = response.Content)
            //    {
            //        jsonRpcResponse = content.ReadAsStringAsync().Result;
            //    }
            //    Check(jsonRpcResponse);
            //    return jsonRpcResponse;
            //}
            //catch (AggregateException ae)
            //{
            //    var err = "";
            //    foreach (var e in ae.InnerExceptions)
            //    {
            //        Exception e2 = e;
            //        while (e2 != null)
            //        {
            //            err += e2.Message + ";";
            //            e2 = e2.InnerException;
            //        }
            //        err += "\n";
            //    }
            //    throw new Exception(err); 
            //}
        }

        public static byte[] InsertAddressVersion(string pubkeyHash, string version)
        {
            var pubkeyHashBytes = Hex.Hex2Bytes(pubkeyHash);
            var versionBytes = Hex.Hex2Bytes(version);

            int space = (int)Math.Floor(20f / versionBytes.Length);
            byte[] extended = new byte[pubkeyHashBytes.Length + versionBytes.Length];
            for (int i = 0, j = 0; i < pubkeyHashBytes.Length; i += space, j++)
            {
                var len = space;
                if (i + space >= pubkeyHashBytes.Length)
                    len = pubkeyHashBytes.Length - i;
                Buffer.BlockCopy(versionBytes, j, extended, i + j, 1);
                Buffer.BlockCopy(pubkeyHashBytes, i, extended, i + j + 1, len);
            }
            return extended;
        }
    }
}
