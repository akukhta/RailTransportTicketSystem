using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class User
    {
        public int userType;
        public int userID;
        public string name, surname, patronymic, passportSeries, passportNumber, job, gender;
        public DateTime birthday;
        public bool isValid;

        public User(bool isValid = true, int userType = 0, int userID = 0, string name = "", string surname = "", 
            string patronymic = "", string passportSeries = "", string passportNumber = "", string job = "", string gender = "", DateTime birthday = new DateTime())
        {
            this.isValid = isValid;
            this.userType = userType;
            this.userID = userID;
            this.name = name;
            this.surname = surname;
            this.patronymic = patronymic;
            this.gender = gender;
            this.birthday = birthday;
            this.passportSeries = passportSeries;
            this.passportNumber = passportNumber;
            this.job = job;
        }

        public byte[] serialise()
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(BitConverter.GetBytes(isValid));
            buffer.AddRange(BitConverter.GetBytes(userType));
            buffer.AddRange(BitConverter.GetBytes(userID));
            buffer.AddRange(BitConverter.GetBytes(name.Length));
            buffer.AddRange(Encoding.Default.GetBytes(name));
            buffer.AddRange(BitConverter.GetBytes(surname.Length));
            buffer.AddRange(Encoding.Default.GetBytes(surname));
            buffer.AddRange(BitConverter.GetBytes(patronymic.Length));
            buffer.AddRange(Encoding.Default.GetBytes(patronymic));
            buffer.AddRange(BitConverter.GetBytes(passportSeries.Length));
            buffer.AddRange(Encoding.Default.GetBytes(passportSeries));
            buffer.AddRange(BitConverter.GetBytes(passportNumber.Length));
            buffer.AddRange(Encoding.Default.GetBytes(passportNumber));
            buffer.AddRange(BitConverter.GetBytes(job.Length));
            buffer.AddRange(Encoding.Default.GetBytes(job));
            buffer.AddRange(BitConverter.GetBytes(gender.Length));
            buffer.AddRange(Encoding.Default.GetBytes(gender));
            buffer.AddRange(BitConverter.GetBytes(birthday.Ticks));
            return buffer.ToArray();
        }

        //private static string readStringFromBuffer(List<byte> buffer)
        //{
        //    int length = BitConverter.ToInt32(buffer.Take(4).ToArray(), 0);
        //    buffer.RemoveRange(0, sizeof(int));
        //    string result = Encoding.Default.GetString(buffer.Take(length).ToArray());
        //    buffer.RemoveRange(0, length);
        //    return result;
        //}

        //private static int readIntFromBuffer(List<byte> buffer)
        //{
        //    int val = BitConverter.ToInt32(buffer.Take(sizeof(int)).ToArray(), 0);
        //    buffer.RemoveRange(0, sizeof(int));
        //    return val;
        //}

        //private static bool readBoolFromBuffer(List<byte> buffer)
        //{
        //    bool val = BitConverter.ToBoolean(buffer.ToArray(), 0);
        //    buffer.RemoveRange(0, sizeof(bool));
        //    return val;
        //}

        public static User deserialise(List<byte> buffer)
        {
            bool isValid = Utilites.readBoolFromBuffer(buffer);
            int userType = Utilites.readIntFromBuffer(buffer);
            int userID = Utilites.readIntFromBuffer(buffer);
            string name = Utilites.readStringFromBuffer(buffer);
            string surname = Utilites.readStringFromBuffer(buffer);
            string patronymic = Utilites.readStringFromBuffer(buffer);                
            string passportSeries = Utilites.readStringFromBuffer(buffer);
            string passportNumber = Utilites.readStringFromBuffer(buffer);
            string job = Utilites.readStringFromBuffer(buffer);
            string gender = Utilites.readStringFromBuffer(buffer);
            DateTime birthday = DateTime.FromBinary(BitConverter.ToInt64(buffer.ToArray(), 0));

            return new User(isValid, userType, userID, name, surname, patronymic, passportSeries, passportNumber, job, gender, birthday);
        }
    }
}
