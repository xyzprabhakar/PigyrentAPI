using Microsoft.AspNetCore.Mvc;

namespace API.Models
{
    public class dtoHeaders
    {   
        public bool IncludeAllLanguage { get; set; }        
        public string? Language { get; set; }        
        public string? Longitude { get; set; }        
        public string? Latitude { get; set; }        
        public string? UserId { get; set; }        
        public string? Token{ get; set; }
    }

    public abstract class ModifyDetails
    {
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        public bool IsActive { get; set; }
        //public string? CreatedBy { get; set; }
        //public DateTime CreatedDt { get; set; }
    }

    public abstract class BasicDetails
    {
        public string Name { get; set; } = null!;
        public string Language { get; set; } = null!;
    }

}
