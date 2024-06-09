using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        
        public UserService(ApplicationDbContext context)
        {
            _context = context;
            
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task ChangePasswordAsync(string email, string newPassword)
        {
            var user = await GetUserByEmailAsync(email);
            if (user != null)
            {
                
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.User.ToListAsync();
        }


        public async Task ChangeEmailAsync(string email, string newEmail)
        {
            var user = await GetUserByEmailAsync(email);
            if (user != null)
            {
                user.Email = newEmail;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAccountAsync(string email)
        {
            var user = await _context.User
                .Include(u => u.SentFriendRequests)
                .Include(u => u.ReceivedFriendRequests)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                // Usuń powiązane FriendRequests
                _context.FriendRequest.RemoveRange(user.SentFriendRequests);
                _context.FriendRequest.RemoveRange(user.ReceivedFriendRequests);

                // Usuń powiązania znajomych
                foreach (var friend in user.Friends.ToList())
                {
                    friend.Friends.Remove(user);
                }

                _context.User.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task CreateUserAsync(User user, string password)
        {
            
            _context.Add(user);
            await _context.SaveChangesAsync();
        }
        
        public async Task AddUserRatingAsync(int userId, int value)
        {
            var userRating = new AppRating
            {
                UserId = userId,
                Value = value,
                RatedOn = DateTime.UtcNow
            };

            _context.AppRating.Add(userRating);

            var user = await _context.User.FindAsync(userId);
            if (user != null)
            {
                user.Points += 5; 
                _context.User.Update(user);
            }

            await _context.SaveChangesAsync();
        }
        
        public async Task<bool> HasUserRatedAppAsync(int userId)
        {
            return await _context.AppRating.AnyAsync(r => r.UserId == userId);
        }
        
    }
}