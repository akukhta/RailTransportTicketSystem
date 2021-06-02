using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Changes
    {
        public int changeSetID, type;
        public Object oldValue, newValue;
        public string TableName;

        public Changes(int changeSetID, int type, Object oldValue, Object newValue, string tableName)
        {
            this.changeSetID = changeSetID;
            this.type = type;
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.TableName = tableName;
        }

        public List<byte> serialise()
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(BitConverter.GetBytes(changeSetID));
            buffer.AddRange(BitConverter.GetBytes(type));
            buffer.AddRange(BitConverter.GetBytes(TableName.Length));
            buffer.AddRange(Encoding.Default.GetBytes(TableName));
            buffer.Add(oldValue != null ? (byte) 1 : (byte)0);
            
            if (oldValue != null)
            {
                if (TableName == "Предприятия")
                {
                    var oldFac = (FactoryInfo)oldValue;
                    buffer.AddRange(oldFac.serialise());
                }
            }

            buffer.Add(newValue != null ? (byte)1 : (byte)0);

            if (newValue != null)
            {
                if (TableName == "Предприятия")
                {
                    var newV = (FactoryInfo)newValue;
                    buffer.AddRange(newV.serialise());
                }
            }

            return buffer;
        }

        public static Changes deserialise(List<byte> buffer)
        {
            int changeSetId = Utilites.readIntFromBuffer(buffer);
            int type = Utilites.readIntFromBuffer(buffer);
            string tableName = Utilites.readStringFromBuffer(buffer);
            byte isValue = buffer[0];
            buffer.RemoveAt(0);

            Object oldValue = null, newValue = null;

            if (isValue == 1)
            {
                if (tableName == "Предприятия")
                {
                    oldValue = FactoryInfo.deserialise(buffer);
                }
            }

            isValue = buffer[0];
            buffer.RemoveAt(0);

            if (isValue == 1)
            {
                if (tableName == "Предприятия")
                {
                    newValue = FactoryInfo.deserialise(buffer);
                }
            }

            return new Changes(changeSetId, type, oldValue, newValue, tableName);
        }

    }
}
