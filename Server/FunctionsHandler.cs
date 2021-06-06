using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Server
{
    class FunctionsHandler
    {
        public enum Operations : byte
        {
            Login,
            CreateRequest,
            GetUsersFromDB,
            GetFactories,
            GetEmployesFactories,
            GetDocuments,
            AddFactory,
            DeleteFactory,
            DeleteUser,
            DeleteUserFactory,
            AddUser,
            AddFactoryUser
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

                case Operations.GetUsersFromDB:
                    answer = GetEmployees(request);
                    break;

                case Operations.GetFactories:
                    answer = GetFactories();
                    break;
                case Operations.GetEmployesFactories:
                    answer = GetEmployeesFactories();
                    break;
                case Operations.GetDocuments:
                    answer = GetDocuments();
                    break;
                case Operations.AddFactory:
                    AddFactory(request);
                    answer = new List<byte>();
                    answer.Add(0x0);
                    break;
                case Operations.DeleteFactory:
                    DeleteFactory(request);
                    answer = new List<byte>();
                    answer.Add(0x0);
                    break;
                case Operations.DeleteUser:
                    DeleteUser(request);
                    answer = new List<byte>();
                    answer.Add(0x0);
                    break;
                case Operations.DeleteUserFactory:
                    DeleteSotrPredpr(request);
                    answer = new List<byte>();
                    answer.Add(0x0);
                    break;
                case Operations.AddUser:
                    AddUser(request);
                    answer = new List<byte>();
                    answer.Add(0x0);
                    break;
                case Operations.AddFactoryUser:
                    AddFactoryUser(request);
                    answer = new List<byte>();
                    answer.Add(0x0);
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
            Int32 lengthOfPass = (BitConverter.ToInt32(buffer.ToArray(), 0));
            buffer.RemoveRange(0, sizeof(Int32));
            string password = Encoding.Default.GetString(buffer.ToArray());
            byte[] answer = db.Login(password).serialise();
            return answer.ToList();
        }

        private List<byte> CreateRequest(List<byte> buffer)
        {
            List<byte> bytes = new List<byte>();
            BussinesTripInfo info = BussinesTripInfo.deserialise(buffer);

            db.AddDocument(info);

            string filename = DocumetGeneration.GenerateDocument(info);

            bytes = File.ReadAllBytes(filename).ToList();

            return bytes;
        }

        private List<byte> GetEmployees(List<byte> buffer)
        {
            List<byte> users = new List<byte>();

            List<User> _users = db.getUsers();
            Int32 usersCount = _users.Count;
            users.AddRange(BitConverter.GetBytes(usersCount));

            for (int i = 0; i < _users.Count; i++)
            {
                users.AddRange(_users[i].serialise());
            }

            return users.ToList();
        }

        private List<byte> GetFactories()
        {
            List<byte> answer = new List<byte>();

            List<FactoryInfo> factories = db.GetFactories();
            Int32 factoriesCount = factories.Count;
            answer.AddRange(BitConverter.GetBytes(factoriesCount));

            for (int i = 0; i < factories.Count; i++)
            {
                answer.AddRange(factories[i].serialise());
            }

            return answer;
        }

        private List<byte> GetEmployeesFactories()
        {
            List<byte> answer = new List<byte>();

            List<EmployeesFactoryInfo> employeesFactoryInfos = db.GetEmployeesFactories();
            Int32 count = employeesFactoryInfos.Count;
            answer.AddRange(BitConverter.GetBytes(count));

            for (int i = 0; i < employeesFactoryInfos.Count; i++)
            {
                answer.AddRange(employeesFactoryInfos[i].serialise());
            }

            return answer;
        }

        private List<byte> GetDocuments()
        {
            List<byte> answer = new List<byte>();

            List<BussinesTripInfo> documents = db.GetDocuments();
            Int32 count = documents.Count;
            answer.AddRange(BitConverter.GetBytes(count));

            for (int i = 0; i < documents.Count; i++)
            {
                answer.AddRange(documents[i].serialise());
            }

            return answer;
        }

        private void AddFactory(List<byte> buffer)
        {
            int userType = Utilites.readIntFromBuffer(buffer);
            int predprID = Utilites.readIntFromBuffer(buffer);
            FactoryInfo newFactory = FactoryInfo.deserialise(buffer);
            db.AddToTable(newFactory, "Предприятия");

            //if (userType == 1)
            //{
            //    db.AddToTable(newFactory, "Предприятия");
            //}
            //else
            //{
            //    db.AddToChangesTable(null, newFactory, 0, "Предприятия", predprID);
            //}

        }

        private void AddFactoryUser(List<byte> buffer)
        {
            User user = User.deserialise(buffer);
            FactoryInfo info = FactoryInfo.deserialise(buffer);
            
            user.factoryID = info.predprID;

            db.AddToTable(user, "Сотрудники-Предприятия");
        }

        private void DeleteFactory(List<byte> buffer)
        {
            FactoryInfo deletingFactory = FactoryInfo.deserialise(buffer);

            db.DeleteFactory(deletingFactory);
        }

        private void DeleteUser(List<byte> buffer)
        {
            User user = User.deserialise(buffer);

            db.DeleteSotr(user);
        }

        private void AddUser(List<byte> request)
        {
            User user = User.deserialise(request);
            db.AddToTable(user, "Сотрудники");
        }

        private void DeleteSotrPredpr(List<byte> buffer)
        {
            FactoryInfo factoryInfo = FactoryInfo.deserialise(buffer);
            User user = User.deserialise(buffer);

            db.DeleteFactoryUser(factoryInfo, user);
        }
    }
}
