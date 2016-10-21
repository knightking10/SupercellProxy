using System;
using System.Net;
using System.Net.Sockets;

namespace SupercellProxy
{
    class Client
    {
        public Socket ClientSocket, ServerSocket;
        private readonly string Host = Config.Host;

        /// <summary>
        /// Client constructor
        /// </summary>
        public Client(Socket s)
        {
            ClientSocket = s;
        }

        /// <summary>
        /// Enqueues the client
        /// </summary>
        public void Enqueue()
        {
            try
            {
                // Connect to the official supercell server
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Host);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, 9339);
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ServerSocket.Connect(remoteEndPoint);

                // Start async recv/send procedure
                Logger.Log("Proxy attached to " + Host + " (" + ServerRemoteAdr + ")!");
                Logger.Log("Starting async recv/send thread..");
                Console.Write(Environment.NewLine);

                new ReceiveSendThread(ClientSocket, ServerSocket);
            }
            catch(Exception ex)
            {
                Logger.Log("Failed to enqueue client (" + ClientRemoteAdr + ")!");
                Logger.Log("Check if you have a working internet connection.");
                Program.WaitAndClose();
            }            
        }

        /// <summary>
        /// Dequeues the client
        /// </summary>
        public void Dequeue()
        {
            ClientSocket.Disconnect(false);
            ServerSocket.Disconnect(false);
        }
  
        /// <summary>
        /// Client IP-address
        /// </summary>
        public IPAddress ClientRemoteAdr
        {
            get
            {
                return ((IPEndPoint)ClientSocket.RemoteEndPoint).Address;
            }
        }

        /// <summary>
        /// Clash Royale Server IP-address
        /// </summary>
        public IPAddress ServerRemoteAdr
        {
            get
            {
                return ((IPEndPoint)ServerSocket.RemoteEndPoint).Address;
            }
        }
    }
}
