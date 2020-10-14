using System;
using System.Collections.Generic;
using System.Globalization;
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

        [HttpGet("GetHistorico")]
        public Entidade GetHistorico(string dispositivo, string dataDe, string dataAte)
        {
            var client = new MongoClient(ConnectionString.conexao);
            var database = client.GetDatabase("sth_helixiot");
            Entidade auxCollection = new Entidade();

            string conexao = string.Format("sth_/_{0}_SmartMeter", dispositivo);
            var colecao = database.GetCollection<BsonDocument>(conexao);

            //var builder = Builders<BsonDocument>.Filter;
            //ISODate("2020-09-21T23:28:54.965Z")
            //string dataAteISO = dataAte.ToString("yyyy-MM-dd'T'HH:mm:ss.fffZ", CultureInfo.InvariantCulture);
            //string dataDeISO = dataDe.ToString("yyyy-MM-dd'T'HH:mm:ss.fffZ", CultureInfo.InvariantCulture);

            // var filtro = builder.Gte("_id.recvTime", dataDeISO); //builder.Lte("recvTime" , dataAteISO) &
            var data = colecao.Find(new BsonDocument()).ToList();
            //var data = colecao.Find(filtro).ToList();
            int i = 0;
            foreach (var document in data)
            {

                SensorModel aux = BsonSerializer.Deserialize<SensorModel>(document);

                Sensor objeto = new Sensor();
                objeto.Data = aux.recvTime;
                objeto.Nome = aux.attrName;
                objeto.Valor = aux.attrValue;

                if (objeto.Data.Date >= Convert.ToDateTime(dataDe) && objeto.Data.Date <= Convert.ToDateTime(dataAte))
                {
                    if (aux.attrName == "energia")
                    {
                        if (auxCollection.DadosRecentes.Energia == null || objeto.Data > auxCollection.DadosRecentes.Energia.Data)
                            auxCollection.DadosRecentes.Energia = objeto;
                        auxCollection.Energia.Add(objeto);
                    }
                    else if (aux.attrName == "temperatura")
                    {
                        if (auxCollection.DadosRecentes.Temperatura == null || objeto.Data > auxCollection.DadosRecentes.Temperatura.Data)
                            auxCollection.DadosRecentes.Temperatura = objeto;
                        auxCollection.Temperatura.Add(objeto);
                    }
                    else if (aux.attrName == "luz")
                    {
                        if (auxCollection.DadosRecentes.Luminosidade == null || objeto.Data > auxCollection.DadosRecentes.Luminosidade.Data)
                            auxCollection.DadosRecentes.Luminosidade = objeto;
                        auxCollection.Luminosidade.Add(objeto);
                    }
                    else if (aux.attrName == "vazao")
                    {
                        if (auxCollection.DadosRecentes.Vazao == null || objeto.Data > auxCollection.DadosRecentes.Vazao.Data)
                            auxCollection.DadosRecentes.Vazao = objeto;
                        auxCollection.Vazao.Add(objeto);
                    }
                }
                else
                {
                    i++;
                }
            }

            return auxCollection;
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
