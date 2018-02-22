using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoList.Models;
using System;

namespace ToDoList.Controllers
{
  public class ItemsController : Controller
  {

    [HttpGet("/categories/{categoryID}/items/new")]
    public ActionResult CreateItemForm(int categoryId)
    {
      Category foundCategory = Category.Find(categoryId);
      return View(foundCategory);

    }
    [Route("/items")]
    public ActionResult ItemIndex()
    {
      return View("/Items/Index", Item.GetAll());
    }

    [HttpGet("/items/new")]
    public ActionResult CreateForm()
    {
      return View();
    }

    [HttpPost("/items")]
    public ActionResult Create()
    {
      Item newItem = new Item(Request.Form["new-item"], Request.Form["raw-date"]);
      newItem.Save();
      List<Item> allItems = Item.GetAll();
      return View("Index", allItems);
    }

    [HttpGet("/items/{id}")]
    public ActionResult Detail(int id)
    {
      Item item = Item.Find(id);
      return View(item);
    }


    [HttpGet("/items/{id}/update")]
    public ActionResult UpdateForm(int id)
    {
      Item thisItem = Item.Find(id);
      return View(thisItem);
    }

    [HttpPost("/items/{id}/update")]
    public ActionResult UpdateItem(int id)
    {
      Item thisItem = Item.Find(id);
      thisItem.Edit(Request.Form["newname"]);
      return RedirectToAction("Detail", "categories", new {Id = thisItem.GetCategoryId()});
    }

    [HttpGet("/items/{id}/delete")]
    public ActionResult Delete(int id)
    {
      Item thisItem = Item.Find(id);
      //int catId = thisItem.GetCategoryId();
      thisItem.Delete();
      return RedirectToAction("Detail", "categories", new {Id = thisItem.GetCategoryId()});
    }
  }
}
