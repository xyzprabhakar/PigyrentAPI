using DTO;
using ProtoBuf;
using ProtoBuf.Grpc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ProductServicesProt
{
    

    [DataContract]
    public class Category
    {
        [DataMember(Order = 1)]
        public string CategoryId { get; set; } = null!;
        [DataMember(Order = 2)]
        public bool IsActive { get; set; }
        [DataMember(Order = 3)]
        public string? ParentCategoryId { get; set; }
        [DataMember(Order = 4)]
        public string? RootCategoryId { get; set; }
        [DataMember(Order = 5)]
        public string? Title { get; set; }
        [Required]
        [DataMember(Order = 6)]
        public string Name { get; set; } = null!;
        [DataMember(Order = 7)]
        public string? ShortDesc { get; set; }        
        [DataMember(Order = 8)]
        public List<string>? Keywords { get; set; }
        [DataMember(Order = 9)]
        public List<CategoryProperty>? CategoryProperties { get; set; }        
        [DataMember(Order = 11)]
        public string? ModifiedBy { get; set; }
        [DataMember(Order = 12)]
        public DateTime ModifiedDt { get; set; }

    }
        [DataContract]
    public class CategoryProperty
    {
        [DataMember(Order = 1)]
        public int PropertyDisplayOrder {get;set;}
        [DataMember(Order = 2)]
        public string Name { get; set; }
        [DataMember(Order = 3)]
        public PropertyType Type {get;set;}
        [DataMember(Order = 4)]
        public int MinLength {get;set;}
        [DataMember(Order = 5)]
        public int MaxLength {get;set;}
        [DataMember(Order = 6)]
        public string? Regx { get; set; }
        [DataMember(Order = 7)]
        public List<string>? Option { get; set; }
    }

    [DataContract]
    public class CategoryRequest
    {
        [DataMember(Order = 1)]        
        public string? CategoryId { get; set; }        
        [DataMember(Order = 2)]
        public string? CategoryName { get; set; }
        [DataMember(Order = 3)]
        public bool ExactCategoryName { get; set; }
        [DataMember(Order = 4)]
        public bool IncludeAll { get; set; }
        [DataMember(Order = 5)]
        public bool IncludeOnlyActive { get; set; }
        [DataMember(Order = 6)]
        public bool IncludeChild { get; set; }
        [DataMember(Order = 7)]
        public bool IncludeProperty { get; set; }
        [DataMember(Order = 8)]
        public bool IncludeDetails { get; set; }
        [DataMember(Order = 9)]
        public string? Language { get; set; }
    }

    [DataContract]
    public class HelloReply
    {
        [DataMember(Order = 1)]
        public string Message { get; set; }
        [DataMember(Order = 2)]
        public DateTime CurrentDt { get; set; }
    }

    [DataContract]
    public class HelloRequest
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }        
    }

    [ServiceContract]
    public interface ICategoryService
    {
        [OperationContract]
        Task<ReturnData> Save(Category request, CallContext context = default);
        [OperationContract]
        Task<ReturnList<Category>> Get(CategoryRequest request, CallContext context = default);        
        [OperationContract]
        Task<HelloReply> SayHelloAsync(HelloRequest request,
            CallContext context = default);
    }
}