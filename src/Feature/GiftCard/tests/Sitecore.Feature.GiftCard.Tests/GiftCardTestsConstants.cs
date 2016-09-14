using Sitecore.Data;

namespace Sitecore.Feature.GiftCard.Tests
{
    internal class GiftCardTestsConstants
    {
        public const int ItemWithChildrenNumberOfChildren = 2;

        public static readonly ID ItemWithChildrenParentItemId = new ID("{B617CB6F-42B1-439E-9773-EF504D26A169}");
        public const string ItemWithChildrenParentItemName = "ParentItemName";

        public static readonly ID ItemWithChildrenFirstChildItemId = new ID("{86D14AE5-14DE-4360-B06B-285157A76563}");
        public const string ItemWithChildrenFirstChildItemName = "FirstChildItemName";

        public static readonly ID ItemWithChildrenSecondChildItemId = new ID("{2C59FFC8-5A4D-42A2-A285-54751D0D7B67}");
        public const string ItemWithChildrenSecondChildItemName = "SecondChildItemName";
    }
}
