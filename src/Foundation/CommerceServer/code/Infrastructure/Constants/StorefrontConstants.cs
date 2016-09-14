using System;

namespace Sitecore.Foundation.CommerceServer.Infrastructure.Constants
{
    using Sitecore.Data;

    /// <summary>
    /// Defines the StorefrontConstants class.
    /// </summary>
    public static class StorefrontConstants
    {
        /// <summary>
        /// ItemTypes enumerator.
        /// </summary>
        public enum ItemTypes
        {
            /// <summary>
            /// Unknown ItemType or null.
            /// </summary>
            Unknown = 0,

            /// <summary>
            /// Category ItemType.
            /// </summary>
            Category,

            /// <summary>
            /// NamedSearch Itemtype.
            /// </summary>
            NamedSearch,

            /// <summary>
            /// Product ItemType.
            /// </summary>
            Product,

            /// <summary>
            /// Secured page ItemType.
            /// </summary>
            SecuredPage,

            /// <summary>
            /// The SelectedProducts ItemType.
            /// </summary>
            SelectedProducts,

            /// <summary>
            /// Standard page ItemType.
            /// </summary>
            StandardPage,

            /// <summary>
            /// Variant ItemType.
            /// </summary>
            Variant
        }

        /// <summary>
        /// Storefront views.
        /// </summary>
        /// TODO:remove unused
        public static class Views
        {
            /// <summary>
            /// The empty view.
            /// </summary>
            public static readonly string Empty = "/Shared/Empty";
        }

        /// <summary>
        /// Defines the System Message constants.
        /// </summary>
        public static class SystemMessages
        {
            /// <summary>
            /// Authentication provider error message.
            /// </summary>
            public static readonly string AuthenticationProviderError = "AuthenticationProviderError";

            /// <summary>
            /// Cart not found error message.
            /// </summary>
            public static readonly string CartNotFoundError = "CartNotFoundError";

            /// <summary>
            /// Could not create user message.
            /// </summary>
            public static readonly string CouldNotCreateUser = "CouldNotCreatedUser";

            /// <summary>
            /// Could not find email body message error message.
            /// </summary>
            public static readonly string CouldNotFindEmailBodyMessageError = "CouldNotFindEmailBodyMessageError";

            /// <summary>
            /// Could not find email subject message error message.
            /// </summary>
            public static readonly string CouldNotFindEmailSubjectMessageError = "CouldNotFindEmailSubjectMessageError";

            /// <summary>
            /// Could not load template message error message.
            /// </summary>
            public static readonly string CouldNotLoadTemplateMessageError = "CouldNotLoadTemplateMessageError";

            /// <summary>
            /// Could not send mail message error message.
            /// </summary>
            public static readonly string CouldNotSendMailMessageError = "CouldNotSendMailMessageError";

            /// <summary>
            /// Could not sent email error message.
            /// </summary>
            public static readonly string CouldNotSentEmailError = "CouldNotSentEmailError";

            /// <summary>
            /// Invalid email error message.
            /// </summary>
            public static readonly string InvalidEmailError = "InvalidEmailError";

            /// <summary>
            /// Invalid password error message.
            /// </summary>
            public static readonly string InvalidPasswordError = "InvalidPasswordError";

            /// <summary>
            /// Mail sent to message message.
            /// </summary>
            public static readonly string MailSentToMessage = "MailSentToMessage";

            /// <summary>
            /// Maximum addresse limit reached message.
            /// </summary>
            public static readonly string MaxAddressLimitReached = "MaxAddresseLimitReached";

            /// <summary>
            /// Maximum loyalty programs to join reached message.
            /// </summary>
            public static readonly string MaxLoyaltyProgramsToJoinReached = "MaxLoyaltyProgramsToJoinReached";

            /// <summary>
            /// Maximum wish list line limit reached message.
            /// </summary>
            public static readonly string MaxWishListLineLimitReached = "MaxWishListLineLimitReached";

            /// <summary>
            /// Maximum wish list limit reached message.
            /// </summary>
            public static readonly string MaxWishListLimitReached = "MaxWishListLimitReached";

            /// <summary>
            /// Password could not be reset message.
            /// </summary>
            public static readonly string PasswordCouldNotBeReset = "PasswordCouldNotBeReset";

            /// <summary>
            /// Password retrieval answer invalid message.
            /// </summary>
            public static readonly string PasswordRetrievalAnswerInvalid = "PasswordRetrievalAnswerInvalid";

            /// <summary>
            /// Password retrieval question invalid message.
            /// </summary>
            public static readonly string PasswordRetrievalQuestionInvalid = "PasswordRetrievalQuestionInvalid";

            /// <summary>
            /// Submit order has empty cart message.
            /// </summary>
            public static readonly string SubmitOrderHasEmptyCart = "SubmitOrderHasEmptyCart";

