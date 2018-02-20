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
      Item newItem = new Item("Go to store", "2009-03-29");

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
      Item testItem = new Item("Mow the lawn", "2008-03-02");

      //act
      testItem.Save();
      List<Item> result = Item.GetAll();
      List<Item> testList = new List<Item>{testItem};

      //assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Item()
    {
      //Arrange, act
      Item firstItem = new Item("Mow the lawn", "2008-03-02");
      Item secondItem = new Item("Mow the lawn", "2008-03-02");

      //Assert
      Assert.AreEqual(firstItem, secondItem);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //arrange
      Item testItem = new Item("Mow the lawn", "2008-03-02");

      //act
      testItem.Save();
      Item savedItem = Item.GetAll()[0];

      int result = savedItem.GetId();
      int testId = testItem.GetId();

      //assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsItemInDatabase_Item()
    {
      //Arrange
      Item testItem = new Item("Mow the lawn", "2008-03-02");
      testItem.Save();

      //act
      Item foundItem = Item.Find(testItem.GetId());

      //assert
      Assert.AreEqual(testItem, foundItem);
    }

    [TestMethod]
    public void GetFormattedDate_FetchDate_Date()
    {
      //arrange
      Item testItem = new Item("Mow the lawn", "2008-03-02");
      DateTime controlDate = new DateTime(2008, 03, 02);

      //act
      testItem.SetDate();
      DateTime result = testItem.GetFormattedDate();

      //assert
      Assert.AreEqual(result, controlDate);
    }

  }
}
