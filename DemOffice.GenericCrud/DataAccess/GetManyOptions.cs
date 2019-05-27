namespace DemOffice.GenericCrud.DataAccess
{
    /// <summary>Class GetManyOptions.</summary>
    public class GetManyOptions
    {
        /// <summary>Gets or sets the start.</summary>
        /// <value>The start.</value>
        public int? Start { get; set; }

        /// <summary>Gets or sets the end.</summary>
        /// <value>The end.</value>
        public int? End { get; set; }

        /// <summary>Gets or sets the sort.</summary>
        /// <value>The sort.</value>
        public string Sort { get; set; }

        /// <summary>Gets or sets the order by.</summary>
        /// <value>The order by.</value>
        public string OrderBy { get; set; }

        /// <summary>Gets or sets the search query.</summary>
        /// <value>The search query.</value>
        public string SearchQuery { get; set; }
    }

    /// <summary>Class GetManyOptions.
    /// Implements the <see cref="T:DemOffice.GenericCrud.DataAccess.GetManyOptions"/></summary>
    /// <typeparam name="TFilter">The type of the t filter.</typeparam>
    public class GetManyOptions<TFilter> : GetManyOptions
    {
        /// <summary>Gets or sets the filter.</summary>
        /// <value>The filter.</value>
        public TFilter Filter { get; set; }
    }
}
