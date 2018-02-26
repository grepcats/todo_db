using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ToDoList.Models;
using System;

namespace ToDoList.Tests
{
  [TestClass]
  public class CategoryTests : IDisposable
  {
    public void Dispose()
    {
      Item.DeleteAll();
      Category.DeleteAll();
    }

    public CategoryTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
    }

    // [TestMethod]
    // public void GetItems_RetrievesAllItemsWithCategoryId_ItemList()
    // {
    //   Category testCategory = new Category("Household Chores", 1);
    //   testCategory.Save();
    //
    //   Item firstItem = new Item("Mow the lawn", "2008-01-01", 1, testCategory.GetId());
    //   firstItem.Save();
    //   Item secondItem = new Item("Do the dishes", "2008-01-01", 2,  testCategory.GetId());
    //   secondItem.Save();
    //
    //   List<Item> testItemList = new List<Item> {firstItem, secondItem};
    //   List<Item> resultItemList = testCategory.GetItems();
    //
    //   CollectionAssert.AreEqual(testItemList, resultItemList);
    // }

    [TestMethod]
    public void GetAll_CategoriesEmptyAtFirst_0()
    {
      //arrange, act
      int result = Category.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueForSameName_Category()
    {
      //Arrange, Act
      Category firstCategory = new Category("Household Chores");
      Category secondCategory = new Category("Household Chores");

      //assert
      Assert.AreEqual(firstCategory, secondCategory);
    }

    [TestMethod]
    public void Save_SavesCategoryToDatabase_CategoryList()
    {
      //arrange
      Category testCategory = new Category("Household Chores");
      testCategory.Save();

      //act
      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};

      //assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_DatabaseAssignsIdToCategory_Id()
    {
      //arrange
      Category testCategory = new Category("Household Chores");
      testCategory.Save();

      //act
      Category savedCategory = Category.GetAll()[0];

      int result = savedCategory.GetId();
      int testId = testCategory.GetId();

      //assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsCategoryInDatabase_Category()
    {
      //arrange
      Category testCategory = new Category("Household Chores");
      testCategory.Save();

      //act
      Category foundCategory = Category.Find(testCategory.GetId());

      //assert
      Assert.AreEqual(testCategory, foundCategory);
    }

    [TestMethod]
    public void Test_AddItem_AddsItemToCatetory()
    {
        //arrange
        Category testCategory = new Category("Household chores");
        testCategory.Save();

        Item testItem = new Item("Mow the lawn", "2008-01-01");
        testItem.Save();

        Item testItem2 = new Item("Water the garden", "2008-01-01");
        testItem2.Save();

        //act
        testCategory.AddItem(testItem);
        testCategory.AddItem(testItem2);

        List<Item> result = testCategory.GetItems();
        List<Item> testList = new List<Item>{testItem, testItem2};

        //assert
        CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetItems_ReturnsAllCategoryItems_ItemList()
    {
        //arrange
        Category testCategory = new Category("Household chores");
        testCategory.Save();

        Item testItem1 = new Item("Mow the lawn", "2008-01-01");
        testItem1.Save();

        Item testItem2 = new Item("Water the garden", "2008-01-01");
        testItem2.Save();

        //Act
        testCategory.AddItem(testItem1);
        List<Item> savedItems = testCategory.GetItems();
        List<Item> testList = new List<Item> {testItem1};

        //assert
        CollectionAssert.AreEqual(testList, savedItems);
    }

    [TestMethod]
    public void Delete_DeletesCategoryAssociationsFromDatabase_CategoryList()
    {
        //Arrange
        Item testItem = new Item("Mow the lawn", "2007-01-01");
        testItem.Save();

        string testName = "Home stuff";
        Category testCategory = new Category(testName);
        testCategory.Save();

        //Act
        testCategory.AddItem(testItem);
        testCategory.Delete();

        List<Category> resultItemCategories = testItem.GetCategories();
        List<Category> testItemCategories = new List<Category>{};

        //Assert
        CollectionAssert.AreEqual(testItemCategories, resultItemCategories);
    }

    // [TestMethod]
    // public void Delete_DeleteCategoryFromDatabase_Void()
    // {
    //   //arrange
    //   Category testCategory1 = new Category("Dog Stuff");
    //   testCategory1.Save();
    //   List<Category> originalList = Category.GetAll(); //should be 1 item
    //   Category testCategory2 = new Category("Cat Stuff");
    //   testCategory2.Save();
    //
    //   //act
    //   testCategory2.Delete();
    //   List<Category> newList = Category.GetAll(); //should be 1 item after deleting testCategory2
    //
    //   //assert
    //   CollectionAssert.AreEqual(originalList, newList);
    // }

    // [TestMethod]
    // public void Delete_DeleteCategoryANDItemsFromDB_Void()
    // {
    //   //arrange
    //   Category testCategory1 = new Category("Dog Stuff", 1);
    //   testCategory1.Save();
    //   Item testItem1 = new Item("Pet dog", "2008-01-01", 1, 1);
    //   Item testItem2 = new Item("Walk dog", "2008-01-01", 2, 1);
    //   testItem1.Save();
    //   testItem2.Save();
    //
    //   Category testCategory2 = new Category("Cat Stuff", 2);
    //   testCategory2.Save();
    //   Item testItem3 = new Item("Pet cat", "2008-01-01", 3, 2);
    //   Item testItem4 = new Item("Walk cat", "2008-01-01", 4, 2);
    //   testItem3.Save();
    //   testItem4.Save();
    //
    //   int numExistingItemsControl = 2;
    //
    //   //act
    //   testCategory1.Delete();
    //   int result = Item.GetAll().Count;
    //
    //   //assert
    //   Assert.AreEqual(numExistingItemsControl, result);
    // }
  }
}
