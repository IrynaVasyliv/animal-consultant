using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalConsultant.Services;
using AnimalConsultant.Services.Models;
using AnimalConsultant.Services.Models.Filters;
using DemOffice.Email.EmailService;
using DemOffice.GenericCrud.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using AnimalConsultant.DAL.Models;
using DemOffice.GenericCrud.DataAccess;
using DemOffice.GenericCrud.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace AnimalConsultant.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGenericService<Questions, Questions, QuestionFilter> _questionService;
        private readonly IReadOnlyGenericService<Users, Users, object> _genericUserService;
        private readonly IGenericService<Pets, Pets, object> _petsService;
        private readonly IGenericService<Comments, Comments, object> _commentService;
        private readonly IGenericService<Articles, Articles, object> _articlesService;
        private readonly IGenericService<Reactions, Reactions, object> _reactionsService;
        private readonly IReadOnlyGenericRepository<Reaction> _reactionsRepository;
       
        private Users user;
        private Users genericUser;

        public AccountController(
            IUserService userService,
            IReadOnlyGenericService<Users,Users, object> genericUserService,
            IGenericService<Questions, Questions, QuestionFilter> questionService,
            IGenericService<Pets, Pets, object> petsService,
            IGenericService<Comments, Comments, object> commentService,
            IEmailService emailService,
            IGenericService<Articles, Articles, object> articlesService,
            IGenericService<Reactions, Reactions, object> reactionsService,
            IReadOnlyGenericRepository<Reaction> reactionsRepository)
        {
            _userService = userService;
            _genericUserService = genericUserService;
            _questionService = questionService;
            _petsService = petsService;
            _commentService = commentService;
            _articlesService = articlesService;
            _reactionsService = reactionsService;
            _reactionsRepository = reactionsRepository;
            user = userService.GetCurrentUserAsync().Result;
            genericUser = _genericUserService.Read(user.Id).Result;
            
        }

        public async Task<IActionResult> Index(int? activeTab)
        {
            var createUsers = new Users()
            {
                FirstName = "Iryna",
                LastName = "Vasyliv",
                Email = "irynamusha@gmail.com",
                Image = "news-4-570x333.jpg",
                UserName = "irynamusha",
                Password = "Motherlode1998!"
            };
            await _userService.CreateUserAsync(createUsers, "vet");
            ViewBag.ActiveTab = activeTab;
            ViewBag.User = genericUser;
            return View(user.Role);
        }

        public async Task<IActionResult> AddPet()
        {
            ViewBag.User = genericUser;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPet(Pets pet)
        {
            var request = HttpContext.Request;
            pet.OwnerId = genericUser.Id;
            pet.Image = Request.Form.Files.FirstOrDefault()?.FileName;
            await _petsService.Create(pet);
            ViewBag.User = genericUser;
            return RedirectToAction("Index", new {activeTab = 2});
        }

        public async Task<IActionResult> AddQuestion()
        {
            ViewBag.User = genericUser;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromForm]Questions question)
        {
            var request = HttpContext.Request;
            question.UserId = user.Id;

            question.AnimalTypeId = genericUser.Pets.FirstOrDefault(x => x.Id == question.PetId).AnimalTypeId;
            question.Image = new List<string>();
            question.CreateDate = DateTime.Now;
            foreach (var formFile in Request.Form.Files)
            {
                question.Image.Add(formFile.FileName);
            }
            await _questionService.Create(question);
            return RedirectToAction("Index", new { activeTab = 3 });
        }

       
        [HttpPost]
        public IActionResult UpdateAccount(Users user)
        {
            return Ok("MF005");
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Comments comment)
        {
            comment.UserId = user.Id;
            comment.CreateDate = DateTime.Now;
            var result = await _commentService.Create(comment);
            var includedComment = await _commentService.Read(result.Id);
            return PartialView("~/Views/Partials/Comment.cshtml", includedComment);
        }

        [HttpGet]
        public async Task AddReaction(long id, string content, bool like)
        {
            var reaction = new Reactions { UserId = user.Id, Liked = like };
            Reaction exists = null;
            if (content == nameof(Comments))
            {
                reaction.CommentId = id;
                exists = await _reactionsRepository.FirstOrDefault(x => x.UserId == user.Id && x.CommentId == id);
            }
            else if (content == nameof(Questions))
            {
                reaction.QuestionId = id;
                exists = await _reactionsRepository.FirstOrDefault(x => x.UserId == user.Id && x.QuestionId == id);
            }

            if (exists == null)
            {
                await _reactionsService.Create(reaction);
            }
            else
            {
                reaction.Id = exists.Id;
                await _reactionsService.Update(reaction);
            }
        }
    }
}
