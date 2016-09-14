namespace Sitecore.Foundation.CommerceServer.Infrastructure.Constants
{
    /// <summary>
    /// Defines the StorefrontConstants class.
    /// </summary>
    public static class CommerceServerStorefrontConstants
    {
        /// <summary>
        /// PipelineNames constants
        /// </summary>
        public static class PipelineNames
        {
            /// <summary>
            /// The translate entity to commerce profile pipeline name
            /// </summary>
            public const string TranslateEntityToCommerceAddressProfile = "translate.entityToCommerceAddressProfile";

            /// <summary>
            /// The translate commerce profile to entity pipeline name
            /// </summary>
            public const string TranslateCommerceAddressProfileToEntity = "translate.commerceAddressProfileToEntity";
        }

        /// <summary>
        /// Known storefront field names.
        /// </summary>
        public static class KnownFieldNames
        {
            /// <summary>
            /// The commerce server payment methods field.
            /// </summary>
            public const string CommerceServerPaymentMethods = "CS Payment Methods";

            /// <summary>
            /// The commerce server shipping methods field.
            /// </summary>
            public const string CommerceServerShippingMethods = "CS Shipping Methods";

            /// <summary>
            /// The country location path field
            /// </summary>
            public const string CountryLocationPath = "Country location path";

            /// <summary>
            /// The country name field.
            /// </summary>
            public const string CountryName = "Name";

            /// <summary>
            /// The country code field.
            /// </summary>
            public const string CountryCode = "Country Code";

            /// <summary>
            /// The payment option value field.
            /// </summary>
            public const string PaymentOptionValue = "Payment Option Value";

            /// <summary>
            /// The region name field.
            /// </summary>
            public const string RegionName = "Name";

            /// <summary>
            /// The shipping option value field.
            /// </summary>
            public const string ShippingOptionValue = "Shipping Option Value";

            /// <summary>
            /// The shipping options location path field.
            /// </summary>
            public const string ShippingOptionsLocationPath = "Shipping Options location path";

            /// <summary>
            /// The supports wishlists field.
            /// </summary>
            public const string SupportsWishLists = "Supports Wishlists";

            /// <summary>
            /// The supports loyalty program field.
            /// </summary>
            public const string SupportsLoyaltyProgram = "Supports Loyalty Program ";

            /// <summary>
            /// The supports girst card payment field.
            /// </summary>
            public const string SupportsGirstCardPayment = "Supports Girft Card Payment";

            /// <summary>
            /// The value field.
            /// </summary>
            public const string Value = "Value";
        }

        /// <summary>
        /// Used to hold some of the default settings for the site
        /// </summary>
        public static class CartConstants
        {
            /// <summary>
            /// Name of the Billing address prefix
            /// </summary>
            public const string BillingAddressNamePrefix = "Billing_";

            /// <summary>
            /// Name of the shipping address prefix
            /// </summary>
            public const string ShippingAddressNamePrefix = "Shipping_";

            /// <summary>
            /// The email address name prefix
            /// </summary>
            public const string EmailAddressNamePrefix = ShippingAddressNamePrefix + "Email_";
        }

        /// <summary>
        /// Defines the Know Commerce Server weakly typed properties.
        /// </summary>
        public static class KnowWeaklyTypesProperties
        {
            /// <summary>
            /// The email text property.
            /// </summary>
            public const string EmailText = "EmailText";

            /// <summary>
            /// The party type property.
            /// </summary>
            public const string PartyType = "PartyType";
        }
    }
}
