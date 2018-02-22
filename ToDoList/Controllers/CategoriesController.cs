using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Controllers
{
  public class CategoriesController : Controller
  {
    [Route("/")]
    public ActionResult Index()
    {
      List<Category> allCats = Category.GetAll();
      return View("Index", allCats);
    }

    [HttpGet("/categories/new")]
    public ActionResult CreateCatForm()
    {
      return View();
    }

    [HttpPost("/categories")]
    public ActionResult CreateCategory()
    {
      Category newCategory = new Category(Request.Form["name"]);
      newCategory.Save();
      return RedirectToAction("Index");
    }
  }
}
