using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class dtoSigninRequest
    {

        public string Email { get; set; } = "";
        public string? IpAddress {get;set;} = "";
        public string Password {get;set;} = "";
        public string? CaptchaId { get; set; }
        public string? CaptchaValue { get; set; }
        public string? DeviceId {get;set;}
        public string? DeviceName { get; set; }
    }

    public class dtoSignInResponse
    {
        public string UserId { get; set; } = "";
        public string Email { get; set; } = "";
        public string NickName { get; set; } = "";
        public string Message { get; set; } = "";
        public string MessageId { get; set; } = "";
        public int StatusId { get; set; }
        public bool Status { get; set; }
        public List<string> roleName { get; set; } = new();
        public string? Token { get; set; }
        public int ExpireIn { get; set; }
    }


    public class dtoUser 
    {        
        public string UserId { get; set; } = null!;
        public string? NickName { get; set; }
        public string Email { get; set; } = null!;
        public int PartitionId { get; set; }
        public string? sponsorUserId { get; set; }
        public int LegId { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        public bool IsActive { get; set; }
        public dtoUserDetail UserDetail { get; set; } = new();
        public dtoUserLoginDetail UserLoginDetail = new();

    }

    public class dtoUserDetail 
    {
        
        public string DetailId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public int PartitionId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string? AlternateEmail { get; set; }
        public string ContactNo { get; set; } = null!;
        public string? AlternateContactNo { get; set; }
        public dtoUserAddress userAddress { get; set; } = new();
        public string Language { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public double CurrentRank { get; set; }
        public bool IsTerminate { get; set; }
        public DateTime TerminateDt { get; set; }
        public string? Remarks { get; set; }
        public string? profileImage { get; set; }

    }

    public class dtoUserAddress
    {
        public string CountryId { get; set; } = null!;
        public string StateId { get; set; } = null!;
        public string CityId { get; set; } = null!;
        public string LocalityId { get; set; } = null!;
        public string Pincode { get; set; } = null!;
        public string Address { get; set; } = null!;
    }

    public class dtoUserLoginDetail
    {
        public string LoginLogId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public int PartitionId { get; set; }
        public string Password { get; set; } = null!;
        public byte[] Salt { get; set; } = null!;
        public bool ForceToChangePassword { get; set; }
        public DateTime PasswordExpiryDt { get; set; }
        public bool IsTempBlock { get; set; }
        public int FailCounter { get; set; }
        public DateTime TempBlockDt { get; set; }
        public string? LastPassword { get; set; }
        public List<string> RegisterDeviceId { get; set; } = new();
        public List<dtoLastLoginHistory> LoginLog { get; set; } = new();
        public List<string> Roles { get; set; } = new();        
    }

    public class dtoLastLoginHistory
    {
        public DateTime LoginDt { get; set; }
        public string IpAddress { get; set; } = null!;
        public string DeviceId { get; set; } = null!;
        public string? DeviceName { get; set; }
        public bool IsSuccess { get; set; }
    }

}
