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
        [Route("GetCategoryByName")]
        public async Task<IActionResult> GetCategoryByName([FromQuery] mdlCategoryNameRequest request)
        {
            mdlCategoryList returnList = new mdlCategoryList();
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    ModelState.AddModelError(nameof(request.Name), enmErrorMessage.IdentifierLength.ToString());
                }
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new ICategoryService.ICategoryServiceClient(channel);
                    returnList = await client.GetCategoryByNameAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetCategoryByName() " + ex.Message);
            }
            return Ok(returnList);
        }

        [HttpGet]
        [Route("GetCategoryById")]
        public async Task<IActionResult> GetCategoryById([FromQuery] mdlCategoryIdRequest request)
        {
            mdlCategoryList returnList = new mdlCategoryList();
            try
            {
                if (string.IsNullOrEmpty(request.CategoryId))
                {
                    ModelState.AddModelError(nameof(request.CategoryId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new ICategoryService.ICategoryServiceClient(channel);
                    returnList = await client.GetCategoryByIdAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetCategoryByName() " + ex.Message);
            }
            return Ok(returnList);
        }

        [HttpGet]
        [Route("GetSubCategoryByName")]
        public async Task<IActionResult> GetSubCategoryByName([FromQuery] mdlSubCategoryNameRequest request)
        {
            mdlSubCategoryList returnList = new mdlSubCategoryList();
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    ModelState.AddModelError(nameof(request.Name), enmErrorMessage.IdentifierLength.ToString());
                }
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new ICategoryService.ICategoryServiceClient(channel);
                    returnList = await client.GetSubCategoryByNameAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetSubCategoryByName() " + ex.Message);
            }
            return Ok(returnList);
        }

        [HttpGet]
        [Route("GetSubCategoryById")]
        public async Task<IActionResult> GetSubCategoryById([FromQuery] mdlSubCategoryIdRequest request)
        {
            mdlSubCategoryList returnList = new mdlSubCategoryList();
            try
            {
                if (string.IsNullOrEmpty(request.CategoryId))
                {
                    ModelState.AddModelError(nameof(request.CategoryId), enmErrorMessage.IdentifierLength.ToString());
                }
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new ICategoryService.ICategoryServiceClient(channel);
                    returnList = await client.GetSubCategoryByIdAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetSubCategoryById() " + ex.Message);
            }
            return Ok(returnList);
        }


        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory([FromQuery] mdlCategoryRequest request)
        {
            mdlCategoryList returnList = new mdlCategoryList();
            try
            {                
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new ICategoryService.ICategoryServiceClient(channel);
                    returnList = await client.GetAllCategoryAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetAllCategory() " + ex.Message);
            }
            return Ok(returnList);
        }


        [HttpGet]
        [Route("GetAllSubCategory")]
        public async Task<IActionResult> GetAllSubCategory([FromQuery] mdlSubCategoryRequest request)
        {
            mdlSubCategoryList returnList = new mdlSubCategoryList();
            try
            {
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new ICategoryService.ICategoryServiceClient(channel);
                    returnList = await client.GetAllSubCategoryAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetAllCategory() " + ex.Message);
            }
            return Ok(returnList);
        }


        [HttpGet]
        [Route("GetAllCategoryIncludeSubCategory")]
        public async Task<IActionResult> GetAllCategoryIncludeSubCategory([FromQuery] mdlCategoryRequest request)
        {
            mdlCategoryList returnList = new mdlCategoryList();
            try
            {
                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new ICategoryService.ICategoryServiceClient(channel);
                    returnList = await client.GetAllCategoryIncludeSubCategoryAsync(request);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MasterController.GetAllCategoryIncludeSubCategory() " + ex.Message);
            }
            return Ok(returnList);
        }


        [HttpPost]
        [Route("SaveCategory")]
        public async Task<IActionResult> SaveCategory([FromBody] mdlCategoryWrapper request)
        {
            mdlCategorySaveResponse returnData = new mdlCategorySaveResponse();
            try
            {
                if (request.Category == null)
                {
                    ModelState.AddModelError(nameof(request.Category), enmErrorMessage.IdentifierRequired.ToString());
                }
                else 
                {
                    if (!string.IsNullOrEmpty(request.Category!.CategoryId) && request.Category.CategoryId.Length != _grpcServices.Value.IdLength )
                    {
                        ModelState.AddModelError(nameof(request.Category.CategoryId), enmErrorMessage.IdentifierLength.ToString());
                    }
                    if (string.IsNullOrWhiteSpace(request.Category.DefaultName))
                    {
                        ModelState.AddModelError(nameof(request.Category.DefaultName), enmErrorMessage.IdentifierRequired.ToString());
                    }
                    if (request.Category.CategoryDetail == null || request.Category.CategoryDetail.Count==0 )
                    {
                        ModelState.AddModelError(nameof(request.Category.CategoryDetail), enmErrorMessage.IdentifierRequired.ToString());
                    }                    
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

        [HttpPost]
        [Route("SaveSubCategory")]
        public async Task<IActionResult> SaveSubCategory([FromBody] mdlSubCategoryWrapper request)
        {
            mdlCategorySaveResponse returnData = new mdlCategorySaveResponse();
            try
            {
                if (request.SubCategory == null)
                {
                    ModelState.AddModelError(nameof(request.SubCategory), enmErrorMessage.IdentifierRequired.ToString());
                }
                else
                {
                    if (!string.IsNullOrEmpty(request.SubCategory!.CategoryId) && request.SubCategory.CategoryId.Length != _grpcServices.Value.IdLength)
                    {
                        ModelState.AddModelError(nameof(request.SubCategory.CategoryId), enmErrorMessage.IdentifierLength.ToString());
                    }
                    if (string.IsNullOrWhiteSpace(request.SubCategory.DefaultName))
                    {
                        ModelState.AddModelError(nameof(request.SubCategory.DefaultName), enmErrorMessage.IdentifierRequired.ToString());
                    }
                    if (request.SubCategory.SubCategoryDetail == null)
                    {
                        ModelState.AddModelError(nameof(request.SubCategory.SubCategoryDetail), enmErrorMessage.IdentifierRequired.ToString());
                    }
                }

                if (ModelState.IsValid)
                {
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                    var client = new ICategoryService.ICategoryServiceClient(channel);
                    returnData = await client.SaveSubCategoryAsync(request);
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