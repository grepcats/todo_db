using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{
  public class Category
  {
    private string _name;
    private int _id;

    public Category(string name, int Id = 0)
    {
      _name = name;
      _id = Id;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public static List<Category> GetAll()
    {
      List<Category> allCategories = new List<Category> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM categories;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int categoryId = rdr.GetInt32(1);
        string categoryName = rdr.GetString(0);
        Category newCategory = new Category(categoryName, categoryId);
        allCategories.Add(newCategory);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCategories;
    }

    public override bool Equals(System.Object otherCategory)
    {
      if (!(otherCategory is Category))
      {
        return false;
      }
      else
      {
        Category newCategory = (Category) otherCategory;
        return this.GetId().Equals(newCategory.GetId());
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO `categories` (`name`, `id`) VALUES (@CategoryName, @CategoryId);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@CategoryName";
      name.Value = this._name;

      MySqlParameter id = new MySqlParameter();
      id.ParameterName = "@CategoryId";
      id.Value = this._id;

      cmd.Parameters.Add(name);
      cmd.Parameters.Add(id);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Item> GetItems()
    {
      List<Item> allCategoryItems = new List<Item> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `items` WHERE `category_id` = @category_id;";

      MySqlParameter categoryId = new MySqlParameter();
      categoryId.ParameterName = "@category_id";
      categoryId.Value = this._id;
      cmd.Parameters.Add(categoryId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        string itemRawDate = rdr.GetString(2);
        int itemCategoryId = rdr.GetInt32(4);
        Item newItem = new Item(itemDescription, itemRawDate, itemId, itemCategoryId);
        newItem.SetDate();
        allCategoryItems.Add(newItem);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return allCategoryItems;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM categories;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM categories WHERE id = @thisId;";

      var cmdItems = conn.CreateCommand() as MySqlCommand;
      cmdItems.CommandText = @"DELETE FROM items WHERE category_id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = _id;
      cmd.Parameters.Add(thisId);
      cmdItems.Parameters.Add(thisId);

      cmdItems.ExecuteNonQuery();
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Category Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * from `categories` WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int categoryId = 0;
      string categoryName = "";

      while (rdr.Read())
      {
        categoryId = rdr.GetInt32(1);
        categoryName = rdr.GetString(0);
      }

      Category foundCategory = new Category(categoryName, categoryId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return foundCategory;
    }

    public List<Item> Sort(string direction)
    {
      List<Item> sortedList = new List<Item>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM `items` WHERE `category_id` = " + this.GetId() + " ORDER BY `formatted_date` " + direction + ";";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        string itemRawDate = rdr.GetString(2);
        int categoryId = rdr.GetInt32(4);

        Item newItem = new Item(itemDescription, itemRawDate, itemId, categoryId);
        newItem.SetDate();
        sortedList.Add(newItem);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return sortedList;
    }
  }
}
