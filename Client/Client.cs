using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientConnection
    {
        private Socket ClientSocket;
        private IPAddress Ip;
        private IPEndPoint ServerEndPoint;
        
        public enum UserType
        {
            DefaultUser,
            Admin,
            Error
        }

        public enum Operations : byte
        {
            Login, 
            CreateRequest,
            GetUsers,
            GetFactories,
            GetEmployeesFactories,
            GetDocuments,
            AddFactory
        }

        public ClientConnection(string IpString = "127.0.0.1", int Port = 5564)
        {
            Ip = IPAddress.Parse(IpString);
            ServerEndPoint = new IPEndPoint(Ip, Port);
            ClientSocket = new Socket(Ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ClientSocket.Blocking = true;
            ClientSocket.Connect(ServerEndPoint);
        }

        private void SendToClient(byte[] Buffer)
        {
            ClientSocket.Send(Buffer);
        }

        private byte[] ReceiveForClient()
        {
            byte[] packetSizeBuffer = new byte[4];
            ClientSocket.Receive(packetSizeBuffer);
            Int32 packetSize = BitConverter.ToInt32(packetSizeBuffer,0);
            byte[] Buffer = new byte[packetSize];
            ClientSocket.Receive(Buffer);
            return Buffer;
        }


        public List<User> GetUsers()
        {
            List<byte> buffer = new List<byte>();
            List<User> users = new List<User>();
            buffer.Add((byte)Operations.GetUsers);

            SendToClient(buffer.ToArray());

            List<byte> answer = ReceiveForClient().ToList();
            Int32 usersCount = BitConverter.ToInt32(answer.ToArray(), 0);
            answer.RemoveRange(0, sizeof(Int32));

            for (int i = 0; i < usersCount; i++)
            {
                users.Add(User.deserialise(answer));
            }

            return users;
        }

        public User Login(string ID)
        {
            List<byte> buffer = new List<byte>();
            
            buffer.Add((byte)Operations.Login);
            buffer.AddRange(BitConverter.GetBytes(ID.Length));
            buffer.AddRange(Encoding.UTF8.GetBytes(ID));
            
            SendToClient(buffer.ToArray());
            
            byte[] answer = ReceiveForClient();

            return User.deserialise(answer.ToList());
        }
        
        /// <summary>
        /// Only for test, for future the request has to be approved. But for now simple generation is also good.
        /// </summary>
        public List<byte> CreateRequest(BussinesTripInfo info)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add((byte)Operations.CreateRequest);
            bytes.AddRange(info.serialise());
            SendToClient(bytes.ToArray());
            List<byte> file = ReceiveForClient().ToList();
            return file;
        }

        public List<FactoryInfo> GetFactories()
        {
            List<byte> bytes = new List<byte>();
            bytes.Add((byte)Operations.GetFactories);
            SendToClient(bytes.ToArray());
            List<byte> answer = ReceiveForClient().ToList();
            Int32 FactoriesCount = BitConverter.ToInt32(answer.ToArray(), 0);
            answer.RemoveRange(0, sizeof(Int32));

            List<FactoryInfo> factories = new List<FactoryInfo>();

            for (int i = 0; i < FactoriesCount; i++)
            {
                factories.Add(FactoryInfo.deserialise(answer));
            }

            return factories;
        }

        public List<EmployeesFactoryInfo> GetEmployeesFactories()
        {
            List<byte> bytes = new List<byte>();
            bytes.Add((byte)Operations.GetEmployeesFactories);
            SendToClient(bytes.ToArray());
            List<byte> answer = ReceiveForClient().ToList();
            Int32 employeesFactoriesCount = BitConverter.ToInt32(answer.ToArray(), 0);
            answer.RemoveRange(0, sizeof(Int32));

            List<EmployeesFactoryInfo> employeesFactories = new List<EmployeesFactoryInfo>();

            for (int i = 0; i < employeesFactoriesCount; i++)
            {
                employeesFactories.Add(EmployeesFactoryInfo.deserialise(answer));
            }

            return employeesFactories;
        }


        public void AddFactory(User user,FactoryInfo info)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add((byte)Operations.AddFactory);
            buffer.AddRange(BitConverter.GetBytes(user.userType));
            buffer.AddRange(BitConverter.GetBytes(user.factoryID));
            buffer.AddRange(info.serialise());
            SendToClient(buffer.ToArray());
            ReceiveForClient();
        }

        public List<BussinesTripInfo> GetDocuments()
        {
            List<byte> bytes = new List<byte>();
            bytes.Add((byte)Operations.GetDocuments);
            SendToClient(bytes.ToArray());
            List<byte> answer = ReceiveForClient().ToList();
            Int32 documentsCount = BitConverter.ToInt32(answer.ToArray(), 0);
            answer.RemoveRange(0, sizeof(Int32));

            List<BussinesTripInfo> documents = new List<BussinesTripInfo>();

            for (int i = 0; i < documentsCount; i++)
            {
                documents.Add(BussinesTripInfo.deserialise(answer));
            }

            return documents;
        }
    }
}
