using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalConsultant.Services;
using AnimalConsultant.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnimalConsultant.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index(int? activeTab)
        {
            var user = await _userService.GetCurrentUserAsync();
            ViewBag.ActiveTab = activeTab;
            return View(user.Role);
        }

        public async Task<IActionResult> AddPet()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPet(Pet pet)
        {
            return RedirectToAction("Index", new {activeTab = 2});
        }

        public async Task<IActionResult> AddQuestion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestion(Question pet)
        {
            return RedirectToAction("Index", new { activeTab = 3 });
        }

        [HttpGet("register")]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostRegister(User user)
        {
            var result = await _userService.CreateUserAsync(user, user.Role ?? "user");

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(string.Join(string.Empty, result.Errors));
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginPost(User user)
        {
            var result = await _userService.PasswordSignInUserAsync(user.Email, user.Password, true, true);
            if (result.Succeeded)
                return RedirectToAction("Index");
            return BadRequest();
        }

        [HttpPost]
        public IActionResult UpdateAccount(User user)
        {
            return Ok("MF005");
        }
    }
}
