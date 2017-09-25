using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace event_todevice_service
{
    [DataContract]
    class SensorEvent
    {
        [DataMember]
        public string timestart { get; set; }
        [DataMember]
        public string dsplalert { get; set; }
        [DataMember]
        public string alerttype { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public string targetalarmdevice { get; set; }
    }
}
