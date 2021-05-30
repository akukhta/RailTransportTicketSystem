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

        public User Login(string password)
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

            string userQuery = "select * from sotr where sotrID = " + id.ToString() + ";";
            command = new SqlCommand(userQuery, conn);
            reader.Close();
            reader = command.ExecuteReader();

            User user = null;

            if (reader.HasRows)
            {

                reader.Read();
                
                int userID = reader.GetInt32(0);
                string name = reader.GetString(1);
                string surname = reader.GetString(2);
                string patronymic = reader.GetString(3);
                string passportSeries = reader.GetString(4);
                string passportNumber = reader.GetString(5);
                string gender = reader.GetString(6);
                DateTime birthday = reader.GetDateTime(7);
                int usertype = reader.GetInt32(8);
                string job = reader.GetString(9);
                user = new User(true, usertype, userID, name, surname, patronymic, passportSeries, passportNumber, job, gender, birthday);
            }

            else
            {
                user = new User(false);
            }


            reader.Close();

            return user;
        }

        public List<FactoryInfo> GetFactories()
        {
            List<FactoryInfo> factories = new List<FactoryInfo>();

            string query = "select * from predpr;";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                return factories;
            }

            while (reader.Read())
            {
                int predpr = reader.GetInt32(0);
                string name = reader.GetString(1);
                string address = reader.GetString(2);

                factories.Add(new FactoryInfo(predpr, name, address));
            }

            reader.Close();
            return factories;
        }

        public List<User> getUsers()
        {
            List<User> users = new List<User>();

            string query = "select * from sotr;";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
                return users;

            while (reader.Read())
            { 
                int userID = reader.GetInt32(0);
                string name = reader.GetString(1);
                string surname = reader.GetString(2);
                string patronymic = reader.GetString(3);
                string passportSeries = reader.GetString(4);
                string passportNumber = reader.GetString(5);
                string gender = reader.GetString(6);
                DateTime birthday = reader.GetDateTime(7);
                int usertype = reader.GetInt32(8);
                string job = reader.GetString(9);
                User user = new User(true, usertype, userID, name, surname, patronymic, passportSeries, passportNumber, job, gender, birthday);
                users.Add(user);
            }

            reader.Close();
            return users;
        }

    }

}
