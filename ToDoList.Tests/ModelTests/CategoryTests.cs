using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ToDoList.Models;
using System;

namespace ToDoList.Tests
{
  public class CategoryTests : IDisposable
  {
    public void Dispose()
    {
      Item.DeleteAll();
      // Category.DeleteAll();
    }

    public CategoryTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
    }

    [TestMethod]
    public void GetItems_RetrievesAllItemsWithCategoryId_ItemList()
    {
      Category testCategory = new Category("Household Chores");
      testCategory.Save();

      Item firstItem = new Item("Mow the lawn", testCategory.GetId(), "2008-01-01");
      firstItem.Save();
      Item secondItem = new Item("Do the dishes", testCategory.GetId(), "2008-01-01");
      secondItem.Save();

      List<Item> testItemList = new List<Item> {firstItem, secondItem};
      List<Item> resultItemList = testCategory.GetItems();

      CollectionAssert.AreEqual(testItemList, resultItemList);
    }

    //write test for Save method also write Save method
  }
}
