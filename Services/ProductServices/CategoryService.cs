
using ProtoBuf.Grpc;
using ProductServicesProt;
using DTO;
using MongoDB.Driver;
using ProductServices.DB;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata.Ecma335;
using System.Linq.Expressions;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ProductServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<tblCategory> _category;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(IOptions<DbSetting> dbSetting, IMapper mapper, ILogger<CategoryService> logger)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);
            _category = mongoDatabase.GetCollection<tblCategory>(dbSetting.Value.CategoryCollection);
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ReturnList<Category>> Get(CategoryRequest request, CallContext context = default)
        {
            ReturnList<Category> categories = new ReturnList<Category>();
            
            var builderFilter = Builders<tblCategory>.Filter;
            var filterDefinition = builderFilter.Empty;
            if (request.IncludeOnlyActive)
            {
                filterDefinition &= builderFilter.Eq(p => p.IsActive, true);
            }
            if (!request.IncludeAll)
            {
                if (!string.IsNullOrEmpty(request.CategoryId))
                {
                    filterDefinition &= builderFilter.Eq(x => x.CategoryId, request.CategoryId);
                }
                if (!string.IsNullOrEmpty(request.CategoryName))
                {
                    if (request.ExactCategoryName)
                    {
                        filterDefinition &= builderFilter.Where(x => x.Name.ToLower() == request.CategoryName.ToLower());
                    }
                    else
                    {
                        var bsonExp = new BsonRegularExpression(new Regex(request.CategoryName, RegexOptions.IgnoreCase));
                        filterDefinition &= builderFilter.Regex(x => x.Name, bsonExp);
                    }

                }
            }
            var builderProjection = Builders<tblCategory>.Projection;
            var builderDefination = builderProjection.Include(p => p.CategoryId)
                .Include(p => p.IsActive)
                .Include(p => p.ParentCategoryId)
                .Include(p => p.RootCategoryId)
                .Include(p => p.Title)
                .Include(p => p.Name)
                .Include(p => p.ModifiedBy)
                .Include(p => p.ModifiedDt);
            if (request.IncludeProperty)
            {
                builderDefination = builderDefination.Include(p => p.CategoryProperties);
            }
            if (request.IncludeDetails)
            {
                builderDefination = builderDefination.Include(p => p.ShortDesc).Include(p => p.Keywords);
            }

            categories.ReturnData = (await _category.Find(filterDefinition)
                    .Project<tblCategory>(builderDefination).ToListAsync())
                    .Select(p => new Category
                    {
                        CategoryId = p.CategoryId!,
                        IsActive = p.IsActive,
                        ParentCategoryId = p.ParentCategoryId,
                        RootCategoryId = p.RootCategoryId,
                        Title = p.Title,
                        Name = p.Name,
                        ShortDesc = p.ShortDesc,
                        Keywords = p.Keywords,
                        CategoryProperties = p?.CategoryProperties?.Select(q => new CategoryProperty { MaxLength = q.MaxLength, MinLength = q.MinLength, Name = q.Name, Option = q.Option, PropertyDisplayOrder = q.PropertyDisplayOrder, Regx = q.Regx, Type = q.Type })?.ToList(),
                        ModifiedBy = p.ModifiedBy,
                        ModifiedDt = p.ModifiedDt
                    }).ToList();

            return categories;
        }

        public async Task<ReturnData> Save(Category request, CallContext context = default)
        {
            ReturnData returnData = new ReturnData() {Status= ReturnStatus.Error, Message="" };
            try
            {
                throw new Exception("test error");
                bool isUpdate = true;
                string categoryId = request.CategoryId;
                if (string.IsNullOrEmpty(categoryId))
                {
                    isUpdate = false;
                }
                var tempData = _category.AsQueryable().Where(p => p.Name.ToLower() == request.Name.ToLower()).ToList();
                if (tempData.Where(p => p.CategoryId != categoryId).Count() > 0)
                {
                    returnData.Message = $"Category name '{request.Name}' already exists";
                    return returnData;
                }
                var model = _mapper.Map<tblCategory>(request);
                if (!isUpdate)
                {
                    
                    model.CreatedDt = model.ModifiedDt;
                    model.CreatedBy = model.ModifiedBy;
                    await _category.InsertOneAsync(model);
                    returnData.Message = $"Inserted successfully";
                    returnData.ReturnId = model.CategoryId!;
                }
                else
                {
                    await _category.ReplaceOneAsync(x => x.CategoryId == categoryId, model);
                    returnData.Message = $"Updated successfully";
                }
                
                returnData.Status = ReturnStatus.Success;
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: CategoryService.Save() "+ ex.Message);
            }
            return returnData;

        }

        public Task<HelloReply> SayHelloAsync(HelloRequest request, CallContext context = default)
        {
            return Task.FromResult(
                    new HelloReply
                    {
                        Message = $"Hello {request.Name}",
                        CurrentDt = DateTime.Now,
                    });
        }
    }
}
