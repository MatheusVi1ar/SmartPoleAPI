using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartPoleAPI.Model;

namespace SmartPoleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SmartMeterController : ControllerBase
    {
        
        private readonly ILogger<SmartMeterController> _logger;

        public SmartMeterController(ILogger<SmartMeterController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            List<SensorArray> Lista = new List<SensorArray>();
            List<string> CollectionList = GetCollection();
            var client = new MongoClient(ConnectionString.conexao);
            var database = client.GetDatabase("sth_helixiot");
            foreach (string collectionName in CollectionList)
            {
                SensorArray auxCollection = new SensorArray();
                auxCollection.Collection = collectionName;

                string name = string.Format("sth_/_{0}_SmartMeter",collectionName);
                var collection = database.GetCollection<BsonDocument>(name);
                
                var data = collection.Find(new BsonDocument()).ToList();
                
                foreach (var document in data)
                {
                    
                    SensorModel aux = BsonSerializer.Deserialize<SensorModel>(document);

                        Sensor objeto = new Sensor();
                        objeto.Data = aux.recvTime;
                        objeto.Nome = aux.attrName;
                        objeto.Valor = aux.attrValue;

                    if (aux.attrName == "energia")
                        auxCollection.Energia.Add(objeto);
                    else if (aux.attrName == "temperatura")
                        auxCollection.Temperatura.Add(objeto);
                    else if(aux.attrName == "luz")
                        auxCollection.Luminosidade.Add(objeto);
                    else if (aux.attrName == "vazao")
                        auxCollection.Vazao.Add(objeto);

                }
                Lista.Add(auxCollection);
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string output = jss.Serialize(Lista);

            return output;
        }

        public List<string> GetCollection()
        {
            var client = new MongoClient(ConnectionString.conexao);
            var database = client.GetDatabase("orion-helixiot");
            var collection = database.GetCollection<BsonDocument>("entities");

            var documents = collection.Find(new BsonDocument()).ToList();
            List<string> lista = new List<string>();

            foreach (var doc in documents)
            {
                var data = (JObject)JsonConvert.DeserializeObject(doc.AsBsonDocument.ToString());
                var _id = (JObject)JsonConvert.DeserializeObject(data["_id"].ToString());
                string id = _id["id"].Value<string>();
                lista.Add(id);
            }            
            return lista;
        }
    }
}
