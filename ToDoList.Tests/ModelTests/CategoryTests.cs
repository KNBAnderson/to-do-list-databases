using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;
using System.Collections.Generic;
using System;

namespace ToDoList.Tests
{
  [TestClass]
  public class CategoryTest : IDisposable
  {

    public void Dispose()
    {
      Category.ClearAll();
      Item.ClearAll();
    }

    [TestMethod]
    public void CategoryConstructor_CreatesInstanceOfCategory_Category()
    {
      Category newCategory = new Category("test category");
      Assert.AreEqual(typeof(Category), newCategory.GetType());
    }

    [TestMethod]
    public void GetName_ReturnsName_String()
    {
      //Arrange
      string name = "Test Category";
      Category newCategory = new Category(name);

      //Act
      string result = newCategory.GetName();

      //Assert
      Assert.AreEqual(name, result);
    }

    [TestMethod]
    public void GetId_ReturnsCategoryId_Int()
    {
      //Arrange
      string name = "Test Category";
      Category newCategory = new Category(name);

      //Act
      int result = newCategory.GetId();

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void GetAll_ReturnsAllCategoryObjects_CategoryList()
    {
      //Arrange
      string name01 = "Work";
      string name02 = "School";
      Category newCategory1 = new Category(name01);
      newCategory1.Save();
      Category newCategory2 = new Category(name02);
      newCategory2.Save();
      List<Category> newList = new List<Category> { newCategory1, newCategory2 };

      //Act
      List<Category> result = Category.GetAll();

      Console.WriteLine(result.Count);
      Console.WriteLine(newList.Count);
      //Assert
      // Assert.AreEqual(newList[1], result[1]);
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void Find_ReturnsCorrectCategory_Category()
    {
      //Arrange
      string name01 = "Work";
      Category newCategory1 = new Category(name01);
      newCategory1.Save();
      //Act
      Category result = Category.Find(newCategory1.GetId());

      //Assert
      Assert.AreEqual(newCategory1, result);
    }

    [TestMethod]
    public void GetItems_ReturnsAllCategoryItems_ItemList()
    {
      //Arrange
      DateTime newDateTime = new DateTime(2001);
      Category testCategory = new Category("Household chores");
      testCategory.Save();
      Item testItem1 = new Item("Mow the lawn", newDateTime);
      testItem1.Save();
      Item testItem2 = new Item("Buy plane ticket", newDateTime);
      testItem2.Save();

      //Act
      testCategory.AddItem(testItem1);
      testCategory.AddItem(testItem2);
      List<Item> savedItems = testCategory.GetItems();
      List<Item> testList = new List<Item> {testItem1, testItem2};
      Console.WriteLine(testList.Count);

      //Assert
      CollectionAssert.AreEqual(testList, savedItems);
    }

    [TestMethod]
    public void Test_AddItem_AddsItemToCategory()
    {
      //Arrange
      DateTime newDateTime = new DateTime(2001);
      Category testCategory = new Category("Household chores");
      testCategory.Save();
      Item testItem = new Item("Mow the lawn", newDateTime);
      testItem.Save();
      Item testItem2 = new Item("Water the garden", newDateTime);
      testItem2.Save();

      //Act
      testCategory.AddItem(testItem);
      testCategory.AddItem(testItem2);
      List<Item> result = testCategory.GetItems();
      List<Item> testList = new List<Item>{testItem, testItem2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Delete_DeletesCategoryAssociationsFromDatabase_CategoryList()
    {
      //Arrange
      DateTime newDateTime = new DateTime(2001);
      Item testItem = new Item("Mow the lawn", newDateTime);
      testItem.Save();
      string testName = "Home stuff";
      Category testCategory = new Category(testName);
      testCategory.Save();

      //Act
      testCategory.AddItem(testItem);
      testCategory.Delete();
      List<Category> resultItemCategories = testItem.GetCategories();
      List<Category> testItemCategories = new List<Category> {};

      //Assert
      CollectionAssert.AreEqual(testItemCategories, resultItemCategories);
    }

  }
}
