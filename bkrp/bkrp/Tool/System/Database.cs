using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    class Database
    {
        public static string connectionString = "SERVER=localhost;DATABASE=fcrp;UID=fcrp;PASSWORD=tuise233;SSL Mode=none;pooling = false;convert zero datetime=True;";
        public static void ExecuteSql(string query, Action<bool> callback)
        {
            using(MySqlConnection dbConn = new MySqlConnection(connectionString))
            {
                dbConn.Open();
                try
                {
                    MySqlCommand myCommand = new MySqlCommand(query, dbConn);
                    myCommand.ExecuteNonQuery();
                    dbConn.Close();
                    callback?.Invoke(false);
                    return;
                }
                catch(Exception ex)
                {
                    Log.Error(ex.Message);
                    Log.Error(ex.StackTrace);
                }
            }
            callback?.Invoke(true);
        }

        public static void ExecuteSql(string query, Action<MySqlDataReader, bool> callback)
        {
            using (MySqlConnection dbConn = new MySqlConnection(connectionString))
            {
                dbConn.Open();
                try
                {
                    MySqlCommand myCommand = new MySqlCommand(query, dbConn);
                    using(MySqlDataReader reader = myCommand.ExecuteReader())
                    {
                        callback?.Invoke(reader, false);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    Log.Error(ex.StackTrace);
                }
            }
            callback?.Invoke(null, true);
        }
    }
}
