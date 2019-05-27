using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using DemOffice.GenericCrud.Mapping;
using DemOffice.GenericCrud.Repositories;
using DemOffice.GenericCrud.Services;
using Microsoft.EntityFrameworkCore;

namespace DemOffice.GenericCrud.Models
{
    /// <summary>
    /// Class GenericSetup.
    /// Base class for <see cref="T:DemOffice.GenericCrud.Mapping.GenericSetup`2"/> and <see cref="T:DemOffice.GenericCrud.Mapping.GenericSetup`3"/>
    /// </summary>
    public abstract class GenericSetup
    {
        internal Type Model { get; set; }

        internal Type ShortModel { get; set; }

        internal Type DalModel { get; set; }

        internal Type SpecificRepository { get; set; }

        internal bool IsReadOnly { get; set; }

        internal Type Filter { get; set; }

        internal abstract void CreateMap(MappingProfile mappingProfile);
    }

    /// <summary>
    /// Class GenericSetup.
    /// Implements the <see cref="T:DemOffice.GenericCrud.Mapping.GenericSetup" />.
    /// Allows to setup entity to work with functionality of <see cref="T:DemOffice.GenericCrud" />.
    /// Is used when data needs to be filtered.
    /// Implements the <see cref="GenericSetup" />
    /// </summary>
    /// <typeparam name="TModel">The type of the t model.</typeparam>
    /// <typeparam name="TSModel">The type of the ts model.</typeparam>
    /// <typeparam name="TDalModel">The type of the t dal model.</typeparam>
    /// <typeparam name="TFilter">The type of the t filter.</typeparam>
    /// <seealso cref="GenericSetup" />
    public class FilteredGenericSetup<TModel, TSModel, TDalModel, TFilter> : GenericSetup
        where TModel : class
        where TSModel : class
        where TDalModel : class
    {
        private readonly Action<AutoMapper.IMappingExpression<TModel, TDalModel>> _mappingOverride;
        private readonly Action<AutoMapper.IMappingExpression<TSModel, TDalModel>> _shortMappingOverride;

        /// <summary>Initializes a new instance of the <see cref="FilteredGenericSetup{TModel, TSModel, TDalModel, TFilter}"/> class.</summary>
        /// <param name="filters">Expressions that can be applied to data when filtering it.</param>
        /// <param name="isReadOnly">If set to <c>true</c> [only ReadOnlyController, ReadOnlyService and ReadOnlyRepository can be used.].</param>
        /// <param name="specificRepository">Allows you to change the default repository for custom.</param>
        /// <param name="getManyIncludes">Includes to apply when you get many objects.</param>
        /// <param name="includes">Includes to apply when you get one object.</param>
        /// <param name="mappingOverride">Specific mapping for models.</param>
        /// <param name="shortMappingOverride">Specific mapping for <typeparamref name="TSModel"/>.</param>
        /// <param name="validate">Data transfer model validation.</param>
        /// <param name="deepSort">Can be defined when the collection is sorted by other inner objects' properties.</param>
        /// <param name="useDbProjection">Defines if mapper should use ProjectTo() when mapping to <typeparamref name="TSModel"/></param>
        public FilteredGenericSetup(
            Func<TFilter, string, IAuthContext, ICollection<Expression<Func<TDalModel, bool>>>> filters,
            bool isReadOnly = false,
            Type specificRepository = null,
            IEnumerable<string> getManyIncludes = null,
            IEnumerable<string> includes = null,
            Action<AutoMapper.IMappingExpression<TModel, TDalModel>> mappingOverride = null,
            Action<AutoMapper.IMappingExpression<TSModel, TDalModel>> shortMappingOverride = null,
            Func<TDalModel, OperationType, DbContext, ICollection<string>> validate = null,
            Dictionary<string, Expression<Func<TDalModel, object>>> deepSort = null,
            bool useDbProjection = false)
        {
            Model = typeof(TModel);
            ShortModel = typeof(TSModel);
            DalModel = typeof(TDalModel);
            SpecificRepository = specificRepository;
            _mappingOverride = mappingOverride;
            _shortMappingOverride = shortMappingOverride;

            var readOnlyRepository = typeof(ReadOnlyGenericRepository<>)
                .MakeGenericType(typeof(TDalModel));

            var repository = typeof(GenericRepository<>)
                .MakeGenericType(typeof(TDalModel));

            readOnlyRepository.GetProperty(nameof(ReadOnlyGenericRepository<object>.GetManyIncludes))
                ?.SetValue(null, getManyIncludes);
            readOnlyRepository.GetProperty(nameof(ReadOnlyGenericRepository<object>.Includes))
                ?.SetValue(null, includes);

            repository.GetProperty(nameof(GenericRepository<object>.Validate))
                ?.SetValue(null, validate);

            readOnlyRepository.GetProperty(nameof(ReadOnlyGenericRepository<TDalModel>.DeepSort))
                ?.SetValue(null, deepSort);

            IsReadOnly = isReadOnly;

            Filter = typeof(TFilter);

            typeof(ReadOnlyGenericService<,,,>)
                .MakeGenericType(typeof(TModel), typeof(TSModel), typeof(TDalModel), typeof(TFilter))
                .GetProperty(nameof(ReadOnlyGenericService<object, object, object, object>.GetFilters))
                ?.SetValue(null, filters);

            typeof(ReadOnlyGenericService<,,,>)
                .MakeGenericType(typeof(TModel), typeof(TSModel), typeof(TDalModel), typeof(TFilter))
                .GetProperty(nameof(ReadOnlyGenericService<object, object, object, object>.UseDbProjection))
                ?.SetValue(null, useDbProjection);
        }

        /// <summary>
        /// Creates the map.This method allows you to specify
        /// the correspondence between values in db and Entity
        /// </summary>
        /// <param name="mappingProfile">The mapping profile.</param>
        internal override void CreateMap(MappingProfile mappingProfile)
        {
            var mapping = mappingProfile.CreateMap<TModel, TDalModel>();
            _mappingOverride?.Invoke(mapping);

            if (Model != ShortModel)
            {
                var shortMapping = mappingProfile.CreateMap<TSModel, TDalModel>();
                _shortMappingOverride?.Invoke(shortMapping);
            }
        }
    }

