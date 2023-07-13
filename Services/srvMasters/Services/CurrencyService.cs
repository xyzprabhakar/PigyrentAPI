using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using srvMasters.DB;
using srvMasters.protos;

namespace srvMasters.Services
{
    public class CurrencyService : ICurrency.ICurrencyBase
    {
        private readonly ILogger<CurrencyService> _logger;
        private readonly IMongoCollection<tblCurrency> _currency;
        private readonly IMapper _mapper;
        private readonly int _IdLength;

        public CurrencyService(IOptions<DbSetting> dbSetting, IMapper mapper,ILogger<CurrencyService> logger)
        {
            _IdLength = dbSetting.Value.IdLength;
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);
            _currency = mongoDatabase.GetCollection<tblCurrency>(dbSetting.Value.CurrencyCollection);
            _mapper = mapper;
            _logger = logger;
        }
        public override Task<mdlCurrencyList> GetCurrency(mdlCurrencyRequest request, ServerCallContext context)
        {
            
            mdlCurrencyList returnList = new mdlCurrencyList() { Currency = { } };
            try
            {
                if (!string.IsNullOrEmpty(request.CurrencyId))
                {
                    if (request.CurrencyId.Length != _IdLength)
                    {
                        return Task.FromResult(returnList);
                    }
                    else
                    {
                        returnList.Currency.AddRange(_mapper.Map<List<mdlCurrency>>(_currency.Find(p => p.CurrencyId == request.CurrencyId).ToEnumerable()));
                    }
                }
                else if (!string.IsNullOrWhiteSpace(request.Code))
                {
                    returnList.Currency.AddRange(_mapper.Map<List<mdlCurrency>>(_currency.Find(p => p.Code.ToUpper() == request.Code.ToUpper()).ToEnumerable()));
                }
                else if (request.AllData)
                {   
                    returnList.Currency.AddRange(_mapper.Map<List<mdlCurrency>>(_currency.Find(_ => true).ToEnumerable()));
                }
            }
            catch (Exception ex)
            {   
                _logger.LogError(ex, "Error: CurrencyServices.GetCurrency() " + ex.Message);
            }
            return Task.FromResult(returnList);

        }

        public override Task<mdlCurrencySaveResponse> SaveCurrency(mdlCurrency request, ServerCallContext context)
        {   
            mdlCurrencySaveResponse returnData = new mdlCurrencySaveResponse() ;
            try
            {
                bool isUpdate = true;
                string Id = request.CurrencyId;
                if (string.IsNullOrEmpty(Id))
                {
                    isUpdate = false;
                }
                var tempData = _currency.AsQueryable().Where(p => p.Code.ToLower() == request.Code.ToLower()).ToList();
                if (tempData.Where(p => p.CurrencyId != Id).Count() > 0)
                {
                    returnData.Message = $"Currency code '{request.Code}' already exists";
                    return Task.FromResult(returnData);
                }
                var model = _mapper.Map<tblCurrency>(request);
                if (!isUpdate)
                {
                    model.CreatedDt = model.ModifiedDt;
                    model.CreatedBy = model.ModifiedBy;
                    _currency.InsertOne(model);
                    returnData.Message = $"Inserted successfully";
                    returnData.CurrencyId = model.CurrencyId!;
                }
                else
                {
                    _currency.ReplaceOne(x => x.CurrencyId == Id, model);
                    returnData.Message = $"Updated successfully";
                }

                returnData.Status = true;
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: CurrencyServices.SaveCurrency() " + ex.Message);
            }
            return Task.FromResult( returnData);
        }

    }
}
