using System.Collections.Generic;
using System.Threading.Tasks;
using Kanch.DataManagement.Model;

namespace Kanch.DataManagement.CampingTripBLL
{
    public interface ICommentsRepository
    {
        Task<IEnumerable<Comment>> GetComments(string campingTripId);
        Task AddComment(Comment comment);
        Task DeleteComment(string commentId);
    }
}
