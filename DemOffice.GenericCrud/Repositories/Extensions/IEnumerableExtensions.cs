using System.Collections.Generic;
using System.Linq;

namespace DemOffice.GenericCrud.Repositories.Extensions
{
    /// <summary>
    /// Class IEnumerableExtensions.
    /// </summary>
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}
