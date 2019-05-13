using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;
using System.Collections.Generic;
using System;

namespace ToDoList.Tests
{
  [TestClass]
  public class ItemTest : IDisposable
  {

    public void Dispose()
    {
      Item.ClearAll();
    }

    public ItemTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=to_do_list_test;";
    }

    [TestMethod]
    public void ItemConstructor_CreatesInstanceOfItem_Item()
    {
      DateTime newDateTime = new DateTime(2001);
      Item newItem = new Item("test", newDateTime);
      Assert.AreEqual(typeof(Item), newItem.GetType());
    }

    [TestMethod]
    public void GetDescription_ReturnsDescription_String()
    {
      //Arrange
      string description = "Walk the dog.";
      DateTime newDateTime = new DateTime(2001);
      Item newItem = new Item(description, newDateTime);

      //Act
      string result = newItem.GetDescription();

      //Assert
      Assert.AreEqual(description, result);
    }

    [TestMethod]
    public void SetDescription_SetDescription_String()
    {
      //Arrange
      DateTime newDateTime = new DateTime(2001);
      string description = "Walk the dog.";
      Item newItem = new Item(description, newDateTime);

      //Act
      string updatedDescription = "Do the dishes";
      newItem.SetDescription(updatedDescription);
      string result = newItem.GetDescription();

      //Assert
      Assert.AreEqual(updatedDescription, result);
    }

    [TestMethod]
    public void GetAll_ReturnsEmptyListFromDatabase_ItemList()
    {
      //Arrange
      List<Item> newList = new List<Item> { };

      //Act
      List<Item> result = Item.GetAll();

      //Assert
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfDescriptionAreTheSame_Item()
    {
      DateTime newDateTime = new DateTime(2001);
      Item firstItem = new Item("Mow the lawn", newDateTime);
      Item secondItem = new Item("Mow the lawn", newDateTime);
      Assert.AreEqual(firstItem, secondItem);
    }
    [TestMethod]
    public void Save_SavesToDatabase_ItemList()
    {
      DateTime newDateTime = new DateTime(2001);
      Item testItem = new Item("Mow the lawn", newDateTime);
      testItem.Save();
      List<Item> result = Item.GetAll();
      List<Item> testList = new List<Item>{testItem};
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetAll_ReturnsItems_ItemList()
    {
      //Arrange
      string description01 = "Walk the dog";
      string description02 = "Wash the dishes";
      DateTime newDateTime = new DateTime(2001);
      Item newItem1 = new Item(description01, newDateTime);
      newItem1.Save();
      Item newItem2 = new Item(description02, newDateTime);
      newItem2.Save();
      List<Item> newList = new List<Item> { newItem1, newItem2 };

      //Act
      List<Item> result = Item.GetAll();
      foreach(Item item in result)
      {
      Console.WriteLine(item.GetDueDate().ToString("F"));
      }

      foreach(Item item in newList)
      {
      Console.WriteLine("NewList:"+item.GetDueDate().ToString("F"));
      }
      //Assert
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void Save_AssignsIdObject_Id()
    {
      DateTime newDateTime = new DateTime(2001);
      Item testItem = new Item("Mow the lawn", newDateTime);
      testItem.Save();
      Item savedItem = Item.GetAll()[0];

      int result = savedItem.GetId();
      int testId = testItem.GetId();

      Assert.AreEqual(testId, result);

    }
    // [TestMethod]
    // public void GetId_ItemsInstantiateWithAnIdAndGetterReturns_Int()
    // {
    //   //Arrange
    //   string description = "Walk the dog.";
    //   Item newItem = new Item(description);
    //
    //   //Act
    //   int result = newItem.GetId();
    //
    //   //Assert
    //   Assert.AreEqual(1, result);
    // }
    //
    [TestMethod]
    public void Find_ReturnsCorrectItemFromDatabase_Item()
    {
      //Arrange
      DateTime newDateTime = new DateTime(2001);
      Item testItem = new Item("Mow the lawn", newDateTime);
      testItem.Save();

      //Act
      Item foundItem = Item.Find(testItem.GetId());

      //Assert
      Assert.AreEqual(testItem, foundItem);
    }

    [TestMethod]
    public void GetCategories_ReturnAllItemsCategories_CategoryList()
    {
      //Arrange
      DateTime newDateTime = new DateTime(2001);
      Item testItem = new Item("Mow the lawn", newDateTime);
      testItem.Save();
      Category testCategory1 = new Category("Home Stuff");
      testCategory1.Save();
      Category testCategory2 = new Category("Work Stuff");
      testCategory2.Save();

      //Act
      testItem.AddCategory(testCategory1);
      List<Category> result = testItem.GetCategories();
      List<Category> testList = new List<Category> {testCategory1};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void AddCategory_AddsCategoryToItem_CategoryList()
    {
      //Arrange
      DateTime newDateTime = new DateTime(2001);
      Item testItem = new Item("Mow the lawn", newDateTime);
      testItem.Save();
      Category testCategory = new Category("Home stuff");
      testCategory.Save();

      //Act
      testItem.AddCategory(testCategory);

      List<Category> result = testItem.GetCategories();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Delete_DeletesItemAssociationsFromDatabase_ItemList()
    {
      //Arrange
      DateTime newDateTime = new DateTime(2001);
      Category testCategory = new Category("Home stuff");
      testCategory.Save();
      string testDescription = "Mow the lawn";
      Item testItem = new Item(testDescription, newDateTime);
      testItem.Save();

      //Act
      testItem.AddCategory(testCategory);
      testItem.Delete();
      List<Item> resultCategoryItems = testCategory.GetItems();
      List<Item> testCategoryItems = new List<Item> {};

      //Assert
      CollectionAssert.AreEqual(testCategoryItems, resultCategoryItems);
    }

    [TestMethod]
    public void CompletedItem_ChangesIsCompletePropertyToTrue_Bool()
    {
      DateTime newDateTime = new DateTime(2001);
      Category testCategory = new Category("Home stuff");
      testCategory.Save();
      string testDescription = "Mow the lawn";
      Item testItem = new Item(testDescription, newDateTime);
      testItem.Save();
      testCategory.AddItem(testItem);

      testItem.AddCategory(testCategory);

      testItem.CompletedItem();
      List<Item> resultCategoryItems = testCategory.GetItems();
      // Item result = resultCategoryItems.GetItem(Find(testItem.GetId()));

      Assert.AreEqual(true, testItem.GetIsComplete());
    }
  }
}
