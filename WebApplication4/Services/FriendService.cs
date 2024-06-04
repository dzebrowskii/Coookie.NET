using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    public class FriendService
    {
        private readonly ApplicationDbContext _context;

        public FriendService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task SendFriendRequestAsync(int fromUserId, int toUserId)
        {
            var friendRequest = new FriendRequest
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Status = "Pending"
            };

            _context.FriendRequest.Add(friendRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FriendRequest>> GetPendingRequestsAsync(int userId)
        {
            return await _context.FriendRequest
                .Where(fr => fr.ToUserId == userId && fr.Status == "Pending")
                .Include(fr => fr.FromUser)
                .ToListAsync();
        }

        public async Task AcceptFriendRequestAsync(int requestId)
        {
            var friendRequest = await _context.FriendRequest.FindAsync(requestId);
            if (friendRequest != null)
            {
                friendRequest.Status = "Accepted";

                var fromUser = await _context.User.FindAsync(friendRequest.FromUserId);
                var toUser = await _context.User.FindAsync(friendRequest.ToUserId);

                if (fromUser != null && toUser != null)
                {
                    fromUser.Friends.Add(toUser);
                    toUser.Friends.Add(fromUser);

                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task DeclineFriendRequestAsync(int requestId)
        {
            var friendRequest = await _context.FriendRequest.FindAsync(requestId);
            if (friendRequest != null)
            {
                _context.FriendRequest.Remove(friendRequest);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<List<User>> GetFriendsAsync(int userId)
        {
            var user = await _context.User.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == userId);
            return user?.Friends.ToList();
        }
        
        public async Task<User> GetUserByIdWithRecipesAsync(int userId)
        {
            return await _context.User
                .Include(u => u.FavoriteRecipes)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
        
        
        
        

    }
}
