using System;

namespace Plato.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Stripped the specified unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public static string Stripped(this Guid guid)
        {
            return guid.ToString().Replace("-", string.Empty).ToUpper();
        }
    }
}
