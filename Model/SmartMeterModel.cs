using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPoleAPI.Model
{
    public class SmartMeterModel
    {
        public class id
        {
            public string _id { get; set; }
            public string type { get; set; }
            public string servicePath { get; set; }
        }

        public class TimeInstant
        {
            public string type { get; set; }
            public double value { get; set; }
        }

        public class Md
        {
            public TimeInstant TimeInstant { get; set; }
        }

        public class Temperatura
        {
            public string value { get; set; }
            public string type { get; set; }
            public Md md { get; set; }
            public List<string> mdNames { get; set; }
            public double creDate { get; set; }
            public double modDate { get; set; }
        }

        public class Vazao
        {
            public string value { get; set; }
            public string type { get; set; }
            public Md md { get; set; }
            public List<string> mdNames { get; set; }
            public double creDate { get; set; }
            public double modDate { get; set; }
        }

        public class Energia
        {
            public string type { get; set; }
            public double creDate { get; set; }
            public double modDate { get; set; }
            public string value { get; set; }
            public List<object> mdNames { get; set; }
        }

        public class Luz
        {
            public string type { get; set; }
            public double creDate { get; set; }
            public double modDate { get; set; }
            public string value { get; set; }
            public List<object> mdNames { get; set; }
        }

        public class Attrs
        {
            public Temperatura temperatura { get; set; }
            public Vazao vazao { get; set; }
            public Energia energia { get; set; }
            public Luz luz { get; set; }
            public TimeInstant TimeInstant { get; set; }
        }


        public id _id { get; set; }
        public List<string> attrNames { get; set; }
        public Attrs attrs { get; set; }
        public double creDate { get; set; }
        public double modDate { get; set; }
        public string lastCorrelator { get; set; }

        public SmartMeterModel()
        {
            _id = new id();
            attrNames = new List<string>();
            attrs = new Attrs();
        }
    }
}
