using API.Models;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        public async Task<IActionResult> SignIn([FromBody] dtoSigninRequest request, [FromServices]IHttpContextAccessor httpContextAccessor, [FromServices]IConfiguration config)
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
                    request.IpAddress = httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                    request.DeviceName = httpContextAccessor?.HttpContext?.Request?.Headers["User-Agent"]??"";
                    request.DeviceId = "";
                    var model = _mapper.Map<mdlLoginRequest>(request);
                    using var channel = GrpcChannel.ForAddress(_grpcServices.Value.UserDetailServices);
                    var client = new IUserDetail.IUserDetailClient (channel);
                    var loginResponse = _mapper.Map<dtoSignInResponse>(await client.LoginAsync(model));
                    loginResponse.Token = GenrateJwtToken(loginResponse, config);
                    returnData = loginResponse;
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

        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] dtoSignUpRequest request, [FromServices] IHttpContextAccessor httpContextAccessor, [FromServices] IConfiguration config)
        {
            mdlUserDetailSaveResponse returnData = new mdlUserDetailSaveResponse();
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
                if (string.IsNullOrWhiteSpace(request.FirstName))
                {
                    ModelState.AddModelError(nameof(request.FirstName), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.LastName))
                {
                    ModelState.AddModelError(nameof(request.LastName), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Contact))
                {
                    ModelState.AddModelError(nameof(request.Contact), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Country))
                {
                    ModelState.AddModelError(nameof(request.Country), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.State))
                {
                    ModelState.AddModelError(nameof(request.State), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Address))
                {
                    ModelState.AddModelError(nameof(request.Address), enmErrorMessage.IdentifierRequired.ToString());
                }
                if (string.IsNullOrWhiteSpace(request.Pincode))
                {
                    ModelState.AddModelError(nameof(request.Pincode), enmErrorMessage.IdentifierRequired.ToString());
                }

                if (ModelState.IsValid)
                {
                    string? Language = httpContextAccessor?.HttpContext?.Request?.Headers["Language"];
                    if (string.IsNullOrEmpty(Language))
                    {
                        Language = config.GetValue<string>("CustomSetting:DefaultLangulage");
                    }
                    string? Currency = httpContextAccessor?.HttpContext?.Request?.Headers["Currency"];
                    if (string.IsNullOrEmpty(Currency))
                    {
                        Currency = config.GetValue<string>("CustomSetting:DefaultCurrency");
                    }
                    mdlUser model=new mdlUser() { 
                        Email= request.Email,
                        IsActive=true,
                        NickName=string.Empty,
                        SponsorUserId=request.SponsorUserId,                       
                        ModifiedDt=Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc) ),
                        ModifiedBy=string.Empty,
                        UserDetail=new mdlUserDetail()
                        {
                            FirstName= request.FirstName,
                            LastName= request.LastName,
                            MiddleName= request.MiddleName,
                            ContactNo=request.Contact,
                            Language= Language,
                            Currency = Currency,
                            IsTerminate=false,
                            UserAddress=new mdlUserAddress()
                            {
                                CountryId=request.Country,
                                StateId=request.State,
                                CityId=request.City,
                                LocalityId=request.LocalityId,
                                Address=request.Address,
                                Pincode=request.Pincode
                            }

                            
                        },
                        UserLoginDetail=new mdlUserLoginDetail()
                        {
                            ForceToChangePassword=false,
                            IsTempBlock=false,
                            Password=request.Password,                            
                        }
                    };
                    string? DeviceName= httpContextAccessor?.HttpContext?.Request?.Headers["User-Agent"] ?? "";
                    model.UserLoginDetail.RegisterDeviceId.Add(DeviceName);                    

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
                _logger.LogError(ex, "Error: UserDetailsController.SignIn() " + ex.Message);
            }
            return Ok(returnData);
        }




        private string GenrateJwtToken(dtoSignInResponse request, IConfiguration config)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]??""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, request.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, request.NickName));
            foreach (var role in request.roleName)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            request.ExpireIn = Convert.ToInt32(config["Jwt:TokenExpiryMin"]);
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        var token = new JwtSecurityToken(config["Jwt:Issuer"],
              config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(request.ExpireIn),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        [Route("GetAllUser")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(new List<string>() { "prabhakar", "Mohan" });
        }

    }
}
