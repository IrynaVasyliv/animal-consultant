using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalConsultant.DAL.Models;
using AnimalConsultant.Services.Models;
using AnimalConsultant.Services.Models.Filters;
using DemOffice.GenericCrud.DataAccess;
using DemOffice.GenericCrud.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnimalConsultant.Controllers
{
    public class ForumController : Controller
    {
        private readonly IReadOnlyGenericService<Questions, Questions, QuestionFilter> _questionService;

        public ForumController(IReadOnlyGenericService<Questions, Questions, QuestionFilter> questionService)
        {
            _questionService = questionService;
        }

        public async Task<IActionResult> Index(
            [FromQuery(Name = "_start")] int? start,
            [FromQuery(Name = "_end")] int? end,
            [FromQuery(Name = "_sort")] string sort,
            [FromQuery(Name = "_order")] string order,
            [FromQuery(Name = "id")] long[] ids,
            [FromQuery(Name = "id_like")] string idString,
            [FromQuery(Name = "q")] string searchQuery,
            [FromQuery] QuestionFilter filter)
        {
            var questions = await  _questionService.ReadList(new GetManyOptions<QuestionFilter>
            {
                Start = start,
                End = end,
                Sort = sort,
                OrderBy = order,
                Filter = filter,
                SearchQuery = searchQuery
            });
            return View(questions.Data.OrderBy(x=>x.Rating).ToList());
        }

        public async Task<IActionResult> Question([FromQuery]long id)
        {
            return View(await _questionService.Read(id));
        }
    }
}
