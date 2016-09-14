using CommerceServer.Core.Catalog;

namespace Sitecore.Foundation.CommerceServer.Search.ComputedFields
{
    using Newtonsoft.Json;
    using Sitecore.Commerce.Connect.CommerceServer;
    using Sitecore.Commerce.Connect.CommerceServer.Catalog;
    using Sitecore.Commerce.Connect.CommerceServer.Search.ComputedFields;
    using Sitecore.Data;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.CommerceServer.Utils;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Defines the VariantInfoComputedField class.
    /// </summary>
    public class VariantInfoComputedField : BaseCommerceVariants<string>
    {
        /// <summary>
        /// Computes the value.
        /// </summary>
        /// <param name="indexable">The indexable.</param>
        /// <returns>The computed value.  In this case we return serialized JSON.</returns>
        public override object ComputeValue(ContentSearch.IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "indexable");
            var validatedItem = GetValidatedItem(indexable);

            if (validatedItem == null)
            {
                return string.Empty;
            }

            List<VariantIndexInfo> variantInfoList = new List<VariantIndexInfo>();

            foreach (var productVariant in this.GetChildVariantsReadOnly(validatedItem.ID, validatedItem.Language.Name))
            {
                var variantInfo = new VariantIndexInfo();

                variantInfo.VariantId = this.GetVariantFieldValue<string>(productVariant, "VariantId");
                variantInfo.BasePrice = this.GetVariantFieldValue<decimal>(productVariant, "BasePriceVariant");
                variantInfo.ListPrice = productVariant.ListPrice;

                variantInfoList.Add(variantInfo);
            }

            return JsonConvert.SerializeObject(variantInfoList);
        }

        /// <summary>
        /// Gets the variant field value.
        /// </summary>
        /// <typeparam name="T">Type of the property to return.</typeparam>
        /// <param name="variant">The variant.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The request value.</returns>
        protected virtual T GetVariantFieldValue<T>(Variant variant, string fieldName)
        {
            if (variant.DataRow.Table.Columns.Contains(fieldName))
            {
                var variantValue = variant[fieldName];
                if (variantValue != null)
                {
                    if (variantValue is T)
                    {
                        return (T)variantValue;
                    }

                    return (T)Convert.ChangeType(variantValue, typeof(T), CultureInfo.InvariantCulture);
                }
            }

            return default(T);
        }

        /// <summary>
        /// Gets the child variants read only.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="language">The language.</param>
        /// <returns>Product variant collection.</returns>
        private IEnumerable<Variant> GetChildVariantsReadOnly(ID itemId, string language)
        {
            var catalogRepository = ContextTypeLoader.CreateInstance<ICatalogRepository>();
            var externalInfo = catalogRepository.GetExternalIdInformation(itemId.Guid);
            if (externalInfo != null && externalInfo.CommerceItemType == CommerceItemType.ProductFamily)
            {
                var culture = CommerceUtility.ConvertLanguageToCulture(language);
                var productFamily = catalogRepository.GetProductReadOnly(externalInfo.CatalogName, externalInfo.ProductId, culture) as ProductFamily;
                if (productFamily != null && productFamily.Variants.Count > 0)
                {
                    return productFamily.Variants;
                }
            }

            return new List<Variant>();
        }
    }
}
