using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace DemOffice.GenericCrud.Controllers
{
    /// <summary>
    /// Class GenericControllerNameAttribute.
    /// Implements the <see cref="Attribute"/>
    /// Implements the <see cref="IControllerModelConvention"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    internal class GenericControllerNameAttribute : Attribute, IControllerModelConvention
    {
        /// <summary>Called to apply the convention to the <see cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.ControllerModel"/>.</summary>
        /// <param name="controller">The <see cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.ControllerModel"/>.</param>
        public void Apply(ControllerModel controller)
        {
            var controllerType = controller.ControllerType.GetGenericTypeDefinition();

            if (controllerType == typeof(ReadOnlyGenericController<,,>)
                || controllerType == typeof(GenericController<,,>))
            {
                var entityType = controller.ControllerType.GenericTypeArguments[0];
                controller.ControllerName = entityType.Name;
            }
        }
    }
}
