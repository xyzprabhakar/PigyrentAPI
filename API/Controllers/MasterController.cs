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
                if (!string.IsNullOrEmpty(request.CurrencyId) && _grpcServices.Value.IdLength != request.CurrencyId.Length)
                {
                    ModelState.AddModelError(nameof(request.CurrencyId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
                    var client =new ICurrency.ICurrencyClient(channel);
                    returnList = await client.GetCurrencyAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
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
                if (!string.IsNullOrEmpty(request.CurrencyId) && _grpcServices.Value.IdLength != request.CurrencyId.Length)
                {
                    ModelState.AddModelError(nameof(request.CurrencyId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Code))
                {
                    ModelState.AddModelError(nameof(request.Code), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
                    var client = new ICurrency.ICurrencyClient(channel);
                    returnData = await client.SaveCurrencyAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: MasterController.GetAllCurrency() " + ex.Message);
            }
            return Ok(returnData);
        }

        [HttpPost]
        [Route(nameof(SaveCountry))]
        public async Task<IActionResult> SaveCountry([FromBody] mdlCountry request)
        {
            mdlCountryStateSaveResponse returnData = new mdlCountryStateSaveResponse();
            try
            {
                if (!string.IsNullOrEmpty(request.CountryId) && _grpcServices.Value.IdLength != request.CountryId.Length)
                {
                    ModelState.AddModelError(nameof(request.CountryId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Code))
                {
                    ModelState.AddModelError(nameof(request.Code), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
                    var client = new ICountryState.ICountryStateClient(channel);
                    returnData = await client.SaveCountryAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: MasterController.SaveCountry() " + ex.Message);
            }
            return Ok(returnData);
        }

        [HttpPost]
        [Route(nameof(SaveState))]
        public async Task<IActionResult> SaveState([FromBody] mdlState request)
        {
            mdlCountryStateSaveResponse returnData = new mdlCountryStateSaveResponse();
            try
            {
                if (string.IsNullOrEmpty(request.CountryId) )
                {
                    ModelState.AddModelError(nameof(request.CountryId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (!string.IsNullOrEmpty(request.StateId) && _grpcServices.Value.IdLength != request.StateId.Length)
                {
                    ModelState.AddModelError(nameof(request.CountryId), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    ModelState.AddModelError(nameof(request.Name), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
                    var client = new ICountryState.ICountryStateClient(channel);
                    returnData = await client.SaveStateAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: MasterController.SaveState() " + ex.Message);
            }
            return Ok(returnData);
        }

        [HttpGet]
        [Route(nameof(GetAllState))]
        public async Task<IActionResult> GetAllState([FromQuery] mdlGetState request)
        {
            mdlStateList returnList = new mdlStateList();
            try
            {
                if (!string.IsNullOrEmpty(request.StateId) && _grpcServices.Value.IdLength != request.StateId.Length)
                {
                    ModelState.AddModelError(nameof(request.StateId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (!string.IsNullOrEmpty(request.CountryId) && _grpcServices.Value.IdLength != request.CountryId.Length)
                {
                    ModelState.AddModelError(nameof(request.CountryId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
                    var client = new ICountryState.ICountryStateClient(channel);
                    returnList = await client.GetStateAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetAllState() " + ex.Message);
            }
            return Ok(returnList);
        }

        [HttpGet]
        [Route(nameof(GetAllCountry))]
        public async Task<IActionResult> GetAllCountry([FromQuery] mdlGetCountry request)
        {
            mdlCountryList returnList = new mdlCountryList();
            try
            {                
                if (!string.IsNullOrEmpty(request.CountryId) && _grpcServices.Value.IdLength != request.CountryId.Length)
                {
                    ModelState.AddModelError(nameof(request.CountryId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
                    var client = new ICountryState.ICountryStateClient(channel);
                    returnList = await client.GetCountryAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetAllCountry() " + ex.Message);
            }
            return Ok(returnList);
        }
    }
}
