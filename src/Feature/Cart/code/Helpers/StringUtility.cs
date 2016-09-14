namespace Sitecore.Feature.Cart.Helpers
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines the StringUtility class.
    /// </summary>
    public static class StringUtility
    {
        /// <summary>
        /// Removes the curly brackets.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The string without the curly brackets.</returns>
        public static string RemoveCurlyBrackets(string value)
        {
            return Regex.Replace(value, "[{}]", string.Empty);
        }
    }
}
