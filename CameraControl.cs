using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MultiFaceRec
{
    class CameraControl
    {
        private int port;
        private string IPAddress;

        public CameraControl(string IPAddress, int port)
        {
            this.IPAddress = IPAddress;
            this.port = port;
        }

        public void SendData(string command)
        {
            try
            {
                using (TcpClient client = new TcpClient(IPAddress, port))
                {
                    byte[] message = System.Text.Encoding.ASCII.GetBytes(command + 0x0D);

                    NetworkStream stream = client.GetStream();

                    stream.Write(message, 0, message.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception connecting to Tcp client: {e}");
            }
        }

    }
}
