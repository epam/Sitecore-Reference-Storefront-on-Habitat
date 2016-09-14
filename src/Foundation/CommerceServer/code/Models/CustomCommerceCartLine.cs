namespace Sitecore.Foundation.CommerceServer.Models
{
	using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
	using Sitecore.Data.Items;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml.Serialization;
	using Newtonsoft.Json;

	/// <summary>
	/// A extension of the default cart line to allow for additional properties
	/// </summary>
	[Serializable]
    public class CustomCommerceCartLine : CommerceCartLine
    {
        [NonSerialized]
        private List<MediaItem> _images;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCommerceCartLine"/> class.
        /// </summary>
        public CustomCommerceCartLine()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCommerceCartLine"/> class.
        /// </summary>
        /// <param name="productCatalog">The product catalog.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="variantId">The variant identifier.</param>
        /// <param name="quantity">The quantity.</param>
        public CustomCommerceCartLine(string productCatalog, string productId, string variantId, uint quantity)
            : base(productCatalog, productId, variantId, quantity)
        {
        }

        /// <summary>
        /// Gets the description as a html string
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public List<MediaItem> Images
        {
            get
            {
                if (this._images != null)
                {
                    return _images;
                }

                this._images = new List<MediaItem>();

                var field = Properties["_product_Images"] as string;
                if (field == null)
                {
                    return this._images;
                }

                var imageIds = field.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (Item mediaItem in imageIds.Select(id => Context.Database.GetItem(id)))
                {
                    this._images.Add(mediaItem);
                }

                return this._images;
            }
        }
    }
}