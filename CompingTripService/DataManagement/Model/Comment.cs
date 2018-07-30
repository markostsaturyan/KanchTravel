using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CampingTripService.DataManagement.Model
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public string CampingTripId { get; set; }
    }
}
