using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiChain.Models
{
    public class StreamManager : ServiceBase
    {
        public StreamManager(JsonRpcClient c) : base(c)
        {

        }

        public string CreateStream(string streamName, bool isOpen = false)
        {
            throw new NotImplementedException();
        }

        public string SubscribeStream(string streamName)
        {
            throw new NotImplementedException();
        }

        public string PublishStream(string streamName, string key, string hex)
        {
            throw new NotImplementedException();
        }

        public string ListStreamKeyItem(string streamName, string key)
        {
            throw new NotImplementedException();
        }
        public string ListStreamPublisherItem(string streamName, string address)
        {
            throw new NotImplementedException();
        }
        public string ListStreamItems(string streamName)
        {
            throw new NotImplementedException();
        }

    }
}
