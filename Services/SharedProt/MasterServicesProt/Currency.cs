using DTO;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MasterServicesProt
{
    
    [DataContract]
    public class Currency
    {
        [DataMember(Order = 1)]
        public string CurrencyId { get; set; } = null!;
        [DataMember(Order = 2)]
        public string Code { get; set; } = null!;
        [DataMember(Order = 3)]
        public string Name { get; set; } = null!;
        [DataMember(Order = 4)]
        public string Symbol { get; set; } = null!;
        [DataMember(Order = 5)]
        public string? ModifiedBy { get; set; }
        [DataMember(Order = 6)]
        public DateTime ModifiedDt { get; set; }
    }
    [ServiceContract]
    public interface ICurrencyService
    {
        [OperationContract]
        Task<ReturnData> Save(Currency request, CallContext context = default);
        [OperationContract]
        Task<Currency?> GetById(RequestData request, CallContext context = default);
        [OperationContract]
        Task<Currency?> GetByCode(RequestData request, CallContext context = default);
        [OperationContract]
        Task<ReturnList<Currency>> GetAll(CallContext context = default);
    }
}
