using API.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MasterServices;
using ProductServices;
using API.Classes;

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
        public async Task<IActionResult> GetAllCurrency([FromQuery]mdlCurrencyRequest request, [FromHeader] dtoHeaders header)
        {
            mdlCurrencyList returnList = new mdlCurrencyList();
            try
            {   
                if (!string.IsNullOrEmpty(request.CurrencyId) && request.CurrencyId.Length != _grpcServices.Value.IdLength)
                {
                    ModelState.AddModelError(nameof(request.CurrencyId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (ModelState.IsValid)
                {
                    Headers.BindLanguage(header, request);
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
                    var client = new IMaster.IMasterClient(channel);
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

        [HttpGet]
        [Route(nameof(GetCountry))]
        public async Task<IActionResult> GetCountry([FromQuery] mdlCountryRequest request, [FromHeader] dtoHeaders header)
        {
            mdlCountryList returnList = new mdlCountryList();
            try
            {
                if (!string.IsNullOrEmpty(request.CountryId) && request.CountryId.Length != _grpcServices.Value.IdLength)
                {
                    ModelState.AddModelError(nameof(request.CountryId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (ModelState.IsValid)
                {
                    Headers.BindLanguage(header, request);
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
                    var client = new IMaster.IMasterClient(channel);
                    returnList = await client.GetCountryAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetCountry() " + ex.Message);
            }
            return Ok(returnList);
        }

        [HttpGet]
        [Route(nameof(GetState))]
        public async Task<IActionResult> GetState([FromQuery] mdlStateRequest request, [FromHeader] dtoHeaders header)
        {
            mdlStateList returnList = new mdlStateList();
            try
            {
                if (!string.IsNullOrEmpty(request.CountryId) && request.CountryId.Length != _grpcServices.Value.IdLength)
                {
                    ModelState.AddModelError(nameof(request.CountryId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (!string.IsNullOrEmpty(request.StateId) && request.StateId.Length != _grpcServices.Value.IdLength)
                {
                    ModelState.AddModelError(nameof(request.StateId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (ModelState.IsValid)
                {
                    Headers.BindLanguage(header, request);
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
                    var client = new IMaster.IMasterClient(channel);
                    returnList = await client.GetStateAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.State() " + ex.Message);
            }
            return Ok(returnList);
        }

        [HttpGet]
        [Route(nameof(GetCity))]
        public async Task<IActionResult> GetCity([FromQuery] mdlCityRequest request, [FromHeader] dtoHeaders header)
        {
            mdlCityList returnList = new mdlCityList();
            try
            {
                if (!string.IsNullOrEmpty(request.CountryId) && request.CountryId.Length != _grpcServices.Value.IdLength)
                {
                    ModelState.AddModelError(nameof(request.CountryId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (!string.IsNullOrEmpty(request.StateId) && request.StateId.Length != _grpcServices.Value.IdLength)
                {
                    ModelState.AddModelError(nameof(request.StateId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (!string.IsNullOrEmpty(request.CityId) && request.CityId.Length != _grpcServices.Value.IdLength)
                {
                    ModelState.AddModelError(nameof(request.CityId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (ModelState.IsValid)
                {
                    Headers.BindLanguage(header, request);
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
                    var client = new IMaster.IMasterClient(channel);
                    returnList = await client.GetCityAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetCity() " + ex.Message);
            }
            return Ok(returnList);
        }

        [HttpGet]
        [Route(nameof(GetLocality))]
        public async Task<IActionResult> GetLocality([FromQuery] mdlLocalityRequest request, [FromHeader] dtoHeaders header)
        {
            mdlLocalityList returnList = new mdlLocalityList();
            try
            {
                if (!string.IsNullOrEmpty(request.CityId) && request.CityId.Length != _grpcServices.Value.IdLength)
                {
                    ModelState.AddModelError(nameof(request.CityId), enmErrorMessage.IdentifierLength.ToString());
                }
                
                if (!string.IsNullOrEmpty(request.LocalityId) && request.LocalityId.Length != _grpcServices.Value.IdLength)
                {
                    ModelState.AddModelError(nameof(request.LocalityId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (ModelState.IsValid)
                {
                    Headers.BindLanguage(header, request);
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
                    var client = new IMaster.IMasterClient(channel);
                    returnList = await client.GetLocalityAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetLocality() " + ex.Message);
            }
            return Ok(returnList);
        }

        [HttpGet]
        [Route(nameof(GetLanguage))]
        public async Task<IActionResult> GetLanguage([FromQuery] mdlLanguageRequest request, [FromHeader] dtoHeaders header)
        {
            mdlLanguageList returnList = new mdlLanguageList();
            try
            {
                if (!string.IsNullOrEmpty(request.LanguageId) && request.LanguageId.Length != _grpcServices.Value.IdLength)
                {
                    ModelState.AddModelError(nameof(request.LanguageId), enmErrorMessage.IdentifierLength.ToString());
                }

                
                if (ModelState.IsValid)
                {
                    Headers.BindLanguage(header, request);
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
                    var client = new IMaster.IMasterClient(channel);
                    returnList = await client.GetLanguageAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetLanguage() " + ex.Message);
            }
            return Ok(returnList);
        }

    }
}

#if(false)

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        

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

#endif
