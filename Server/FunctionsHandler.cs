using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class FunctionsHandler
    {
        public enum Operations : byte
        {
            Login
        }

        private Database db;

        public FunctionsHandler()
        {
            db = new Database();
        }

        public List<byte> HandleRequest(List<byte> request)
        {
            byte func = request[0];
            request.RemoveAt(0);
            List<byte> answer = new List<byte>();

            switch ((Operations)func)
            {
                case Operations.Login:
                    answer = Sign(request);
                    break;

                default:
                    break;

                //case 1:
                //    answer = Sotr(request);
                //    break;

                //case 2:
                //    answer = Predpr(request);
                //    break;

                //case 3:
                //    answer = SotrPredpr(request);
                //    break;

                //case 4:
                //    break;

            }
            return answer;
        }

        private List<byte> Sign(List<byte> buffer)
        {
            List<byte> answer = new List<byte>();
            Int32 lengthOfPass = (BitConverter.ToInt32(answer.ToArray(),0));
            answer.RemoveRange(0, sizeof(Int32));
            string password = buffer.ToArray().ToString();
            answer.Add(db.Login(password));
            return answer;
        }

        private List<byte> Sotr(List<byte> buffer)
        {
            return new List<byte>();
        }

        private List<byte> Predpr(List<byte> buffer)
        {
            return new List<byte>();
        }

        private List<byte> SotrPredpr(List<byte> buffer)
        {
            return new List<byte>();
        }

    }
}
