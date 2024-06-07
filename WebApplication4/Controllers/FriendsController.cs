using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    public class FriendsController : Controller
    {
        private readonly UserService _userService;
        private readonly FriendService _friendService;
        private readonly ApplicationDbContext _context;

        public FriendsController(UserService userService, FriendService friendService, ApplicationDbContext context)
        {
            _userService = userService;
            _friendService = friendService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Friend/AddFriend
        [HttpGet]
        public IActionResult AddFriend()
        {
            return View();
        }
        
        public async Task<IActionResult> ShowFriends()
        {
            var user = await _userService.GetUserByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var friends = await _friendService.GetFriendsAsync(user.Id);
            return View(friends);
        }

        // POST: Friend/AddFriend
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFriend(string email)
        {
            var currentUserEmail = User.Identity.Name;
            var currentUser = await _userService.GetUserByEmailAsync(currentUserEmail);

            var friendUser = await _friendService.GetUserByEmailAsync(email);
            if (friendUser == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return View();
            }

            await _friendService.SendFriendRequestAsync(currentUser.Id, friendUser.Id);
            TempData["SuccessMessage"] = "Friend request sent.";

            return View();
        }

        // GET: Friend/Requests
        [HttpGet]
        public async Task<IActionResult> Requests()
        {
            var currentUserEmail = User.Identity.Name;
            var currentUser = await _userService.GetUserByEmailAsync(currentUserEmail);

            var pendingRequests = await _friendService.GetPendingRequestsAsync(currentUser.Id);
            return View(pendingRequests);
        }

        // POST: Friend/AcceptRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptRequest(int requestId)
        {
            await _friendService.AcceptFriendRequestAsync(requestId);
            TempData["SuccessMessage"] = "Friend request accepted.";
            return RedirectToAction("Requests");
        }

        // POST: Friend/DeclineRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeclineRequest(int requestId)
        {
            await _friendService.DeclineFriendRequestAsync(requestId);
            TempData["SuccessMessage"] = "Friend request declined.";
            return RedirectToAction("Requests");
        }
        
        public async Task<IActionResult> FriendDetails(int userId)
        {
            var user = await _friendService.GetUserByIdWithRecipesAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return PartialView("_FriendDetails", user);
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveFriend(int friendId)
        {
            var email = User.Identity.Name;
            var user = await _userService.GetUserByEmailAsync(email);

            if (user != null)
            {
                await _friendService.RemoveFriendAsync(user.Id, friendId);
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        
        
        
        

        
    }
}
