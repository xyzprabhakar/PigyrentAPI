using AutoMapper;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using srvProduct.DB;
using srvProduct.protos;
using System.Transactions;

namespace srvProduct.Services
{
    public class CategoryService : ICategoryService.ICategoryServiceBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        private readonly ProductContext _productContext;
        public CategoryService(ProductContext productContext, IMapper mapper, ILogger<CategoryService> logger) {
            _productContext = productContext;
            _productContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _mapper = mapper;
            _logger = logger;
        }

        public override Task<mdlCategorySaveResponse> SaveCategory(mdlCategoryWrapper request, ServerCallContext context)
        {
            mdlCategorySaveResponse returnData = new mdlCategorySaveResponse();            
            try
            {

                bool isUpdate = true;
                string categoryId = request.Category.CategoryId;
                Guid categoryIdGuid=Guid.Empty;
                //validate
                if (string.IsNullOrEmpty(categoryId))
                {
                    isUpdate = false;
                }
                if (isUpdate)
                {
                    if (!Guid.TryParse(categoryId, out categoryIdGuid))
                    {
                        returnData.StatusId = Constant.INVALID_ID;
                        returnData.Message = $"Invalid CategoryId";
                        return Task.FromResult(returnData);
                    }
                }                
                else if (string.IsNullOrWhiteSpace(request.Category.DefaultName))
                {
                    returnData.StatusId = Constant.REQUIRED;
                    returnData.Message = $"Required {nameof(request.Category.DefaultName)}";
                    return Task.FromResult(returnData);
                }
                if ((request.Category.CategoryDetail?.Count ?? 0) == 0)
                {
                    returnData.StatusId = Constant.REQUIRED;
                    returnData.Message = $"Required {nameof(request.Category.CategoryDetail)}";
                    return Task.FromResult(returnData);
                }

                //check wheather the category already exists                
                if (_productContext.tblCategoryMaster.Where(p => p.DefaultName == request.Category.DefaultName && p.CategoryId != categoryIdGuid).Any())
                {
                    returnData.StatusId = Constant.ALREADY_EXIST;
                    returnData.Message = $"Already exists {request.Category.DefaultName}";
                    return Task.FromResult(returnData);
                }
                var ctMaster = _mapper.Map<tblCategoryMaster>(request.Category);
                if (!isUpdate)
                {
                    _productContext.tblCategoryMaster.Add(ctMaster);
                    categoryId=ctMaster.CategoryId.ToString("N");
                }
                else
                {
                    var existingCategory =_productContext.tblCategoryMaster.Where(p=>p.CategoryId==categoryIdGuid)
                        .Include(p=>p.CategoryDetail!.Where(p=>!p.IsDeleted)).FirstOrDefault();
                    if (existingCategory == null)
                    {
                        returnData.StatusId = Constant.INVALID_ID;
                        returnData.Message = $"Invalid CategoryId";
                        return Task.FromResult(returnData);
                    }
                    foreach (var exCategory in existingCategory.CategoryDetail!)
                    {
                        if (request.AllLanguage)
                        {
                            exCategory.IsDeleted = true;
                            exCategory.ModifiedBy= ctMaster.ModifiedBy;
                            exCategory.ModifiedDt = ctMaster.ModifiedDt;
                        }
                        else if (request.Language.Contains(exCategory.Language, StringComparer.OrdinalIgnoreCase))
                        {
                            exCategory.IsDeleted = true;
                            exCategory.ModifiedBy = ctMaster.ModifiedBy;
                            exCategory.ModifiedDt = ctMaster.ModifiedDt;
                        }
                    }
                    existingCategory.ModifiedBy = ctMaster.ModifiedBy;
                    existingCategory.ModifiedDt = ctMaster.ModifiedDt;
                    existingCategory.DefaultName = ctMaster.DefaultName;
                    existingCategory.IsActive = ctMaster.IsActive;
                    existingCategory.ImageUrl = ctMaster.ImageUrl;
                    _productContext.Update(existingCategory);
                    var categoryDetails=ctMaster.CategoryDetail!.Where(p=> request.AllLanguage || request.Language.Contains(p.Language, StringComparer.OrdinalIgnoreCase));
                    foreach (var ctdetails in categoryDetails)
                    {
                        ctdetails.CategoryDetailId = Guid.NewGuid();
                        ctdetails.CategoryId = existingCategory.CategoryId;
                    }
                    _productContext.AddRange(categoryDetails);
                }
                _productContext.SaveChanges();
                categoryId = categoryIdGuid.ToString("N");
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


        public override Task<mdlCategorySaveResponse> SaveSubCategory(mdlSubCategoryWrapper request, ServerCallContext context)
        {
            mdlCategorySaveResponse returnData = new mdlCategorySaveResponse();
            try
            {

                bool isUpdate = true;
                string categoryId = request.SubCategory.CategoryId;
                string subCategoryId = request.SubCategory.SubCategoryId;
                Guid categoryIdGuid = Guid.Empty, subCategoryIdGuid= Guid.Empty;
                
                //validate
                if (string.IsNullOrEmpty(subCategoryId))
                {
                    isUpdate = false;
                }
                if (!Guid.TryParse(categoryId, out categoryIdGuid))
                {
                    returnData.StatusId = Constant.INVALID_ID;
                    returnData.Message = $"Invalid CategoryId";
                    return Task.FromResult(returnData);
                }
                if (isUpdate)
                {
                    if (!Guid.TryParse(subCategoryId, out subCategoryIdGuid))
                    {
                        returnData.StatusId = Constant.INVALID_ID;
                        returnData.Message = $"Invalid SubCategoryId";
                        return Task.FromResult(returnData);
                    }
                }
                else if (string.IsNullOrWhiteSpace(request.SubCategory.DefaultName))
                {
                    returnData.StatusId = Constant.REQUIRED;
                    returnData.Message = $"Required {nameof(request.SubCategory.DefaultName)}";
                    return Task.FromResult(returnData);
                }
                if ((request.SubCategory.SubCategoryDetail?.Count ?? 0) == 0)
                {
                    returnData.StatusId = Constant.REQUIRED;
                    returnData.Message = $"Required {nameof(request.SubCategory.SubCategoryDetail)}";
                    return Task.FromResult(returnData);
                }

                //check wheather the category already exists                
                if (!_productContext.tblCategoryMaster.Where(p=>p.CategoryId==categoryIdGuid).Any())
                {
                    returnData.StatusId = Constant.INVALID_ID;
                    returnData.Message = $"Invalid CategoryId";
                    return Task.FromResult(returnData);
                }
                if (_productContext.tblSubCategoryMaster.Where(p => p.DefaultName == request.SubCategory.DefaultName && p.SubCategoryId != subCategoryIdGuid).Any())
                {
                    returnData.StatusId = Constant.ALREADY_EXIST;
                    returnData.Message = $"Already exists {request.SubCategory.DefaultName}";
                    return Task.FromResult(returnData);
                }


                var ctMaster = _mapper.Map<tblSubCategoryMaster>(request.SubCategory);
                if (!isUpdate)
                {
                    _productContext.tblSubCategoryMaster.Add(ctMaster);
                    subCategoryId = ctMaster.SubCategoryId.ToString("N");
                }
                else
                {
                    var existingCategory = _productContext.tblSubCategoryMaster.Where(p => p.SubCategoryId == subCategoryIdGuid)
                        .Include(p => p.SubCategoryDetail!.Where(p=>!p.IsDeleted))
                        .ThenInclude(q=>q.Properties.Where(p=>!p.IsDeleted))
                        .FirstOrDefault();
                    if (existingCategory == null)
                    {
                        returnData.StatusId = Constant.INVALID_ID;
                        returnData.Message = $"Invalid SubCategoryId";
                        return Task.FromResult(returnData);
                    }
                    bool Isdeleted = false;
                    foreach (var exCategory in existingCategory.SubCategoryDetail!)
                    {
                        Isdeleted = false;
                        if (request.AllLanguage)
                        {
                            exCategory.IsDeleted = true;
                            Isdeleted = true;
                            
                        }
                        else if (request.Language.Contains(exCategory.Language, StringComparer.OrdinalIgnoreCase))
                        {
                            exCategory.IsDeleted = true;
                        
                        }
                        if (Isdeleted)
                        {
                            exCategory.ModifiedBy = ctMaster.ModifiedBy;
                            exCategory.ModifiedDt = ctMaster.ModifiedDt;
                            if (exCategory.Properties != null)
                            {
                                foreach (var prop in exCategory.Properties)
                                {
                                    prop.IsDeleted = true;
                                }
                            }
                            
                        }
                    }
                    existingCategory.CategoryId = ctMaster.CategoryId;
                    existingCategory.ModifiedBy = ctMaster.ModifiedBy;
                    existingCategory.ModifiedDt = ctMaster.ModifiedDt;
                    existingCategory.DefaultName = ctMaster.DefaultName;
                    existingCategory.IsActive = ctMaster.IsActive;
                    existingCategory.ImageUrl = ctMaster.ImageUrl;
                    _productContext.Update(existingCategory);
                    var details = ctMaster.SubCategoryDetail!.Where(p => request.AllLanguage || request.Language.Contains(p.Language, StringComparer.OrdinalIgnoreCase));
                    foreach (var ctdetails in details)
                    {
                        ctdetails.SubCategoryDetailId = Guid.NewGuid();
                        ctdetails.SubCategoryId = existingCategory.SubCategoryId;
                        if (ctdetails.Properties != null)
                        {
                            foreach (var prop in ctdetails.Properties)
                            {
                                prop.PropertyId = Guid.NewGuid();
                            }
                        }
                    }
                    _productContext.AddRange(details);
                }
                _productContext.SaveChanges();
                categoryId = categoryIdGuid.ToString("N");
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


        public override Task<mdlCategoryList> GetAllCategory(mdlCategoryRequest request, ServerCallContext context)
        {
            mdlCategoryList returnData = new mdlCategoryList() { Category = { } };
            try
            {

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                        new System.Transactions.TransactionOptions()
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                        }))
                {

                    returnData.Category.AddRange(
                    _mapper.Map<List<mdlCategory>>(
                     _productContext.tblCategoryMaster.Where(p => p.IsActive || request.IncludeActiveOnly)
                        .Include(p => p.CategoryDetail!.Where(q => !q.IsDeleted && (request.IncludeAllLanguage ||
                        request.Language.Contains(q.Language))))));
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
            mdlSubCategoryList returnData = new mdlSubCategoryList() { SubCategory = {  } };
            try
            {

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                        new System.Transactions.TransactionOptions()
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                        }))
                {

                    var subCategoryList = _productContext.tblSubCategoryMaster.Where(p => p.IsActive || request.IncludeActiveOnly).AsQueryable();


                    if (request.IncludeProperty)
                    {
                        subCategoryList = subCategoryList.
                            Include(p =>
                                    p.SubCategoryDetail!.Where(q =>
                                                                    !q.IsDeleted && (request.IncludeAllLanguage ||
                                                                    request.Language.Contains(q.Language))
                                                                    )
                                    )
                            .ThenInclude(p => p.Properties);
                    }
                    else
                    {
                        subCategoryList = subCategoryList.
                            Include(p =>
                                    p.SubCategoryDetail!.Where(q =>
                                                                    !q.IsDeleted && (request.IncludeAllLanguage ||
                                                                    request.Language.Contains(q.Language))
                                                                    )
                                    );
                    }
                    returnData.SubCategory.AddRange(_mapper.Map<List<mdlSubCategory>>(subCategoryList));
                }

                returnData.Status = true;
                returnData.StatusId = Constant.LOADED;

            }
            catch (Exception ex)
            {
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: CategoryServices.GetAllSubCategory() " + ex.Message);
            }
            return Task.FromResult(returnData);

        }

