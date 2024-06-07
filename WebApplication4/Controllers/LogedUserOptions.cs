using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    public class LoggedUserOptions : Controller
    {
        private readonly UserService _userService;
        private readonly RecipeService _recipeService;
        private readonly ApplicationDbContext _context;

        public LoggedUserOptions(ApplicationDbContext context,UserService userService, RecipeService recipeService)
        {
            _userService = userService;
            _recipeService = recipeService;
            _context = context;
        }

        // GET: MyAccount
        public async Task<IActionResult> MyAccount()
        {
            var email = User.Identity.Name; // Pobierz email aktualnie zalogowanego użytkownika
            Console.WriteLine($"Logged-in user's email: {email}");
            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return View(user);
        }
        
        public async Task<IActionResult> SavedRecipes()
        {
            var email = User.Identity.Name;
            var user = await _userService.GetUserByEmailAsync(email);

            if (user != null)
            {
                var savedRecipes = await _context.User
                    .Where(u => u.Id == user.Id)
                    .SelectMany(u => u.FavoriteRecipes)
                    .ToListAsync();

                return View(savedRecipes);
            }

            return View(new List<Recipe>());
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveFavoriteRecipe(int recipeId)
        {
            var email = User.Identity.Name;
            var userId = await _userService.GetUserByEmailAsync(email);
            await _recipeService.RemoveFavoriteRecipeAsync(userId.Id, recipeId);
            return RedirectToAction("SavedRecipes");
        }

        
        public IActionResult Analysis()
        {
            return View();
        }
    }
}