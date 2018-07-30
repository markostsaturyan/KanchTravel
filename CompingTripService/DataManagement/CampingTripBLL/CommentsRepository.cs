using System.Collections.Generic;
using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public class CommentsRepository:ICommentsRepository
    {
        private CommentContext commentContext;

        public CommentsRepository(IOptions<Settings> settings)
        {
            commentContext = new CommentContext(settings);
        }

        public async Task<IEnumerable<Comment>> GetComments(string campingTripId)
        {
            return await commentContext.Comments.Find(Builders<Comment>.Filter.Eq(s => s.CampingTripId, campingTripId)).ToListAsync();
        }


        public async Task AddComment(Comment comment)
        {
            await commentContext.Comments.InsertOneAsync(comment);
        }

        public async Task DeleteComment(string commentId)
        {
            await commentContext.Comments.DeleteOneAsync(Builders<Comment>.Filter.Eq(s => s.Id, commentId));
        }
    }
}
