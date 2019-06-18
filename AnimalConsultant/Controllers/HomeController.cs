using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AnimalConsultant.Models;

namespace AnimalConsultant.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddComment()
        {
            return Ok("MF000");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Quiz()
        {
            return View();
        }

        public IActionResult GetQuizQuestion(int index)
        {
            if (index == 5)
            {
                return PartialView("~/Views/Partials/QuizResult.cshtml");
            }

            return PartialView("~/Views/Partials/QuizQuestion.cshtml");
        }
    }
}
