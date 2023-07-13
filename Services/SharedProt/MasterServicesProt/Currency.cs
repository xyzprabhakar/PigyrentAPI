using DTO;
using ProtoBuf;
using ProtoBuf.Grpc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace MasterServicesProt
{
    

    [DataContract]
    public class mdlCurrency
    {
        [DataMember(Order = 1)]
        public string CurrencyId { get; set; } = null!;
        //[DataMember(Order = 1)]
        //public string CurrencyCode { get; set; } = null!;
        //[DataMember(Order = 3)]
        //public string Name { get; set; } = null!;
        //[DataMember(Order = 4)]
        //public string Symbol { get; set; } = null!;
        //[DataMember(Order = 5)]
        //public string? ModifiedBy { get; set; }
        //[DataMember(Order = 6)]
        //public DateTime ModifiedDt { get; set; }
    }
    [ServiceContract]
    public interface ICurrencyService
    {   
        [OperationContract]
        Task<mdlCurrency?> GetById(RequestData request, CallContext context = default);
        [OperationContract]
        Task<mdlCurrency?> GetByCode(RequestData request, CallContext context = default);
        [OperationContract]
        Task<ReturnList<mdlCurrency>> GetAll(CallContext context = default);
        [OperationContract]
        Task<ReturnData> SaveCurrency(RequestData request, CallContext context = default);
        
    }
}
