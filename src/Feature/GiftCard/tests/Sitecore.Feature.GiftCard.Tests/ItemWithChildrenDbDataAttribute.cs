using Ploeh.AutoFixture;
using Sitecore.FakeDb;
using Sitecore.Foundation.Testing.Attributes;

namespace Sitecore.Feature.GiftCard.Tests
{
    /// <summary>
    /// The attribute allows to build a sitecore item with two children.
    /// </summary>
    public class ItemWithChildrenDbDataAttribute : AutoDbDataAttribute
    {
        public ItemWithChildrenDbDataAttribute(string parentItemId = GiftCardTestsConstants.ItemWithChildrenParentItemName)
        {
            var db = Fixture.Create<Db>();
            db.Add(new DbItem(parentItemId, GiftCardTestsConstants.ItemWithChildrenParentItemId)
                    {
                        new DbItem(GiftCardTestsConstants.ItemWithChildrenFirstChildItemName, GiftCardTestsConstants.ItemWithChildrenFirstChildItemId),
                        new DbItem(GiftCardTestsConstants.ItemWithChildrenSecondChildItemName, GiftCardTestsConstants.ItemWithChildrenSecondChildItemId)
                    });
        }
    }
}
