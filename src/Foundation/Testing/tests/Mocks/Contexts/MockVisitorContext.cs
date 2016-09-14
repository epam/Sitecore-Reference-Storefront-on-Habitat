namespace Sitecore.Foundation.Testing.Mocks.Contexts
{
    using Sitecore.Commerce.Contacts;
    using Sitecore.Commerce.Entities.Customers;
    using Sitecore.Foundation.CommerceServer.Interfaces;

    public class MockVisitorContext : IVisitorContext
    {
        public MockVisitorContext() { }

        public MockVisitorContext(string userId, string userName, string visitorId, IContactFactory contactFactory = null, CommerceUser commerceUser = null)
        {
            ContactFactory = contactFactory;
            UserId = userId;
            UserName = userName;
            CommerceUser = commerceUser;
            VisitorId = visitorId;
        }

        public IContactFactory ContactFactory { get; }

        public string UserId { get; }

        public string UserName { get; }

        public CommerceUser CommerceUser { get; }

        public string VisitorId { get; }

        public string GetCustomerId()
        {
            return null;
        }

        public void SetCommerceUser(CommerceUser user)
        {
        }
    }
}