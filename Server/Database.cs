﻿using System;
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


        public int getDocumentID()
        {
            string query = "select max(documentID) from changes;";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataReader reader = command.ExecuteReader();
            int documentID = (new Random()).Next();

            try
            {
                reader.Read();
                documentID = reader.GetInt32(0) + 1;
            }
            catch (Exception ex)
            {
                documentID = (new Random()).Next();
            }

            reader.Close();
            return documentID;
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
                reader.Close();
                string factoryQuery = "select * from sotrpredpr where sotrID = " + userID + ";";
                SqlCommand subCommand = new SqlCommand(factoryQuery, conn);
                var FactoryReader = subCommand.ExecuteReader();
                int factoryID = 0;

                if (FactoryReader.HasRows)
                {
                    FactoryReader.Read();
                    factoryID = FactoryReader.GetInt32(1);
                }
                
                FactoryReader.Close();
                user = new User(true, usertype, userID, name, surname, patronymic, passportSeries, passportNumber, job, gender, birthday, factoryID);
            }

            else
            {
                user = new User(false);
                reader.Close();
            }

            return user;
        }

        public List<FactoryInfo> GetFactories()
        {
            List<FactoryInfo> factories = new List<FactoryInfo>();

            string query = "select * from predpr where isValid = 1;";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                reader.Close();
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
            {
                reader.Close();
                return users;
            }

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

        public List<EmployeesFactoryInfo> GetEmployeesFactories()
        {
            List<EmployeesFactoryInfo> employeesFactoryInfos = new List<EmployeesFactoryInfo>();

            string query = "select * from sotrpredpr;";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                reader.Close();
                return employeesFactoryInfos;
            }

            while(reader.Read())
            {
                int UserID = reader.GetInt32(0);
                int FactoryID = reader.GetInt32(1);
                employeesFactoryInfos.Add(new EmployeesFactoryInfo(UserID, FactoryID));
            }

            reader.Close();
            return employeesFactoryInfos;
        }

        public string getFile(int documentID)
        {
            string query = "select fileName from changes where documentID = " + documentID + ";";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataReader reader = command.ExecuteReader();
            string fileName = "";

            if (reader.HasRows)
            {
                reader.Read();
                fileName = reader.GetString(0);
            }

            reader.Close();
            return fileName;
        }

        public List<BussinesTripInfo> GetDocuments()
        {
            List<BussinesTripInfo> documents = new List<BussinesTripInfo>();

            string query = "select * from changes;";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                reader.Close();
                return documents;
            }

            while (reader.Read())
            {
                string name = reader.GetString(0);
                string surname = reader.GetString(1);
                string patronymic = reader.GetString(2);
                DateTime start = reader.GetDateTime(3);
                DateTime end = reader.GetDateTime(4);
                string destinationPlace = reader.GetString(5);
                string reason = reader.GetString(6);
                string sender = reader.GetString(7);
                int documentID = reader.GetInt32(9);

                documents.Add(new BussinesTripInfo(name, surname, patronymic, "", destinationPlace, reason, sender, start, end, documentID));
            }

            reader.Close();
            return documents;
        }

        public void AddDocument(BussinesTripInfo info, string fileName, int documentID)
        {
            string query = "insert into changes (name, surname, patronymic," +
                "startdate, enddate, destinationPlace, reason, sender, fileName, documentID) values (\'" + info.name + "\', \'" + info.surname + "\', \'" + info.patronymic +
                "\',\'" + info.from.ToString("yyyy-MM-dd") + "\',\'" + info.to.ToString("yyyy-MM-dd") + "\',\'" +
                info.destinationPlace + "\', \'" + info.reason + "\',\'" + info.fullNameOfSender + "\',\'" + fileName + "\', " +
                documentID.ToString() + ");";



            SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();
        }

        public void AddToTable(Object newValue, string tableName, object additionalValues = null)
        {
            string query = null;

            if (tableName == "Предприятия")
            {
                FactoryInfo factory = (FactoryInfo)newValue;
                query = "insert into predpr(predprID, name, address, isValid) values(" + factory.predprID + ", \'" + factory.name
                    + "\', \'" + factory.address + "\', 1);";
                string password = (string)additionalValues;
            }

            if (tableName == "Сотрудники")
            {
                User user = (User)newValue;
                query = "insert into sotr values(" + user.userID + ", \'" + user.name
                    + "\', \'" + user.surname + "\', \'" + user.patronymic + "\', \'" + user.passportSeries
                    + "\', \'" + user.passportNumber + "\', \'" + user.gender + "\', \'" +
                    user.birthday.ToString("yyyy-MM-dd") + "\', " + user.userType + ", \'" + user.job + "\');";

                string password = (string)additionalValues;
                SqlCommand _command = new SqlCommand(query, conn);
                _command.ExecuteNonQuery();
                query = "insert into sign values(" + user.userID + ", \'" + password + "\');";
            }

            if (tableName == "Сотрудники-Предприятия")
            {
                User user = (User)newValue;
                query = "insert into sotrpredpr values(" + user.userID + "," + user.factoryID + ");";
            }

            SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();
        }

        public void DeleteFactory(FactoryInfo factory)
        {
            string query = "delete from predpr where predprID = " + factory.predprID + ";";
            
            SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();
        }

        public void DeleteSotr(User user)
        {
            string query = "delete from sotr where sotrID = " + user.userID + ";";

            SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();

            query = "delete from sign where sotrID = " + user.userID + ";";
            command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();
        }

        public void DeleteFactoryUser(FactoryInfo factory, User user)
        {
            string query = "delete from sotrpredpr where sotrID = " + user.userID + " and predprID = " + factory.predprID + ";";

            SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();
        }

        public void AddToChangesTable(Object oldValue, Object newValue, int operationsType, string table, int prepdrID)
        {
            if (table == "Предприятия")
            {
                FactoryInfo oldV = (FactoryInfo)oldValue;
                FactoryInfo newV = (FactoryInfo)newValue;

                if (operationsType == 0)
                {
                    AddToTable(newV, table);
                    string query = "insert into TablesChanges(newValue, type, predprID, tableName) values(" + newV.predprID +
                        ", 0, " + prepdrID + ",\'" + table + "\', 0);";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.ExecuteNonQuery();
                }

            }
        }

    }

}
