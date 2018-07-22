using System.Collections.Generic;

namespace UserManagement.DataManagement.DataAccesLayer.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string CampingTripId { get; set; }
    }
}
