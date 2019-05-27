using System;
using Microsoft.EntityFrameworkCore;

namespace DemOffice.GenericCrud.DataAccess
{
    /// <inheritdoc />
    /// <summary>Interface IDBContextProvider
    /// Implements the <see cref="T:System.IDisposable" /></summary>
    public interface IDbContextProvider : IDisposable
    {
        /// <summary>Creates this instance.</summary>
        /// <returns>DbContext.</returns>
        DbContext Create();
    }
}
