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
    }

}
