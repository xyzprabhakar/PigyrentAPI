using System;

namespace API.Models
{
    public class dtoAboutUs
    {
        public string AboutUsId { get; set; } = string.Empty;
        public string DefaultName { get; set; } = null!;
        public List<dtoAboutUsDetail> AboutUsDetail { get; set; } = new();
        public int DisplayOrder { get; set; }
    }

    public class dtoAboutUsDetail
    {
        public string AboutUsDetailId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Heading { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }


    public class dtoJoinUs
    {
        public string JoinUsId { get; set; } = string.Empty;
        public string DefaultName { get; set; } = null!;
        public List<dtoJoinUsDetail> JoinUsDetail { get; set; } = new();
        public int DisplayOrder { get; set; }
    }

    public class dtoJoinUsDetail
    {
        public string JoinUsDetailId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Heading { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    public class dtoFAQ
    {
        public string FAQId { get; set; } = string.Empty;
        public string DefaultQuestion { get; set; } = null!;
        public List<dtoFAQDetail> FAQDetail { get; set; } = new();
        public int DisplayOrder { get; set; }
    }

    public class dtoFAQDetail
    {
        public string FAQDetailId { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;        
    }

    public class dtoOffice
    {
        public string OfficeId { get; set; } = string.Empty;
        public bool isActive { get; set; }
        public bool isHeadOffice { get; set; }
        public string DefaultLocation { get; set; }=string.Empty;        
        public List<dtoOfficeDetail> OfficeDetail { get; set; } = new();
        public int DisplayOrder { get; set; }
    }

    public class dtoOfficeDetail
    {
        public string OfficeDetailId { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Address{ get; set; } = string.Empty;
        public string? ContactNo { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public string? Image{ get; set; }
        public string Language { get; set; } = string.Empty;
    }


}
