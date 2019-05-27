namespace DemOffice.GenericCrud.Models
{
    /// <summary>
    /// Class OperationResult.
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Generates successful result
        /// </summary>
        /// <value>
        /// A successful result
        /// </value>
        public static OperationResult Ok => new OperationResult { Success = true };

        /// <summary>
        /// Gets or sets a value indicating whether operation was successful.
        /// </summary>
        /// <value>
        /// A value indicating whether operation was successful.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets a message that explains why operation was not successful.
        /// </summary>
        /// <value>
        /// A message that explains why operation was not successful.
        /// </value>
        public string Message { get; set; }
    }
}
