using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace DemOffice.GenericCrud.Models
{
    /// <summary>Class GenericCrudSetup.</summary>
    public class GenericCrudSetup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericCrudSetup"/> class.
        /// </summary>
        /// <param name="mvcBuilder">The mvc builder.</param>
        public GenericCrudSetup(IMvcBuilder mvcBuilder)
        {
            MvcBuilder = mvcBuilder;
        }

        /// <summary>
        /// Gets the mvc builder.
        /// </summary>
        public IMvcBuilder MvcBuilder { get; }

        /// <summary>Gets or sets the mappings.</summary>
        /// <value>The mappings.</value>
        public IReadOnlyCollection<GenericSetup> Setup { get; set; } = new List<GenericSetup>();

        /// <summary>Gets or sets root path to the api.</summary>
        /// <value>The mappings.</value>
        public string ApiRootPath { get; set; } = string.Empty;
    }
}
