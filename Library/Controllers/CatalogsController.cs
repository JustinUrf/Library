using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using Library.Models;
using System.Linq;

namespace Library.Controllers
{
  public class CatalogsController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public CatalogsController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {
      return View(_db.Catalogs.ToList());
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Catalog catalog)
    {
      _db.Catalogs.Add(catalog);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Catalog thisCatalog = _db.Catalogs
                            .Include(books => books.Books)
                            .ThenInclude(book => book.JoinEntities)
                            .ThenInclude(join => join.Author)
                            .FirstOrDefault(catalog => catalog.CatalogId == id);
      return View(thisCatalog);
    }

    public ActionResult Edit(int id)
    {
      Catalog thisCatalog = _db.Catalogs.FirstOrDefault(catalog => catalog.CatalogId == id);
      return View(thisCatalog);
    }

    [HttpPost]
    public ActionResult Edit(Catalog catalog)
    {
      _db.Catalogs.Update(catalog);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Catalog thisCatalog = _db.Catalogs.FirstOrDefault(catalog => catalog.CatalogId == id);
      return View(thisCatalog);
    }
  }
}