    /// <summary>
    /// Class GenericSetup.
    /// Implements the <see cref="T:DemOffice.GenericCrud.Mapping.GenericSetup`3" />.
    /// Allows to setup entity to work with functionality of <see cref="T:DemOffice.GenericCrud" />.
    /// Is used when data needs to be filtered and TSModel is useless.
    /// Implements the <see cref="FilteredGenericSetup{TModel, TModel, TDalModel, TFilter}" />
    /// </summary>
    /// <typeparam name="TModel">The type of the t model.</typeparam>
    /// <typeparam name="TDalModel">The type of the t dal model.</typeparam>
    /// <typeparam name="TFilter">The type of the t filter.</typeparam>
    /// <seealso cref="FilteredGenericSetup{TModel, TModel, TDalModel, TFilter}" />
    public class
        FilteredGenericSetup<TModel, TDalModel, TFilter> : FilteredGenericSetup<TModel, TModel,
            TDalModel, TFilter>
        where TModel : class
        where TDalModel : class
    {
        /// <summary>Initializes a new instance of the <see cref="FilteredGenericSetup{TModel, TDalModel, TFilter}"/> class.</summary>
        /// <param name="filters">Expressions that can be applied to data when filtering it.</param>
        /// <param name="isReadOnly">If set to <c>true</c> [only ReadOnlyController, ReadOnlyService and ReadOnlyRepository can be used.].</param>
        /// <param name="specificRepository">Allows you to change the default repository for custom.</param>
        /// <param name="getManyIncludes">Includes to apply when you get many objects.</param>
        /// <param name="includes">Includes to apply when you get one object.</param>
        /// <param name="mappingOverride">Specific mapping for models.</param>
        /// <param name="validate">Data transfer model validation.</param>
        /// <param name="deepSort">Can be defined when the collection is sorted by other inner objects' properties.</param>
        /// <param name="useDbProjection">Defines if mapper should use ProjectTo() when mapping to <typeparamref name="TModel"/></param>
        public FilteredGenericSetup(
            Func<TFilter, string, IAuthContext, ICollection<Expression<Func<TDalModel, bool>>>> filters,
            bool isReadOnly = false,
            Type specificRepository = null,
            IEnumerable<string> getManyIncludes = null,
            IEnumerable<string> includes = null,
            Action<AutoMapper.IMappingExpression<TModel, TDalModel>> mappingOverride = null,
            Func<TDalModel, OperationType, DbContext, ICollection<string>> validate = null,
            Dictionary<string, Expression<Func<TDalModel, object>>> deepSort = null,
            bool useDbProjection = false)
            : base(filters, isReadOnly, specificRepository, getManyIncludes, includes, mappingOverride, validate: validate, deepSort: deepSort, useDbProjection: useDbProjection)
        {
        }
    }

