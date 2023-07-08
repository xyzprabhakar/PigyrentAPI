using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf.Grpc;

namespace DTO
{

    [DataContract]
    public class ReturnData
    {
        [DataMember(Order = 1)]
        public ReturnStatus Status { get; set; }
        [DataMember(Order = 2)]
        public string? Message { get; set; }
        [DataMember(Order = 3)]
        public string? ReturnId { get; set; }
    }
    [DataContract]
    public class ReturnList<T>
    {   
        [DataMember(Order = 1)]
        public List<T>? ReturnData { get; set; }
    }


}
