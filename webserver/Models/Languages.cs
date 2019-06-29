using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace webserver.Models
{
    public class Languages:BasicDb
    {
        MySqlCommand command;

        public Languages()
        {
            Open();
            this.con.Open();
            command = this.con.CreateCommand();
        }

        public List<LanguagesModel> get(int id)
        {

            Languages language;
            language = new Languages();
            List<LanguagesModel> data = new List<LanguagesModel>();
            string query = "SELECT * FROM languages ";
            if (id != 0)
            {
                query += " WHERE idLanguage = @id ";
            }

            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            try
            {
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LanguagesModel model;
                    model = new LanguagesModel();
                    model.IdLanguage = (int)reader["idLanguage"];
                    model.Name = (String)reader["Name"];

                    data.Add(model);
                }
                reader.Close();
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public int Insert(string name)
        {
            Languages language;
            language = new Languages();
            string query = "INSERT INTO languages (Name) VALUES (@name); SELECT MAX(idLanguage) FROM languages";
            command.CommandText = query;
            command.Parameters.AddWithValue("@name", name);
            try
            {
                int status = (int)command.ExecuteScalar();
                return status;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


        }
        public int Update(int id, string name)
        {
            Languages language;
            language = new Languages();
            string query = "UPDATE languages SET Name = @name WHERE idLanguage = @id";
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            try
            {
                int status = command.ExecuteNonQuery();
                return status;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int Delete(int id)
        {
            Languages language;
            language = new Languages();
            string query = "DELETE  FROM languages WHERE idLanguage = @id";
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);

            try
            {
                int status = command.ExecuteNonQuery();
                return status;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}