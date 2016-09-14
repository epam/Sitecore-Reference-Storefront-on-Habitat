namespace Sitecore.Feature.Catalog.Models
{
    using System;
    using System.Collections.Generic;
    using Sitecore.Foundation.CommerceServer.Models;

    /// <summary>
    /// Enumeration of possible comparison properties
    /// </summary>
    public enum VariantPropertiesComparisonProperty
    {
        /// <summary>
        /// Property representing the product's color
        /// </summary>
        ProductColor,

        /// <summary>
        /// Property representing the product's size
        /// </summary>
        Size
    }

    /// <summary>
    /// EqualityComparer class to compare <see cref="VariantViewModel"/> specific properties
    /// </summary>
    public class VariantPropertiesEqualityComparer : IEqualityComparer<VariantViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the VariantPropertiesEqualityComparer class taking in the <see cref="VariantPropertiesComparisonProperty"/> property representing what to compare
        /// </summary>
        /// <param name="comparisonProperty">The property to use in the comparison</param>
        public VariantPropertiesEqualityComparer(VariantPropertiesComparisonProperty comparisonProperty)
        {
            this.ComparisonProperty = comparisonProperty;
        }

        /// <summary>
        /// Gets or sets the <see cref="VariantPropertiesComparisonProperty"/> to determine which property to compare
        /// </summary>
        protected VariantPropertiesComparisonProperty ComparisonProperty { get; set; }

        /// <summary>
        /// Equality method based on <see cref="VariantPropertiesComparisonProperty"/>
        /// </summary>
        /// <param name="x">The first variant to compare</param>
        /// <param name="y">The second variant to compare</param>
        /// <returns>True if equal, false if not </returns>
        public bool Equals(VariantViewModel x, VariantViewModel y)
        {
            var areEqual = false;
            switch (this.ComparisonProperty)
            {
                case VariantPropertiesComparisonProperty.ProductColor:
                    {
                        areEqual = x.ProductColor.Equals(y.ProductColor, StringComparison.OrdinalIgnoreCase);
                        break;
                    }

                case VariantPropertiesComparisonProperty.Size:
                    {
                        areEqual = x.Size.Equals(y.Size, StringComparison.OrdinalIgnoreCase);
                        break;
                    }

                default:
                    {
                        areEqual = (x.Size.Equals(y.Size, StringComparison.OrdinalIgnoreCase)) &&
                                   (x.ProductColor.Equals(y.ProductColor, StringComparison.OrdinalIgnoreCase));
                        break;
                    }
            }

            return areEqual;
        }

        /// <summary>
        /// Returns the hashcode of the property specified in <see cref="VariantPropertiesComparisonProperty"/>
        /// </summary>
        /// <param name="obj">The <see cref="VariantViewModel"/> to get hashcode from </param>
        /// <returns>The hashcode</returns>
        public int GetHashCode(VariantViewModel obj)
        {
            switch (this.ComparisonProperty)
            {
                case VariantPropertiesComparisonProperty.ProductColor:
                    {
                        return obj.ProductColor.GetHashCode();
                    }

                case VariantPropertiesComparisonProperty.Size:
                    {
                        return obj.Size.GetHashCode();
                    }

                default:
                    {
                        return obj.GetHashCode();
                    }
            }
        }
    }
}