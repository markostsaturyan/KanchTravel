using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Kanch.DataManagement.Model
{
    public class CommentContext
    {
        private readonly IMongoDatabase database;

        public CommentContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Comment> Comments
        {
            get
            {
                return database.GetCollection<Comment>("Comments");
            }
        }
    }
}
