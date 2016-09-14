namespace Sitecore.Foundation.CommerceServer.Extensions
{
    using Managers;
    using Models;
    using System;
    using System.Globalization;

    /// <summary>
    /// Some utility extension methods for simple types
    /// </summary>
    public static class SimpleTypesExtensions
    {
        /// <summary>
        /// Turns a decimal value into a currency string
        /// </summary>
        /// <param name="currency">The decimal object to act on</param>
        /// <param name="currencyCode">The currency code.</param>
        /// <returns>
        /// A decimal formatted as a string
        /// </returns>
        public static string ToCurrency(this decimal? currency, string currencyCode)
        {
            if (currency.HasValue)
            {
                return currency.Value.ToCurrency(currencyCode);
            }
            else
            {
                return 0M.ToCurrency(currencyCode);
            }
        }

        /// <summary>
        /// Turns a decimal value into a currency string
        /// </summary>
        /// <param name="currency">The decimal object to act on</param>
        /// <param name="currencyCode">The currency code.</param>
        /// <returns>
        /// A decimal formatted as a string
        /// </returns>
        public static string ToCurrency(this decimal currency, string currencyCode)
        {
            CurrencyInformationModel currencyInfo = StorefrontManager.GetCurrencyInformation(currencyCode);

            NumberFormatInfo info = (NumberFormatInfo)CultureInfo.GetCultureInfo(currencyInfo.CurrencyNumberFormatCulture).NumberFormat.Clone();
            info.CurrencySymbol = currencyInfo != null ? currencyInfo.Symbol : currencyCode;
            info.CurrencyPositivePattern = currencyInfo.SymbolPosition;
            return currency.ToString("C", info);
        }

        /// <summary>
        /// To the displayed date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The formatted date based on the selected culture.</returns>
        public static string ToDisplayedDate(this DateTime date)
        {
            return date.ToString("d", Context.Language.CultureInfo);
        }
    }
}