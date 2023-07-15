using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using Microsoft.Extensions.Options;
using API.Models;
using System.ServiceModel.Channels;

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
        public async Task<IActionResult> GetCategory()
        {
            throw new NotImplementedException();
            //ReturnList <Category> returnList = new ();
            //if (!string.IsNullOrEmpty(categoryRequest.CategoryId) && categoryRequest.CategoryId.Length!=24)
            //{
            //    ModelState.AddModelError(nameof(CategoryRequest.CategoryId), enmErrorMessage.IdentifierLength.ToString());
            //}
            //if (ModelState.IsValid)
            //{
            //    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
            //    var client = channel.CreateGrpcService<ICategoryService>();
            //    returnList = await client.Get(categoryRequest);
            //}
            //else
            //{
            //    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            //}
            //return Ok( returnList);

        }

        [HttpPost]
        [Route("SaveCategory")]
        public async Task<IActionResult> SaveCategory()
        {
            throw new NotImplementedException();
            //ReturnData returnData = new ();
            //try
            //{
            //    if (string.IsNullOrWhiteSpace(request.Name))
            //    {
            //        ModelState.AddModelError(nameof(Category.Name), enmErrorMessage.IdentifierRequired.ToString());
            //    }
            //    if (ModelState.IsValid)
            //    {
            //        using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
            //        var client = channel.CreateGrpcService<ICategoryService>();
            //        returnData = await client.Save(request);
            //    }
            //    else
            //    {
            //        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    returnData.Message = ex.Message;
            //    _logger.LogError(ex, "Error: ProductController.SaveCategory() " + ex.Message);
            //}
            //return Ok(returnData);
            
        }
    }
}