            /// <summary>
            /// Tracking not enabled message.
            /// </summary>
            public static readonly string TrackingNotEnabled = "TrackingNotEnabled";

            /// <summary>
            /// Unknown membership provider error message.
            /// </summary>
            public static readonly string UnknownMembershipProviderError = "UnknownMembershipProviderError";

            /// <summary>
            /// Update user profile error message.
            /// </summary>
            public static readonly string UpdateUserProfileError = "UpdateUserProfileError";

            /// <summary>
            /// User already exists message.
            /// </summary>
            public static readonly string UserAlreadyExists = "UserAlreadyExists";

            /// <summary>
            /// User name for email exists message.
            /// </summary>
            public static readonly string UserNameForEmailExists = "UserNameForEmailExists";

            /// <summary>
            /// User name invalid message.
            /// </summary>
            public static readonly string UserNameInvalid = "UserNameInvalid";

            /// <summary>
            /// User not found error message.
            /// </summary>
            public static readonly string UserNotFoundError = "UserNotFoundError";

            /// <summary>
            /// User rejected error message.
            /// </summary>
            public static readonly string UserRejectedError = "UserRejectedError";

            /// <summary>
            /// The default currency not set exception message.
            /// </summary>
            public static readonly string DefaultCurrencyNotSetException = "DefaultCurrencyNotSetException";

            /// <summary>
            /// The invalid currency error message.
            /// </summary>
            public static readonly string InvalidCurrencyError = "InvalidCurrencyError";
        }

        /// <summary>
        /// Used to hold some of the default settings for the site
        /// </summary>
        public static class Settings
        {
            /// <summary>
            /// The default site name
            /// </summary>
            public static readonly string WebsiteName = "Storefront";

            /// <summary>
            /// The default number of items per page
            /// </summary>
            public static readonly int DefaultItemsPerPage = 12;

            /// <summary>
            /// The default currency to be applied.  This is temporary until the multi-currency support is integrated in all facets of the sytem.
            /// </summary>
            public static readonly string DefaultCurrencyCode = "USD";
        }

        /// <summary>
        /// Contains Engagement Plan creation information.  The following Ids and names will be used to create the engagment plans for the site
        /// </summary>
        /// TODO:remove unused
        public static class EngagementPlans
        {
            /// <summary>
            /// The Abandoned Carts engagement plan Id to use when creating the Abandon carts eaplan.
            /// </summary>
            public static readonly string AbandonedCartsEaPlanId = "{7138ACC1-329C-4070-86DD-6A53D6F57AC5}";

            /// <summary>
            /// The Abandoned Carts name
            /// </summary>
            public static readonly string AbandonedCartsEaPlanName = "Abandoned Carts";

            /// <summary>
            /// The New Order Placed engagement plan template Id.
            /// </summary>
            public static readonly string NewOrderPlacedEaPlanId = "{7CA697EA-5CCA-4B59-85A3-D048B285E6B4}";

            /// <summary>
            /// The New Order Placed name
            /// </summary>
            public static readonly string NewOrderPlacedEaPlanName = "New Order Placed";

            /// <summary>
            /// The Products Back In Stock engagement plan template Id.
            /// </summary>
            public static readonly string ProductsBackInStockEaPlanId = "{36B4083E-F7F7-4E60-A747-75DDBEC6BB4B}";

            /// <summary>
            /// The Products Back In Stock name
            /// </summary>
            public static readonly string ProductsBackInStockEaPlanName = "Products Back In Stock";

            /// <summary>
            /// The WishList Created engagement plan template Id.
            /// </summary>
            public static readonly string WishListCreatedEaPlanId = "{C6BD2A27-3528-4107-8764-DB010EA400FF}";

            /// <summary>
            /// The WishList Created engagement plan template Id.
            /// </summary>
            public static readonly string WishListCreatedEaPlanName = "Wish List Created";
        }

        /// <summary>
        /// Known storefront field names.
        /// </summary>
        public static class KnownFieldNames
        {
            /// <summary>
            /// The cancel field.
            /// </summary>
            public static readonly string Cancel = "Cancel";

            /// <summary>
            /// The create user field.
            /// </summary>
            public static readonly string CreateUser = "Create user";

            /// <summary>
            /// The customer message1 field.
            /// </summary>
            public static readonly string CustomerMessage1 = "Customer Message 1";

            /// <summary>
            /// The customer message2
            /// </summary>
            public static readonly string CustomerMessage2 = "Customer Message 2";

            /// <summary>
            /// The email
            /// </summary>
            public static readonly string Email = "Email";

            /// <summary>
            /// The email address placeholder field.
            /// </summary>
            public static readonly string EmailAddressPlaceholder = "Email Address Placeholder";

