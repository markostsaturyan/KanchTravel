using System.Collections.Generic;

namespace CampingTripService.DataManagement.Model
{
    public class Comment
    {
        public int UserId { get; set; }
        public string Text { get; set; }
    }
    public class History:CampingTrip
    {
        public double Raiting { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
