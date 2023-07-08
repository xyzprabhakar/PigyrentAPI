using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    [DataContract]
    public class RequestData
    {
        [DataMember(Order = 1)]
        public string RequestId { get; set; } = null!;
    }
}
