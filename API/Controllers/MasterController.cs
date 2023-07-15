using API.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using ProtoBuf.Grpc.Client;
using srvMasters.protos;
using System.Net.Mail;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly ILogger<MasterController> _logger;
        private readonly IOptions<GRPCServices> _grpcServices;
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;
        public MasterController(IOptions<GRPCServices> grpcServices, ILogger<MasterController> logger, IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            _logger = logger;
            _grpcServices = grpcServices;
            _apiBehaviorOptions = apiBehaviorOptions;
        }

        [HttpGet]
        [Route(nameof(GetAllCurrency))]
        public async Task<IActionResult> GetAllCurrency([FromQuery]mdlCurrencyRequest request)
        {
            mdlCurrencyList returnList = new mdlCurrencyList();
            try {
                using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
                var client =new ICurrency.ICurrencyClient(channel);
                returnList = await client.GetCurrencyAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetAllCurrency() " + ex.Message);
            }
            return Ok(returnList);
        }

        [HttpPost]
        [Route(nameof(SaveCurrency))]
        public async Task<IActionResult> SaveCurrency([FromBody] mdlCurrency request)
        {
            mdlCurrencySaveResponse returnData = new mdlCurrencySaveResponse();
            try
            {
                using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
                var client = new ICurrency.ICurrencyClient(channel);
                returnData = await client.SaveCurrencyAsync(request);
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: MasterController.GetAllCurrency() " + ex.Message);
            }
            return Ok(returnData);
        }
    }
}
