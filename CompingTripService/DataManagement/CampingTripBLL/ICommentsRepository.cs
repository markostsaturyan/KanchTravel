using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public interface ICommentsRepository
    {
        Task<IEnumerable<Comment>> GetComments(string campingTripId);
        Task AddComment(Comment comment);
        Task DeleteComment(string commentId);
    }
}
