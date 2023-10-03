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
}
