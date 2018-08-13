using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CampingTripService.DataManagement.Model
{
    public class CampingTripId
    {
        [DataMember]
        public string CampingTripID { get; set; }
    }
}
