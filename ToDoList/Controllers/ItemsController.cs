using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System;
using System.IO;

namespace ToDoList.Controllers
{
  public class ItemsController : Controller
  {
      [HttpGet("/items")]
     public ActionResult Index()
     {
       List<Item> allItems = Item.GetAll();
       return View(allItems);
     }

      [HttpGet("/items/new")]
    public ActionResult New()
    {
      return View();
    }

      [HttpPost("/items")]
    public ActionResult Create(string itemDescription)
    {
      Item newItem = new Item(itemDescription);
      newItem.Save();
      List<Item> allItems = Item.GetAll();
      return RedirectToAction("Index", allItems);
    }

    [HttpGet("/items/{id}")]
    public ActionResult Show(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Item selectedItem = Item.Find(id);
      List<Category> itemCategories = selectedItem.GetCategories();
      List<Category> allCategories = Category.GetAll();
      model.Add("selectedItem", selectedItem);
      model.Add("itemCategories", itemCategories);
      model.Add("allCategories", allCategories);
      return View(model);
    }

    [HttpGet("/items/delete")]
    public ActionResult DeleteAll()
    {
      Item.ClearAll();
      List<Item> allItems = Item.GetAll();
      return RedirectToAction("Index", allItems);
    }

    [HttpGet("/items/{id}/edit")]
    public ActionResult Edit(int id, int categoryId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Console.WriteLine(categoryId);
      Category category = Category.Find(categoryId);
      model.Add("itemCategories", category);
      Item selectedItem = Item.Find(id);
      model.Add("selectedItem", selectedItem);
      return View(model);
    }

    [HttpPost("/items/{id}")]
    public ActionResult Update(int id, string newDescription)
    {
      Item selectedItem = Item.Find(id);
      Dictionary<string, object> model = new Dictionary<string, object>();
      selectedItem.Edit(newDescription);
      List<Category> itemCategories = selectedItem.GetCategories();
      List<Category> allCategories = Category.GetAll();
      model.Add("itemCategories", itemCategories);
      model.Add("selectedItem", selectedItem);
      model.Add("allCategories", allCategories);
      return View("Show", model);
    }

    [HttpPost("/items/{itemId}/categories/new")]
     public ActionResult AddCategory(int itemId, int categoryId)
     {
       Item item = Item.Find(itemId);
       Category category = Category.Find(categoryId);
       item.AddCategory(category);
       return RedirectToAction("Show",  new { id = itemId });
     }
  }
}
