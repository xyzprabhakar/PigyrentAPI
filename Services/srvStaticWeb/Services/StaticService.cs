using AutoMapper;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using srvStaticWeb;
using srvStaticWeb.DB;
using System.Transactions;

namespace srvStaticWeb.Services
{
    public class StaticWebService : IStaticWeb.IStaticWebBase 
    {
        private readonly IMapper _mapper;
        private readonly ILogger<StaticWebService> _logger;
        private readonly WebContext _context;
        public StaticWebService(ILogger<StaticWebService> logger, WebContext context, IMapper mapper)
        {

            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _mapper = mapper;
            _logger = logger;
        }

        public override Task<mdlAboutUsList> GetAboutUs(mdlAboutUsRequest request, ServerCallContext context)
        {
            mdlAboutUsList returnData = new mdlAboutUsList() { AboutUs = { } };
            try
            {

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                        new System.Transactions.TransactionOptions()
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                        }))
                {

                    returnData.AboutUs.AddRange(
                    _mapper.Map<List<mdlAboutUs>>(
                     _context.tblAboutUsMaster.Where(p => p.IsActive )
                        .Include(p => p.AboutUsDetail!.Where(q => !q.IsDeleted && (request.IncludeAllLanguage ||
                        request.Language.Contains(q.Language))))));
                }

