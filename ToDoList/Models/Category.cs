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

    public void AddItem(Item newItem)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO categories_items (category_id, item_id) VALUES (@CategoryId, @ItemId);";

        MySqlParameter category_id = new MySqlParameter();
        category_id.ParameterName = "@CategoryId";
        category_id.Value = _id;
        cmd.Parameters.Add(category_id);

        MySqlParameter item_id = new MySqlParameter();
        item_id.ParameterName = "@ItemId";
        item_id.Value = newItem.GetId();
        cmd.Parameters.Add(item_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }

    public List<Item> GetItems()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT item_id FROM categories_items WHERE category_id = @CategoryId;";

        MySqlParameter categoryIdParameter = new MySqlParameter();
        categoryIdParameter.ParameterName = "@CategoryId";
        categoryIdParameter.Value = _id;
        cmd.Parameters.Add(categoryIdParameter);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        List<int> itemIds = new List<int> {};
        while(rdr.Read())
        {
            int itemId = rdr.GetInt32(0);
            itemIds.Add(itemId);
        }
        rdr.Dispose();

        List<Item> items = new List<Item> {};
        foreach (int itemId in itemIds)
        {
            var itemQuery = conn.CreateCommand() as MySqlCommand;
            itemQuery.CommandText = @"SELECT * FROM items WHERE id = @ItemId;";

            MySqlParameter itemIdParameter = new MySqlParameter();
            itemIdParameter.ParameterName = "@ItemId";
            itemIdParameter.Value = itemId;
            itemQuery.Parameters.Add(itemIdParameter);

            var itemQueryRdr = itemQuery.ExecuteReader() as MySqlDataReader;
            while(itemQueryRdr.Read())
            {
                int thisItemId = itemQueryRdr.GetInt32(0);
                string itemDescription = itemQueryRdr.GetString(1);
                string rawDate = itemQueryRdr.GetString(2);
                Item foundItem = new Item(itemDescription, rawDate, thisItemId);
                foundItem.SetDate();
                items.Add(foundItem);
            }
            itemQueryRdr.Dispose();
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }

        return items;
    }

    public void Delete()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        MySqlCommand cmd = new MySqlCommand("DELETE FROM categories WHERE id = @CategoryId; DELETE FROM categories_items WHERE category_id = @CategoryId;", conn);
        MySqlParameter categoryIdParameter = new MySqlParameter();
        categoryIdParameter.ParameterName = "@CategoryId";
        categoryIdParameter.Value = this.GetId();

        cmd.Parameters.Add(categoryIdParameter);
        cmd.ExecuteNonQuery();

        if (conn != null)
        {
            conn.Close();
        }
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

    public List<Item> SortAsc()
    {
      List<Item> sortedList = new List<Item>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM `items` WHERE `category_id` = @thisId ORDER BY -`formatted_date` DESC;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = this.GetId();
      cmd.Parameters.Add(thisId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        string itemRawDate = rdr.GetString(2);
        int categoryId = rdr.GetInt32(4);

        Item newItem = new Item(itemDescription, itemRawDate, itemId);
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

    public List<Item> SortDesc()
    {
      List<Item> sortedList = new List<Item>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM `items` WHERE `category_id` = @thisId ORDER BY `formatted_date` DESC;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = this.GetId();
      cmd.Parameters.Add(thisId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        string itemRawDate = rdr.GetString(2);
        int categoryId = rdr.GetInt32(4);

        Item newItem = new Item(itemDescription, itemRawDate, itemId);
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
