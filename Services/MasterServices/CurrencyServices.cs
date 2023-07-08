using AutoMapper;
using DTO;
using MasterServices.DB;
using MasterServicesProt;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProtoBuf.Grpc;

namespace MasterServices
{
    public class CurrencyServices : ICurrencyService
    {
        private readonly IMongoCollection<tblCurrency> _currency;
        private readonly IMapper _mapper;
        
        public CurrencyServices(IOptions<DbSetting> dbSetting, IMapper mapper)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);
            _currency = mongoDatabase.GetCollection<tblCurrency>(dbSetting.Value.CurrencyCollection);
            _mapper = mapper;
        }

        public async Task<ReturnList<Currency>> GetAll(CallContext context = default)
        {
            ReturnList<Currency> returnData= new ReturnList<Currency>();
            returnData.ReturnData = _mapper.Map<List< Currency>>(await _currency.AsQueryable().ToListAsync());
            return returnData;
        }

        public async Task<Currency?> GetByCode(RequestData request, CallContext context = default)
        {
            return _mapper.Map<Currency>(await _currency.AsQueryable().Where(p => p.Code.ToLower() == request.RequestId.ToLower()).FirstOrDefaultAsync());
        }

        public async Task<Currency?> GetById(RequestData request, CallContext context = default)
        {
            return _mapper.Map<Currency>(await _currency.FindAsync(p=>p.CurrencyId== request.RequestId));
        }

        public async Task<ReturnData> Save(Currency request, CallContext context = default)
        {
            ReturnData returnData = new ReturnData() { Status = ReturnStatus.Error, Message = "" };
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
                    return returnData;
                }
                var model = _mapper.Map<tblCurrency>(request);
                if (!isUpdate)
                {

                    model.CreatedDt = model.ModifiedDt;
                    model.CreatedBy = model.ModifiedBy;
                    await _currency.InsertOneAsync(model);
                    returnData.Message = $"Inserted successfully";
                    returnData.ReturnId = model.CurrencyId!;
                }
                else
                {
                    await _currency.ReplaceOneAsync(x => x.CurrencyId == Id, model);
                    returnData.Message = $"Updated successfully";
                }

                returnData.Status = ReturnStatus.Success;
            }
            catch (Exception ex) 
            {
                returnData.Message = ex.Message;
            }
            return returnData;
        }
    }
}
