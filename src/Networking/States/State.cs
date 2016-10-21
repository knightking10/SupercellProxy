using System.Net.Sockets;

namespace SupercellProxy
{
    class State
    {
        public Socket socket;
        public const int BufferSize = 2048;
        public byte[] buffer = new byte[BufferSize];
        public byte[] packet = new byte[0];
    }
}
