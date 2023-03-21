using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Library.Models;
using System.Linq;

namespace Library.Controllers
{
  public class AuthorController : Controller
  {
    private readonly LibraryContext _db;
  
    public AuthorController(LibraryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Authors.ToList());
    }

    [Authorize]
    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Author author)
    {
      _db.Authors.Add(author);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      ViewBag.MachineId = new SelectList(_db.Books, "MachineId", "MachineName");
      Author thisAuthor = _db.Authors
                                  .Include(author => author.JoinEntities)
                                  .ThenInclude(join => join.Book)
                                  .FirstOrDefault(author => author.AuthorId == id);
      return View(thisAuthor);
    }

    [Authorize]
    public ActionResult AddBook(int id)
    {
      Author thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      ViewBag.MachineId = new SelectList(_db.Books, "BookId", "BookName");
      return View(thisAuthor);
    }

    [HttpPost]
    public ActionResult AddBook(Author author, int bookId)
    {
      #nullable enable
      AuthorBook? joinEntity = _db.AuthorBooks.FirstOrDefault(join => (join.BookId == bookId && join.AuthorId == author.AuthorId));
      #nullable disable
      if (joinEntity == null && bookId != 0)
      {
        _db.AuthorBooks.Add(new AuthorBook() { AuthorId = author.AuthorId, BookId = bookId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = author.AuthorId});

    }

    [Authorize]
    public ActionResult Edit(int id)
    {
      Author thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      return View(thisAuthor);
    }
    
    public ActionResult Edit(Author author)
    {
      _db.Authors.Update(author);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = author.AuthorId});
    }
    
    [Authorize]
    public ActionResult DeleteJoin(int joinId)
    {
      AuthorBook joinEntry = _db.AuthorBooks.FirstOrDefault(entry => entry.AuthorBookId == joinId);
      int engineerId = joinEntry.AuthorId;
      _db.AuthorBooks.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = engineerId});
    }

    [Authorize]
    public ActionResult Delete(int id)
    {
      Author thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      return View(thisAuthor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Author thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      _db.Authors.Remove(thisAuthor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
