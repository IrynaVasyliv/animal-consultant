using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalConsultant.Services;
using AnimalConsultant.Services.Models;
using DemOffice.Email.EmailService;
using Microsoft.AspNetCore.Mvc;

namespace AnimalConsultant.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AuthenticationController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpGet("register")]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostRegister(Users user)
        {
            var result = await _userService.CreateUserAsync(user, user.Role ?? "user");

            if (result.Succeeded)
            {
                var message = System.IO.File.ReadAllText("SendEmail.html");
                _emailService.Send(user.Email, message, "Become the Animal Shelter expert", true, sender: "animalshelter@gmail.com");

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
        public async Task<IActionResult> LoginPost(Users user)
        {
            var result = await _userService.PasswordSignInUserAsync(user.Email, user.Password, true, true);
            if (result.Succeeded)
                return RedirectToAction("Index", "Account");
            return BadRequest();
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignOutUserAsync();
            return RedirectToAction("Login", "Authentication");
        }

    }
}
