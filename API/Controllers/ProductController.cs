using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using ProductServicesProt;
using DTO;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        
        private readonly ILogger<ProductController> _logger;
        private readonly IOptions<GRPCServices> _grpcServices;

        public ProductController(IOptions<GRPCServices> grpcServices, ILogger<ProductController> logger)
        {
            _grpcServices = grpcServices;
            _logger = logger;
        }

        
        [HttpGet]
        [Route("GetCategory")]
        public async Task<IActionResult> GetCategory([FromQuery] CategoryRequest categoryRequest,
             [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            ReturnList <Category> returnList = new ();
            if (!string.IsNullOrEmpty(categoryRequest.CategoryId) && categoryRequest.CategoryId.Length!=24)
            {
                ModelState.AddModelError(nameof(CategoryRequest.CategoryId),"At least 24 char");
            }
            if (ModelState.IsValid)
            {
                using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
                var client = channel.CreateGrpcService<ICategoryService>();
                returnList = await client.Get(categoryRequest);
            }
            else
            {
                return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }
            return Ok( returnList);

        }

        [HttpPost]
        [Route("SaveCategory")]
        public async Task<ReturnData> SaveCategory([FromBody] Category request)
        {

            using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
            var client = channel.CreateGrpcService<ICategoryService>();
            var reply = await client.Save(request);
            return reply;
        }
    }
}