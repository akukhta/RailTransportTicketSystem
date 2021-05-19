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
            Login
        }

        public ClientConnection(string IpString = "127.0.0.1", int Port = 5564)
        {
            Ip = IPAddress.Parse(IpString);
            ServerEndPoint = new IPEndPoint(Ip, Port);
            ClientSocket = new Socket(Ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ClientSocket.Bind(ServerEndPoint);
        }

        private void SendToClient(byte[] Buffer)
        {
            ClientSocket.Send(Buffer);
        }

        private byte[] ReceiveForClient()
        {
            byte[] Buffer = new byte[ClientSocket.Available];
            ClientSocket.Receive(Buffer);
            return Buffer;
        }

        public UserType Login(string ID)
        {
            List<byte> buffer = new List<byte>();
            
            buffer.Add((byte)Operations.Login);
            buffer.AddRange(BitConverter.GetBytes(ID.Length));
            buffer.AddRange(Encoding.UTF8.GetBytes(ID));
            
            SendToClient(buffer.ToArray());
            
            byte[] answer = ReceiveForClient();

            switch (answer[0])
            {
                case 0:
                    return UserType.DefaultUser;
;
                case 1:
                    return UserType.Admin;

                case 2:
                    return UserType.Error;

                default:
                    return UserType.Error;
            }
        
        }
    }
}
