﻿using System;
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
            CreateRequest
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
        public void CreateRequest(BussinesTripInfo info)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add((byte)Operations.CreateRequest);
            bytes.AddRange(info.serialise());
            SendToClient(bytes.ToArray());
        }
    }
}
