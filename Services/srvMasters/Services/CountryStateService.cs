using AutoMapper;
using Grpc.Core;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using srvMasters;
using srvMasters.DB;
using srvMasters.protos;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

namespace srvMasters.Services
{
    public class CountryStateService : ICountryState.ICountryStateBase
    {
        private readonly ILogger<CountryStateService> _logger;
        private readonly IMongoCollection<tblCountry> _country;
        private readonly IMongoCollection<tblState> _state;
        private readonly IMapper _mapper;
        private readonly int _IdLength;
        public CountryStateService(IOptions<DbSetting> dbSetting, IMapper mapper, ILogger<CountryStateService> logger)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);
            _country = mongoDatabase.GetCollection<tblCountry>(dbSetting.Value.CountryCollection);
            _state = mongoDatabase.GetCollection<tblState>(dbSetting.Value.StateCollection);
            _IdLength = dbSetting.Value.IdLength;
            _mapper = mapper;
            _logger = logger;

        }

        public override Task<mdlCountryList> GetCountry(mdlGetCountry request, ServerCallContext context)
        {
            mdlCountryList returnData = new() { Country = { } };
            try
            {

                var builderFilter = Builders<tblCountry>.Filter;
                var filterDefinition = builderFilter.Empty;
                if (request.OnlyActive)
                {
                    filterDefinition &= builderFilter.Eq(p => p.IsActive, true);
                }
                if (!string.IsNullOrEmpty(request.CountryId) && request.CountryId.Length == _IdLength)
                {
                    filterDefinition &= builderFilter.Eq(p => p.CountryId, request.CountryId);
                }
                else if (!string.IsNullOrWhiteSpace(request.CountryCode))
                {
                    filterDefinition &= builderFilter.Where(x => x.Code.ToUpper() == request.CountryCode.ToUpper());
                }
                else if (!string.IsNullOrWhiteSpace(request.CountryName))
                {
                    var bsonExp = new BsonRegularExpression(new Regex(request.CountryName, RegexOptions.IgnoreCase));
                    filterDefinition &= builderFilter.Regex(x => x.Name, bsonExp);
                }
                else if (!request.AllData)
                {
                    return Task.FromResult(returnData);
                }
                returnData.Country.AddRange(_mapper.Map<List<mdlCountry>>(_country.Find(filterDefinition).ToEnumerable()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: CountryStateServices.GetCountry() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }

        public override Task<mdlStateList> GetState(mdlGetState request, ServerCallContext context)
        {
            mdlStateList returnData = new() { State = { } };
            try
            {
                var builderFilter = Builders<tblState>.Filter;
                var filterDefinition = builderFilter.Empty;
                if (request.OnlyActive)
                {
                    filterDefinition &= builderFilter.Eq(p => p.IsActive, true);
                }
                if (!string.IsNullOrEmpty(request.StateId) && request.StateId.Length == _IdLength)
                {
                    filterDefinition &= builderFilter.Eq(p => p.CountryId, request.CountryId);
                }
                else if (!string.IsNullOrEmpty(request.CountryId) && request.CountryId.Length == _IdLength)
                {
                    filterDefinition &= builderFilter.Eq(p => p.CountryId, request.CountryId);
                }
                else if (!request.AllData)
                {
                    return Task.FromResult(returnData);
                }
                returnData.State.AddRange(_mapper.Map<List<mdlState>>(_state.Find(filterDefinition).ToEnumerable()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: CountryStateServices.GetState() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }

        public override Task<mdlCountryStateSaveResponse> SaveCountry(mdlCountry request, ServerCallContext context)
        {
            mdlCountryStateSaveResponse returnData = new mdlCountryStateSaveResponse();
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
                    returnData.Message = $"code '{request.Code}' already exists";
                    return Task.FromResult(returnData);
                }
                var model = _mapper.Map<tblCountry>(request);
                if (!isUpdate)
                {
                    model.CreatedDt = model.ModifiedDt;
                    model.CreatedBy = model.ModifiedBy;
                    _country.InsertOne(model);
                    returnData.Message = $"Inserted successfully";
                    returnData.MessageId = model.CountryId!;
                }
                else
                {
                    _country.ReplaceOne(x => x.CountryId == Id, model);
                    returnData.Message = $"Updated successfully";
                }
                returnData.Status = true;
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: CountryStateServices.SaveCountry() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }

        public override Task<mdlCountryStateSaveResponse> SaveState(mdlState request, ServerCallContext context)
        {
            mdlCountryStateSaveResponse returnData = new mdlCountryStateSaveResponse();
            try
            {
                bool isUpdate = true;
                string Id = request.StateId;
                if (string.IsNullOrEmpty(Id))
                {
                    isUpdate = false;
                }
                var tempData = _state.AsQueryable().Where(p =>p.CountryId==request.CountryId && p.Code.ToLower() == request.Code.ToLower()).ToList();
                if (tempData.Where(p => p.StateId != Id).Count() > 0)
                {
                    returnData.Message = $"code '{request.Code}' already exists";
                    return Task.FromResult(returnData);
                }
                var model = _mapper.Map<tblState>(request);
                if (!isUpdate)
                {
                    model.CreatedDt = model.ModifiedDt;
                    model.CreatedBy = model.ModifiedBy;
                    _state.InsertOne(model);
                    returnData.Message = $"Inserted successfully";
                    returnData.MessageId = model.StateId!;
                }
                else
                {
                    _state.ReplaceOne(x => x.StateId == Id, model);
                    returnData.Message = $"Updated successfully";
                }
                returnData.Status = true;
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                _logger.LogError(ex, "Error: CountryStateServices.SaveState() " + ex.Message);
            }
            return Task.FromResult(returnData);
        }

    }
}