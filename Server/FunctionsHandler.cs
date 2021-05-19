using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class FunctionsHandler
    {
        public List<byte> HandleRequest(List<byte> request)
        {
            byte func = request[0];
            request.RemoveAt(0);
            List<byte> answer;

            switch (func)
            {
                case 0:
                    answer = Sign(request);
                    break;

                case 1:
                    answer = Sotr(request);
                    break;

                case 2:
                    answer = Predpr(request);
                    break;

                case 3:
                    answer = SotrPredpr(request);
                    break;

                case 4:
                    break

            }
        }

        private List<byte> Sign(List<byte> buffer)
        {
            return new List<byte>();
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
