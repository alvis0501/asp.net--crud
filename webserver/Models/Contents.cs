using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace webserver.Models
{
    public class Contents:BasicDb
    {
        MySqlCommand command;
        public Contents()
        {
            Open();
            this.con.Open();
            command = this.con.CreateCommand();
        }

        public List<ContentsModel> get(int idContent)
        {

            Contents content;
            content = new Contents();
            List<ContentsModel> data = new List<ContentsModel>();
            String query = "SELECT idContent,DATE_FORMAT(createdDate,'%d/%m/%Y') AS date,idContentType as cont FROM contents ";
            if (idContent != 0)
            {
                query += " WHERE idContent = @idContent ";
            }

            command.CommandText = query;
            command.Parameters.AddWithValue("@idContent", idContent);
            try
            {
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ContentsModel model;
                    model = new ContentsModel();
                    model.IdContent = (int)reader["idContent"];
                    model.IdContentType = (int)reader["cont"];
                    model.CreatedDate = (String)reader["date"];
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
        public int Insert(int idcont)
        {
            Contents content;
            content = new Contents();
            String query = "INSERT INTO contents (idContentType) VALUES (@idcont); SELECT MAX(idContent) FROM contents";
            command.CommandText = query;
            command.Parameters.AddWithValue("@idcont", idcont);
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
        public int Update(int idcon, int idcont)
        {
            Contents content;
            content = new Contents();
            String query = "UPDATE contents SET idContentType = @idcont WHERE idContent = @idcon";
            command.CommandText = query;
            command.Parameters.AddWithValue("@idcon", idcon);
            command.Parameters.AddWithValue("@idcont", idcont);
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
        public int Delete(int idcon)
        {
            Contents content;
            content = new Contents();
            String query = "DELETE  FROM contents WHERE idContent = @idcon";
            command.CommandText = query;
            command.Parameters.AddWithValue("@idcon", idcon);

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