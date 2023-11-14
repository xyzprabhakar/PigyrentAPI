using API.Models;
using AutoMapper;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StaticContentServices;
using System.Security.Cryptography;
using UserDetail;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserDetailsController> _logger;
        private readonly IOptions<GRPCServices> _grpcServices;
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

        public UserDetailsController(IOptions<GRPCServices> grpcServices, ILogger<UserDetailsController> logger, IOptions<ApiBehaviorOptions> apiBehaviorOptions, IMapper mapper)
        {
            _grpcServices = grpcServices;
            _logger = logger;
            _apiBehaviorOptions = apiBehaviorOptions;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("SaveUser")]
        public async Task<IActionResult> SaveUser([FromBody] dtoUser request)
        {
            mdlUserDetailSaveResponse returnData = new mdlUserDetailSaveResponse();
            bool IsUpdate = false;
            try
            {
                if (!string.IsNullOrEmpty(request.UserId))
                {
                    IsUpdate = true;
                }
                if (IsUpdate)
                {
                    if (request.UserId.Length != _grpcServices.Value.IdLength)
                    {
                        ModelState.AddModelError(nameof(request.UserId), enmErrorMessage.IdentifierLength.ToString());
                    }
                }
                else
                {
                    if (request.UserLoginDetail == null)
                    {
                        ModelState.AddModelError(nameof(request.UserLoginDetail), enmErrorMessage.IdentifierRequired.ToString());
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(request.UserLoginDetail.Password))
                        {
                            ModelState.AddModelError(nameof(request.UserLoginDetail.Password), enmErrorMessage.IdentifierRequired.ToString());
                        }
                    }

                }
                
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    ModelState.AddModelError(nameof(request.Email), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (request.UserDetail == null)
                {
                    ModelState.AddModelError(nameof(request.UserLoginDetail), enmErrorMessage.IdentifierRequired.ToString());
                }
                else 
                {
                    if (string.IsNullOrWhiteSpace(request.UserDetail.FirstName))
                    {
                        ModelState.AddModelError(nameof(request.UserDetail.FirstName), enmErrorMessage.IdentifierRequired.ToString());
                    }
                    if (string.IsNullOrWhiteSpace(request.UserDetail.LastName))
                    {
                        ModelState.AddModelError(nameof(request.UserDetail.LastName), enmErrorMessage.IdentifierRequired.ToString());
                    }
                    if (string.IsNullOrWhiteSpace(request.UserDetail.Language))
                    {
                        ModelState.AddModelError(nameof(request.UserDetail.Language), enmErrorMessage.IdentifierRequired.ToString());
                    }
                    if (string.IsNullOrWhiteSpace(request.UserDetail.ContactNo))
                    {
                        ModelState.AddModelError(nameof(request.UserDetail.ContactNo), enmErrorMessage.IdentifierRequired.ToString());
                    }
                    if (request.UserDetail.userAddress == null)
                    {
                        ModelState.AddModelError(nameof(request.UserDetail.userAddress), enmErrorMessage.IdentifierRequired.ToString());
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(request.UserDetail.userAddress.CountryId ))
                        {
                            ModelState.AddModelError(nameof(request.UserDetail.userAddress.CountryId), enmErrorMessage.IdentifierRequired.ToString());
                        }
                        if (string.IsNullOrWhiteSpace(request.UserDetail.userAddress.StateId))
                        {
                            ModelState.AddModelError(nameof(request.UserDetail.userAddress.StateId), enmErrorMessage.IdentifierRequired.ToString());
                        }
                        if (string.IsNullOrWhiteSpace(request.UserDetail.userAddress.Pincode))
                        {
                            ModelState.AddModelError(nameof(request.UserDetail.userAddress.Pincode), enmErrorMessage.IdentifierRequired.ToString());
                        }
                    }

                }

                if (ModelState.IsValid)
                {
                    mdlUser model = _mapper.Map<mdlUser>(request);
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.UserDetailServices);
                    var client = new IUserDetail.IUserDetailClient(channel);
                    returnData = await client.CreateUserAsync(model);
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: UserDetailsController.SaveUser()" + ex.Message);
            }
            return Ok(returnData);
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] dtoSigninRequest request)
        {
            dtoSignInResponse returnData = new dtoSignInResponse();
            try
            {
                
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    ModelState.AddModelError(nameof(request.Email), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    ModelState.AddModelError(nameof(request.Password), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<mdlLoginRequest>(request);
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.UserDetailServices);
                    var client = new IUserDetail.IUserDetailClient (channel);
                    returnData = _mapper.Map<dtoSignInResponse>( await client.LoginAsync(model));
                }
                else
                {
                    return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: UserDetailsController.SignIn() " + ex.Message);
            }
            return Ok(returnData);
        }

    }
}
