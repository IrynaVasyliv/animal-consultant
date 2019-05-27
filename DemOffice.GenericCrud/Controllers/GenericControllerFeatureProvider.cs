using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DemOffice.GenericCrud.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace DemOffice.GenericCrud.Controllers
{
    /// <summary>
    /// Class GenericControllerFeatureProvider.
    /// Implements the <see cref="IApplicationFeatureProvider{ControllerFeature}"/>
    /// </summary>
    internal class GenericControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IReadOnlyCollection<GenericSetup> _setup;

        /// <summary>Initializes a new instance of the <see cref="GenericControllerFeatureProvider"/> class.</summary>
        /// <param name="setup">The setup.</param>
        public GenericControllerFeatureProvider(IReadOnlyCollection<GenericSetup> setup)
        {
            _setup = setup;
        }

        /// <summary>Updates the <paramref name="feature" /> instance.</summary>
        /// <param name="parts">The list of <see cref="T:Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPart"/> instances in the application.</param>
        /// <param name="feature">The feature instance to populate.</param>
        /// <remarks>
        /// <see cref="T:Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPart"/> instances in <paramref name="parts" /> appear in the same ordered sequence they
        /// are stored in <see cref="P:Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPartManager.ApplicationParts"/>. This ordering may be used by the feature
        /// provider to make precedence decisions.
        /// </remarks>
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var map in _setup)
            {
                var typeName = map.Model + "Controller";

                if (feature.Controllers.Any(t => t.Name == typeName))
                {
                    return;
                }

                var controllerType = map.IsReadOnly
                    ? typeof(ReadOnlyGenericController<,,>)
                        .MakeGenericType(map.Model, map.ShortModel, map.Filter ?? typeof(object)).GetTypeInfo()
                    : typeof(GenericController<,,>)
                        .MakeGenericType(map.Model, map.ShortModel, map.Filter ?? typeof(object)).GetTypeInfo();

                feature.Controllers.Add(controllerType);
            }
        }
    }
}
