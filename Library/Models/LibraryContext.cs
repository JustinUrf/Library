using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Library.Models
{
  public class LibraryContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<UserBook> UserBooks { get; set; }
    public DbSet<AuthorBook> AuthorBooks { get; set; }
    public LibraryContext(DbContextOptions options) : base(options) { }
  }
}