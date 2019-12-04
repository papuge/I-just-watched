using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IJustWatched.Data;
using IJustWatched.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IJustWatched.Components
{
    public class CommentsViewComponent: ViewComponent
    {
        private readonly IJustWatchedContext _context;

        public CommentsViewComponent(IJustWatchedContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int reviewId)
        {
            var comments = _context.Comments
                .Where(comment => comment.CommentedReview.Id == reviewId)
                .Include(comment => comment.Author);
            return View("_Comments", comments);
        }
        
    }
}