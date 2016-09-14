namespace Sitecore.Feature.Account.Tests
{
    using Ploeh.AutoFixture;
    using Sitecore.FakeDb;
    using Sitecore.Foundation.Testing.Attributes;

    public class AutoProfileDbDataAttribute : AutoDbDataAttribute
    {
        public AutoProfileDbDataAttribute()
        {
            var db = Fixture.Create<Db>();
            db.Add(new DbItem(Templates.SiteName.Name, Templates.SiteName.ID)
                    {
                        new DbItem(Templates.Global.Name, Templates.Global.ID)
                    });
        }
    }
}
