using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

                if (objeto.Data.Date >= DateTime.ParseExact(dataDe, "dd/MM/yyyy", null) && objeto.Data.Date <= DateTime.ParseExact(dataAte, "dd/MM/yyyy", null))
                {
                    if (aux.attrName.ToUpper() == "ENERGIA")
                    {
                        auxCollection.Energia.Add(objeto);
                    }
                    else if (aux.attrName.ToUpper() == "TEMPERATURA")
                    {
                        auxCollection.Temperatura.Add(objeto);
                    }
                    else if (aux.attrName == "LUZ" || aux.attrName.ToUpper() == "LUMINOSIDADE")
                    {
                        auxCollection.Luminosidade.Add(objeto);
                    }
                    else if (aux.attrName.ToUpper() == "VAZAO")
                    {
                        auxCollection.Vazao.Add(objeto);
                    }
                    else if (aux.attrName.ToUpper() == "GAS")
                    {
                        auxCollection.Gas.Add(objeto);
                    }
                    else if (aux.attrName.ToUpper() == "UMIDADE")
                    {
                        auxCollection.Umidade.Add(objeto);
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

        [HttpPost("PostLogin")]
        public async Task<bool> PostLogin([FromBody] UsuarioModel Usuario)
        {
            string URL_HELIX = "http://143.107.145.32";
            string GET_ENTITIES = ":1026/v2/entities/";
            bool output = false;

            using (HttpClient cliente = new HttpClient())
            {
                cliente.DefaultRequestHeaders.Add("Accept", "application/json");
                cliente.DefaultRequestHeaders.Add("fiware-service", "helixiot");
                cliente.DefaultRequestHeaders.Add("fiware-servicepath", "/");

                try
                {
                    HttpResponseMessage resposta = await cliente.GetAsync(URL_HELIX + GET_ENTITIES + Usuario.Login);
                    if (resposta.IsSuccessStatusCode)
                    {
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        UsuarioJson usuariojson = JsonConvert.DeserializeObject<UsuarioJson>(conteudo);

                        output = (usuariojson.senha.value == Usuario.Senha);
                    }
                }
                catch
                {
                    output = false;
                }
                return output;
            }            
        }
    }
}