        public override async Task<mdlCategoryList> GetAllCategoryIncludeSubCategory(mdlCategoryRequest request, ServerCallContext context)
        {

            var retunData = await GetAllCategory(request, context);
            if (retunData.Status)
            {
                mdlSubCategoryRequest subRequest = new(){ 
                    IncludeActiveOnly=request.IncludeActiveOnly ,
                    IncludeAllLanguage=request.IncludeAllLanguage,
                    IncludeProperty=false                    
                };
                subRequest.Language.AddRange(request.Language);
                var subCategoryData = await GetAllSubCategory(subRequest, context);
                foreach (var category in retunData.Category)
                {
                    category.SubCategory.AddRange(subCategoryData.SubCategory.Where(p => p.CategoryId == category.CategoryId));
                }
            }
            return retunData;

        }

        public override Task<mdlCategoryList> GetCategoryByName(mdlCategoryNameRequest request, ServerCallContext context)
        {
            mdlCategoryList returnData = new mdlCategoryList() { Category = { } };
            try
            {

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                        new System.Transactions.TransactionOptions()
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                        }))
                {
                    string keyword = $"%{request.Name}%";
                    returnData.Category.AddRange(
                    _mapper.Map<List<mdlCategory>>(
                     _productContext.tblCategoryMaster.Where(p => p.IsActive && EF.Functions.Like( p.DefaultName, keyword))
                        .Include(p => p.CategoryDetail!.Where(q => !q.IsDeleted && (request.IncludeAllLanguage ||
                        request.Language.Contains(q.Language))))));
                }
                returnData.Status = true;
                returnData.StatusId = Constant.LOADED;

            }
            catch (Exception ex)
            {
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: CategoryServices.GetCategoryByName() " + ex.Message);
            }
            return Task.FromResult(returnData);

        }

        public override Task<mdlCategoryList> GetCategoryById(mdlCategoryIdRequest request, ServerCallContext context)
        {
            mdlCategoryList returnData = new mdlCategoryList() { Category = { } };
            try
            {
                string categoryId = request.CategoryId;
                Guid categoryIdGuid = Guid.Empty;
                //validate
                if (!Guid.TryParse(categoryId, out categoryIdGuid))
                {
                    returnData.StatusId = Constant.INVALID_ID;
                    returnData.Message = $"Invalid CategoryId";
                    return Task.FromResult(returnData);
                }

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                        new System.Transactions.TransactionOptions()
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                        }))
                {
                    returnData.Category.AddRange(
                    _mapper.Map<List<mdlCategory>>(
                     _productContext.tblCategoryMaster.Where(p => p.CategoryId==categoryIdGuid && p.IsActive )
                        .Include(p => p.CategoryDetail!.Where(q => !q.IsDeleted && (request.IncludeAllLanguage ||
                        request.Language.Contains(q.Language))))));
                }
                returnData.Status = true;
                returnData.StatusId = Constant.LOADED;

            }
            catch (Exception ex)
            {
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: CategoryServices.GetCategoryByName() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }



        public override Task<mdlSubCategoryList> GetSubCategoryByName(mdlSubCategoryNameRequest request, ServerCallContext context)
        {
            mdlSubCategoryList returnData = new mdlSubCategoryList() { SubCategory = { } };
            try
            {

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                        new System.Transactions.TransactionOptions()
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                        }))
                {
                    string keyword = $"%{request.Name}%";
                    returnData.SubCategory.AddRange(
                    _mapper.Map<List<mdlSubCategory>>(
                     _productContext.tblSubCategoryMaster.Where(p => p.IsActive && EF.Functions.Like(p.DefaultName, keyword))
                        .Include(p => p.SubCategoryDetail!.Where(q => !q.IsDeleted && (request.IncludeAllLanguage ||
                        request.Language.Contains(q.Language))))));
                }
                returnData.Status = true;
                returnData.StatusId = Constant.LOADED;

            }
            catch (Exception ex)
            {
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: CategoryServices.GetSubCategoryByName() " + ex.Message);
            }
            return Task.FromResult(returnData);

        }

        public override Task<mdlSubCategoryList> GetSubCategoryById(mdlSubCategoryIdRequest request, ServerCallContext context)
        {
            mdlSubCategoryList returnData = new mdlSubCategoryList() { SubCategory = { } };
            try
            {
                string categoryId = request.CategoryId;
                string SubCategoryId = request.SubCategoryId;
                Guid categoryIdGuid = Guid.Empty;
                Guid subCategoryIdGuid = Guid.Empty;

                //validate
                if (string.IsNullOrWhiteSpace(categoryId) && string.IsNullOrWhiteSpace(SubCategoryId))
                {
                    returnData.StatusId = Constant.REQUIRED;
                    returnData.Message = $"Invalid SubCategoryId";
                    return Task.FromResult(returnData);
                }
                else if (!string.IsNullOrEmpty(SubCategoryId) && !Guid.TryParse(SubCategoryId, out subCategoryIdGuid))
                {
                    returnData.StatusId = Constant.INVALID_ID;
                    returnData.Message = $"Invalid SubCategoryId";
                    return Task.FromResult(returnData);
                }
                else if (!string.IsNullOrEmpty(categoryId) && !Guid.TryParse(categoryId, out categoryIdGuid))
                {
                    returnData.StatusId = Constant.INVALID_ID;
                    returnData.Message = $"Invalid categoryId";
                    return Task.FromResult(returnData);
                }

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                        new System.Transactions.TransactionOptions()
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                        }))
                {

                    var query = _productContext.tblSubCategoryMaster.Where(p => p.IsActive).AsQueryable();
                    if (!string.IsNullOrEmpty(SubCategoryId))
                    {
                        query = query.Where(p => p.SubCategoryId == subCategoryIdGuid);
                    }
                    else
                    {
                        query = query.Where(p => p.CategoryId == categoryIdGuid);
                    }

                    returnData.SubCategory.AddRange(
                    _mapper.Map<List<mdlSubCategory>>(
                     query.Include(p => p.SubCategoryDetail!.Where(q => !q.IsDeleted && (request.IncludeAllLanguage ||
                        request.Language.Contains(q.Language))))));
                }
                returnData.Status = true;
                returnData.StatusId = Constant.LOADED;

            }
            catch (Exception ex)
            {
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: CategoryServices.GetSubCategoryById() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }
    }
}