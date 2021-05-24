using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Server
{
    class Server
    {
        private Socket masterSocket;
        private IPAddress ip;
        private IPEndPoint connectionEndPoint;
        private List<Socket> connectedSockets;
        private Thread networkThread;
        private UIUpdater uIUpdater;
        private FunctionsHandler functionsHandler;

        public Server(UIUpdater uIUpdater, string ipAddr = "127.0.0.1", int port = 5564)
        {
            ip = IPAddress.Parse(ipAddr);
            connectionEndPoint = new IPEndPoint(ip, port);
            masterSocket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            masterSocket.Bind(connectionEndPoint);
            connectedSockets = new List<Socket>();
            connectedSockets.Add(masterSocket);
            this.uIUpdater = uIUpdater;
            functionsHandler = new FunctionsHandler();
        }

        public void start()
        {
            if (networkThread == null || networkThread.ThreadState != ThreadState.Running)
            {
                networkThread = new Thread(new ThreadStart(networkListeningThreadFunc));
                networkThread.Start();
            }
        }

        public void stop()
        {
            if (networkThread.ThreadState == ThreadState.Running)
            {
                networkThread.Abort();
                uIUpdater.LogMessage("Сервер выключен!");
            }
        }

        private void networkListeningThreadFunc()
        {
            masterSocket.Listen(100);
            List<Socket> sockets = connectedSockets;
            uIUpdater.LogMessage("Сервер включен!");
            while (true)
            {
                Socket.Select(sockets, null, null, -1);

                for (int i = 0; i < sockets.Count; i++)
                {
                    if (sockets[i] == masterSocket)
                    {
                        connectedSockets.Add(masterSocket.Accept());
                        uIUpdater.LogMessage((((IPEndPoint)connectedSockets.Last().RemoteEndPoint).Address.ToString() + " подключился!"));
                        uIUpdater.AddClient((((IPEndPoint)connectedSockets.Last().RemoteEndPoint).Address.ToString()));
                    }
                    else
                    {
                        byte[] buffer = new byte[sockets[i].Available];
                        sockets[i].Receive(buffer);
                        uIUpdater.LogMessage(buffer.ToString());
                        List<byte> answer = functionsHandler.HandleRequest(buffer.ToList());
                        sockets[i].Send(answer.ToArray());
                    }
                }
            }
        }
    }
}
