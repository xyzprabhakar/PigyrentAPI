using srvMasters.Currency;
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
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
