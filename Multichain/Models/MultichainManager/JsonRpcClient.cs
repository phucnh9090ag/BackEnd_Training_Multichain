using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MultiChain.Models
{
    public class JsonRpcClient
    {
        string _rpcName = "";
        string _rpcPassword = "";
        string _nodeIp = "";
        int _nodePort = 0;
        string _chainName = "";

        public JsonRpcClient(string rpcName, string rpcPassword, string nodeIp, int nodePort, string chainName)
        {
            _rpcName = rpcName;
            _rpcPassword = rpcPassword;
            _nodeIp = nodeIp;
            _nodePort = nodePort;
            _chainName = chainName;
        }

        public static dynamic Get(string json)
        {
            dynamic result = JsonConvert.DeserializeObject(json);
            if (result.error != null)
                throw new Exception(result.error.message.ToString());
            return result.result;
        }

        public static string Indent(string json)
        {
            var obj = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(obj,Formatting.Indented);
        }

        public string JsonRpcRequest(string method, params object[] args)
        {
            Dictionary<string, object> mcArgs = new Dictionary<string, object>()
            {
                { "method",method },
                { "chain_name", _chainName },
                { "params", args }
            };

            try
            {
                string jsonRpcRequest = JsonConvert.SerializeObject(mcArgs);
                string jsonRpcResponse = "";
                using (var handler = new HttpClientHandler() { Credentials = new NetworkCredential(_rpcName, _rpcPassword) })
                using (var client = new HttpClient(handler))
                using (var response = client.PostAsync(string.Format("http://{0}:{1}/", _nodeIp, _nodePort), new StringContent(jsonRpcRequest, Encoding.UTF8, "text/plain")).Result)
                using (var content = response.Content)
                {
                    jsonRpcResponse = content.ReadAsStringAsync().Result;
                }
                return jsonRpcResponse;
            }
            catch (AggregateException ae)
            {
                var err = "";
                foreach (var e in ae.InnerExceptions)
                {
                    Exception e2 = e;
                    while (e2 != null)
                    {
                        err += e2.Message + ";";
                        e2 = e2.InnerException;
                    }
                    err += "\n";
                }
                throw new Exception(err);
            }
        }

    }
}