    /// <summary>
    /// Class GenericSetup.
    /// Implements the <see cref="FilteredGenericSetup{TModel, TSModel, TDalModel, Object}" />
    /// Allows to setup entity to work with functionality of <see cref="T:DemOffice.GenericCrud" />.
    /// Is used when data doesn't need to be filtered.
    /// </summary>
    /// <typeparam name="TModel">The type of the t model.</typeparam>
    /// <typeparam name="TSModel">The type of the ts model.</typeparam>
    /// <typeparam name="TDalModel">The type of the t dal model.</typeparam>
    /// <seealso cref="FilteredGenericSetup{TModel, TSModel, TDalModel, Object}" />
    public class GenericSetup<TModel, TSModel, TDalModel> : FilteredGenericSetup<TModel, TSModel, TDalModel, object>
        where TModel : class
        where TSModel : class
        where TDalModel : class
    {
        /// <summary>Initializes a new instance of the <see cref="GenericSetup{TModel, TSModel, TDalModel}"/> class.</summary>
        /// <param name="filters">Expressions that can be applied to data when filtering it.</param>
        /// <param name="isReadOnly">If set to <c>true</c> [only ReadOnlyController, ReadOnlyService and ReadOnlyRepository can be used.].</param>
        /// <param name="specificRepository">Allows you to change the default repository for custom.</param>
        /// <param name="getManyIncludes">Includes to apply when you get many objects.</param>
        /// <param name="includes">Includes to apply when you get one object.</param>
        /// <param name="mappingOverride">Specific mapping for models.</param>
        /// /// <param name="shortMappingOverride">Specific mapping for <typeparamref name="TSModel"/>.</param>
        /// <param name="validate">Data transfer model validation.</param>
        /// <param name="deepSort">Can be defined when the collection is sorted by other inner objects' properties.</param>
        /// <param name="useDbProjection">Defines if mapper should use ProjectTo() when mapping to <typeparamref name="TSModel"/></param>
        public GenericSetup(
            bool isReadOnly = false,
            Type specificRepository = null,
            IEnumerable<string> getManyIncludes = null,
            IEnumerable<string> includes = null,
            Action<IMappingExpression<TModel, TDalModel>> mappingOverride = null,
            Action<IMappingExpression<TSModel, TDalModel>> shortMappingOverride = null,
            Func<string, IAuthContext, ICollection<Expression<Func<TDalModel, bool>>>> filters = null,
            Func<TDalModel, OperationType, DbContext, ICollection<string>> validate = null,
            Dictionary<string, Expression<Func<TDalModel, object>>> deepSort = null,
            bool useDbProjection = false)
            : base((o, query, context) => filters?.Invoke(query, context), isReadOnly, specificRepository, getManyIncludes, includes, mappingOverride, shortMappingOverride, validate, deepSort, useDbProjection)
        {
        }
    }

    /// <summary>
    /// Class GenericSetup.
    /// Implements the <see cref="FilteredGenericSetup{TModel, TModel, TDalModel, Object}" />
    /// Allows to setup entity to work with functionality of <see cref="T:DemOffice.GenericCrud" />.
    /// Is used when data doesn't need to be filtered and TSModel is useless.
    /// </summary>
    /// <typeparam name="TModel">The type of the t model.</typeparam>
    /// <typeparam name="TDalModel">The type of the t dal model.</typeparam>
    /// <seealso cref="FilteredGenericSetup{TModel, TModel, TDalModel, Object}" />
    public class GenericSetup<TModel, TDalModel> : FilteredGenericSetup<TModel, TModel, TDalModel, object>
        where TModel : class
        where TDalModel : class
    {
        /// <summary>Initializes a new instance of the <see cref="GenericSetup{TModel, TDalModel}"/> class.</summary>
        /// <param name="filters">Expressions that can be applied to data when filtering it.</param>
        /// <param name="isReadOnly">If set to <c>true</c> [only ReadOnlyController, ReadOnlyService and ReadOnlyRepository can be used.].</param>
        /// <param name="specificRepository">Allows you to change the default repository for custom.</param>
        /// <param name="getManyIncludes">Includes to apply when you get many objects.</param>
        /// <param name="includes">Includes to apply when you get one object.</param>
        /// <param name="mappingOverride">Specific mapping for models.</param>
        /// <param name="validate">Data transfer model validation.</param>
        /// <param name="deepSort">Can be defined when the collection is sorted by other inner objects' properties.</param>
        /// <param name="useDbProjection">Defines if mapper should use ProjectTo() when mapping to <typeparamref name="TModel"/></param>
        public GenericSetup(
            bool isReadOnly = false,
            Type specificRepository = null,
            IEnumerable<string> getManyIncludes = null,
            IEnumerable<string> includes = null,
            Action<IMappingExpression<TModel, TDalModel>> mappingOverride = null,
            Func<string, IAuthContext, ICollection<Expression<Func<TDalModel, bool>>>> filters = null,
            Func<TDalModel, OperationType, DbContext, ICollection<string>> validate = null,
            Dictionary<string, Expression<Func<TDalModel, object>>> deepSort = null,
            bool useDbProjection = false)
            : base((o, query, context) => filters?.Invoke(query, context), isReadOnly, specificRepository, getManyIncludes, includes, mappingOverride, validate: validate, deepSort: deepSort, useDbProjection: useDbProjection)
        {
        }
    }
}
