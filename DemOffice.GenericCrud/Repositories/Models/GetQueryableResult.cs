using System.Linq;

namespace DemOffice.GenericCrud.Repositories.Models
{
    /// <summary>
    /// Class GetQueryableResult.
    /// </summary>
    /// <typeparam name="T">IQueryable Data</typeparam>
    public class GetQueryableResult<T>
    {
        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>The total count.</value>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public IQueryable<T> Data { get; set; }
    }
}
