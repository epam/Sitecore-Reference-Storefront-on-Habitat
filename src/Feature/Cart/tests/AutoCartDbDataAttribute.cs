using Ploeh.AutoFixture;
using Sitecore.Data;
using Sitecore.FakeDb;
using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
using Sitecore.Foundation.Testing.Attributes;
using CommerceTemplates = Sitecore.Foundation.CommerceServer.Templates;

namespace Sitecore.Feature.Cart.Tests
{
    public class AutoCartDbDataAttribute : AutoDbDataAttribute
    {
        public AutoCartDbDataAttribute()
        {
            var db = Fixture.Create<Db>();
            var homeItem = new DbItem("Habitat/Home");
            var currencyItemId = new ID();
            homeItem.Fields.Add(new DbField("Default Currency", CommerceTemplates.CommerceStorefront.Fields.DefaultCurrency)
            {
                Value = currencyItemId.ToString()
            });

            var globalItem = new DbItem("Habitat/Global");

            db.Add(homeItem);
            db.Add(globalItem);
            db.Add(new DbItem("Default Currency", currencyItemId));
            db.Add(new DbItem("Commerce/Storefront Configuration")
            {
                ParentID = ItemIDs.RootID,
                Children =
                {
                    new DbItem(StorefrontConstants.KnowItemNames.Currencies)
                    {
                        Children = { new DbItem( "Default Currency") }
                    }
                }
            });
        }
    }
}