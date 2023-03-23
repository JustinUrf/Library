using System.Collections.Generic;

namespace Library.Models
{
  public class User
  {
    public string UserId { get; set; }
    public List<Book> Books { get; set; }
  }
}