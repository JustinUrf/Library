namespace Library.Models
{
  public class UserBook
  {
    public int UserBookId { get; set; }
    // public ApplicationUser User { get; set; }
    public string UserId { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }

  }
}