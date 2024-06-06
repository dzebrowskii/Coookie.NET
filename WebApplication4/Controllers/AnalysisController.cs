using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication4.Services;
using OfficeOpenXml;
using Newtonsoft.Json;
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
            TempData["AnalysisData"] = JsonConvert.SerializeObject(ingredientUsage);

            return Json(ingredientUsage);
        }
        
        
        [HttpGet]
        public IActionResult DownloadFoodAnalysis()
        {
            var jsonData = TempData["AnalysisData"] as string;

            if (jsonData == null)
            {
                return NotFound("Analysis data not found.");
            }

            var data = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Analysis");

                worksheet.Cells[1, 1].Value = "Ingredient";
                worksheet.Cells[1, 2].Value = "Count";

                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1].Value = item.Key;
                    worksheet.Cells[row, 2].Value = item.Value;
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Analysis.xlsx");
            }
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
                
                TempData["FinancialAnalysisData"] = JsonConvert.SerializeObject(data.ToList());

                return Json(analysisData);
            }
            catch (Exception ex)
            {
                // Logowanie wyjątku
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        
        [HttpGet]
        public IActionResult DownloadFinancialAnalysis()
        {
            var jsonData = TempData["FinancialAnalysisData"] as string;

            if (jsonData == null)
            {
                return NotFound("Analysis data not found.");
            }

            // Deserializacja danych z dodatkowym sprawdzeniem
            var dataList = JsonConvert.DeserializeObject<List<KeyValuePair<string, decimal>>>(jsonData);
            var data = dataList
                .Where(kvp => !string.IsNullOrEmpty(kvp.Key) && kvp.Value != 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Financial Analysis");

                worksheet.Cells[1, 1].Value = "Date";
                worksheet.Cells[1, 2].Value = "Amount";

                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1].Value = item.Key;
                    worksheet.Cells[row, 2].Value = item.Value;
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FinancialAnalysis.xlsx");
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
                
                TempData["CaloricAnalysisData"] = JsonConvert.SerializeObject(data.ToList());

                return Json(analysisData);

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
            
            
        }
        
        [HttpGet]
        public IActionResult DownloadCaloricAnalysis()
        {
            var jsonData = TempData["CaloricAnalysisData"] as string;

            if (jsonData == null)
            {
                return NotFound("Analysis data not found.");
            }

            // Deserializacja danych z dodatkowym sprawdzeniem
            var dataList = JsonConvert.DeserializeObject<List<KeyValuePair<string, decimal>>>(jsonData);
            var data = dataList
                .Where(kvp => !string.IsNullOrEmpty(kvp.Key) && kvp.Value != 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Caloric Analysis");

                worksheet.Cells[1, 1].Value = "Date";
                worksheet.Cells[1, 2].Value = "Calories";

                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1].Value = item.Key;
                    worksheet.Cells[row, 2].Value = item.Value;
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CaloricAnalysis.xlsx");
            }
        }

        
        












        
        
    }
}