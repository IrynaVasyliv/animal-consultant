using System.Threading.Tasks;
using DemOffice.GenericCrud.Repositories.Exceptions;
using DemOffice.GenericCrud.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemOffice.GenericCrud.Controllers
{
    /// <summary>
    /// This controller allows to enable write/read access for DB entities in easy configuration
    /// See also <see cref="ReadOnlyGenericController{TModel, SModel, TFilter}" /> for read operations
    /// Implements the <see cref="ReadOnlyGenericController{TModel,TSModel,TFilter}" />
    /// </summary>
    /// <typeparam name="TModel">Entity element</typeparam>
    /// <typeparam name="TSModel">The type of the s model.</typeparam>
    /// <typeparam name="TFilter">Filter for TModel</typeparam>
    /// <seealso cref="ReadOnlyGenericController{TModel,TSModel,TFilter}" />
    [Route("[controller]")]
    [GenericControllerName]
    public class GenericController<TModel, TSModel, TFilter> : ReadOnlyGenericController<TModel, TSModel, TFilter>
        where TModel : class
        where TSModel : class
    {
        private readonly IGenericService<TModel, TSModel, TFilter> _service;

        /// <summary>Initializes a new instance of the <see cref="GenericController{TModel, SModel, TFilter}"/> class.</summary>
        /// <param name="readOnlyService">The read only service.</param>
        /// <param name="service">The service.</param>
        public GenericController(
            IReadOnlyGenericService<TModel, TSModel, TFilter> readOnlyService,
            IGenericService<TModel, TSModel, TFilter> service)
            : base(readOnlyService)
        {
            _service = service;
        }

        /// <summary>Id in route segments specified only for
        /// compatibility with certain clients and
        /// will be ignored upon action execution.</summary>
        /// <param name="item">The item.</param>
        /// <returns><see cref="OkResult"/> on success, otherwise, <see cref="BadRequestResult"/>.</returns>
        [HttpPost("{id?}")]
        public async Task<IActionResult> Create(TModel item)
        {
            try
            {
                return Ok(await _service.Create(item));
            }
            catch (ValidationException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>Id in route segments specified only for
        /// compatibility with certain clients and
        /// will be ignored upon action execution</summary>
        /// <param name="item">Update  the specified entity by identifier.</param>
        /// <returns><see cref="OkResult"/> on success, otherwise, <see cref="BadRequestResult"/>.</returns>
        [HttpPut("{id?}")]
        public async Task<IActionResult> Update(TModel item)
        {
            try
            {
                return Ok(await _service.Update(item));
            }
            catch (ValidationException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>Deletes the specified entity by identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns><see cref="OkResult"/> on success, otherwise, <see cref="BadRequestResult"/>.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                await _service.Delete(id);
                return Ok();
            }
            catch (ValidationException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
