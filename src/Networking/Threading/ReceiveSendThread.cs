using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SupercellProxy
{
    class ReceiveSendThread : IDisposable
    {
        public Socket ClientSocket, ServerSocket;

        /// <summary>
        /// Async send/receive thread constructor
        /// </summary>
        public ReceiveSendThread(Socket cs, Socket ss)
        {
            this.ClientSocket = cs;
            this.ServerSocket = ss;

            try
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    ClientState clientState = new ClientState() { socket = ClientSocket };
                    ServerState serverState = new ServerState() { socket = ServerSocket };

                    ServerSocket.BeginReceive(serverState.buffer, 0, State.BufferSize, 0, new AsyncCallback(DataReceived), serverState);
                    ClientSocket.BeginReceive(clientState.buffer, 0, State.BufferSize, 0, new AsyncCallback(DataReceived), clientState);
                }));
            }
            catch(Exception ex)
            {
                Logger.Log("Exception during the client/server receive procedure (" + ex.GetType() + ")!", LogType.EXCEPTION);
                Logger.Log("Please check if you changed state/buffer variables.", LogType.EXCEPTION);
                Program.WaitAndClose();
            }
        }

        /// <summary>
        /// DataReceive callback
        /// </summary>
        private void DataReceived(IAsyncResult ar)
        {
            try
            {
                State state = (State)ar.AsyncState;
                Socket socket = state.socket;
                int bytesReceived = socket.EndReceive(ar);
                Handle(bytesReceived, socket, state);
                socket.BeginReceive(state.buffer, 0, State.BufferSize, 0, new AsyncCallback(DataReceived), state);
            }
            catch(Exception ex)
            {
                Logger.Log("Exception during the client/server receive procedure (" + ex.GetType() + ")!", LogType.EXCEPTION);
                Logger.Log("1. Either client or server disconnected the proxy because of OutOfSync packet(s)", LogType.EXCEPTION);
                Logger.Log("2. The client disconnected under weird circumstances", LogType.EXCEPTION);
                Logger.Log("3. Other manual packet modifications cause this", LogType.EXCEPTION);
                Program.WaitAndClose();
            }
        }

        /// <summary>
        /// Handles the received data
        /// </summary>
        private void Handle(int bytesReceived, Socket socket, State state)
        {
            int bytesRead = 0;
            int payloadLength, bytesAvailable, bytesNeeded;
            while (bytesRead < bytesReceived)
            {
                bytesAvailable = bytesReceived - bytesRead;
                if (bytesReceived > 0)
                {
                    if (state.packet.Length >= 7)
                    {
                        payloadLength = BitConverter.ToInt32(new byte[1].Concat(state.packet.Skip(2).Take(3)).Reverse().ToArray(), 0);
                        bytesNeeded = payloadLength - (state.packet.Length - 7);
                        if (bytesAvailable >= bytesNeeded)
                        {
                            state.packet = state.packet.Concat(state.buffer.Skip(bytesRead).Take(bytesNeeded)).ToArray();
                            bytesRead += bytesNeeded;
                            bytesAvailable -= bytesNeeded;

                            if (state.GetType() == typeof(ClientState))
                            {
                                Packet ClientPacket = new Packet(state.packet, PacketDestination.FROM_CLIENT);
                                ClientPacket.Export();                           
                                Logger.Log(ClientPacket.ID + " (" + ClientPacket.DecryptedPayload.Length + " bytes)", LogType.PACKET);                      
                                ServerSocket.Send(ClientPacket.Rebuilt);
                                ClientPacket = null;
                            }
                            else if (state.GetType() == typeof(ServerState))
                            {
                                Packet ServerPacket = new Packet(state.packet, PacketDestination.FROM_SERVER);
                                ServerPacket.Export();
                                Logger.Log(ServerPacket.ID + " (" + ServerPacket.DecryptedPayload.Length + " bytes)", LogType.PACKET);
                                ClientSocket.Send(ServerPacket.Rebuilt);
                                ServerPacket = null;
                            }         
                            state.packet = new byte[0];
                        }
                        else
                        {
                            state.packet = state.packet.Concat(state.buffer.Skip(bytesRead).Take(bytesAvailable)).ToArray();
                            bytesRead = bytesReceived;
                            bytesAvailable = 0;
                        }
                    }
                    else if (bytesAvailable >= 7)
                    {
                        state.packet = state.packet.Concat(state.buffer.Skip(bytesRead).Take(7)).ToArray();
                        bytesRead += 7;
                        bytesAvailable -= 7;
                    }
                    else
                    {
                        state.packet = state.packet.Concat(state.buffer.Skip(bytesRead).Take(bytesAvailable)).ToArray();
                        bytesRead = bytesReceived;
                        bytesAvailable = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Memory-friendly dispose method
        /// </summary>
        public virtual void Dispose()
        {
            ClientSocket.Disconnect(false);
            ServerSocket.Disconnect(false);
            GC.SuppressFinalize(this);
        }
    }
}