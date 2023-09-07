using AutoMapper;
using Grpc.Core;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using srvProduct;
using srvProduct.DB;
using srvProduct.protos;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace srvProduct.Services
{
    public class CategoryService : ICategoryService.ICategoryServiceBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        private readonly ProductContext _productContext;
        public CategoryService(ProductContext productContext, IMapper mapper, ILogger<CategoryService> logger) {
            _productContext = productContext;
            _mapper = mapper;
            _logger = logger;
        }
#if (false)
        private readonly IMongoCollection<tblCategoryMaster> _category;
        private readonly IMongoCollection<tblCategoryDetail> _categoryDetail;
        private readonly IMongoCollection<tblSubCategoryMaster> _subCategory;
        private readonly IMongoCollection<tblSubCategoryDetail> _subCategoryDetail;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        private IOptions<CustomSetting> _dbSetting;
        private readonly ProductContext _productContext;
        public CategoryService(ProductContext productContext , IOptions<CustomSetting> dbSetting, IMapper mapper, ILogger<CategoryService> logger)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);
            _category = mongoDatabase.GetCollection<tblCategoryMaster>(dbSetting.Value.CategoryMasterCollection);
            _categoryDetail = mongoDatabase.GetCollection<tblCategoryDetail>(dbSetting.Value.CategoryMasterCollection);
            _subCategory = mongoDatabase.GetCollection<tblSubCategoryMaster>(dbSetting.Value.CategoryMasterCollection);
            _subCategoryDetail = mongoDatabase.GetCollection<tblSubCategoryDetail>(dbSetting.Value.CategoryMasterCollection);
            _dbSetting = dbSetting;
            _mapper = mapper;
            _logger = logger;
            _productContext = productContext;
        }

        public override Task<mdlCategorySaveResponse> SaveCategory(mdlCategoryWrapper request, ServerCallContext context)
        {
            mdlCategorySaveResponse returnData = new mdlCategorySaveResponse();
            tblCategoryMaster? existingCategory = null;
            try
            {

                bool isUpdate = true;
                string categoryId = request.Category.CategoryId;
                if (string.IsNullOrEmpty(categoryId))
                {
                    isUpdate = false;
                }


                if (_category.Find(p => p.CategoryId != categoryId && p.DefaultName.ToLower() == request.Category.DefaultName.ToLower()).Any())
                {
                    returnData.StatusId = Constant.ALREADY_EXIST;
                    returnData.Message = $"Already exists {request.Category.DefaultName}";
                    return Task.FromResult(returnData);
                }

                if (isUpdate)
                {
                    if ((categoryId?.Length ?? 0) != _dbSetting.Value.IdLength)
                    {
                        returnData.StatusId = Constant.INVALID_ID;
                        returnData.Message = $"Invalid CategoryId";
                        return Task.FromResult(returnData);
                    }
                    existingCategory = _category.Find(p => p.CategoryId == categoryId).FirstOrDefault();
                    if (existingCategory == null)
                    {
                        returnData.StatusId = Constant.INVALID_ID;
                        returnData.Message = $"Invalid CategoryId";
                        return Task.FromResult(returnData);
                    }
                }
                else
                {
                    var ctMaster = _mapper.Map<tblCategoryMaster>(request.Category);
                    ctMaster.CreatedDt = ctMaster.ModifiedDt;
                    ctMaster.CreatedBy = ctMaster.ModifiedBy;
                    _category.InsertOne(ctMaster);
                    categoryId = ctMaster.CategoryId ?? "";
                }

                //save category details
                var ctDetails = _mapper.Map<List<tblCategoryDetail>>(request.Category.CategoryDetail);
                foreach (var ctdetail in ctDetails.Where(p => !string.IsNullOrEmpty(p.CategoryDetailId)))
                {
                    _categoryDetail.ReplaceOne(p => p.CategoryDetailId == ctdetail.CategoryDetailId, ctdetail);
                }


                if (!isUpdate)
                {
                    returnData.Message = $"Inserted successfully";
                    returnData.StatusId = Constant.INSERT_SUCCESSFULLY;
                    returnData.MessageId = categoryId;
                }
                else
                {
                    returnData.StatusId = Constant.UPDATED_SUCCESSFULLY;
                    returnData.Message = $"Updated successfully";
                    returnData.MessageId = categoryId;
                }
                returnData.Status = true;
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                _logger.LogError(ex, "Error: CategoryService.SaveCategory() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }




        public override  Task<mdlCategorySaveResponse> SaveSubCategory(mdlSubCategoryWrapper request, ServerCallContext context)
        {
            mdlCategorySaveResponse returnData = new mdlCategorySaveResponse();
            tblSubCategoryMaster? existingCategory = null;
            try
            {

                bool isUpdate = true;
                string subCategoryId = request.SubCategory.SubCategoryId;
                string categoryId = request.SubCategory.CategoryId;

                if (string.IsNullOrEmpty(subCategoryId))
                {
                    isUpdate = false;
                }

                if ((categoryId?.Length ?? 0) != _dbSetting.Value.IdLength)
                {
                    returnData.StatusId = Constant.INVALID_PARENT_ID;
                    returnData.Message = $"Invalid CategoryId";
                    return Task.FromResult(returnData);
                }
                else if (!_category.Find(p => p.CategoryId == categoryId).Any())
                {
                    returnData.StatusId = Constant.INVALID_PARENT_ID;
                    returnData.Message = $"Invalid CategoryId";
                    return Task.FromResult(returnData);
                }

                if (_subCategory.Find(p => p.SubCategoryId != subCategoryId && p.DefaultName.ToLower() == request.SubCategory.DefaultName.ToLower()).Any())
                {
                    returnData.StatusId = Constant.ALREADY_EXIST;
                    returnData.Message = $"Already exists {request.SubCategory.DefaultName}";
                    return Task.FromResult(returnData);
                }

                if (isUpdate)
                {
                    if ((subCategoryId?.Length ?? 0) != _dbSetting.Value.IdLength)
                    {
                        returnData.StatusId = Constant.INVALID_ID;
                        returnData.Message = $"Invalid sub CategoryId";
                        return Task.FromResult(returnData);
                    }
                    existingCategory = _subCategory.Find(p => p.CategoryId == categoryId).FirstOrDefault();
                    if (existingCategory == null)
                    {
                        returnData.StatusId = Constant.INVALID_ID;
                        returnData.Message = $"Invalid sub CategoryId";
                        return Task.FromResult(returnData);
                    }
                }
                else
                {
                    var ctMaster = _mapper.Map<tblSubCategoryMaster>(request.SubCategory);
                    ctMaster.CreatedDt = ctMaster.ModifiedDt;
                    ctMaster.CreatedBy = ctMaster.ModifiedBy;
                    _subCategory.InsertOne(ctMaster);
                    categoryId = ctMaster.CategoryId ?? "";
                }

                //save category details
                var ctDetails = _mapper.Map<List<tblSubCategoryDetail>>(request.SubCategory.SubCategoryDetail);
                foreach (var ctdetail in ctDetails.Where(p => !string.IsNullOrEmpty(p.SubCategoryDetailId)))
                {
                    _subCategoryDetail.ReplaceOne(p => p.SubCategoryDetailId == ctdetail.SubCategoryDetailId, ctdetail);
                }


                if (!isUpdate)
                {
                    returnData.Message = $"Inserted successfully";
                    returnData.StatusId = Constant.INSERT_SUCCESSFULLY;
                    returnData.MessageId = categoryId;
                }
                else
                {
                    returnData.StatusId = Constant.UPDATED_SUCCESSFULLY;
                    returnData.Message = $"Updated successfully";
                    returnData.MessageId = categoryId;
                }
                returnData.Status = true;
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                _logger.LogError(ex, "Error: CategoryService.SaveSubCategory() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }

        public override Task<mdlCategoryList> GetAllCategory(mdlCategoryRequest request, ServerCallContext context)
        {
            mdlCategoryList returnData = new mdlCategoryList() { Category = { } };
            try
            {
                var builderFilter = Builders<tblCategoryMaster>.Filter;
                var filterDefinition = builderFilter.Empty;
                if (request.IncludeActiveOnly)
                {
                    filterDefinition &= builderFilter.Eq(p => p.IsActive, true);
                }
                
                var builderProjection = Builders<tblCategoryMaster>.Projection;
                var builderDefination = builderProjection.Include(p => p.CategoryId)
                    .Include(p => p.IsActive)
                    .Include(p => p.DefaultName)
                    .Include(p => p.ImageUrl)                    
                    .Include(p => p.ModifiedBy)
                    .Include(p => p.ModifiedDt);                
                returnData.Category.AddRange(_mapper.Map<List<mdlCategory>>(_category.Find(filterDefinition).Project<tblCategoryMaster>(builderDefination).ToEnumerable()));

                var tempCategoryId=returnData.Category.Select(p=>p.CategoryId).ToList();
                var ctDetails = _categoryDetail.AsQueryable().Where(p => tempCategoryId.Contains(p.CategoryId!) && (request.IncludeAllLanguage || request.Language.Contains(p.Language)));
                foreach (var ct in returnData.Category)
                {
                    ct.CategoryDetail.AddRange(_mapper.Map <List<mdlCategoryDetails>> (ctDetails.Where(p=>p.CategoryId== ct.CategoryId)));
                }
                returnData.Status = true;
                returnData.StatusId = Constant.LOADED;

            }
            catch (Exception ex)
            {
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: CategoryServices.GetAllCategory() " + ex.Message);
            }
            return Task.FromResult(returnData);

        }


        public override Task<mdlSubCategoryList> GetAllSubCategory(mdlSubCategoryRequest request, ServerCallContext context)
        {
            mdlSubCategoryList returnData = new mdlSubCategoryList() { SubCategory = { } };
            try
            {
                var builderFilter = Builders<tblSubCategoryMaster>.Filter;
                var filterDefinition = builderFilter.Empty;
                if (request.IncludeActiveOnly)
                {
                    filterDefinition &= builderFilter.Eq(p => p.IsActive, true);
                }

                var builderProjection = Builders<tblSubCategoryMaster>.Projection;
                var builderDefination = builderProjection
                    .Include(p=>p.SubCategoryId)
                    .Include(p => p.CategoryId)
                    .Include(p => p.IsActive)
                    .Include(p => p.DefaultName)
                    .Include(p => p.ImageUrl)
                    .Include(p => p.ModifiedBy)
                    .Include(p => p.ModifiedDt);
                //if (request.IncludeProperty)
                //{
                //    builderDefination = builderDefination.Include(p => p.Properties);
                //}
                returnData.SubCategory.AddRange(_mapper.Map<List<mdlSubCategory>>(_subCategory.Find(filterDefinition).Project<tblSubCategoryMaster>(builderDefination).ToEnumerable()));

                var tempCategoryId = returnData.SubCategory.Select(p => p.SubCategoryId).ToList();
                var ctDetails = _subCategoryDetail.AsQueryable().Where(p => tempCategoryId.Contains(p.SubCategoryId!) && (request.IncludeAllLanguage || request.Language.Contains(p.Language)));
                foreach (var ct in returnData.SubCategory)
                {
                    ct.SubCategoryDetail.AddRange(_mapper.Map<List<mdlSubCategoryDetails>>(ctDetails.Where(p => p.SubCategoryId == ct.SubCategoryId)));
                }
                returnData.Status = true;
                returnData.StatusId = Constant.LOADED;

            }
            catch (Exception ex)
            {
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: CategoryServices.GetAllCategory() " + ex.Message);
            }
            return Task.FromResult(returnData);

        }

        public override async Task<mdlCategoryList> GetAllCategoryIncludeSubCategory(mdlCategoryRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
            //var retunData=await GetAllCategory(request, context);
            //var subCategoryData =await GetAllSubCategory(request, context);
            //foreach (var category in retunData.Category) {
            //    category.SubCategory.AddRange(subCategoryData.SubCategory.Where(p => p.CategoryId == category.CategoryId));
            //}
            //return retunData;

        }
#endif
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