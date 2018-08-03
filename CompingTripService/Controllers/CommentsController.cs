using System.Collections.Generic;
using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model;
using Microsoft.AspNetCore.Mvc;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/Comments")]
    public class CommentsController : Controller
    {
        private readonly ICommentsRepository commentsRepository;

        public CommentsController(ICommentsRepository commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        // Get: api/Comments/
        [HttpGet("{id}")]
        public async Task<IEnumerable<CommentFull>> Get(string id)
        {
            var comments = await this.commentsRepository.GetComments(id);
            var commentsFull = new List<CommentFull>();
            foreach(var comment in comments)
            {
                commentsFull.Add(new CommentFull(comment));
            }
            return commentsFull;
        }

        // POST: api/Comments
        [HttpPost]
        public async void Post([FromBody]Comment comment)
        {
            await this.commentsRepository.AddComment(comment);
        }
       
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            this.commentsRepository.DeleteComment(id);
        }
    }
}
