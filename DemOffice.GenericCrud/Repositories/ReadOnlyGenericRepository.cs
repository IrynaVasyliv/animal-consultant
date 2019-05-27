using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DemOffice.GenericCrud.DataAccess;
using DemOffice.GenericCrud.Repositories.Extensions;
using DemOffice.GenericCrud.Repositories.Models;
using DemOffice.GenericCrud.Services;
using Microsoft.EntityFrameworkCore;

namespace DemOffice.GenericCrud.Repositories
{
    /// <summary>
    /// Interface IReadOnlyGenericRepository
    /// </summary>
    /// <typeparam name="TDalModel">The type of the t dal model.</typeparam>
    public interface IReadOnlyGenericRepository<TDalModel>
        where TDalModel : class
    {
        /// <summary>
        /// Reads the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="doNotUseIncludes">Denies to include inner objects in order to update entity correctly. Set to true when updating entity</param>
        /// <returns>TDalModel.</returns>
        Task<TDalModel> Read(long id, bool doNotUseIncludes = false);

        /// <summary>
        /// Finds first match for the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>TDalModel.</returns>
        Task<TDalModel> FirstOrDefault(Expression<Func<TDalModel, bool>> filter);

        /// <summary>
        /// Reads the many.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>GetQueryableResult{TDalModel}.</returns>
        Task<GetQueryableResult<TDalModel>> ReadMany(IEnumerable<long> ids);

        /// <summary>
        /// Reads the list.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="filters">The filters.</param>
        /// <returns>GetQueryableResult{TDalModel}.</returns>
        Task<GetQueryableResult<TDalModel>> ReadList(
            GetManyOptions options,
            ICollection<Expression<Func<TDalModel, bool>>> filters);

        /// <summary>
        /// Counts the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>System.Int32.</returns>
        Task<int> Count(Expression<Func<TDalModel, bool>> predicate);
    }

    /// <summary>
    /// Class ReadOnlyGenericRepository.
    /// Implements the <see cref="IReadOnlyGenericRepository{TDalModel}" />
    /// </summary>
    /// <typeparam name="TDalModel">The type of the t dal model.</typeparam>
    /// <seealso cref="IReadOnlyGenericRepository{TDalModel}" />
    public class ReadOnlyGenericRepository<TDalModel> :
        IReadOnlyGenericRepository<TDalModel>
        where TDalModel : class
    {
        protected IAuthContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyGenericRepository{TDalModel}"/> class.
        /// </summary>
        /// <param name="dbContextProvider">The database context provider.</param>
        /// <param name="context">The application context service.</param>
        public ReadOnlyGenericRepository(IDbContextProvider dbContextProvider, IAuthContext context)
        {
            DbContextProvider = dbContextProvider;
            Context = context;
        }

        /// <summary>
        /// Gets or sets the includes.
        /// </summary>
        /// <value>The includes.</value>
        public static IEnumerable<string> Includes { get; set; }

        /// <summary>
        /// Gets or sets the get many includes.
        /// </summary>
        /// <value>The get many includes.</value>
        public static IEnumerable<string> GetManyIncludes { get; set; }

        /// <summary>
        /// Gets or sets the deep sort.
        /// </summary>
        /// <value>The deep sort.</value>
        public static Dictionary<string, Expression<Func<TDalModel, object>>> DeepSort { get; set; }

        /// <summary>
        /// Gets the database context provider.
        /// </summary>
        /// <value>The database context provider.</value>
        protected IDbContextProvider DbContextProvider { get; }

        /// <summary>
        /// Reads the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="doNotUseIncludes">Denies to include inner objects in order to update entity correctly. Set to true when updating entity</param>
        /// <returns>TDalModel.</returns>
        public virtual async Task<TDalModel> Read(long id, bool doNotUseIncludes = false)
        {
            var db = DbContextProvider.Create();
            var dbSet = db.Set<TDalModel>().AsQueryable();

            if (!doNotUseIncludes)
            {
                dbSet = Includes.OrEmptyIfNull().Aggregate(dbSet, (current, entity) => current.Include(entity));
            }

            return await dbSet.GetById(id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Finds first match for the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>TDalModel.</returns>
        public async Task<TDalModel> FirstOrDefault(Expression<Func<TDalModel, bool>> filter)
        {
            var dbSet = GetIncludedDbSet(Includes);

            return await dbSet.FirstOrDefaultAsync(filter);
        }

        /// <summary>
        /// Reads the many.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>GetQueryableResult{TDalModel}.</returns>
        public virtual async Task<GetQueryableResult<TDalModel>> ReadMany(IEnumerable<long> ids)
        {
            var dbSet = GetIncludedDbSet(Includes);

            var result = new GetQueryableResult<TDalModel>
            {
                TotalCount = await dbSet.CountAsync(),
                Data = dbSet.GetByIds(ids)
            };

            return result;
        }

        /// <summary>
        /// Reads the list.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="filters">The filters.</param>
        /// <returns>GetQueryableResult{TDalModel}.</returns>
        public virtual async Task<GetQueryableResult<TDalModel>> ReadList(
            GetManyOptions options,
            ICollection<Expression<Func<TDalModel, bool>>> filters)
        {
            var dbSet = GetIncludedDbSet(GetManyIncludes);

            dbSet = filters.OrEmptyIfNull().Aggregate(dbSet, (current, filter) => current.Where(filter));

            var ordered = dbSet;

            if (options?.Sort != null && DeepSort != null && DeepSort.Keys.Contains(options.Sort.ToLower()))
            {
                ordered = options.OrderBy.ToLower() == "desc"
                    ? ordered.OrderByDescending(DeepSort[options.Sort.ToLower()])
                    : ordered.OrderBy(DeepSort[options.Sort.ToLower()]);
            }
            else if (options?.Sort != null && options.OrderBy != null)
            {
                ordered = ordered.ApplyOrder(options.Sort, options.OrderBy);
            }

            if (options?.Start != null)
            {
                ordered = ordered.Skip(options.Start.Value);
            }

            if (options?.End != null)
            {
                ordered = ordered.Take(options.End.Value - (options.Start ?? 0));
            }

            var result = new GetQueryableResult<TDalModel>
            {
                TotalCount = await dbSet.CountAsync(),
                Data = ordered
            };

            return result;
        }

        /// <summary>
        /// Counts the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>System.Int32.</returns>
        public virtual async Task<int> Count(Expression<Func<TDalModel, bool>> predicate)
        {
            var dbSet = GetIncludedDbSet();

            return await dbSet.CountAsync(predicate);
        }

        private IQueryable<TDalModel> GetIncludedDbSet(IEnumerable<string> includes = null)
        {
            var db = DbContextProvider.Create();
            var dbSet = db.Set<TDalModel>().AsQueryable();

            dbSet = includes.OrEmptyIfNull().Aggregate(dbSet, (current, entity) => current.Include(entity));

            return dbSet;
        }
    }
}
