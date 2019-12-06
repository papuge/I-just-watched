using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IJustWatched.Data;
using IJustWatched.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace IJustWatched.Components
{
    public class TagsViewComponent: ViewComponent
    {
        private readonly IJustWatchedContext _context;

        public TagsViewComponent(IJustWatchedContext context)
        {
            _context = context;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(int reviewId)
        {
            var tags = await _context.TagsReviews
                .Where(row => row.TaggedReview.Id == reviewId)
                .Select(row => row.TagInReview)
                .ToListAsync();
            var tagStrings = tags.Select(tag => tag.ToString());
            var oneString = string.Join(" ", tagStrings);
            return new HtmlContentViewComponentResult(new HtmlString(
                $"<h6>{oneString}</h6>"
                ));
        }
    }
}