namespace Sitecore.Foundation.CommerceServer.Interfaces
{
    using Analytics.Tracking;

    public interface IContactFactory
    {
        string GetUserId(Contact contact);

        string GetUserId();

        string GetContact();
    }
}
