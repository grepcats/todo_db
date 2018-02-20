using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Controllers
{
  public class ItemsController : Controller
  {
    [Route("/")]
    public ActionResult Index()
    {
      return View("Index", Item.GetAll());
    }

    [HttpGet("/items/new")]
    public ActionResult CreateForm()
    {
      return View();
    }

    [HttpPost("/items")]
    public ActionResult Create()
    {
      Item newItem = new Item(Request.Form["new-item"]);
      List<Item> allItems = Item.GetAll();
      return View("Index", allItems);
    }

    // [HttpGet("/items/{id}")]
    // public ActionResult Detail(int id)
    // {
    //   Item item = Item.Find(id - 1);
    //   return View(item);
    // }
  }
}
