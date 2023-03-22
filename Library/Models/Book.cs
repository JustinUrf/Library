using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
  public class Book
  {
    public int BookId { get; set; }
    [Required(ErrorMessage = "The book's synopsis can't be empty!")]
    public string BookName { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "You must add your book to a Catalog.")]
    public int Copies { get; set; }
    public int MaxCopies { get; set; }
    public int CatalogId { get; set; }
    public Catalog Catalog {get; set; }
    public List<AuthorBook> JoinEntities { get;}
    
  }
}