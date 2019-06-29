using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace webserver.Models
{
    public class BasicDb
    {
        public MySqlConnection con;

        public void Open()
        {
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            con = new MySqlConnection(conn);

        }
        public void Close()
        {
            con.Close();
        }
    }
}