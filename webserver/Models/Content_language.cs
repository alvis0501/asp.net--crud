using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace webserver.Models
{
    public class Content_language:BasicDb
    {
        MySqlCommand command;

        public Content_language()
        {
            Open();
            this.con.Open();
            command = this.con.CreateCommand();
        }

         public List<Content_languageModel> get(int idcon, int idlanguage)
        {

            Content_language content_language;
            content_language = new Content_language();
            List<Content_languageModel> data = new List<Content_languageModel>();
            String query = "SELECT * FROM contents_languages ";
            Boolean checkco = false;
            Boolean checkla = false;
            if (idcon != 0)
            {
                checkco = true;
                query += " WHERE idContent = @idcontent ";
            }
            if (idlanguage != 0)
            {
               checkla = true;
               if (checkco)
                {
                    query += " AND idLanguage = @idlang ";
                }
                else
                {
                    query += " WHERE idLanguage = @idlang ";
                }
            }
            
            command.CommandText = query;
            if (checkco)
                command.Parameters.AddWithValue("@idcontent", idcon);
            if (checkla)
                command.Parameters.AddWithValue("@idlang", idlanguage);
            try
            {
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Content_languageModel model;
                    model = new Content_languageModel();
                    model.idContent = (int)reader["idContent"];
                    model.idLanguage = (int)reader["idLanguage"];
                    model.desp1 = (string)reader["Description1"];
                    model.desp2 = (string)reader["Description2"];
                    model.desp3 = (string)reader["Description3"];
                    model.desp4 = (string)reader["Description4"];
                    model.desp5 = (string)reader["Description5"];
                    model.op1 = (int)reader["op1"];
                    model.op2 = (int)reader["op2"];
                    model.op3 = (int)reader["op3"];
                    model.op4 = (int)reader["op4"];
                    model.op5 = (int)reader["op5"];
                    model.image = (string)reader["Image"];
                    model.audio = (string)reader["Audio"];
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
         public int Insert(string idContent, string idlang, string desp1, string desp2, string desp3, string desp4, string desp5, string op1, string op2, string op3, string op4, string op5, string image, string audio)
        {
            Content_language content_language;
            content_language = new Content_language();
            String query = "INSERT INTO contents_languages VALUES (@idcon,@desp1,@desp2,@desp3,@desp4,@desp5,@op1,@op2,@op3,@op4,@op5,@idlang,@image,@audio)";
            command.CommandText = query;
            command.Parameters.AddWithValue("@idcon", idContent);
            command.Parameters.AddWithValue("@idlang", idlang);
            command.Parameters.AddWithValue("@desp1", desp1);
            command.Parameters.AddWithValue("@desp2", desp2);
            command.Parameters.AddWithValue("@desp3", desp3);
            command.Parameters.AddWithValue("@desp4", desp4);
            command.Parameters.AddWithValue("@desp5", desp5);
            command.Parameters.AddWithValue("@op1", op1);
            command.Parameters.AddWithValue("@op2", op2);
            command.Parameters.AddWithValue("@op3", op3);
            command.Parameters.AddWithValue("@op4", op4);
            command.Parameters.AddWithValue("@op5", op5);
            command.Parameters.AddWithValue("@image", image);
            command.Parameters.AddWithValue("@audio", audio);
            try
            {
                int status = (int)command.ExecuteNonQuery();
                return status;
            }
            catch (Exception e)
            {
                throw new Exception( e.Message);
            }
            

        }
         public int Update(string idContent_old, string idLang_old, string idContent, string idlang, string desp1, string desp2, string desp3, string desp4, string desp5, string op1, string op2, string op3, string op4, string op5, string image, string audio)
        {
            Content_language content_language;
            content_language = new Content_language();
            string audio_query = "";
            string image_query = "";
            bool check_image = false;
            bool check_audio = false;
            if (image != "")
            {
                check_image = true;
                image_query += " Image=@image, ";
            }
            if (audio != "")
            {
                check_audio = true;
                audio_query += "Audio=@audio ";
            }
            String query = "UPDATE contents_languages SET idContent = @idcon,idLanguage=@idlang,Description1=@desp1,Description2=@desp2,Description3=@desp3," +
                "Description4=@desp4,Description5=@desp5,op1=@op1,op2=@op2,op3=@op3,op4=@op4,op5=@op5," + image_query+ audio_query+ " WHERE idContent = @idcont_o AND idLanguage = @idlang_o";
            command.CommandText = query;
            command.Parameters.AddWithValue("@idcont_o", idContent_old);
            command.Parameters.AddWithValue("@idlang_o", idLang_old);
            command.Parameters.AddWithValue("@idcon", idContent);
            command.Parameters.AddWithValue("@idlang", idlang);
            command.Parameters.AddWithValue("@desp1", desp1);
            command.Parameters.AddWithValue("@desp2", desp2);
            command.Parameters.AddWithValue("@desp3", desp3);
            command.Parameters.AddWithValue("@desp4", desp4);
            command.Parameters.AddWithValue("@desp5", desp5);
            command.Parameters.AddWithValue("@op1", op1);
            command.Parameters.AddWithValue("@op2", op2);
            command.Parameters.AddWithValue("@op3", op3);
            command.Parameters.AddWithValue("@op4", op4);
            command.Parameters.AddWithValue("@op5", op5);
            if(check_image)
                command.Parameters.AddWithValue("@image", image);
            if(check_audio)
                command.Parameters.AddWithValue("@audio", audio);
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
        public int Delete(int idcon, int idlang)
        {
            Content_language content_language;
            content_language = new Content_language();
            String query = "DELETE  FROM contents_languages WHERE idContent = @idcont_o AND idLanguage = @idlang_o";
            command.CommandText = query;
            command.Parameters.AddWithValue("@idcont_o", idcon);
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