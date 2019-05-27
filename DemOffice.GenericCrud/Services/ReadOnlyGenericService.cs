using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DemOffice.GenericCrud.DataAccess;
using DemOffice.GenericCrud.Repositories;
using DemOffice.GenericCrud.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace DemOffice.GenericCrud.Services
{
    /// <summary>
    /// Interface IReadOnlyGenericService
    /// </summary>
    /// <typeparam name="TModel">The type of the t model.</typeparam>
    /// <typeparam name="TSModel">The type of the ts model.</typeparam>
    /// <typeparam name="TFilter">The type of the t filter.</typeparam>
    public interface IReadOnlyGenericService<TModel, TSModel, TFilter>
        where TModel : class
        where TSModel : class
    {
        /// <summary>Reads the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TModel.</returns>
        Task<TModel> Read(long id);

        /// <summary>Reads the many.</summary>
        /// <param name="ids">The ids.</param>
        /// <returns>GetListResult{TModel}.</returns>
        Task<GetListResult<TSModel>> ReadMany(IEnumerable<long> ids);

        /// <summary>Reads the list.</summary>
        /// <param name="options">The options.</param>
        /// <returns>GetListResult{TModel}.</returns>
        Task<GetListResult<TSModel>> ReadList(GetManyOptions<TFilter> options);
    }

    /// <summary>
    /// Class ReadOnlyGenericService.
    /// Implements the <see cref="IReadOnlyGenericService{TModel,TSModel,TFilter}" />
    /// </summary>
    /// <typeparam name="TModel">The type of the t model.</typeparam>
    /// <typeparam name="TSModel">The type of the ts model.</typeparam>
    /// <typeparam name="TDalModel">The type of the t dal model.</typeparam>
    /// <typeparam name="TFilter">The type of the t filter.</typeparam>
    /// <seealso cref="IReadOnlyGenericService{TModel,TSModel,TFilter}" />
    public class
        ReadOnlyGenericService<TModel, TSModel, TDalModel, TFilter> : IReadOnlyGenericService<TModel, TSModel, TFilter>
        where TModel : class
        where TSModel : class
        where TDalModel : class
    {
        private readonly IReadOnlyGenericRepository<TDalModel> _repository;

        /// <summary>Initializes a new instance of the <see cref="ReadOnlyGenericService{TModel, TSModel, TDalModel, TFilter}"/> class.</summary>
        /// <param name="repository">The repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="context">The authorization context</param>
        public ReadOnlyGenericService(IReadOnlyGenericRepository<TDalModel> repository, IMapper mapper, IAuthContext context)
        {
            _repository = repository;
            Mapper = mapper;
            Context = context;
        }

        /// <summary>Gets or sets the get filters.</summary>
        /// <value>The get filters.</value>
        public static Func<TFilter, string, IAuthContext, ICollection<Expression<Func<TDalModel, bool>>>> GetFilters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ProjectTo() should be used when mapping to <typeparamref name="TSModel"/>
        /// </summary>
        /// <value>
        /// A value indicating whether ProjectTo() should be used when mapping to <typeparamref name="TSModel"/>.
        /// </value>
        public static bool UseDbProjection { get; set; }

        /// <summary>Gets the mapper.</summary>
        /// <value>The mapper.</value>
        protected IMapper Mapper { get; }

        /// <summary>Gets the authorization context.</summary>
        /// <value>The authorization context.</value>
        protected IAuthContext Context { get; }

        /// <summary>Reads the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TModel.</returns>
        public async Task<TModel> Read(long id)
        {
            return Mapper.Map<TDalModel, TModel>(await _repository.Read(id));
        }

        /// <summary>Reads the many.</summary>
        /// <param name="ids">The ids.</param>
        /// <returns>GetListResult{TModel}.</returns>
        public async Task<GetListResult<TSModel>> ReadMany(IEnumerable<long> ids)
        {
            var result = await _repository.ReadMany(ids);
            var mappedResult = new GetListResult<TSModel>
            {
                TotalCount = result.TotalCount,
                Data = Map(result.Data)
            };

            return mappedResult;
        }

        /// <summary>Reads the list.</summary>
        /// <param name="options">The options.</param>
        /// <returns>GetListResult{TModel}.</returns>
        public async Task<GetListResult<TSModel>> ReadList(GetManyOptions<TFilter> options)
        {
            var filters = GetFilters?.Invoke(options.Filter, options.SearchQuery, Context);

            var result = await _repository.ReadList(options, filters);

            var mappedResult = new GetListResult<TSModel>
            {
                TotalCount = result.TotalCount,
                Data = Map(result.Data)
            };

            return mappedResult;
        }

        private IEnumerable<TSModel> Map(IQueryable<TDalModel> data)
        {
            return UseDbProjection
                ? data.ProjectTo<TSModel>(Mapper.ConfigurationProvider)
                : data.AsEnumerable().Select(Mapper.Map<TSModel>);
        }
    }
}
