using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace DemOffice.GenericCrud.Conventions
{
    /// <summary>
    /// Api Prefix Convention
    /// </summary>
    public class ApiPrefixConvention : IApplicationModelConvention
    {
        private readonly string _prefix;
        private readonly Func<ControllerModel, bool> _controllerSelector;
        private readonly AttributeRouteModel _onlyPrefixRoute;
        private readonly AttributeRouteModel _fullRoute;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiPrefixConvention"/> class.
        /// </summary>
        /// <param name="prefix">Api Prefix</param>
        /// <param name="controllerSelector">Affected controllers</param>
        public ApiPrefixConvention(string prefix, Func<ControllerModel, bool> controllerSelector)
        {
            _prefix = prefix;
            _controllerSelector = controllerSelector;

            // Prepare AttributeRouteModel local instances, ready to be added to the controllers

            // This one is meant to be combined with existing route attributes
            _onlyPrefixRoute = new AttributeRouteModel(new RouteAttribute(prefix));

            // This one is meant to be added as the route for api controllers that do not specify any route attribute
            _fullRoute = new AttributeRouteModel(new RouteAttribute("[controller]"));
        }

        /// <summary>
        /// Apply conventions
        /// </summary>
        /// <param name="application">Application</param>
        public void Apply(ApplicationModel application)
        {
            // Loop through any controller matching our selector
            foreach (var controller in application.Controllers.Where(_controllerSelector))
            {
                // Either update existing route attributes or add a new one
                if (controller.Selectors.Any(x => x.AttributeRouteModel != null))
                {
                    AddPrefixesToExistingRoutes(controller);
                }
                else
                {
                    AddNewRoute(controller);
                }
            }
        }

        private void AddPrefixesToExistingRoutes(ControllerModel controller)
        {
            foreach (var selectorModel in controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList())
            {
                // Merge existing route models with the api prefix
                var originalAttributeRoute = selectorModel.AttributeRouteModel;
                selectorModel.AttributeRouteModel =
                    AttributeRouteModel.CombineAttributeRouteModel(_onlyPrefixRoute, originalAttributeRoute);
            }
        }

        private void AddNewRoute(ControllerModel controller)
        {
            // The controller has no route attributes, lets add a default api convention
            var defaultSelector = controller.Selectors.First(s => s.AttributeRouteModel == null);
            defaultSelector.AttributeRouteModel = _fullRoute;
        }
    }
}
