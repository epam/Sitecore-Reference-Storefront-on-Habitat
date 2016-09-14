namespace Sitecore.Foundation.Indexing.Models
{
  using System.Collections.Generic;

  public class SearchResults : ISearchResults
  {
    public IEnumerable<ISearchResult> Results { get; set; }
    public int TotalNumberOfResults { get; set; }
    public IQuery Query { get; set; }
  }
}