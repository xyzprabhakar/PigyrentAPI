using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using Microsoft.Extensions.Options;
using API.Models;
using System.ServiceModel.Channels;
using srvMasters.protos;
using srvProduct.protos;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        
        private readonly ILogger<ProductController> _logger;
        private readonly IOptions<GRPCServices> _grpcServices;
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

        public ProductController(IOptions<GRPCServices> grpcServices, ILogger<ProductController> logger, IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            _grpcServices = grpcServices;
            _logger = logger;
            _apiBehaviorOptions = apiBehaviorOptions;
        }

        
        [HttpGet]
        [Route("GetCategory")]        
        public async Task<IActionResult> GetCategory([FromQuery]mdlCategoryRequest request)
        {
            mdlCategoryList returnList = new mdlCategoryList();
            try
            {
                if (!string.IsNullOrEmpty(request.CategoryId) && _grpcServices.Value.IdLength != request.CategoryId.Length)
                {
                    ModelState.AddModelError(nameof(request.CategoryId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new ICategoryService.ICategoryServiceClient(channel);
                    returnList = await client.GetCategoryAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetCategory() " + ex.Message);
            }
            return Ok(returnList);

            
        }

        [HttpPost]
        [Route("SaveCategory")]
        public async Task<IActionResult> SaveCategory([FromBody]mdlCategory request)
        {
            mdlCategorySaveResponse returnData = new mdlCategorySaveResponse();
            try
            {
                if (!string.IsNullOrEmpty(request.CategoryId) && _grpcServices.Value.IdLength != request.CategoryId.Length)
                {
                    ModelState.AddModelError(nameof(request.CategoryId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    ModelState.AddModelError(nameof(request.Name), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new ICategoryService.ICategoryServiceClient(channel);
                    returnData = await client.SaveCategoryAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: MasterController.SaveCategory() " + ex.Message);
            }
            return Ok(returnData);
        }
    }
}