            /// <summary>
            /// The email missing message
            /// </summary>
            public static readonly string EmailMissingMessage = "Email Missing Message";

            /// <summary>
            /// The facebook button field.
            /// </summary>
            public static readonly string FacebookButton = "Facebook Button";

            /// <summary>
            /// The facebook text field.
            /// </summary>
            public static readonly string FacebookText = "Facebook Text";

            /// <summary>
            /// The first name
            /// </summary>
            public static readonly string FirstName = "First Name";

            /// <summary>
            /// The first name placeholder field.
            /// </summary>
            public static readonly string FirstNamePlaceholder = "First Name Placeholder";

            /// <summary>
            /// The fill form message
            /// </summary>
            public static readonly string FillFormMessage = "Fill Form Message";

            /// <summary>
            /// The guest checkout button
            /// </summary>
            public static readonly string GuestCheckoutButton = "Guest Checkout Button";

            /// <summary>
            /// The last name
            /// </summary>
            public static readonly string LastName = "Last Name";

            /// <summary>
            /// The last name placeholder field.
            /// </summary>
            public static readonly string LastNamePlaceholder = "Last Name Placeholder";

            /// <summary>
            /// The password
            /// </summary>
            public static readonly string Password = "Password";

            /// <summary>
            /// The passwords do not match message field.
            /// </summary>
            public static readonly string PasswordsDoNotMatchMessage = "Passwords Do Not Match Message";

            /// <summary>
            /// The password length message field.
            /// </summary>
            public static readonly string PasswordLengthMessage = "Password Length Message";

            /// <summary>
            /// The password missing message field.
            /// </summary>
            public static readonly string PasswordMissingMessage = "Password Missing Message";

            /// <summary>
            /// The password again field.
            /// </summary>
            public static readonly string PasswordAgain = "Password Again";

            /// <summary>
            /// The password placholder field.
            /// </summary>
            public static readonly string PasswordPlaceholder = "Password Placeholder";

            /// <summary>
            /// The registering
            /// </summary>
            public static readonly string Registering = "Registering";

            /// <summary>
            /// The sign in button field.
            /// </summary>
            public static readonly string SignInButton = "Sign In Button";

            /// <summary>
            /// The signing button
            /// </summary>
            public static readonly string SigningButton = "Signing Button";

            /// <summary>
            /// The body field.
            /// </summary>
            public static readonly string Body = "Body";

            /// <summary>
            /// The subject field.
            /// </summary>
            public static readonly string Subject = "Subject";

            /// <summary>
            /// The key field.
            /// </summary>
            public static readonly string Key = "Key";

            /// <summary>
            /// The value field.
            /// </summary>
            public static readonly string Value = "Value";

            /// <summary>
            /// The sender email address field.
            /// </summary>
            public static readonly string SenderEmailAddress = "Sender Email Address";

            /// <summary>
            /// The maximum number of addresses field.
            /// </summary>
            public static readonly string MaxNumberOfAddresses = "Max Number of Addresses";

            /// <summary>
            /// The maximum number of wishlists field.
            /// </summary>
            public static readonly string MaxNumberOfWishLists = "Max Number of WishLists";

            /// <summary>
            /// The maxnumber of wish list items field.
            /// </summary>
            public static readonly string MaxNumberOfWishListItems = "Max Number of WishList Items";

            /// <summary>
            /// The "Use index file for product status in lists" field.
            /// </summary>
            public static readonly string UseIndexFileForProductStatusInLists = "Use Index File For Product Status In Lists";

            /// <summary>
            /// The map key field.
            /// </summary>
            public static readonly string MapKey = "Map Key";

            /// <summary>
            /// The named searches field.
            /// </summary>
            [Obsolete]
            public static readonly string NamedSearches = "Named Searches";

            /// <summary>
            /// The title field.
            /// </summary>
            public static readonly string Title = "Title";

            /// <summary>
            /// The product list field.
            /// </summary>
            public static readonly string ProductList = "Product List";

            /// <summary>
            /// The currency description field.
            /// </summary>
            public static readonly string CurrencyDescription = "Currency Description";

            /// <summary>
            /// The currency symbol field.
            /// </summary>
            public static readonly string CurrencySymbol = "Currency Symbol";

            /// <summary>
            /// The currency symbol field.
            /// </summary>
            public static readonly string CurrencySymbolPosition = "Currency Symbol Position";

            /// <summary>
            /// The currency number format culture field.
            /// </summary>
            public static readonly string CurrencyNumberFormatCulture = "Currency Number Format Culture";

            /// <summary>
            /// The supported currencies field.
            /// </summary>
            public static readonly string SupportedCurrencies = "Supported Currencies";
        }

