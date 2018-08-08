using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanch.ProfileComponents.HelperClasses
{
    public class ServiceRequestResponse
    {
        public string Id { get; set; }

        public int ProviderId { get; set; }

        public string CampingTripId { get; set; }

        public DateTime ResponseValidityPeriod { get; set; }

        public string ProviderRole { get; set; }

        public double Price { get; set; }
    }
}
