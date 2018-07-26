namespace UserManagement.DataManagement.DataAccesLayer.Models
{
    public class ServiceRequestResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string CampingTripId { get; set; }

        public double Price { get; set; }
    }
}
