using API.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;
        public MasterController(IOptions<GRPCServices> grpcServices, ILogger<MasterController> logger, IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            _logger = logger;
            _grpcServices = grpcServices;
            _apiBehaviorOptions = apiBehaviorOptions;
        }

        [HttpGet]
        [Route(nameof(GetAllCurrency))]
        public async Task<IActionResult> GetAllCurrency([FromQuery] string? currencyId, [FromQuery] string? code, [FromQuery] bool allData)
        {
            throw new NotImplementedException();
            
            //ReturnList<> returnList = new() { ReturnData=new ()};
            //bool loadById=false;
            //RequestData requestData = new RequestData() { RequestId=string.Empty};
            //try
            //{
            //    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.MasterServices);
            //    var client = channel.CreateGrpcService<ICurrencyService>();

            //    if (!string.IsNullOrEmpty(currencyId) && currencyId.Length != _grpcServices.Value.IdLength)
            //    {
            //        ModelState.AddModelError(nameof(currencyId), enmErrorMessage.IdentifierLength.ToString());
            //        requestData.RequestId = currencyId;
            //        loadById = true;
            //    }

            //    if (ModelState.IsValid)
            //    {
            //        if (loadById)
            //        {
            //            var tempData = await client.GetById(requestData);
            //            if (tempData != null)
            //            {
            //                returnList.ReturnData.Add(tempData);
            //            }
            //        }
            //        else if (!string.IsNullOrWhiteSpace(code))
            //        {
            //            requestData.RequestId = code;
            //            var tempData = await client.GetByCode(requestData);
            //            if (tempData != null)
            //            {
            //                returnList.ReturnData.Add(tempData);
            //            }
            //        }
            //        else if (allData)
            //        {
                        
            //            returnList =await client.GetAll()??new ReturnList<mdlCurrency>();
            //        }

            //    }
            //    else
            //    {
            //        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            //    }
            //}
            //catch (Exception ex)
            //{   
            //    _logger.LogError(ex, "Error: MasterController.GetAllCurrency() " + ex.Message);
            //}
            //return Ok( returnList);
        }

        [HttpPost]
        [Route(nameof(SaveCurrency))]
        public async Task<IActionResult> SaveCurrency()//[FromBody] mdlCurrency request)
        {

            throw new NotImplementedException();
            //RequestData requestData = new RequestData() { RequestId = "Hi"};
            //ReturnData returnData = new();
            //try
            //{
            //    //if (string.IsNullOrWhiteSpace(request.CurrencyCode))
            //    //{
            //    //    ModelState.AddModelError(nameof(mdlCurrency.CurrencyCode), enmErrorMessage.IdentifierRequired.ToString());
            //    //}
            //    if (ModelState.IsValid)
            //    {
            //        using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
            //        var client = channel.CreateGrpcService<ICurrencyService>();
            //        var temp = await client.GetAll();
            //        //var tempData=await client.GetById1(requestData);
            //        returnData = await client.SaveCurrency(requestData);
            //    }
            //    else
            //    {
            //        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    returnData.Message = ex.Message;
            //    _logger.LogError(ex, "Error: MasterController.SaveCurrency() " + ex.Message);
            //}
            //return Ok(returnData);
        }
    }
}
