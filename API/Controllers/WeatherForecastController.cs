using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using ProductServicesProt;
using DTO;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public async Task<IEnumerable<WeatherForecast>> GetAsync()
        //{
            
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)],
        //        Message= ""
        //    })
        //    .ToArray();
        //}

        [HttpGet]
        [Route("GetCategory")]
        public async Task<ReturnList<Category>> GetCategory([FromQuery] CategoryRequest categoryRequest)
        {
            
                using var channel = GrpcChannel.ForAddress("https://localhost:7040/");
                var client = channel.CreateGrpcService<ICategoryService>();
                var reply = await client.Get(categoryRequest);
            return reply;
        }

        [HttpPost]
        [Route("SaveCategory")]
        public async Task<ReturnData> SaveCategory([FromBody] Category request)
        {

            using var channel = GrpcChannel.ForAddress("https://localhost:7040/");
            var client = channel.CreateGrpcService<ICategoryService>();
            var reply = await client.Save(request);
            return reply;
        }
    }
}