                returnData.Status = true;
                returnData.StatusId = Constant.LOADED;

            }
            catch (Exception ex)
            {
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: StaticWebService.GetAboutUs() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }

        public override Task<mdlStaticWebSaveResponse> SetAboutUs(mdlAboutUs request, ServerCallContext context)
        {
            mdlStaticWebSaveResponse returnData = new mdlStaticWebSaveResponse();
            try
            {

                bool isUpdate = true;
                string aboutUsId = request.AboutUsId;
                
                Guid aboutUsIdGuid = Guid.Empty;

                //validate
                if (string.IsNullOrEmpty(aboutUsId))
                {
                    isUpdate = false;
                }
                if (!Guid.TryParse(aboutUsId, out aboutUsIdGuid))
                {
                    returnData.StatusId = Constant.INVALID_ID;
                    returnData.Message = $"Invalid AboutUsId";
                    return Task.FromResult(returnData);
                }
                
                else if (string.IsNullOrWhiteSpace(request.DefaultName))
                {
                    returnData.StatusId = Constant.REQUIRED;
                    returnData.Message = $"Required {nameof(request.DefaultName)}";
                    return Task.FromResult(returnData);
                }
                if ((request.AboutUsDetail?.Count ?? 0) == 0)
                {
                    returnData.StatusId = Constant.REQUIRED;
                    returnData.Message = $"Required {nameof(request.AboutUsDetail)}";
                    return Task.FromResult(returnData);
                }

                //check wheather the category already exists                
                if (!_context.tblAboutUsMaster.Where(p => p.AboutUsId == aboutUsIdGuid).Any())
                {
                    returnData.StatusId = Constant.INVALID_ID;
                    returnData.Message = $"Invalid AboutUsId";
                    return Task.FromResult(returnData);
                }
                if (_context.tblAboutUsMaster.Where(p => p.DefaultName == request.DefaultName && p.AboutUsId != aboutUsIdGuid).Any())
                {
                    returnData.StatusId = Constant.ALREADY_EXIST;
                    returnData.Message = $"Already exists {request.DefaultName}";
                    return Task.FromResult(returnData);
                }


                var master = _mapper.Map<tblAboutUsMaster>(request);
                if (!isUpdate)
                {
                    _context.tblAboutUsMaster.Add(master);
                    aboutUsId = master.AboutUsId.ToString("N");
                }
                else
                {
                    var existingData = _context.tblAboutUsMaster.Where(p => p.AboutUsId == aboutUsIdGuid)
                        .Include(p => p.AboutUsDetail!.Where(p=>!p.IsDeleted))                        
                        .FirstOrDefault();
                    if (existingData == null)
                    {
                        returnData.StatusId = Constant.INVALID_ID;
                        returnData.Message = $"Invalid aboutUsId";
                        return Task.FromResult(returnData);
                    }
                    
                    foreach (var exDetail in existingData.AboutUsDetail!)
                    {                        
                        if (request.AboutUsDetail!.Any(q=>q.Language== exDetail.Language))
                        {
                            exDetail.IsDeleted = true;
                            exDetail.ModifiedBy = master.ModifiedBy;
                            exDetail.ModifiedDt = master.ModifiedDt;
                        }                        
                    }
                    existingData.AboutUsId = master.AboutUsId;
                    existingData.ModifiedBy = master.ModifiedBy;
                    existingData.ModifiedDt = master.ModifiedDt;
                    existingData.DefaultName = master.DefaultName;
                    existingData.IsActive = master.IsActive;
                    existingData.DisplayOrder = master.DisplayOrder;
                    _context.Update(existingData);
                    var details = master.AboutUsDetail;
                    foreach (var ctdetails in details!)
                    {
                        ctdetails.AboutUsDetailId = Guid.NewGuid();
                        ctdetails.AboutUsId= master.AboutUsId;
                    }
                    _context.AddRange(details);
                }
                _context.SaveChanges();
                aboutUsId = aboutUsIdGuid.ToString("N");
                if (!isUpdate)
                {
                    returnData.Message = $"Inserted successfully";
                    returnData.StatusId = Constant.INSERT_SUCCESSFULLY;
                    returnData.MessageId = aboutUsId;
                }
                else
                {
                    returnData.StatusId = Constant.UPDATED_SUCCESSFULLY;
                    returnData.Message = $"Updated successfully";
                    returnData.MessageId = aboutUsId;
                }
                returnData.Status = true;
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                _logger.LogError(ex, "Error: StaticWebService.SetAboutUs() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }

        public override Task<mdlJoinUsList> GetJoinUs(mdlJoinUsRequest request, ServerCallContext context)
        {
            mdlJoinUsList returnData = new mdlJoinUsList() { JoinUs = { } };
            try
            {

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                        new System.Transactions.TransactionOptions()
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                        }))
                {

                    returnData.JoinUs.AddRange(
                    _mapper.Map<List<mdlJoinUs>>(
                     _context.tblJoinUsMaster.Where(p => p.IsActive)
                        .Include(p => p.JoinUsDetail!.Where(q => !q.IsDeleted && (request.IncludeAllLanguage ||
                        request.Language.Contains(q.Language))))));
                }

                returnData.Status = true;
                returnData.StatusId = Constant.LOADED;

            }
            catch (Exception ex)
            {
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: StaticWebService.GetJoinUs() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }

        public override Task<mdlStaticWebSaveResponse> SetJoinUs(mdlJoinUs request, ServerCallContext context)
        {
            mdlStaticWebSaveResponse returnData = new mdlStaticWebSaveResponse();
            try
            {

                bool isUpdate = true;
                string joinUsId = request.JoinUsId;

                Guid joinUsIdGuid = Guid.Empty;

                //validate
                if (string.IsNullOrEmpty(joinUsId))
                {
                    isUpdate = false;
                }
                if (!Guid.TryParse(joinUsId, out joinUsIdGuid))
                {
                    returnData.StatusId = Constant.INVALID_ID;
                    returnData.Message = $"Invalid JoinUsId";
                    return Task.FromResult(returnData);
                }

                else if (string.IsNullOrWhiteSpace(request.DefaultName))
                {
                    returnData.StatusId = Constant.REQUIRED;
                    returnData.Message = $"Required {nameof(request.DefaultName)}";
                    return Task.FromResult(returnData);
                }
                if ((request.JoinUsDetail?.Count ?? 0) == 0)
                {
                    returnData.StatusId = Constant.REQUIRED;
                    returnData.Message = $"Required {nameof(request.JoinUsDetail)}";
                    return Task.FromResult(returnData);
                }

                //check wheather the category already exists                
                if (!_context.tblJoinUsMaster.Where(p => p.JoinUsId == joinUsIdGuid).Any())
                {
                    returnData.StatusId = Constant.INVALID_ID;
                    returnData.Message = $"Invalid JoinUsId";
                    return Task.FromResult(returnData);
                }
                if (_context.tblJoinUsMaster.Where(p => p.DefaultName == request.DefaultName && p.JoinUsId != joinUsIdGuid).Any())
                {
                    returnData.StatusId = Constant.ALREADY_EXIST;
                    returnData.Message = $"Already exists {request.DefaultName}";
                    return Task.FromResult(returnData);
                }


                var master = _mapper.Map<tblJoinUsMaster>(request);
                if (!isUpdate)
                {
                    _context.tblJoinUsMaster.Add(master);
                    joinUsId = master.JoinUsId.ToString("N");
                }
                else
                {
                    var existingData = _context.tblJoinUsMaster.Where(p => p.JoinUsId == joinUsIdGuid)
                        .Include(p => p.JoinUsDetail!.Where(p => !p.IsDeleted))
                        .FirstOrDefault();
                    if (existingData == null)
                    {
                        returnData.StatusId = Constant.INVALID_ID;
                        returnData.Message = $"Invalid JoinUsId";
                        return Task.FromResult(returnData);
                    }

                    foreach (var exDetail in existingData.JoinUsDetail!)
                    {
                        if (request.JoinUsDetail!.Any(q => q.Language == exDetail.Language))
                        {
                            exDetail.IsDeleted = true;
                            exDetail.ModifiedBy = master.ModifiedBy;
                            exDetail.ModifiedDt = master.ModifiedDt;
                        }
                    }
                    existingData.JoinUsId = master.JoinUsId;
                    existingData.ModifiedBy = master.ModifiedBy;
                    existingData.ModifiedDt = master.ModifiedDt;
                    existingData.DefaultName = master.DefaultName;
                    existingData.IsActive = master.IsActive;
                    existingData.DisplayOrder = master.DisplayOrder;
                    _context.Update(existingData);
                    var details = master.JoinUsDetail;
                    foreach (var ctdetails in details!)
                    {
                        ctdetails.JoinUsDetailId = Guid.NewGuid();
                        ctdetails.JoinUsId= master.JoinUsId;
                    }
                    _context.AddRange(details);
                }
                _context.SaveChanges();
                joinUsId = joinUsIdGuid.ToString("N");
                if (!isUpdate)
                {
                    returnData.Message = $"Inserted successfully";
                    returnData.StatusId = Constant.INSERT_SUCCESSFULLY;
                    returnData.MessageId = joinUsId;
                }
                else
                {
                    returnData.StatusId = Constant.UPDATED_SUCCESSFULLY;
                    returnData.Message = $"Updated successfully";
                    returnData.MessageId = joinUsId;
                }
                returnData.Status = true;
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                _logger.LogError(ex, "Error: StaticWebService.SetJoinUs() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }

        public override Task<mdlFAQList> GetFAQ(mdlFAQRequest request, ServerCallContext context)
        {
            mdlFAQList returnData = new mdlFAQList() { FAQs = { } };
            try
            {

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                        new System.Transactions.TransactionOptions()
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                        }))
                {

                    returnData.FAQs.AddRange(
                    _mapper.Map<List<mdlFAQ>>(
                     _context.tblFAQMaster.Where(p => p.IsActive)
                        .Include(p => p.FAQDetail!.Where(q => !q.IsDeleted && (request.IncludeAllLanguage ||
                        request.Language.Contains(q.Language))))));
                }

                returnData.Status = true;
                returnData.StatusId = Constant.LOADED;

            }
            catch (Exception ex)
            {
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: StaticWebService.GetFAQ() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }

        public override Task<mdlStaticWebSaveResponse> SetFAQ(mdlFAQ request, ServerCallContext context)
        {
            mdlStaticWebSaveResponse returnData = new mdlStaticWebSaveResponse();
            try
            {

                bool isUpdate = true;
                string FAQId = request.FAQId;

                Guid FAQIdGuid = Guid.Empty;

                //validate
                if (string.IsNullOrEmpty(FAQId))
                {
                    isUpdate = false;
                }
                if (!Guid.TryParse(FAQId, out FAQIdGuid))
                {
                    returnData.StatusId = Constant.INVALID_ID;
                    returnData.Message = $"Invalid JoinUsId";
                    return Task.FromResult(returnData);
                }
                else if (string.IsNullOrWhiteSpace(request.DefaultQuestion))
                {
                    returnData.StatusId = Constant.REQUIRED;
                    returnData.Message = $"Required {nameof(request.DefaultQuestion)}";
                    return Task.FromResult(returnData);
                }
                if ((request.FAQDetail?.Count ?? 0) == 0)
                {
                    returnData.StatusId = Constant.REQUIRED;
                    returnData.Message = $"Required {nameof(request.FAQDetail)}";
                    return Task.FromResult(returnData);
                }

                            
                if (!_context.tblFAQMaster.Where(p => p.FAQId == FAQIdGuid).Any())
                {
                    returnData.StatusId = Constant.INVALID_ID;
                    returnData.Message = $"Invalid FAQId";
                    return Task.FromResult(returnData);
                }
                if (_context.tblFAQMaster.Where(p => p.DefaultQuestion == request.DefaultQuestion && p.FAQId != FAQIdGuid).Any())
                {
                    returnData.StatusId = Constant.ALREADY_EXIST;
                    returnData.Message = $"Already exists {request.DefaultQuestion}";
                    return Task.FromResult(returnData);
                }


                var master = _mapper.Map<tblFAQMaster>(request);
                if (!isUpdate)
                {
                    _context.tblFAQMaster.Add(master);
                    FAQId = master.FAQId.ToString("N");
                }
                else
                {
                    var existingData = _context.tblFAQMaster.Where(p => p.FAQId == FAQIdGuid)
                        .Include(p => p.FAQDetail!.Where(p => !p.IsDeleted))
                        .FirstOrDefault();
                    if (existingData == null)
                    {
                        returnData.StatusId = Constant.INVALID_ID;
                        returnData.Message = $"Invalid FAQId";
                        return Task.FromResult(returnData);
                    }

                    foreach (var exDetail in existingData.FAQDetail!)
                    {
                        if (request.FAQDetail!.Any(q => q.Language == exDetail.Language))
                        {
                            exDetail.IsDeleted = true;
                            exDetail.ModifiedBy = master.ModifiedBy;
                            exDetail.ModifiedDt = master.ModifiedDt;
                        }
                    }
                    existingData.FAQId = master.FAQId;
                    existingData.ModifiedBy = master.ModifiedBy;
                    existingData.ModifiedDt = master.ModifiedDt;
                    existingData.DefaultQuestion = master.DefaultQuestion;
                    existingData.IsActive = master.IsActive;
                    existingData.DisplayOrder = master.DisplayOrder;
                    _context.Update(existingData);
                    var details = master.FAQDetail;
                    foreach (var ctdetails in details!)
                    {
                        ctdetails.FAQDetailId = Guid.NewGuid();
                        ctdetails.FAQId = master.FAQId;
                    }
                    _context.AddRange(details);
                }
                _context.SaveChanges();
                FAQId = FAQIdGuid.ToString("N");
                if (!isUpdate)
                {
                    returnData.Message = $"Inserted successfully";
                    returnData.StatusId = Constant.INSERT_SUCCESSFULLY;
                    returnData.MessageId = FAQId;
                }
                else
                {
                    returnData.StatusId = Constant.UPDATED_SUCCESSFULLY;
                    returnData.Message = $"Updated successfully";
                    returnData.MessageId = FAQId;
                }
                returnData.Status = true;
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                returnData.StatusId = Constant.INTERNAL_SERVER_ERROR;
                _logger.LogError(ex, "Error: StaticWebService.SetJoinUs() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }


        

    }
}