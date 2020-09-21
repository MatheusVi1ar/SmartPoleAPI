using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPoleAPI.Model
{
    public class MongoDBModel
    {
        public string recvTime { get; set; }
        public string attrName { get; set; }
        public string attrType { get; set; }
        public string attrValue { get; set; }
    }
}
