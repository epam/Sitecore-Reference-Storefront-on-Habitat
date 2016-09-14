using ConnectQuerySort = Sitecore.Commerce.Connect.CommerceServer.Search.Models.CommerceQuerySort;

namespace Sitecore.Foundation.CommerceServer.Search.Models
{
    public class CommerceQuerySort : ConnectQuerySort
    {
        public CommerceQuerySort(ConnectQuerySort querySort)
        {
            Name = querySort.Name;
            DisplayName = querySort.DisplayName;
        }
    }
}