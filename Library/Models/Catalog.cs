using System.Collections.Generic;

namespace Library.Models
{
  public class Catalog
  {
    public int CatalogId { get; set; }
    public string CatalogName { get; set; }
    public List<Book> Books { get; set; }
  }
}