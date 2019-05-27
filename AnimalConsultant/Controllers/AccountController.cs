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

        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetCurrentUserAsync();
            return View(user.Role);
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
    }
}
