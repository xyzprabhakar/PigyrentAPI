using Amazon.Runtime.Internal;
using AutoMapper;
using DTO;
using MasterServices.DB;
using MasterServicesProt;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProtoBuf.Grpc;
using System.Runtime.CompilerServices;

namespace MasterServices
{
    public class CountryStateServices : ICountryStateService
    {
        private readonly IMongoCollection<tblCountry> _country;
        private readonly IMongoCollection<tblState> _state;
        private readonly IMapper _mapper;
        private readonly ILogger<CountryStateServices> _logger;
        public CountryStateServices(IOptions<DbSetting> dbSetting, IMapper mapper, ILogger<CountryStateServices> logger)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);
            _country = mongoDatabase.GetCollection<tblCountry>(dbSetting.Value.CountryCollection);
            _state = mongoDatabase.GetCollection<tblState>(dbSetting.Value.StateCollection);
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReturnList<State>> GetAllState(CallContext context = default)
        {
            ReturnList<State> returnData = new ();
            returnData.ReturnData = _mapper.Map<List<State>>(await _state.AsQueryable().ToListAsync());
            return returnData;
        }

        public async Task<ReturnList<Country>> GetCountries(RequestData request, CallContext context = default)
        {
            ReturnList<Country> returnData = new ();
            returnData.ReturnData = _mapper.Map<List<Country>>(await _country.AsQueryable().ToListAsync());
            return returnData;
        }
        public async Task< ReturnList<string>> GetCountryCurrency(RequestData request, CallContext context = default)
        {
            ReturnList<string> returnData = new()
            {
                ReturnData= (await _country.Find(p => p.CountryId == request.RequestId).Project(p => p.Currency).FirstOrDefaultAsync()) ?? new List<string>()
            };            
            return returnData;
        }

        public async Task<ReturnList<State>> GetCountryStates(RequestData request, CallContext context = default)
        {
            ReturnList<State> returnData = new()
            { ReturnData= _mapper.Map<List<State>>(await _state.Find(p => p.CountryId == request.RequestId).ToListAsync()) };                        
            return returnData;
        }

        public async Task<State> GetState(RequestData request, CallContext context = default)
        {
            return _mapper.Map<State>(await _state.Find(p => p.StateId == request.RequestId).FirstOrDefaultAsync());            
        }

        public async Task<ReturnList<CountryTopCity>> GetTopCities(RequestData request, CallContext context = default)
        {   
            ReturnList<CountryTopCity> returnData = new()
            {
                ReturnData = await _country.Find(p => p.CountryId == request.RequestId).Project(p => p.Cities).FirstOrDefaultAsync() 
            };
            return returnData;
        }

        public async Task<ReturnData> SaveCountry(Country request, CallContext context = default)
        {
            ReturnData returnData = new ReturnData() { Status = ReturnStatus.Error, Message = "" };
            try
            {
                bool isUpdate = true;
                string Id = request.CountryId;
                if (string.IsNullOrEmpty(Id))
                {
                    isUpdate = false;
                }
                var tempData = _country.AsQueryable().Where(p => p.Code.ToLower() == request.Code.ToLower()).ToList();
                if (tempData.Where(p => p.CountryId != Id).Count() > 0)
                {
                    returnData.Message = $"Country code '{request.Code}' already exists";
                    return returnData;
                }
                var model = _mapper.Map<tblCountry>(request);
                if (!isUpdate)
                {

                    model.CreatedDt = model.ModifiedDt;
                    model.CreatedBy = model.ModifiedBy;
                    await _country.InsertOneAsync(model);
                    returnData.Message = $"Inserted successfully";
                    returnData.ReturnId = model.CountryId!;
                }
                else
                {
                    await _country.ReplaceOneAsync(x => x.CountryId == Id, model);
                    returnData.Message = $"Updated successfully";
                }
                returnData.Status = ReturnStatus.Success;
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: CountryStateServices.SaveCountry() " + ex.Message);
            }
            return returnData;

        }

        public async Task<ReturnData> SaveState(State request, CallContext context = default)
        {
            ReturnData returnData = new ReturnData() { Status = ReturnStatus.Error, Message = "" };
            try
            {
                bool isUpdate = true;
                string Id = request.CountryId;
                if (string.IsNullOrEmpty(Id))
                {
                    isUpdate = false;
                }
                var tempData = _state.AsQueryable().Where(p =>p.CountryId ==request.CountryId && p.Code.ToLower() == request.Code.ToLower()).ToList();
                if (tempData.Where(p => p.CountryId != Id).Count() > 0)
                {
                    returnData.Message = $"State code '{request.Code}' already exists";
                    return returnData;
                }
                var model = _mapper.Map<tblState>(request);
                if (!isUpdate)
                {

                    model.CreatedDt = model.ModifiedDt;
                    model.CreatedBy = model.ModifiedBy;
                    await _state.InsertOneAsync(model);
                    returnData.Message = $"Inserted successfully";
                    returnData.ReturnId = model.StateId!;
                }
                else
                {
                    await _state.ReplaceOneAsync(x => x.StateId == Id, model);
                    returnData.Message = $"Updated successfully";
                }
                returnData.Status = ReturnStatus.Success;
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: CountryStateServices.SaveState() " + ex.Message);
            }
            return returnData;

        }
    }
}