        /// <summary>
        /// Known template names.
        /// </summary>
        public static class KnownTemplateNames
        {
            /// <summary>
            /// The commerce named search template name.
            /// </summary>
            public static readonly string CommerceNamedSearch = "Commerce Named Search";

            /// <summary>
            /// The named search template name.
            /// </summary>
            public static readonly string NamedSearch = "Named Search";
        }

        /// <summary>
        /// Known template item IDs.
        /// </summary>
        public static class KnownTemplateItemIds
        {
            /// <summary>
            /// The ID of the Standard Page template.
            /// </summary>
            public static readonly ID StandardPage = new ID("{16E859D2-6542-407A-AC65-F34BCAD3EB3D}");

            /// <summary>
            /// The ID of the Secured Page template.
            /// </summary>
            public static readonly ID SecuredPage = new ID("{02CCCF95-7BE5-4549-81F9-AC97A22D6816}");
        }

        /// <summary>
        /// Known template names.
        /// </summary>
        public static class KnowItemNames
        {
            /// <summary>
            /// The mails template name
            /// </summary>
            public static readonly string Mails = "Mails";

            /// <summary>
            /// The lookups template name
            /// </summary>
            public static readonly string Lookups = "Lookups";

            /// <summary>
            /// The system messages template name
            /// </summary>
            public static readonly string SystemMessages = "System Messages";

            /// <summary>
            /// The inventory statuses template name
            /// </summary>
            public static readonly string InventoryStatuses = "Inventory Statuses";

            /// <summary>
            /// The relationships template name.
            /// </summary>
            public static readonly string Relationships = "Relationships";

            /// <summary>
            /// The order statuses template name.
            /// </summary>
            public static readonly string OrderStatuses = "Order Statuses";

            /// <summary>
            /// The currencies lookup folder.
            /// </summary>
            public static readonly string Currencies = "Currencies";

            /// <summary>
            /// The currency display folder.
            /// </summary>
            public static readonly string CurrencyDisplay = "Currency Display";
        }
        
        /// <summary>
        /// Used to store strings using in query strings
        /// </summary>
        public static class QueryStrings
        {
            /// <summary>
            /// User for the order confirmation id.
            /// </summary>
            public const string ConfirmationId = "confirmationId";

            /// <summary>
            /// Used for paging
            /// </summary>
            public const string Paging = "pg";

            /// <summary>
            /// Used for site content paging
            /// </summary>
            public const string SiteContentPaging = "scpg";

            /// <summary>
            /// Used for the sorting field
            /// </summary>
            public const string Sort = "s";

            /// <summary>
            /// Used for the sorting field direction
            /// </summary>
            public const string SortDirection = "sd";

            /// <summary>
            /// Used for facets
            /// </summary>
            public const string Facets = "f";

            /// <summary>
            /// Used for separating facets
            /// </summary>
            public const char FacetsSeparator = '|';

            /// <summary>
            /// Used for the search keyword
            /// </summary>
            public const string SearchKeyword = "q";

            /// <summary>
            /// Used for page size
            /// </summary>
            public const string PageSize = "ps";

            /// <summary>
            /// Used for site content page size.
            /// </summary>
            public const string SiteContentPageSize = "scps";
        }

        /// <summary>
        /// Contains the names of index fields.
        /// </summary>
        public static class KnownIndexFields
        {
            /// <summary>
            /// The name of the instocklocations index field.
            /// </summary>
            public static readonly string InStockLocations = "instocklocations";

            /// <summary>
            /// The name of the outofstocklocations index field.
            /// </summary>
            public static readonly string OutOfStockLocations = "outofstocklocations";

            /// <summary>
            /// The name of the orderablelocations index field.
            /// </summary>
            public static readonly string OrderableLocations = "orderablelocations";

            /// <summary>
            /// The name of the preorderable index field.
            /// </summary>
            public static readonly string PreOrderable = "preorderable";

            /// <summary>
            /// The child categories sequence index field.
            /// </summary>
            public static readonly string ChildCategoriesSequence = "childcategoriessequence";
        }

        /// <summary>
        /// Contains the names of Sitecore item fields.
        /// </summary>
        public static class ItemFields
        {
            /// <summary>
            /// The name of the DisplayInSearchResults field.
            /// </summary>
            public static readonly string DisplayInSearchResults = "DisplayInSearchResults";

            /// <summary>
            /// The name of the Title field.
            /// </summary>
            public static readonly string Title = "Title";

            /// <summary>
            /// The name of the SummaryText field.
            /// </summary>
            public static readonly string SummaryText = "SummaryText";
        }

        /// <summary>
        /// Contains style class names that are used in source code.
        /// </summary>
        public static class StyleClasses
        {
            /// <summary>
            /// The class to change product search results page size.
            /// </summary>
            public static readonly string ChangePageSize = "changePageSize";
        }
    }
}
