using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace webserver.Models
{
    public class Lessons:BasicDb
    {
        MySqlCommand command;

        public Lessons()
        {
            Open();
            this.con.Open();
            command = this.con.CreateCommand();
        }

        public List<LessonsModel> get(int id)
        {


            Lessons lesson;
            lesson = new Lessons();
            List<LessonsModel> data = new List<LessonsModel>();
            String query = "SELECT * FROM lessons ";
            if (id != 0)
            {
                query += " WHERE idLesson = @id ";
            }

            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            try
            {
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LessonsModel model = new LessonsModel();
                    model.idLesson = (int)reader["idLesson"];
                    model.idContent = (int)reader["idContent"];
                    model.idLevel = (int)reader["idLevel"];
                    model.active = (string)reader["Active"];
                    model.order = (int)reader["Order"];
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
        public int Insert(int idcon, int idlevel, string active, int order)
        {
            Lessons lesson;
            lesson = new Lessons();
            String query = "INSERT INTO lessons (idContent, idLevel, Active,`Order`) VALUES (@idcon, @idlevel, @active,@order); SELECT MAX(idLesson) FROM lessons";
            command.CommandText = query;
            command.Parameters.AddWithValue("@idcon", idcon);
            command.Parameters.AddWithValue("@idlevel", idlevel);
            command.Parameters.AddWithValue("@active", active);
            command.Parameters.AddWithValue("@order", order);
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
        public int Update(int id, int idcon, int idlevel, string active, int order)
        {
            Lessons lesson;
            lesson = new Lessons();
            String query = "UPDATE lessons SET idContent = @idcon,idLevel = @idlevel,`Order` = @order,Active = @active WHERE idLesson = @id";
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@idcon", idcon);
            command.Parameters.AddWithValue("@idlevel", idlevel);
            command.Parameters.AddWithValue("@active", active);
            command.Parameters.AddWithValue("@order", order);
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
            Lessons lesson;
            lesson = new Lessons();
            String query = "DELETE  FROM lessons WHERE idLesson = @id";
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