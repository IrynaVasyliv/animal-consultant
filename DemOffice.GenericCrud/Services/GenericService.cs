using System.Threading.Tasks;
using AutoMapper;
using DemOffice.GenericCrud.Repositories;

namespace DemOffice.GenericCrud.Services
{
    /// <summary>
    /// Interface IGenericService
    /// Implements the <see cref="IReadOnlyGenericService{TModel,TSModel,TFilter}" />
    /// Implements the <see cref="IReadOnlyGenericService{TModel,TSModel,TFilter}" />
    /// </summary>
    /// <typeparam name="TModel">The type of the t model.</typeparam>
    /// <typeparam name="TSModel">The type of the ts model.</typeparam>
    /// <typeparam name="TFilter">The type of the t filter.</typeparam>
    /// <seealso cref="IReadOnlyGenericService{TModel,TSModel,TFilter}" />
    public interface IGenericService<TModel, TSModel, TFilter> : IReadOnlyGenericService<TModel, TSModel, TFilter>
        where TModel : class
        where TSModel : class
    {
        /// <summary>Creates the specified item.</summary>
        /// <param name="item">The item.</param>
        /// <returns>TModel.</returns>
        Task<TModel> Create(TModel item);

        /// <summary>Updates the specified item.</summary>
        /// <param name="item">The item.</param>
        /// <returns>TModel.</returns>
        Task<TModel> Update(TModel item);

        /// <summary>Deletes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// /// <returns>Task</returns>
        Task Delete(long id);
    }

    /// <summary>
    /// Class GenericService.
    /// Implements the <see cref="ReadOnlyGenericService{TModel,TSModel,TDalModel,TFilter}" />
    /// Implements the <see cref="IGenericService{TModel,TSModel,TFilter}" />
    /// </summary>
    /// <typeparam name="TModel">The type of the t model.</typeparam>
    /// <typeparam name="TSModel">The type of the ts model.</typeparam>
    /// <typeparam name="TDalModel">The type of the t dal model.</typeparam>
    /// <typeparam name="TFilter">The type of the t filter.</typeparam>
    /// <seealso cref="ReadOnlyGenericService{TModel,TSModel,TDalModel,TFilter}" />
    /// <seealso cref="IGenericService{TModel,TSModel,TFilter}" />
    public class GenericService<TModel, TSModel, TDalModel, TFilter> :
        ReadOnlyGenericService<TModel, TSModel, TDalModel, TFilter>, IGenericService<TModel, TSModel, TFilter>
        where TModel : class
        where TSModel : class
        where TDalModel : class
    {
        private readonly IGenericRepository<TDalModel> _repository;

        /// <summary>Initializes a new instance of the <see cref="GenericService{TModel, TSModel, TDalModel, TFilter}"/> class.</summary>
        /// <param name="readOnlyRepository">The read only repository.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="mapper">The mapper.</param>
        public GenericService(
            IReadOnlyGenericRepository<TDalModel> readOnlyRepository,
            IGenericRepository<TDalModel> repository,
            IMapper mapper,
            IAuthContext context)
            : base(readOnlyRepository, mapper, context)

        {
            _repository = repository;
        }

        /// <summary>Creates the specified item.</summary>
        /// <param name="item">The item.</param>
        /// <returns>TModel.</returns>
        public async Task<TModel> Create(TModel item)
        {
            var result = await _repository.Create(Mapper.Map<TModel, TDalModel>(item));
            return Mapper.Map<TDalModel, TModel>(result);
        }

        /// <summary>Updates the specified item.</summary>
        /// <param name="item">The item.</param>
        /// <returns>TModel.</returns>
        public async Task<TModel> Update(TModel item)
        {
            var id = (long)item.GetType().GetProperty("Id").GetValue(item, null);

            // doNotUseIncludeing is set to true because updating with included objects doesn't work correct.
            // So we need to ignore inner objects, but update relations by setting only id.
            var entity = await _repository.Read(id, true);

            var result = await _repository.Update(Mapper.Map(item, entity));
            return Mapper.Map<TDalModel, TModel>(result);
        }

        /// <summary>Deletes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        public async Task Delete(long id)
        {
            await _repository.Delete(id);
        }
    }
}
