﻿using System;
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
            Login,
            CreateRequest
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

                case Operations.CreateRequest:
                    answer = CreateRequest(request);
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
            Int32 lengthOfPass = (BitConverter.ToInt32(buffer.ToArray(),0));
            buffer.RemoveRange(0, sizeof(Int32));
            string password = Encoding.Default.GetString(buffer.ToArray());
            byte[] answer = db.Login(password).serialise();
            return answer.ToList();
        }

        private List<byte> CreateRequest(List<byte> buffer)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(0x00);
            BussinesTripInfo info = BussinesTripInfo.deserialise(buffer);
            DocumetGeneration.GenerateDocument(info);
            return bytes;
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
