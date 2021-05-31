using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class EmployeesFactoryInfo
    {
        public int UserID, FactoryID;

        public EmployeesFactoryInfo(int UserID, int FactoryID)
        {
            this.UserID = UserID;
            this.FactoryID = FactoryID;
        }

        public List<byte> serialise()
        {
            List<byte> buffer = new List<byte>();

            buffer.AddRange(BitConverter.GetBytes(UserID));
            buffer.AddRange(BitConverter.GetBytes(FactoryID));

            return buffer;
        }

        public static EmployeesFactoryInfo deserialise(List<byte> buffer)
        {
            int UserID = Utilites.readIntFromBuffer(buffer);
            int FactoryID = Utilites.readIntFromBuffer(buffer);

            return new EmployeesFactoryInfo(UserID, FactoryID);
        }
    }
}
