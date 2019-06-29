using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace webserver.Models
{
    public class Level:BasicDb
    {
        MySqlCommand command;

        public Level()
        {
            Open();
            this.con.Open();
            command = this.con.CreateCommand();
        }

        public List<LevelModel> get(int id)
        {

            Level level;
            level = new Level();
            List<LevelModel> data = new List<LevelModel>();
            String query = "SELECT * FROM level ";
            if (id != 0)
            {
                query += " WHERE idLevel = @id ";
            }

            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            try
            {
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LevelModel model;
                    model = new LevelModel();
                    model.idLevel = (int)reader["idLevel"];
                    model.idSubLevel = (int)reader["idSubLevel"];

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
        public int Insert(int idsub)
        {
            Level level;
            level = new Level();
            String query = "INSERT INTO level (idSubLevel) VALUES (@idsub); SELECT MAX(idLevel) FROM level";
            command.CommandText = query;
            command.Parameters.AddWithValue("@idsub", idsub);
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
        public int Update(int id, int idsub)
        {
            Level level;
            level = new Level();
            String query = "UPDATE level SET idSubLevel = @idsub WHERE idLevel = @id";
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@idsub", idsub);
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
            Level level;
            level = new Level();
            String query = "DELETE  FROM level WHERE idLevel = @id";
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