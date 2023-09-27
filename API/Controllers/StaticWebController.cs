using API.Models;
using AutoMapper;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using srvProduct.protos;
using srvStaticWeb.protos;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaticWebController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<StaticWebController> _logger;
        private readonly IOptions<GRPCServices> _grpcServices;
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

        public StaticWebController(IOptions<GRPCServices> grpcServices, ILogger<StaticWebController> logger, IOptions<ApiBehaviorOptions> apiBehaviorOptions, IMapper mapper)
        {
            _grpcServices = grpcServices;
            _logger = logger;
            _apiBehaviorOptions = apiBehaviorOptions;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAboutUs")]
        public async Task<IActionResult> GetAboutUs([FromQuery] mdlAboutUsRequest request)
        {   
            mdlAboutUsList returnList = new mdlAboutUsList();
            try
            {   
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.StaticWebServices);
                    var client = new IStaticWeb.IStaticWebClient(channel);
                    returnList = await client.GetAboutUsAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: StaticWebController.GetAboutUs() " + ex.Message);
            }
            return Ok(returnList);
        }


        [HttpGet]
        [Route("GetJoinUs")]
        public async Task<IActionResult> GetJoinUs([FromQuery] mdlJoinUsRequest request)
        {
            mdlJoinUsList returnList = new mdlJoinUsList();
            try
            {
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.StaticWebServices);
                    var client = new IStaticWeb.IStaticWebClient(channel);
                    returnList = await client.GetJoinUsAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: StaticWebController.GetJoinUs() " + ex.Message);
            }
            return Ok(returnList);
        }


        [HttpGet]
        [Route("GetFAQUs")]
        public async Task<IActionResult> GetFAQUs([FromQuery] mdlFAQRequest request)
        {
            mdlFAQList returnList = new mdlFAQList();
            try
            {
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.StaticWebServices);
                    var client = new IStaticWeb.IStaticWebClient(channel);
                    returnList = await client.GetFAQAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: StaticWebController.GetFAQ() " + ex.Message);
            }
            return Ok(returnList);
        }


        [HttpGet]
        [Route("GetOffice")]
        public async Task<IActionResult> GetOffice([FromQuery] mdlOfficeRequest request)
        {
            mdlOfficeList returnList = new mdlOfficeList();
            try
            {
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.StaticWebServices);
                    var client = new IStaticWeb.IStaticWebClient(channel);
                    returnList = await client.GetOfficeAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: StaticWebController.GetOffice() " + ex.Message);
            }
            return Ok(returnList);
        }


        [HttpGet]
        [Route("GetContactUs")]
        public async Task<IActionResult> GetContactUs([FromQuery] mdlContactUsRequest request)
        {
            mdlContactUsList returnList = new mdlContactUsList();
            try
            {
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.StaticWebServices);
                    var client = new IStaticWeb.IStaticWebClient(channel);
                    returnList = await client.GetContactUsAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: StaticWebController.GetContactUs() " + ex.Message);
            }
            return Ok(returnList);
        }

        [HttpGet]
        [Route("GetComplaint")]
        public async Task<IActionResult> GetComplaint([FromQuery] mdlComplaintRequest request)
        {
            mdlComplaintList returnList = new mdlComplaintList();
            try
            {
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.StaticWebServices);
                    var client = new IStaticWeb.IStaticWebClient(channel);
                    returnList = await client.GetComplaintAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: StaticWebController.GetComplaint() " + ex.Message);
            }
            return Ok(returnList);
        }


        [HttpPost]
        [Route("SaveAboutUs")]
        public async Task<IActionResult> SaveAboutUs([FromBody] dtoAboutUs request)
        {
            mdlStaticWebSaveResponse returnData = new mdlStaticWebSaveResponse();
            try
            {
                if (!string.IsNullOrEmpty(request.AboutUsId) && request.AboutUsId.Length != _grpcServices.Value.IdLength)
                {
                    ModelState.AddModelError(nameof(request.AboutUsId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.DefaultName))
                {
                    ModelState.AddModelError(nameof(request.DefaultName), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (request.AboutUsDetail == null || request.AboutUsDetail.Count()==0)
                {
                    ModelState.AddModelError(nameof(request.AboutUsDetail), enmErrorMessage.IdentifierRequired.ToString());
                }

                if (ModelState.IsValid)
                {
                    mdlAboutUs model = _mapper.Map<mdlAboutUs>(request);
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new IStaticWeb.IStaticWebClient(channel);
                    returnData = await client.SetAboutUsAsync(model);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: StaticWebController.SaveAboutUs()" + ex.Message);
            }
            return Ok(returnData);
        }

        [HttpPost]
        [Route("SaveJoinUs")]
        public async Task<IActionResult> SaveJoinUs([FromBody] dtoJoinUs request)
        {
            mdlStaticWebSaveResponse returnData = new mdlStaticWebSaveResponse();
            try
            {
                if (!string.IsNullOrEmpty(request.JoinUsId) && request.JoinUsId.Length != _grpcServices.Value.IdLength)
                {
                    ModelState.AddModelError(nameof(request.JoinUsId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.DefaultName))
                {
                    ModelState.AddModelError(nameof(request.DefaultName), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (request.JoinUsDetail == null || request.JoinUsDetail.Count() == 0)
                {
                    ModelState.AddModelError(nameof(request.JoinUsDetail), enmErrorMessage.IdentifierRequired.ToString());
                }

                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new IStaticWeb.IStaticWebClient(channel);
                    returnData = await client.SetJoinUsAsync(_mapper.Map<mdlJoinUs>( request));
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: StaticWebController.SaveJoinUs() " + ex.Message);
            }
            return Ok(returnData);
        }

        [HttpPost]
        [Route("SaveFAQ")]
        public async Task<IActionResult> SaveFAQ([FromBody] dtoFAQ request)
        {
            mdlStaticWebSaveResponse returnData = new mdlStaticWebSaveResponse();
            try
            {
                if (!string.IsNullOrEmpty(request.FAQId) && request.FAQId.Length != _grpcServices.Value.IdLength)
                {
                    ModelState.AddModelError(nameof(request.FAQId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.DefaultQuestion))
                {
                    ModelState.AddModelError(nameof(request.DefaultQuestion), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (request.FAQDetail == null || request.FAQDetail.Count() == 0)
                {
                    ModelState.AddModelError(nameof(request.FAQDetail), enmErrorMessage.IdentifierRequired.ToString());
                }                

                if (ModelState.IsValid)
                {
                    
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new IStaticWeb.IStaticWebClient(channel);
                    returnData = await client.SetFAQAsync(_mapper.Map<mdlFAQ>( request));
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: StaticWebController.SaveFAQ() " + ex.Message);
            }
            return Ok(returnData);
        }


        [HttpPost]
        [Route("SaveOffice")]
        public async Task<IActionResult> SaveOffice([FromBody] dtoOffice request)
        {
            mdlStaticWebSaveResponse returnData = new mdlStaticWebSaveResponse();
            try
            {
                if (!string.IsNullOrEmpty(request.OfficeId) && request.OfficeId.Length != _grpcServices.Value.IdLength)
                {
                    ModelState.AddModelError(nameof(request.OfficeId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.DefaultLocation))
                {
                    ModelState.AddModelError(nameof(request.DefaultLocation), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (request.OfficeDetail == null || request.OfficeDetail.Count() == 0)
                {
                    ModelState.AddModelError(nameof(request.OfficeDetail), enmErrorMessage.IdentifierRequired.ToString());
                }

                if (ModelState.IsValid)
                {                   
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new IStaticWeb.IStaticWebClient(channel);
                    returnData = await client.SetOfficeAsync(_mapper.Map<mdlOffice>(request));
                    
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: StaticWebController.SaveOffice() " + ex.Message);
            }
            return Ok(returnData);
        }

        [HttpPost]
        [Route("SaveContactUs")]
        public async Task<IActionResult> SaveContactUs([FromBody] mdlContactUs request)
        {
            mdlStaticWebSaveResponse returnData = new mdlStaticWebSaveResponse();
            try
            {
                
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    ModelState.AddModelError(nameof(request.Name), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    ModelState.AddModelError(nameof(request.Email), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Subject))
                {
                    ModelState.AddModelError(nameof(request.Subject), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Messages))
                {
                    ModelState.AddModelError(nameof(request.Messages), enmErrorMessage.IdentifierRequired.ToString());
                }

                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new IStaticWeb.IStaticWebClient(channel);
                    returnData = await client.SetContactUsAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: StaticWebController.SaveContactUs() " + ex.Message);
            }
            return Ok(returnData);
        }

        [HttpPost]
        [Route("SaveComplaint")]
        public async Task<IActionResult> SaveComplaint([FromBody] mdlComplaint request)
        {
            mdlStaticWebSaveResponse returnData = new mdlStaticWebSaveResponse();
            try
            {

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    ModelState.AddModelError(nameof(request.Name), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    ModelState.AddModelError(nameof(request.Email), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Subject))
                {
                    ModelState.AddModelError(nameof(request.Subject), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Messages))
                {
                    ModelState.AddModelError(nameof(request.Messages), enmErrorMessage.IdentifierRequired.ToString());
                }

                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new IStaticWeb.IStaticWebClient(channel);
                    returnData = await client.SetComplaintAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: StaticWebController.SaveComplaint() " + ex.Message);
            }
            return Ok(returnData);
        }

    }
}
