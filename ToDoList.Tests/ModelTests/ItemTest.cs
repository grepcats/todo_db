using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ToDoList.Models;
using System;

namespace ToDoList.Tests
{
  [TestClass]
  public class ItemTests : IDisposable
  {
    public void Dispose()
    {
      Item.DeleteAll();
    }

    public ItemTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
    }

    [TestMethod]
    public void GetDescription_FetchTheDescription_String()
    {
      //arrange
      string controlDesc = "Go to store";
      Item newItem = new Item("Go to store");

      //act
      string result = newItem.GetDescription();

      //assert
      Assert.AreEqual(result, controlDesc);
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Item.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Save_SavesToDatabase_ItemList()
    {
      //arrange
      Item testItem = new Item("Go to store");

      //act
      int result = Item.GetAll().Count;

      //assert
      Assert.AreEqual(0, result);
    }
  }
}
