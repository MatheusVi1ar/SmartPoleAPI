using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPoleAPI.Model
{
    public class SensorModel
    {
            public ObjectId _id { get; set; }
            public DateTime recvTime { get; set; }
            public string attrName { get; set; }
            public string attrType { get; set; }
            public string attrValue { get; set; }
    }
}
