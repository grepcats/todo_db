using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ToDoList.Models;
using System;

namespace ToDoList.Tests
{
  [TestClass]
  public class ItemTest
  {
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
  }
}
