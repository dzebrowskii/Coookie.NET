using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Services;
using WebApplication4.Models;


namespace WebApplication4.Controllers
{
    public class AnalysisController : Controller
    {
        private readonly AnalysisService _analysisService;
        private readonly UserService _userService;

        public AnalysisController(AnalysisService analysisService, UserService userService)
        {
            _analysisService = analysisService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult FoodAnalysis()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FoodAnalysis2()
        {
            var email = User.Identity.Name;
            var userId = await _userService.GetUserByEmailAsync(email);

            if (userId == null)
            {
                return BadRequest("User not found.");
            }

            var ingredientUsage = await _analysisService.GetIngredientUsageAsync(userId.Id);

            return Json(ingredientUsage);
        }
        
        [HttpGet]
        public IActionResult FinancialAnalysis()
        {
            return View();
        }
        
        public IActionResult CaloricAnalysis()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> GenerateFinancialAnalysis()
        {
            try
            {
                var email = User.Identity.Name;
                var user = await _userService.GetUserByEmailAsync(email);
        
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var data = await _analysisService.GenerateFinancialAnalysisDataAsync(user.Id);

                // Konwersja danych do odpowiedniego formatu
                var analysisData = data
                    .Select(d => new 
                    { 
                        date = d.Key, 
                        amount = d.Value 
                    })
                    .ToList();

                // Logowanie danych
                Console.WriteLine("Data to be sent to client:");
                foreach (var item in analysisData)
                {
                    Console.WriteLine($"{item.date}: {item.amount}");
                }

                return Json(analysisData);
            }
            catch (Exception ex)
            {
                // Logowanie wyjątku
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> GenerateCaloricAnalysis()
        {
            try
            {
                var email = User.Identity.Name;
                var user = await _userService.GetUserByEmailAsync(email);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var data = await _analysisService.GenerateCaloricAnalysisDataAsync(user.Id);
                
                var analysisData = data
                    .Select(d => new
                    {
                        date = d.Key,
                        amount = d.Value
                    })
                    .ToList();

                return Json(analysisData);

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }












        
        
    }
}