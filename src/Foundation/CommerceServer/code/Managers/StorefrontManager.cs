﻿namespace Sitecore.Foundation.CommerceServer.Managers
{
    using Helpers;
    using Infrastructure.Constants;
    using Interfaces;
    using Models;
    using Sitecore.Commerce.Entities.Inventory;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.CommerceServer.Utils;
    using System;
    using System.Globalization;
    using System.Web;

    /// <summary>
    /// The manager for storefronts
    /// </summary>
    public static class StorefrontManager
    {
        private const string IndexNameFormat = "sitecore_{0}_index";

        private static bool _enforceHttps = Convert.ToBoolean(Sitecore.Configuration.Settings.GetSetting("Storefront.EnforceHTTPS", "true"), CultureInfo.InvariantCulture);

        /// <summary>
        /// Gets the current sitecontext
        /// </summary>
        public static ISiteContext CurrentSiteContext
        {
            get
            {
                return ContextTypeLoader.CreateInstance<ISiteContext>();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enforce HTTPS].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enforce HTTPS]; otherwise, <c>false</c>.
        /// </value>
        public static bool EnforceHttps
        {
            get
            {
                return _enforceHttps;
            }

            set
            {
                _enforceHttps = value;
            }
        }

        /// <summary>
        /// Gets the Current Storefront being accessed
        /// </summary>
        public static CommerceStorefront CurrentStorefront
        {
            get
            {
                CommerceStorefront storefront;

                var siteContext = ContextTypeLoader.CreateInstance<ISiteContext>();

                string path = Context.Site.RootPath + Context.Site.StartItem;

                if (!siteContext.CurrentContext.Items.Contains(path))
                {
                    storefront = ContextTypeLoader.CreateInstance<CommerceStorefront>(Context.Database.GetItem(path));

                    siteContext.CurrentContext.Items[path] = storefront;
                }

                storefront = siteContext.CurrentContext.Items[path] as CommerceStorefront;

                return storefront;
            }
        }

        /// <summary>
        /// Gets the commerce item.
        /// </summary>
        /// <value>
        /// The commerce item.
        /// </value>
        public static Item CommerceItem
        {
            get
            {
                return Context.Database.GetItem("/sitecore/Commerce");
            }
        }

        /// <summary>
        /// Gets the storefront configuration item.
        /// </summary>
        /// <value>
        /// The storefront configuration item.
        /// </value>
        public static Item StorefrontConfigurationItem
        {
            get
            {
                return Context.Database.GetItem("/sitecore/Commerce/Storefront Configuration");
            }
        }

        /// <summary>
        /// Gets the URL for the current storefronts home page
        /// </summary>
        public static string StorefrontHome
        {
            get
            {
                return "/";
            }
        }

        /// <summary>
        /// Returns a proper local URI for a route
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The store front url.</returns>
        public static string StorefrontUri(string route)
        {
            return route;
        }

        /// <summary>
        /// Used to return an external link in HTTP or HTTPS depending on the current state of the request.
        /// </summary>
        /// <param name="externalLink">The base URL string.</param>
        /// <returns>Returns the external link.</returns>
        public static string ExternalUri(string externalLink)
        {
            if (HttpContext.Current.Request.IsSecureConnection && StorefrontManager.EnforceHttps)
            {
                return "https://" + externalLink;
            }
            else
            {
                return "http://" + externalLink;
            }
        }

        /// <summary>
        /// Gets the customer currency.
        /// </summary>
        /// <returns>Returns the current customer currency.</returns>
        public static string GetCustomerCurrency()
        {
            // In the future we will get the current user currency but for now we simply return the home node default.
            return CurrentStorefront.DefaultCurrency;
        }

        /// <summary>
        /// Sets the customer currency.
        /// </summary>
        /// <param name="currency">The currency.</param>
        public static void SetCustomerCurrency(string currency)
        {
            // In the future we can set the currently selected user currency but for now we leave a place holder method.
        }

        /// <summary>
        /// Selects the external URI based on the security of the current request connection.
        /// </summary>
        /// <param name="unsecuredConnection">The unsecured connection.</param>
        /// <param name="securedConnection">The secured connection.</param>
        /// <returns>The proper url to use.</returns>
        public static string SelectExternalUri(string unsecuredConnection, string securedConnection)
        {
            if (HttpContext.Current.Request.IsSecureConnection && StorefrontManager.EnforceHttps)
            {
                return securedConnection;
            }
            else
            {
                return unsecuredConnection;
            }
        }

        /// <summary>
        /// Returns a secure HTTPS link.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The HTTPS link.</returns>
        public static string SecureStorefrontUri(string route)
        {
            if (HttpContext.Current.Request.IsSecureConnection)
            {
                return route;
            }
            else if (StorefrontManager.EnforceHttps)
            {
                UrlBuilder builder = new UrlBuilder(HttpContext.Current.Request.Url);

                if (!route.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    route = "/" + route;
                }

                return string.Format(CultureInfo.InvariantCulture, "https://{0}{1}", builder.Host, route);
            }

            return route;
        }

        /// <summary>
        /// Gets the HTML system message.
        /// </summary>
        /// <param name="messageKey">The message key.</param>
        /// <returns>The system message as an HtmlString/</returns>
        public static HtmlString GetHtmlSystemMessage(string messageKey)
        {
            return new HtmlString(GetSystemMessage(messageKey));
        }

        /// <summary>
        /// Gets the system message.
        /// </summary>
        /// <param name="messageKey">The message key.</param>
        /// <param name="insertBracketsWhenNotFound">if set to <c>true</c> and the itemName is not found, the itemName is returned with surrounding brackets.</param>
        /// <returns>
        /// A system message based on the key
        /// </returns>
        public static string GetSystemMessage(string messageKey, bool insertBracketsWhenNotFound = true)
        {
            Item lookupItem = null;

            return Lookup(StorefrontConstants.KnowItemNames.SystemMessages, messageKey, out lookupItem, insertBracketsWhenNotFound);
        }

        /// <summary>
        /// Gets the name of the product stock status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>A stock status localizable name from the site content</returns>
        public static string GetProductStockStatusName(StockStatus status)
        {
            if (status == null)
            {
                return string.Empty;
            }

            Item lookupItem = null;

            return Lookup(StorefrontConstants.KnowItemNames.InventoryStatuses, status.Name, out lookupItem, true);
        }

        /// <summary>
        /// Gets the name of the relationship.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="lookupItem">The lookup item.</param>
        /// <returns>
        /// A relationship name localizable from the site content.
        /// </returns>
        public static string GetRelationshipName(string name, out Item lookupItem)
        {
            return Lookup(StorefrontConstants.KnowItemNames.Relationships, name, out lookupItem, true);
        }

        /// <summary>
        /// Gets the localized name of the order status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>An order status localizable name from the site content</returns>
        public static string GetOrderStatusName(string status)
        {
            if (status == null)
            {
                return string.Empty;
            }

            Item lookupItem = null;

            return Lookup(StorefrontConstants.KnowItemNames.OrderStatuses, status, out lookupItem, true);
        }

        /// <summary>
        /// Gets the currency information.
        /// </summary>
        /// <param name="currency">The currency.</param>
        /// <returns>Returns information about the currency.</returns>
        public static CurrencyInformationModel GetCurrencyInformation(string currency)
        {
            string displayKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", currency, Context.Language.Name);
            Item item = StorefrontManager.StorefrontConfigurationItem.Axes.GetItem(string.Concat(StorefrontConstants.KnowItemNames.CurrencyDisplay, "/", displayKey));
            if (item != null)
            {
                return new CurrencyInformationModel(item);
            }

            item = StorefrontManager.StorefrontConfigurationItem.Axes.GetItem(string.Concat(StorefrontConstants.KnowItemNames.Currencies, "/", currency));
            if (item != null)
            {
                return new CurrencyInformationModel(item);
            }

            return null;
        }

        /// <summary>
        /// Lookups a specific node in the "Lookups" global area based on the given table and item name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="lookupItem">The lookup item.</param>
        /// <param name="insertBracketsWhenNotFound">if set to <c>true</c> and the itemName is not found, the itemName is returned with surrounding brackets.</param>
        /// <returns>
        /// The located item value if the item was found;  Otherwise string.Empty if the itemName is empty or [itemName] if no item was defined.
        /// </returns>
        public static string Lookup(string tableName, string itemName, out Item lookupItem, bool insertBracketsWhenNotFound)
        {
            Assert.ArgumentNotNullOrEmpty(tableName, "tableName");

            lookupItem = null;

            if (string.IsNullOrWhiteSpace(itemName))
            {
                return string.Empty;
            }

            Item item = CurrentStorefront.GlobalItem.Axes.GetItem(string.Concat(StorefrontConstants.KnowItemNames.Lookups, "/", tableName, "/", itemName));
            if (item == null)
            {
                if (insertBracketsWhenNotFound)
                {
                    return string.Format(CultureInfo.InvariantCulture, "[{0}]", itemName);
                }

                return itemName;
            }

            lookupItem = item;
            return item[StorefrontConstants.KnownFieldNames.Value];
        }
    }
}
