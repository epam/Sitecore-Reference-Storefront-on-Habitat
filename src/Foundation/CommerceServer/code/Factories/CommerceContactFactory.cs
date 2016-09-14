namespace Sitecore.Foundation.CommerceServer.Factories
{
    using Sitecore.Analytics.Tracking;
    using Sitecore.Commerce.Contacts;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    public class CommerceContactFactory : IContactFactory
    {
        private readonly ContactFactory _contactFactory;

        public CommerceContactFactory()
        {
            _contactFactory = new ContactFactory();
        }
        public string GetUserId(Contact contact)
        {
            return _contactFactory.GetUserId(contact);
        }

        public string GetUserId()
        {
            return _contactFactory.GetUserId();
        }

        public string GetContact()
        {
            return _contactFactory.GetContact();
        }
    }
}
