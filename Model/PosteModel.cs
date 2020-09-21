using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPorteAPI.Model
{
    public class PosteModel
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("vazao")]
        public double Vazao { get; set; }
        [BsonElement("temperature")]
        public double Temperatura { get; set; }
        [BsonElement("luz")]
        public double Luminosidade { get; set; }
        [BsonElement("energia")]
        public double Energia { get; set; }

        public string recvTime { get; set; }
        public string attrName { get; set; }
        public string attrType { get; set; }
        public string attrValue { get; set; }
    }
}
