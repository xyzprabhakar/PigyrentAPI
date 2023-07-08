using DTO;
using Grpc.Net.Client;
using MasterServicesProt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductServicesProt;
using ProtoBuf.Grpc.Client;
using System.Net.Mail;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly ILogger<MasterController> _logger;
        private readonly IOptions<GRPCServices> _grpcServices;

        public MasterController(IOptions<GRPCServices> grpcServices, ILogger<MasterController> logger)
        {
            _logger = logger;
            _grpcServices= grpcServices;
        }

        [HttpGet]
        [Route(nameof(GetAllCurrency))]
        public async Task<ReturnList<Currency>> GetAllCurrency([FromQuery]string currencyId, [FromQuery] string codeId, [FromQuery] string allData)
        {
            using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
            var client = channel.CreateGrpcService<ICurrencyService>();
            if (!string.IsNullOrEmpty(currencyId))
            {
                if (currencyId.Length != 24) 
                {
                    //throw new HttpException(400, "Bad Request");
                    //ModelState.AddModelError(string.Empty,"Id should be 24 char long");
                    //retur
                }
            }
            //if(allData)
            //var reply = await client.GetAll();
            //return reply;
            throw new NotImplementedException();
        }
    }
}
