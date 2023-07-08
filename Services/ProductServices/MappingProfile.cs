using AutoMapper;
using ProductServices.DB;
using ProductServicesProt;

namespace ProductServices
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryProperty, tblCategoryProperty>();
            CreateMap<Category, tblCategory>();
        }
    }
}
