using System;
using System.Threading.Tasks;
using DemOffice.GenericCrud.DataAccess;
using DemOffice.GenericCrud.Repositories.Models;
using DemOffice.GenericCrud.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using DemOffice.GenericCrud.Repositories.Extensions;

namespace DemOffice.GenericCrud.Controllers
{
    /// <summary>
    /// This controller allows to enable read access for entities in easy configuration
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller" />
    /// </summary>
    /// <typeparam name="TModel">The type of the t model.</typeparam>
    /// <typeparam name="TSModel">The type of the s model.</typeparam>
    /// <typeparam name="TFilter">The type of the t filter.</typeparam>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]")]
    [GenericControllerName]
    public class ReadOnlyGenericController<TModel, TSModel, TFilter> : Controller
        where TModel : class
        where TSModel : class
    {
        private readonly IReadOnlyGenericService<TModel, TSModel, TFilter> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyGenericController{TModel, TSModel, TFilter}"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public ReadOnlyGenericController(IReadOnlyGenericService<TModel, TSModel, TFilter> service)
        {
            _service = service;
        }

        /// <summary>
        /// Reads the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TModel.</returns>
        [HttpGet("{id}")]
        public Task<TModel> Read([FromRoute] long id)
        {
            return _service.Read(id);
        }

        /// <summary>
        /// Reads the list.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="order">The order.</param>
        /// <param name="ids">The list of id's, separated by "|"</param>
        /// <param name="searchQuery">The search query.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>IEnumerable{TSModel}.</returns>
        [HttpGet]
        public async Task<IEnumerable<TSModel>> ReadList(
            [FromQuery(Name = "_start")] int? start,
            [FromQuery(Name = "_end")] int? end,
            [FromQuery(Name = "_sort")] string sort,
            [FromQuery(Name = "_order")] string order,
            [FromQuery(Name = "id")] long[] ids,
            [FromQuery(Name = "id_like")] string idString,
            [FromQuery(Name = "q")] string searchQuery,
            [FromQuery] TFilter filter)
        {
            GetListResult<TSModel> result;

            if (ids?.Any() == true || idString != null)
            {
                result = _service.ReadMany(ids
                    .OrEmptyIfNull()
                    .Union(idString?.Split('|')
                               .Select(x => long.Parse(x))
                               .ToList() ?? new List<long>())).Result;
            }
            else
            {
                result = _service.ReadList(new GetManyOptions<TFilter>
                {
                    Start = start,
                    End = end,
                    Sort = sort,
                    OrderBy = order,
                    Filter = filter,
                    SearchQuery = searchQuery
                }).Result;
            }

            Request.HttpContext.Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());
            return result.Data;
        }
    }
}
