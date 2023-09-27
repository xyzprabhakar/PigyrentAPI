using Microsoft.AspNetCore.Mvc;

namespace API.Models
{
    public class dtoHeaders
    {
        [FromHeader]
        public bool IncludeAllLanguage { get; set; }
        [FromHeader]
        public string? Language { get; set; }
    }
}
