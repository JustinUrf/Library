using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
  public class Book
  {
    public int BookId { get; set; }
    [Required(ErrorMessage = "The book's synopsis can't be empty!")]
    public string Synopsis { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "You must add your book to a Catalog.")]
    public int CatalogId { get; set; }
    public List<AuthorBook> JoinEtntites { get;}
    public ApplicationUser User { get; set; }
    
  }
}