using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace webserver.Models
{
    public class Users :BasicDb
    {
        MySqlCommand command;

        public Users()
        {
            Open();
            this.con.Open();
            command = this.con.CreateCommand();
        }
        public int authCheck(string email, string pass)
        {
            Users users;
            users = new Users();

            String query = "SELECT idUser  FROM users WHERE email=@email and password=@pass";

            command.CommandText = query;
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@pass", pass);
            try
            {
                int count = (int)command.ExecuteScalar();

                return count;
            }
            catch (Exception e)
            {
                throw new Exception("No account");
            }
        }

        public List<UsersModel> get(int id)
        {


            Users users;
            users = new Users();
            List<UsersModel> data = new List<UsersModel>();
            String query = "SELECT *,DATE_FORMAT(birthdate,'%d/%m/%Y') AS birth_date,DATE_FORMAT(createdDate,'%d/%m/%Y') AS created_date FROM users ";
            if (id != 0)
            {
                query += " WHERE idUser = @id ";
            }

            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            try
            {
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UsersModel model = new UsersModel();
                    model.active = (string)reader["active"];
                    model.idLanguageFirst = (int)reader["idLanguageFirst"];
                    model.idLanguageSecond = (int)reader["idLanguageSecond"];
                    model.idUserType = (int)reader["idUserType"];
                    model.idUser = (int)reader["idUser"];
                    model.email = (string)reader["email"];
                    model.password = (string)reader["password"];
                    model.firstName = (string)reader["firstName"];
                    model.birthdate = (string)reader["birth_date"];
                    model.createdDate = (string)reader["created_Date"];
                    model.avatar = (string)reader["avatar"];
                    model.gender = (string)reader["gender"];
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
        public int Insert(string email, string password, string fname, string lname, string birthdate, string avatar, string gender, string facebook_id, int idlangf, int idlangs, string active, int idusertype)
        {
            Users users;
            users = new Users();
            String query = "INSERT INTO users (email, `password`, firstName,lastName,birthdate,avatar,gender,active,facebook_id,idLanguageFirst,idLanguageSecond,idUserType) " +
                " VALUES (@email, @password,@fname,@lname,@birthdate,@avatar,@gender,@active,@facebook,@idlangf,@idlangs,@idusertype); SELECT MAX(idUser) FROM users";
            command.CommandText = query;
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@fname", fname);
            command.Parameters.AddWithValue("@lname", lname);
            command.Parameters.AddWithValue("@birthdate", birthdate);
            command.Parameters.AddWithValue("@avatar", avatar);
            command.Parameters.AddWithValue("@gender", gender);
            command.Parameters.AddWithValue("@facebook", facebook_id);
            command.Parameters.AddWithValue("@idlangf", idlangf);
            command.Parameters.AddWithValue("@idlangs", idlangs);
            command.Parameters.AddWithValue("@active", active);
            command.Parameters.AddWithValue("@idusertype", idusertype);
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
        public int Update(int id, string email, string password, string fname, string lname, string birthdate, string avatar, string gender, string facebook_id, int idlangf, int idlangs, string active, int idusertype)
        {
            Users users;
            users = new Users();
            string avatar_query = "";
            bool check_avatar = false;
            if (avatar != "error")
            {
                check_avatar = true;
                avatar_query += " avatar = @avatar, ";
            }
            String query = "UPDATE users SET idLanguageFirst = @idlangf,idLanguageSecond = @idlangs,idUserType = @idusertype,email = @email,`password` = @password, " + avatar_query +
                "gender = @gender,facebook_id = @facebook,active = @active,firstName=@fname,lastName=@lname WHERE idUser = @id";
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@fname", fname);
            command.Parameters.AddWithValue("@lname", lname);
            command.Parameters.AddWithValue("@birthdate", birthdate);
            if (check_avatar)
                command.Parameters.AddWithValue("@avatar", avatar);
            command.Parameters.AddWithValue("@gender", gender);
            command.Parameters.AddWithValue("@facebook", facebook_id);
            command.Parameters.AddWithValue("@idlangf", idlangf);
            command.Parameters.AddWithValue("@idlangs", idlangs);
            command.Parameters.AddWithValue("@active", active);
            command.Parameters.AddWithValue("@idusertype", idusertype);
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
            Users users;
            users = new Users();
            String query = "DELETE  FROM users WHERE idUser = @id";
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
        public bool checkUnique_email(string email)
        {
            Users users = new Users();
            String query = "SELECT COUNT(idUser) FROM users WHERE email = @email";
            command.CommandText = query;
            command.Parameters.AddWithValue("@email", email);
            try
            {
                long count = (long)command.ExecuteScalar();
                if (count == 0)
                    return false;
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool checkUnique_face(string face_id)
        {
            Users users = new Users();
            String query = "SELECT COUNT(idUser) FROM users WHERE facebook_id = @face_id";
            command.CommandText = query;
            command.Parameters.AddWithValue("@face_id", face_id);
            try
            {
                long count = (long)command.ExecuteScalar();
                if (count == 0)
                    return false;
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}