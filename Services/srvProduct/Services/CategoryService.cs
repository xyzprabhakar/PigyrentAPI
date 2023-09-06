using AutoMapper;
using Grpc.Core;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using srvProduct;
using srvProduct.DB;
using srvProduct.protos;
using System.Text.RegularExpressions;

namespace srvProduct.Services
{
    public class CategoryService : ICategoryService.ICategoryServiceBase
    {
        private readonly IMongoCollection<tblCategory> _category;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        private readonly int _IdLength;
        public CategoryService(IOptions<DbSetting> dbSetting, IMapper mapper, ILogger<CategoryService> logger)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);
            _category = mongoDatabase.GetCollection<tblCategory>(dbSetting.Value.CategoryCollection);
            _IdLength = dbSetting.Value.IdLength;
            _mapper = mapper;
            _logger = logger;
        }

        
        public override async Task<mdlCategorySaveResponse> SaveCategory(mdlCategoryWrapper request, ServerCallContext context)
        {
            mdlCategorySaveResponse returnData = new mdlCategorySaveResponse();
            try
            {

                bool isUpdate = true;
                string categoryId = request.Category.CategoryId;
                if (string.IsNullOrEmpty(categoryId))
                {
                    isUpdate = false;
                }

                foreach (var categoryDetail in request.Category.CategoryDetail) {
                    var tempData = _category.AsQueryable().Where(p => p.CategoryDetail.Where(q => q.Name.ToLower() == categoryDetail.Name.ToLower() 
                    && q.Language.ToLower()== categoryDetail.Language.ToLower()).Any()).ToList();
                    if (tempData.Where(p => p.CategoryId != categoryId).Count() > 0)
                    {
                        returnData.StatusId = 1001;
                        returnData.Message = $"Category name '{categoryDetail.Name}' in language {categoryDetail.Language} already exists";
                        return Task.FromResult(returnData);
                    }
                }
                if (request.Category.SubCategory.Count > 0)
                {
                    request.Category.SubCategory.Clear();
                }
                
                var model = _mapper.Map<tblCategory>(request.Category);
                if (!isUpdate)
                {
                    model.CreatedDt = model.ModifiedDt;
                    model.CreatedBy = model.ModifiedBy;
                    _category.InsertOne(model);
                    returnData.Message = $"Inserted successfully";
                    returnData.StatusId = 101;
                    returnData.MessageId = model.CategoryId!;
                }
                else
                {
                    if (request.AllLanguage)
                    {
                        _category.ReplaceOne(x => x.CategoryId == categoryId, model);
                    }
                    else
                    {
                        //remove all language expect from selected one
                        //model
                        // var existing=_category.AsQueryable().Where(p => p.CategoryId == model.CategoryId).Take(1).FirstOrDefault();
                        //existing.CategoryDetail.RemoveAll(p => request.Language.Contains(p.Language));
                    }
                    
                    returnData.StatusId = 102;
                    returnData.Message = $"Updated successfully";
                }
                returnData.Status = true;
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                returnData.StatusId = 2000;
                _logger.LogError(ex, "Error: CategoryService.Save() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }

        public override Task<mdlCategorySaveResponse> SaveSubCategory(mdlSubCategoryWrapper request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
    //public class CategoryService : ICategoryService.ICategoryServiceBase
    //{
    //    private readonly IMongoCollection<tblCategory> _category;
    //    private readonly IMapper _mapper;
    //    private readonly ILogger<CategoryService> _logger;
    //    private readonly int _IdLength;
    //    public CategoryService(IOptions<DbSetting> dbSetting, IMapper mapper, ILogger<CategoryService> logger)
    //    {
    //        var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
    //        var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);
    //        _category = mongoDatabase.GetCollection<tblCategory>(dbSetting.Value.CategoryCollection);
    //        _IdLength = dbSetting.Value.IdLength;
    //        _mapper = mapper;
    //        _logger = logger;

    //    }

    //    public override Task<mdlCategoryList> GetCategory(mdlCategoryRequest request, ServerCallContext context)
    //    {
    //        mdlCategoryList returnData = new mdlCategoryList() { Category = { } };
    //        try { 
    //        var builderFilter = Builders<tblCategory>.Filter;
    //        var filterDefinition = builderFilter.Empty;
    //        if (request.IncludeActive)
    //        {
    //            filterDefinition &= builderFilter.Eq(p => p.IsActive, true);
    //        }
    //        if (!request.IncludeAll)
    //        {
    //            if (!string.IsNullOrEmpty(request.CategoryId))
    //            {
    //                filterDefinition &= builderFilter.Eq(x => x.CategoryId, request.CategoryId);
    //            }
    //            if (!string.IsNullOrEmpty(request.CategoryName))
    //            {
    //                if (request.ExactCategoryName)
    //                {
    //                    filterDefinition &= builderFilter.Where(x => x.Name.ToLower() == request.CategoryName.ToLower());
    //                }
    //                else
    //                {
    //                    var bsonExp = new BsonRegularExpression(new Regex(request.CategoryName, RegexOptions.IgnoreCase));
    //                    filterDefinition &= builderFilter.Regex(x => x.Name, bsonExp);
    //                }

    //            }
    //        }
    //        var builderProjection = Builders<tblCategory>.Projection;
    //        var builderDefination = builderProjection.Include(p => p.CategoryId)
    //            .Include(p => p.IsActive)
    //            .Include(p => p.ParentCategoryId)
    //            .Include(p => p.RootCategoryId)
    //            .Include(p => p.Title)
    //            .Include(p => p.Name)
    //            .Include(p => p.ModifiedBy)
    //            .Include(p => p.ModifiedDt);
    //        if (request.IncludeProperty)
    //        {
    //            builderDefination = builderDefination.Include(p => p.CategoryProperties);
    //        }
    //        if (request.IncludeDetails)
    //        {
    //            builderDefination = builderDefination.Include(p => p.ShortDesc).Include(p => p.Keywords);
    //        }

    //            returnData.Category.AddRange(_mapper.Map<List<mdlCategory>>(_category.Find(filterDefinition).Project<tblCategory>(builderDefination).ToEnumerable()));
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error: CategoryServices.GetCategory() " + ex.Message);
    //        }            
    //        return Task.FromResult(returnData);
    //    }


    //    public override Task<mdlCategorySaveResponse> SaveCategory(mdlCategory request, ServerCallContext context)
    //    {
    //        mdlCategorySaveResponse returnData = new mdlCategorySaveResponse();
    //        try
    //        {

    //            bool isUpdate = true;
    //            string categoryId = request.CategoryId;
    //            if (string.IsNullOrEmpty(categoryId))
    //            {
    //                isUpdate = false;
    //            }
    //            var tempData = _category.AsQueryable().Where(p => p.Name.ToLower() == request.Name.ToLower()).ToList();
    //            if (tempData.Where(p => p.CategoryId != categoryId).Count() > 0)
    //            {
    //                returnData.Message = $"Category name '{request.Name}' already exists";
    //                return Task.FromResult(returnData);
    //            }
    //            var model = _mapper.Map<tblCategory>(request);
    //            if (!isUpdate)
    //            {

    //                model.CreatedDt = model.ModifiedDt;
    //                model.CreatedBy = model.ModifiedBy;
    //                _category.InsertOne(model);
    //                returnData.Message = $"Inserted successfully";
    //                returnData.MessageId = model.CategoryId!;
    //            }
    //            else
    //            {
    //                _category.ReplaceOne(x => x.CategoryId == categoryId, model);
    //                returnData.Message = $"Updated successfully";
    //            }
    //            returnData.Status = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            returnData.Message = ex.Message;
    //            _logger.LogError(ex, "Error: CategoryService.Save() " + ex.Message);
    //        }
    //        return Task.FromResult(returnData);
    //    }
    //}



}