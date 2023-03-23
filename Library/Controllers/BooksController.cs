using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
// using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Library.Controllers
{
  [Authorize]
  public class BooksController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public BooksController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {
      // string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      // ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      // List<Book> userBooks = _db.Books
      //                     .Where(entry => entry.User.Id == currentUser.Id)
      //                     .Include(book => book.Catalog)
      //                     .ToList();
      return View(_db.Books.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.CatalogId = new SelectList(_db.Catalogs, "CatalogId", "CatalogName");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Book thisBook, int CatalogId, int Copies)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.CatalogId = new SelectList(_db.Catalogs, "CatalogId", "CatalogName");
        return View(thisBook);
      }
      else
      {
        thisBook.MaxCopies = Copies; 
        _db.Books.Add(thisBook);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      Book thisBook = _db.Books
          .Include(book => book.Catalog)
          .Include(book => book.JoinEntities)
          .ThenInclude(join => join.Author)
          .FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    public ActionResult Edit(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      ViewBag.CatalogId = new SelectList(_db.Catalogs, "CatalogId", "CatalogName");
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult Edit(Book book, int MaxCopies)
    {
      book.Copies = MaxCopies;
      _db.Books.Update(book);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      _db.Books.Remove(thisBook);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddAuthor(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(books => books.BookId == id);
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "AuthorName");
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult AddAuthor(Book book, int authorId)
    {
      #nullable enable
      AuthorBook? joinEntity = _db.AuthorBooks.FirstOrDefault(join => (join.AuthorId == authorId && join.BookId == book.BookId));
      #nullable disable
      if (joinEntity == null && authorId != 0)
      {
        _db.AuthorBooks.Add(new AuthorBook() { AuthorId = authorId, BookId = book.BookId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = book.BookId });
    }   

    [Authorize]
    public ActionResult Checkout(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(model => model.BookId == id);
      return View(thisBook);
    }

    [HttpPost]
    public async Task<ActionResult> Checkout(Book book)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      // book.Copies = book.Copies - 1;
      #nullable enable
      UserBook? joinEntity = _db.UserBooks.FirstOrDefault(join => (join.UserId == currentUser.UserName && join.BookId == book.BookId));
      #nullable disable
      if (joinEntity == null)
      {
        Book thisBook = _db.Books.FirstOrDefault(model => model.BookId == book.BookId);
        thisBook.Copies = thisBook.Copies - 1;
        _db.Books.Update(thisBook);
        _db.UserBooks.Add(new UserBook() { UserId = currentUser.UserName, BookId = book.BookId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = book.BookId });
    }


    public ActionResult Error(Error error)
    {
      return View(error);
    }

    public ActionResult AddCopy(int id)
    {
      Book thisbook = _db.Books.FirstOrDefault(model => model.BookId == id);
      return View(thisbook);
    }

    [HttpPost]
    public ActionResult AddCopy(Book book, int num)
    {
      if (num == 0)
      {
        Error error = new Error {};
        string errorMessage = "Input Needs To Be A NUMBER Greater Than 0 If You Want To Add Copies!";
        ViewBag.errorMessage1 = errorMessage;
        error.ErrorMessage = errorMessage;
        error.StoredId = book.BookId;
        return RedirectToAction("Error", error);
      }
      Book thisbook = _db.Books.FirstOrDefault(model => model.BookId == book.BookId);
      thisbook.Copies += num;
      thisbook.MaxCopies += num;
      _db.Update(thisbook);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = thisbook.BookId });
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      AuthorBook joinEntry = _db.AuthorBooks.FirstOrDefault(entry => entry.AuthorBookId == joinId);
      int bookId = joinEntry.BookId;
      _db.AuthorBooks.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new{ id = bookId});
    } 
  }
}