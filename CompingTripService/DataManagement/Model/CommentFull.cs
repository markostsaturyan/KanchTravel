using CampingTripService.DataManagement.Model.Users;

namespace CampingTripService.DataManagement.Model
{
    public class CommentFull
    {
        public CommentFull(Comment comment)
        {
            this.Id = comment.Id;
            this.Text = comment.Text;
            this.CampingTripID = comment.CampingTripId;
        }

        public string Id { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public string CampingTripID { get; set; }
    }
}
