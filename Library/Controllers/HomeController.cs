using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace Library.Controllers
{
  public class HomeController: Controller
  {
    private readonly LibraryContext _db;

    public HomeController(LibraryContext db)
    {
      _db = db;
    }

    [HttpGet("/")]
    public ActionResult Index()
    {
      Author[] theseAuthors = _db.Authors.ToArray();
      Catalog[] theseCatalogs = _db.Catalogs.ToArray();
      Book[] theseBooks = _db.Books.ToArray();
      Dictionary<string,object[]> model = new Dictionary<string, object[]>();
      model.Add("catalogs", theseCatalogs);
      model.Add("books", theseBooks);
      model.Add("authors", theseAuthors);
      return View(model);
    }
  }
}