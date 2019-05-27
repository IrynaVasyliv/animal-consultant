using System;

namespace DemOffice.GenericCrud.Repositories.Exceptions
{
    /// <summary>Class ValidationException.
    /// Implements the <see cref="System.Exception"/></summary>
    internal class ValidationException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="ValidationException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ValidationException(string message)
            : base(message)
        {
        }
    }
}
