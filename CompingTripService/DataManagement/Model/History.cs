using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampingTripService.DataManagement.Model
{
    public class Comment
    {
        public int UserId { get; set; }
        public string Text { get; set; }
    }
    public class History:CampingTrip
    {
        public int Raiting { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
