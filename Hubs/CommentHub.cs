using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IJustWatched.Data;
using IJustWatched.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace IJustWatched.Hubs
{
    public class CommentHub: Hub
    {
        private readonly IJustWatchedContext _context;
        private readonly UserManager<User> _userManager;

        public CommentHub(IJustWatchedContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public async Task SendComment(string userName, string content, int reviewId)
        {
            var author = await _userManager.FindByNameAsync(userName);
            var comment = new Comment
            {
                Author = author,
                CommentedReview = _context.Reviews.FirstOrDefault(r => r.Id == reviewId),
                Content = content,
                CreationDateTime = DateTime.Now
            };
            _context.Add(comment);
            _context.SaveChanges();
            await Clients.All.SendAsync("ReceiveComment", reviewId, userName, author.Id,
                author.PhotoUrl, content,
                comment.CreationDateTime.ToString(CultureInfo.CurrentCulture));
        }
    }
}