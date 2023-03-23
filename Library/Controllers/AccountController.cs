using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using Library.ViewModels;

namespace Library.Controllers
{
  public class AccountController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, LibraryContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    { 
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<UserBook> usersBooks = _db.UserBooks
                                     .Where(entry => entry.UserId == currentUser.UserName)
                                     .Include(entry => entry.Book)
                                     .ToList();
      // List<Book> userBooks = _db.Books
      //                           .Include(book => book.JoinEntities2)
      //                           .ToList(); 
      // User newUser = new User {};
      // newUser.UserId = currentUser.UserName;
      // newUser.UserId = currentUser.UserName;
      // newUser.Books = userBooks;
      // ViewBag.thisUser = currentUser.UserName;
      // TempData["UserName"] = currentUser.UserName;
      return View(usersBooks);
    }
    








    //  Author thisAuthor = _db.Authors
    //                               .Include(author => author.JoinEntities)
    //                               .ThenInclude(join => join.Book)
    //                               .FirstOrDefault(author => author.AuthorId == id);
    //   return View(thisAuthor);

    // string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //   ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    //   List<Item> userItems = _db.Items
    //                       .Where(entry => entry.User.Id == currentUser.Id)
    //                       .Include(item => item.Category)
    //                       .ToList();
    //   return View(userItems);

    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register (RegisterViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      else
      {
        ApplicationUser user = new ApplicationUser { UserName = model.Email };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          foreach (IdentityError error in result.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
          return View(model);
        }
      }
    }

        public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      else
      {
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          ModelState.AddModelError("", "There is something wrong with your email or username. Please try again.");
          return View(model);
        }
      }
    }

    [HttpPost]
    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index");
    }
  }
}