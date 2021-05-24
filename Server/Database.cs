using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Server
{
    class Database
    {
        private SqlConnection conn;
        
        public Database(string connectionString = "Data Source = WIN-QKTTIEU1VPU; Initial Catalog = KomDok; Integrated Security = true")
        {
            conn = new SqlConnection(connectionString);
            conn.Open();

            if (conn.State != System.Data.ConnectionState.Open)
                throw new Exception("Can't connect to data base!");
        }

        private void insertQuery(string query)
        {
            var q = conn.CreateCommand();
            q.CommandText = query;
            q.ExecuteNonQuery();
        }

        public byte Login(string password)
        {
            string query = "select sotrID from sign where password = \'" + password + "\';";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataReader reader = command.ExecuteReader();
            int id = 0;

            if (reader.HasRows)
            {
                reader.Read();
                id = reader.GetInt32(0);
            }

            query = "select usertype from sotr where sotrID = " + id.ToString() + ";";
            command = new SqlCommand(query, conn);
            byte userType = 2;
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    reader.Read();
                    userType = reader.GetByte(0);
                }
            }

            return userType;
        }
    }

}
