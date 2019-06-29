using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace webserver.Models
{
    public class Level_Language : BasicDb
    {
        MySqlCommand command;

        public Level_Language()
        {
            Open();
            this.con.Open();
            command = this.con.CreateCommand();
        }

        public List<Level_LanguageModel> get(int idlevel, int idlanguage)
        {

            Level_Language level_language;
            level_language = new Level_Language();
            List<Level_LanguageModel> data = new List<Level_LanguageModel>();
            String query = "SELECT * FROM level_language ";
            Boolean checkle = false;
            Boolean checkla = false;
            if (idlevel != 0)
            {
                checkle = true;
                query += " WHERE idLevel = @idlevel ";
            }
            if (idlanguage != 0)
            {
                checkla = true;
                if (checkle)
                {
                    query += " AND idLanguage = @idlang ";
                }
                else
                {
                    query += " WHERE idLanguage = @idlang ";
                }
            }

            command.CommandText = query;
            if (checkle)
                command.Parameters.AddWithValue("@idlevel", idlevel);
            if (checkla)
                command.Parameters.AddWithValue("@idlang", idlanguage);
            try
            {
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Level_LanguageModel model;
                    model = new Level_LanguageModel();
                    model.idLevel = (int)reader["idLevel"];
                    model.idLanguage = (int)reader["idLanguage"];
                    model.Name = (string)reader["Name"];
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
        public int Insert(int idlevel, int idlang, string name)
        {
            Level_Language level_language;
            level_language = new Level_Language();
            String query = "INSERT INTO level_language VALUES (@idlevel,@idlang,@name)";
            command.CommandText = query;
            command.Parameters.AddWithValue("@idlevel", idlevel);
            command.Parameters.AddWithValue("@idlang", idlang);
            command.Parameters.AddWithValue("@name", name);
            try
            {
                int status = (int)command.ExecuteNonQuery();
                return status;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


        }
        public int Update(int idlevel_old, int idlang_old, int idlevel, int idlang, string name)
        {
            Level_Language level_language;
            level_language = new Level_Language();
            String query = "UPDATE level_language SET idLevel = @idlevel,idLanguage=@idlang,Name=@name WHERE idLevel = @idlevel_o AND idLanguage = @idlang_o";
            command.CommandText = query;
            command.Parameters.AddWithValue("@idlevel_o", idlevel_old);
            command.Parameters.AddWithValue("@idlang_o", idlang_old);
            command.Parameters.AddWithValue("@idlevel", idlevel);
            command.Parameters.AddWithValue("@idlang", idlang);
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
        public int Delete(int idlevel, int idlang)
        {
            Level_Language level_language;
            level_language = new Level_Language();
            String query = "DELETE  FROM level_language WHERE idLevel = @idlevel_o AND idLanguage = @idlang_o";
            command.CommandText = query;
            command.Parameters.AddWithValue("@idlevel_o", idlevel);
            command.Parameters.AddWithValue("@idlang_o", idlang);

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