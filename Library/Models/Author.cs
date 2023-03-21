using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace Library.Models
{
  public class Author
  {
    public int AuthorId { get; set; }
    public string AuthorName { get; set; }
    public DateTime AuthorBirthDay { get; set; }
    public List<AuthorBook> JoinEntities { get; }
  }
}