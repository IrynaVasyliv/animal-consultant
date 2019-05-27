using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemOffice.GenericCrud.DataAccess;
using DemOffice.GenericCrud.Models;
using DemOffice.GenericCrud.Repositories.Exceptions;
using DemOffice.GenericCrud.Services;
using Microsoft.EntityFrameworkCore;

namespace DemOffice.GenericCrud.Repositories
{
    /// <summary>Interface IGenericRepository
    /// Implements the <see cref="IReadOnlyGenericRepository{TDalModel}"/></summary>
    /// <typeparam name="TDalModel">The type of the model.</typeparam>
    public interface IGenericRepository<TDalModel> : IReadOnlyGenericRepository<TDalModel>
        where TDalModel : class
    {
        /// <summary>Creates the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Model.</returns>
        Task<TDalModel> Create(TDalModel entity);

        /// <summary>Updates the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Model.</returns>
        Task<TDalModel> Update(TDalModel entity);

        /// <summary>Deletes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        Task Delete(long id);
    }

    /// <summary>
    /// Class GenericRepository.
    /// Implements the <see cref="ReadOnlyGenericRepository{TDalModel}"/>
    /// Implements the <see cref="T:DemOffice.GenericCrud.Repositories.IGenericRepository`1"/>
    /// </summary>
    /// <typeparam name="T">Entity element</typeparam>
    public class GenericRepository<T> : ReadOnlyGenericRepository<T>, IGenericRepository<T>
        where T : class
    {
        /// <summary>Initializes a new instance of the <see cref="GenericRepository{T}"/> class.</summary>
        /// <param name="dbContextProvider">The database context provider.</param>
        /// <param name="context">The application context service.</param>
        public GenericRepository(IDbContextProvider dbContextProvider, IAuthContext context)
            : base(dbContextProvider, context)
        {
        }

        /// <summary>Gets or sets the validate.</summary>
        /// <value>The validate.</value>
        public static Func<T, OperationType, DbContext, ICollection<string>> Validate { get; set; }

        /// <summary>Creates the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Model.</returns>
        public virtual async Task<T> Create(T entity)
        {
            var db = DbContextProvider.Create();

            ValidateImpl(entity, OperationType.Create, db);

            await db.AddAsync(entity);
            await db.SaveChangesAsync();

            return entity;
        }

        /// <summary>Updates the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Model.</returns>
        public virtual async Task<T> Update(T entity)
        {
            var db = DbContextProvider.Create();

            ValidateImpl(entity, OperationType.Update, db);

            db.Update(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        /// <summary>Deletes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        public virtual async Task Delete(long id)
        {
            var db = DbContextProvider.Create();
            var entity = await db.Set<T>().FindAsync(id);

            ValidateImpl(entity, OperationType.Delete, db);

            if (entity == null)
            {
                throw new Exception("Entity was not found");
            }

            db.Attach(entity);
            db.Remove(entity);
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Validates the implementation.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="operationType">Type of the operation.</param>
        /// <param name="context">The DB context.</param>
        /// <exception cref="ValidationException">Validation exception.</exception>
        protected void ValidateImpl(T entity, OperationType operationType, DbContext context)
        {
            var errors = Validate?.Invoke(entity, operationType, context);
            if (errors?.Any() ?? false)
            {
                throw new ValidationException(string.Join(Environment.NewLine, errors));
            }
        }
    }
}
