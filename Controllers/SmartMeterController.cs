using System;
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

        [HttpPost("GetHistorico")]
        public string GetHistorico([FromBody]string dispositivo, DateTime dataDe, DateTime dataAte)
        {
            var client = new MongoClient(ConnectionString.conexao);
            var database = client.GetDatabase("sth_helixiot");
            Entidade auxCollection = new Entidade();

            string conexao = string.Format("sth_/_{0}_SmartMeter", dispositivo);
            var colecao = database.GetCollection<BsonDocument>(conexao);

            var builder = Builders<BsonDocument>.Filter;
            var filtro = builder.Lte("recvTime", dataAte) & builder.Gte("recvTime", "dataDe");

            var data = colecao.Find(filtro).ToList();

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
                else if (aux.attrName == "luz")
                    auxCollection.Luminosidade.Add(objeto);
                else if (aux.attrName == "vazao")
                    auxCollection.Vazao.Add(objeto);

            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string output = jss.Serialize(auxCollection);

            return output;
        }

        [HttpGet]
        public List<string> GetDispositivos()
        {
            var client = new MongoClient(ConnectionString.conexao);
            var database = client.GetDatabase("orion-helixiot");
            var collection = database.GetCollection<BsonDocument>("entities");

            var filtro = Builders<BsonDocument>.Filter.AnyEq("_id.type", "SmartMeter");

            var documents = collection.Find(filtro).ToList();
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
