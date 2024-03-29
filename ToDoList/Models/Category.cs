using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{
    public class Category
    {
        private string _name;
        private int _id;

        public Category(string name, int id = 0)
        {
            _name = name;
            _id = id;
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
                bool idEquality = this.GetId().Equals(newCategory.GetId());
                bool nameEquality = this.GetName().Equals(newCategory.GetName());
                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetId().GetHashCode();
        }

        public string GetName()
        {
            return _name;
        }

        public int GetId()
        {
            return _id;
        }

        public static void ClearAll()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"DELETE FROM category;";
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
            cmd.CommandText = @"INSERT INTO category (name) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

        }


        public static List<Category> GetAll()
        {
          List<Category> allCategories = new List<Category> {};
          MySqlConnection conn = DB.Connection();
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT * FROM category;";
          MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
          while(rdr.Read())
          {
            int categoryId = rdr.GetInt32(0);
            string categoryDescription = rdr.GetString(1);
            Category newCategory = new Category(categoryDescription, categoryId);
            allCategories.Add(newCategory);
          }
          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
          return allCategories;
        }

        public static Category Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM category WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int CategoryId = 0;
            string categoryDescription = "";

            while(rdr.Read())
            {
              CategoryId = rdr.GetInt32(0);
              categoryDescription = rdr.GetString(1);
            }
            Category newCategory = new Category(categoryDescription, CategoryId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newCategory;
        }

        public List<Item> GetItems()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT item.* FROM category
          JOIN category_item ON (category.id = category_item.category_id)
          JOIN item ON (category_item.item_id = item.id)
           WHERE category.id = @CategoryId;";
          MySqlParameter categoryIdParameter = new MySqlParameter();
          categoryIdParameter.ParameterName = "@CategoryId";
          categoryIdParameter.Value = _id;
          cmd.Parameters.Add(categoryIdParameter);
          MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
          List<Item> items = new List<Item> {};
          while(rdr.Read())
          {
            int itemId = rdr.GetInt32(0);
            string itemDescription = rdr.GetString(1);
            DateTime itemDueDate = rdr.GetDateTime(2);
            bool itemIsComplete = rdr.GetBoolean(3);
            Item newItem = new Item(itemDescription, itemDueDate, itemIsComplete, itemId);
            items.Add(newItem);
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
            MySqlCommand cmd = new MySqlCommand(@"DELETE FROM category WHERE id = @CategoryId; DELETE FROM category_item WHERE category_id = @CategoryId;", conn);
            MySqlParameter categoryIdParameter = new MySqlParameter();
            categoryIdParameter.ParameterName = "@CategoryId";
            categoryIdParameter.Value = this.GetId();
            cmd.Parameters.Add(categoryIdParameter);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Close();
            }
        }

        public void AddItem(Item newItem)
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"INSERT INTO category_item (category_id, item_id) VALUES (@CategoryId, @ItemId);";
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
    }
}
