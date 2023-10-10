using ProductServices;
using System;

namespace API.Models
{
 
    public class dtoCategory
    {
        public string? CategoryId { get; set; }
        public string DefaultName { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public List<dtoCategoryDetail>? CategoryDetail { get; set; }
    }

    public class dtoCategoryDetail
    {
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string? ShortDesc { get; set; }
    }

    public class dtoSubCategory
    {
        public string? SubCategoryId { get; set; }
        public string? CategoryId { get; set; }
        public string DefaultName { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public List<dtoSubCategoryDetail>? SubCategoryDetail { get; set; }
    }

    public class dtoSubCategoryDetail
    {
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string? ShortDesc { get; set; } = string.Empty;
        public List<string> Keywords { get; set; } = new();
        public List<dtoProperty> Properties { get; set; } = new();
    }

    public class dtoProperty
    {
        public int PropertyDisplayOrder { get; set; }
        public string Name { get; set; } = null!;
        public enmPropertyType Type { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public string? Regx { get; set; }
        public List<string>? Option { get; set; } = new List<string>();
    }

    


}
