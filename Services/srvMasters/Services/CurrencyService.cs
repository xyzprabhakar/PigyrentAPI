using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using srvMasters.protos;

namespace srvMasters.Services
{
    public class CurrencyService : ICurrency.ICurrencyBase
    {
        private readonly ILogger<CurrencyService> _logger;
        public CurrencyService(ILogger<CurrencyService> logger)
        {
            _logger = logger;
        }
        public override Task<mdlReturnList> GetCurrency(mdlCurrencyRequest request, ServerCallContext context)
        {
            mdlReturnList returnList = new mdlReturnList() {Status= enmReturnStatus.Success,Messages="testing"};
            mdlCurrency currency = new mdlCurrency() { Code="test", Name="name" };
            returnList.DataObject = Any.Pack(currency);
            return Task.FromResult(returnList);

        }
    }
}
