using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SmartPoleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private const string CONNECTION_STRING = "mongodb://helix:H3l1xNG@143.107.145.32:27000/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false";
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var client = new MongoClient(CONNECTION_STRING);
            var database = client.GetDatabase("sth_helixiot");
            var collection = database.GetCollection<BsonDocument>("sth_ /_urn:ngsi-ld:Casa:010_Casa");

            object p = collection.Find("");
            return null;
        }
    }
}
