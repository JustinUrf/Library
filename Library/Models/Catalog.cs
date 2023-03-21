using System.Collections.Generic;

namespace Library.Models
{
  public class Catalog
  {
    public int CategoryId { get; set; }
    public string CatalogueName { get; set; }
    public List<Book> Books { get; set; }
  }
}