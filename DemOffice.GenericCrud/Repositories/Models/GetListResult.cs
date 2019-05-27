using System.Collections.Generic;

namespace DemOffice.GenericCrud.Repositories.Models
{
    /// <summary>Class GetListResult.</summary>
    /// <typeparam name="T">Entity element</typeparam>
    public class GetListResult<T>
    {
        /// <summary>Gets or sets the total count.</summary>
        /// <value>The total count.</value>
        public int TotalCount { get; set; }

        /// <summary>Gets or sets the data.</summary>
        /// <value>The data.</value>
        public IEnumerable<T> Data { get; set; }
    }
}
