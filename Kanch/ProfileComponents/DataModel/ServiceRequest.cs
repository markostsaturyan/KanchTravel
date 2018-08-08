using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanch.ProfileComponents.DataModel
{
    public class ServiceRequest
    {
        public string Id { get; set; }

        public int ProviderId { get; set; }

        public string CampingTripId { get; set; }

        public DateTime RequestValidityPeriod { get; set; }
    }
}
