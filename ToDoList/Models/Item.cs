using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{
  public class Item
  {
    private string _description;
    private int _id;
    private string _rawDate;
    private DateTime _formattedDate;

    public Item (string description, string rawDate, int Id = 0)
    {
      _description = description;
      _id = Id;
      _rawDate = rawDate;
      _formattedDate = new DateTime();
    }

    public DateTime GetFormattedDate()
    {
      return _formattedDate;
    }

    public void SetDate()
    {
      string[] dateArray = _rawDate.Split('-');
      List<int> intDateList = new List<int>{};
      foreach (string num in dateArray)
      {
        intDateList.Add(Int32.Parse(num));
      }
      _formattedDate = new DateTime(intDateList[0], intDateList[1], intDateList[2]);
    }

    public string GetDescription()
    {
      return _description;
    }

    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }

    public int GetId()
    {
      return _id;
    }

    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        string itemRawDate = rdr.GetString(2);
        Item newItem = new Item(itemDescription, itemRawDate, itemId);
        newItem.SetDate();
        allItems.Add(newItem);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allItems;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM items;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO `items` (`description`, `raw_date`) VALUES (@ItemDescription, @RawDate);";

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@ItemDescription";
      description.Value = this._description;

      MySqlParameter rawDate = new MySqlParameter();
      rawDate.ParameterName = "@RawDate";
      rawDate.Value = this._rawDate;

      cmd.Parameters.Add(description);
      cmd.Parameters.Add(rawDate);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is Item))
      {
        return false;
      }
      else
      {
        Item newItem = (Item) otherItem;
        bool idEquality = (this.GetId() == newItem.GetId());
        bool descriptionEquality = (this.GetDescription() == newItem.GetDescription());
        return (idEquality && descriptionEquality);
      }
    }
    // public static void ClearAll()
    // {
    //   _instances.Clear();
    // }

    public static Item Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `items` WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int itemId = 0;
      string itemDescription = "";
      string itemRawDate = "";

      while (rdr.Read())
      {
        itemId = rdr.GetInt32(0);
        itemDescription = rdr.GetString(1);
        itemRawDate = rdr.GetString(2);
      }

      Item foundItem = new Item(itemDescription, itemRawDate, itemId);
      foundItem.SetDate();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return foundItem;
    }
  }
}
