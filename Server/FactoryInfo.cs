using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class FactoryInfo
    {
        int predprID;
        string name, address;

        public FactoryInfo(int predprID, string name, string address)
        {
            this.predprID = predprID;
            this.name = name;
            this.address = address;
        }

        public byte[] serialise()
        {
            List<byte> buffer = new List<byte>();

            buffer.AddRange(BitConverter.GetBytes(predprID));
            
            int nameLength = name.Length;
            buffer.AddRange(BitConverter.GetBytes(nameLength));
            buffer.AddRange(Encoding.Default.GetBytes(name));
            
            int addressLength = address.Length;
            buffer.AddRange(BitConverter.GetBytes(addressLength));
            buffer.AddRange(Encoding.Default.GetBytes(address));

            return buffer.ToArray();
        }

        public static FactoryInfo deserialise(List<byte> buffer)
        {
            int prepdrID = Utilites.readIntFromBuffer(buffer);
            string name = Utilites.readStringFromBuffer(buffer);
            string address = Utilites.readStringFromBuffer(buffer);

            return new FactoryInfo(prepdrID, name, address);
        }
    }
}
