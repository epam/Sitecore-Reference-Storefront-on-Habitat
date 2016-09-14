using Sitecore.Foundation.CommerceServer.Interfaces;

namespace Sitecore.Feature.Cart.Models.JsonResults
{
    using Foundation.CommerceServer.Extensions;
    using Foundation.CommerceServer.Managers;
    using Helpers;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Entities.Shipping;
    using Sitecore.Feature.Base.Models.JsonResults;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Links;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Emits the Json result of a cart line request.
    /// </summary>
    public class CartLineBaseJsonResult : BaseJsonResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CartLineBaseJsonResult"/> class.
		/// </summary>
		/// <param name="line">The line.</param>
		/// <param name="productItemResolver"></param>
		public CartLineBaseJsonResult(CustomCommerceCartLine line, IProductResolver productItemResolver)
		{
			this.DiscountOfferNames = new List<string>();

			var product = (CommerceCartProduct)line.Product;
			//var productItem = ProductItemResolver.ResolveCatalogItem(product.ProductId, product.ProductCatalog, true);
			var productItem = productItemResolver.ResolveCatalogItem(product.ProductId, product.ProductCatalog, true);
			if (line.Images.Count > 0)
			{
				this.Image = line.Images[0].GetImageUrl(100, 100);
			}

			var userCurrency = StorefrontManager.GetCustomerCurrency();

			this.DisplayName = product.DisplayName;
			this.Color = product.Properties["Color"] as string;
			this.LineDiscount = ((CommerceTotal)line.Total).LineItemDiscountAmount.ToCurrency(this.GetCurrencyCode(userCurrency, ((CommerceTotal)line.Total).CurrencyCode));
			this.Quantity = line.Quantity.ToString(Context.Language.CultureInfo);
			this.LinePrice = product.Price.Amount.ToCurrency(this.GetCurrencyCode(userCurrency, product.Price.CurrencyCode));
			this.LineTotal = line.Total.Amount.ToCurrency(this.GetCurrencyCode(userCurrency, line.Total.CurrencyCode));
			this.ExternalCartLineId = StringUtility.RemoveCurlyBrackets(line.ExternalCartLineId);
			this.ProductUrl = product.ProductId.Equals(StorefrontManager.CurrentStorefront.GiftCardProductId, StringComparison.OrdinalIgnoreCase)
				? StorefrontManager.StorefrontUri("/buygiftcard")
				: LinkManager.GetDynamicUrl(productItem);
		}

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		/// <value>
		/// The image.
		/// </value>
		public string Image { get; set; }

		/// <summary>
		/// Gets or sets the display name.
		/// </summary>
		/// <value>
		/// The display name.
		/// </value>
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets the color.
		/// </summary>
		/// <value>
		/// The color.
		/// </value>
		public string Color { get; set; }

		/// <summary>
		/// Gets or sets the line discount.
		/// </summary>
		/// <value>
		/// The line discount.
		/// </value>
		public string LineDiscount { get; set; }

		/// <summary>
		/// Gets or sets the discount offer names.
		/// </summary>
		/// <value>
		/// The discount offer names.
		/// </value>
		public List<string> DiscountOfferNames { get; set; }

		/// <summary>
		/// Gets or sets the quantity.
		/// </summary>
		/// <value>
		/// The quantity.
		/// </value>
		public string Quantity { get; set; }

		/// <summary>
		/// Gets or sets the line price.
		/// </summary>
		/// <value>
		/// The line price.
		/// </value>
		public string LinePrice { get; set; }

		/// <summary>
		/// Gets or sets the line total.
		/// </summary>
		/// <value>
		/// The line total.
		/// </value>
		public string LineTotal { get; set; }

		/// <summary>
		/// Gets or sets the external cart line identifier.
		/// </summary>
		/// <value>
		/// The external cart line identifier.
		/// </value>
		public string ExternalCartLineId { get; set; }

		/// <summary>
		/// Gets or sets the product URL.
		/// </summary>
		/// <value>
		/// The product URL.
		/// </value>
		public string ProductUrl { get; set; }

		/// <summary>
		/// Gets or sets the line shipping options.
		/// </summary>
		/// <value>The line shipping options.</value>
		public IEnumerable<ShippingOption> ShippingOptions { get; set; }

		/// <summary>
		/// Gets the currency code.
		/// </summary>
		/// <param name="userCurrency">The user currency.</param>
		/// <param name="currency">The currency.</param>
		/// <returns>The proper currency to use.</returns>
		protected string GetCurrencyCode(string userCurrency, string currency)
		{
			if (string.IsNullOrWhiteSpace(currency))
			{
				return userCurrency;
			}

			return currency;
		}
	}
}