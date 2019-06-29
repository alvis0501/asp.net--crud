using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace webserver.Models
{
    public class UserType:BasicDb
    {
         MySqlCommand command;
        public UserType()
        {
            Open();
            this.con.Open();
            command = this.con.CreateCommand();
        }

        public List<UsertypeModel> get(int id)
        {

            UserType usertype;
            usertype = new UserType();
            List<UsertypeModel> data = new List<UsertypeModel>();
            String query = "SELECT * FROM usertype ";
            if(id !=0){
                query += " WHERE idUserType = @id ";
            }
            
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            try
            {
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UsertypeModel model;
                    model = new UsertypeModel();
                    model.IdUserType = (int)reader["idUserType"];
                    model.Description = (String)reader["Description"];
                   
                    data.Add(model);
                }
                reader.Close();
                return data;
            }
            catch (Exception e)
            {
                throw new Exception( e.Message);
            }
            
        }
        public int Insert(string desp)
        {
            UserType usertype;
            usertype = new UserType();
            String query = "INSERT INTO usertype (Description) VALUES (@desp); SELECT MAX(idUserType) FROM usertype";
            command.CommandText = query;
            command.Parameters.AddWithValue("@desp", desp);
            try
            {
                int status = (int)command.ExecuteScalar();
                return status;
            }
            catch (Exception e)
            {
                throw new Exception( e.Message);
            }
            

        }
        public int Update(int id, string desp)
        {
            UserType usertype;
            usertype = new UserType();
            String query = "UPDATE usertype SET Description = @desp WHERE idUserType = @id";
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@desp", desp);
            try
            {
                int status = command.ExecuteNonQuery();
                return status;
            }
            catch (Exception e)
            {
                throw new Exception( e.Message);
            }
        }
        public int Delete(int id)
        {
            UserType usertype;
            usertype = new UserType();
            String query = "DELETE  FROM usertype WHERE idUserType = @id";
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
           
            try
            {
                int status = command.ExecuteNonQuery();
                return status;
            }
            catch (Exception e)
            {
                throw new Exception( e.Message);
            }
        }
    }
}