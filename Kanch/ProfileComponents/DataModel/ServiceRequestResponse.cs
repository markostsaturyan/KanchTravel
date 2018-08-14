using System;
using System.Windows.Input;

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

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public ICommand SelectDriver { get; set; }
        public ICommand SelectGuide { get; set; }
        public ICommand SelectPhotographer { get; set; }
    }
}
