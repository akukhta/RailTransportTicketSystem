using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Utilites
    {
        public static string readStringFromBuffer(List<byte> buffer)
        {
            int length = BitConverter.ToInt32(buffer.Take(4).ToArray(), 0);
            buffer.RemoveRange(0, sizeof(int));
            string result = Encoding.Default.GetString(buffer.Take(length).ToArray());
            buffer.RemoveRange(0, length);
            return result;
        }

        public static int readIntFromBuffer(List<byte> buffer)
        {
            int val = BitConverter.ToInt32(buffer.Take(sizeof(int)).ToArray(), 0);
            buffer.RemoveRange(0, sizeof(int));
            return val;
        }

        public static bool readBoolFromBuffer(List<byte> buffer)
        {
            bool val = BitConverter.ToBoolean(buffer.ToArray(), 0);
            buffer.RemoveRange(0, sizeof(bool));
            return val;
        }
    }
}
