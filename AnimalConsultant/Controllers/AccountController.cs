using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalConsultant.Services;
using AnimalConsultant.Services.Models;
using AnimalConsultant.Services.Models.Filters;
using DemOffice.GenericCrud.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnimalConsultant.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGenericService<Questions, Questions, QuestionFilter> _questionService;
        private readonly IReadOnlyGenericService<Users, Users, object> _genericUserService;
        private Users user;
        private Users genericUser;

        public AccountController(IUserService userService, IReadOnlyGenericService<Users, Users, object> genericUserService, IGenericService<Questions, Questions, QuestionFilter> questionService)
        {
            _userService = userService;
            _genericUserService = genericUserService;
            _questionService = questionService;
            user = userService.GetCurrentUserAsync().Result;
            genericUser = _genericUserService.Read(user.Id).Result;
        }

        public async Task<IActionResult> Index(int? activeTab)
        {
            ViewBag.ActiveTab = activeTab;
            ViewBag.User = genericUser;
            return View(user.Role);
        }

        public async Task<IActionResult> AddPet()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPet(Pets pet)
        {
            return RedirectToAction("Index", new {activeTab = 2});
        }

        public async Task<IActionResult> AddQuestion()
        {
            ViewBag.User = genericUser;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestion(Questions question)
        {
            question.UserId = user.Id;

            question.AnimalTypeId = genericUser.Pets.FirstOrDefault(x => x.Id == question.PetId).AnimalTypeId;
            question.Image = new List<string>();
            foreach (var formFile in question.Images)
            {
                question.Image.Add(formFile.Name);
            }
            await _questionService.Create(question);
            return RedirectToAction("Index", new { activeTab = 3 });
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
                return RedirectToAction("Index");
            return BadRequest();
        }

        [HttpPost]
        public IActionResult UpdateAccount(Users user)
        {
            return Ok("MF005");
        }
    }
}
