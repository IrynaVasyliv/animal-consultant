using System;
using System.Collections.Generic;
using AutoMapper;
using DemOffice.GenericCrud.Controllers;
using DemOffice.GenericCrud.Conventions;
using DemOffice.GenericCrud.DataAccess;
using DemOffice.GenericCrud.Mapping;
using DemOffice.GenericCrud.Models;
using DemOffice.GenericCrud.Repositories;
using DemOffice.GenericCrud.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DemOffice.GenericCrud
{
    /// <summary>Class MvcBuilderExtensions.</summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>Adds the generic crud.</summary>
        /// <typeparam name="TDbContextProvider">Implementation of <see cref="IDbContextProvider"/>></typeparam>
        /// <param name="mvcBuilder">The MVC builder.</param>
        /// <param name="setup">The data setup action.</param>
        /// <returns>IServiceCollection.</returns>
        public static IMvcBuilder AddGenericCrud<TDbContextProvider>(
            this IMvcBuilder mvcBuilder,
            Action<GenericCrudSetup> setup)
            where TDbContextProvider : class, IDbContextProvider
        {
            var options = new GenericCrudSetup(mvcBuilder);

            setup(options);

            var services = mvcBuilder.Services;

            services.AddTransient(typeof(IReadOnlyGenericRepository<>), typeof(ReadOnlyGenericRepository<>));
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddAutoMapper(x => { x.AddProfile(new MappingProfile(options.Setup)); });

            mvcBuilder.AddSetup(options.Setup);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAuthContext, AuthContext>();

            services.AddTransient<IDbContextProvider, TDbContextProvider>();

            if (string.IsNullOrEmpty(options.ApiRootPath) == false)
            {
                // Customized API root path provided.
                // Instantiate the convention with the right selector.
                var prefixConvention = new ApiPrefixConvention(options.ApiRootPath, (c) => c.ControllerType.Namespace == "DemOffice.GenericCrud.Controllers");

                // Insert the convention within the MVC options
                services.Configure<MvcOptions>(opts => opts.Conventions.Insert(0, prefixConvention));
            }

            return mvcBuilder;
        }

        /// <summary>
        /// Adds controllers, services and repositories for specified types in setup
        /// </summary>
        /// <param name="mvcBuilder">The MVC builder</param>
        /// <param name="setup">Setup for specified types</param>
        /// <returns><see cref="IMvcBuilder"/></returns>
        internal static IMvcBuilder AddSetup(this IMvcBuilder mvcBuilder, IReadOnlyCollection<GenericSetup> setup)
        {
            var services = mvcBuilder.Services;

            mvcBuilder
                .ConfigureApplicationPartManager(p =>
                    p.FeatureProviders.Add(new GenericControllerFeatureProvider(setup)));

            foreach (var setupItem in setup)
            {
                services.AddTransient(
                    typeof(IReadOnlyGenericService<,,>).MakeGenericType(
                        setupItem.Model,
                        setupItem.ShortModel,
                        setupItem.Filter ?? typeof(object)),
                    typeof(ReadOnlyGenericService<,,,>).MakeGenericType(
                        setupItem.Model,
                        setupItem.ShortModel,
                        setupItem.DalModel,
                        setupItem.Filter ?? typeof(object)));

                if (setupItem.SpecificRepository != null)
                {
                    var type = setupItem.SpecificRepository.IsGenericType ?
                        setupItem.SpecificRepository.MakeGenericType(setupItem.DalModel) :
                        setupItem.SpecificRepository;

                    services.AddTransient(typeof(IReadOnlyGenericRepository<>).MakeGenericType(setupItem.DalModel), type);
                }

                if (!setupItem.IsReadOnly)
                {
                    services.AddTransient(
                        typeof(IGenericService<,,>).MakeGenericType(
                            setupItem.Model,
                            setupItem.ShortModel,
                            setupItem.Filter ?? typeof(object)),
                        typeof(GenericService<,,,>).MakeGenericType(
                            setupItem.Model,
                            setupItem.ShortModel,
                            setupItem.DalModel,
                            setupItem.Filter ?? typeof(object)));

                    if (setupItem.SpecificRepository != null)
                    {
                        var type = setupItem.SpecificRepository.IsGenericType ?
                            setupItem.SpecificRepository.MakeGenericType(setupItem.DalModel) :
                            setupItem.SpecificRepository;

                        services.AddTransient(typeof(IGenericRepository<>).MakeGenericType(setupItem.DalModel), type);
                    }
                }
            }

            return mvcBuilder;
        }
    }
}
