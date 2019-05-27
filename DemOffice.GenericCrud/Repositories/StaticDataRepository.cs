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
    /// <summary>Class StaticDataRepository.
    /// Implements the <see cref="T:DemOffice.GenericCrud.Repositories.ReadOnlyGenericRepository`1"/></summary>
    /// <typeparam name="TDalModel">The type of the model.</typeparam>
    public abstract class StaticDataRepository<TDalModel> : ReadOnlyGenericRepository<TDalModel>
        where TDalModel : class
    {
        /// <summary>Initializes a new instance of the <see cref="StaticDataRepository{TDALModel}"/> class.</summary>
        /// <param name="dbContextProvider">The database context provider.</param>
        /// <param name="context">The application context service.</param>
        protected StaticDataRepository(IDbContextProvider dbContextProvider, IAuthContext context)
            : base(dbContextProvider, context)
        {
        }

        /// <summary>Reads the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="doNotUseIncludes">Denies to include inner objects in order to update entity correctly. Set to true when updating entity</param>
        /// <returns>TDalModel.</returns>
        public override async Task<TDalModel> Read(long id, bool doNotUseIncludes = false)
        {
            return GetCollection()
                .AsQueryable()
                .GetById(id)
                .SingleOrDefault();
        }

        /// <summary>Reads the many.</summary>
        /// <param name="ids">The ids.</param>
        /// <returns>GetQueryableResult{TDalModel} .</returns>
        public override async Task<GetQueryableResult<TDalModel>> ReadMany(IEnumerable<long> ids)
        {
            var collection = GetCollection().AsQueryable();

            var result = new GetQueryableResult<TDalModel>
            {
                TotalCount = collection.Count(),
                Data = collection.GetByIds(ids)
            };

            return result;
        }

        /// <summary>Reads the list.</summary>
        /// <param name="options">The options.</param>
        /// <param name="filters">The filters.</param>
        /// <returns>GetQueryableResult{TDalModel}.</returns>
        public override async Task<GetQueryableResult<TDalModel>> ReadList(
            GetManyOptions options,
            ICollection<Expression<Func<TDalModel, bool>>> filters)
        {
            var collection = GetCollection();
            var ordered = collection.AsQueryable();

            if (options.Sort != null && options.OrderBy != null)
            {
                ordered = ordered.ApplyOrder(options.Sort, options.OrderBy);
            }

            foreach (var filter in filters.OrEmptyIfNull())
            {
                ordered = ordered.Where(filter);
            }

            if (options.Start.HasValue)
            {
                ordered = ordered.Skip(options.Start.Value);
            }

            if (options.End.HasValue)
            {
                ordered = ordered.Take(options.End.Value - (options.Start ?? 0));
            }

            var result = new GetQueryableResult<TDalModel>
            {
                TotalCount = collection.Count,
                Data = ordered
            };

            return result;
        }

        /// <summary>Gets the collection.</summary>
        /// <returns>List{TDalModel}.</returns>
        protected abstract List<TDalModel> GetCollection();
    }
}
