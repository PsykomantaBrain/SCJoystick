using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SCReceiverTest
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient listener = new UdpClient(8899);
            IPEndPoint ep = null;

            byte[] data = new byte[128];

            listener.JoinMulticastGroup(IPAddress.Parse("230.0.0.1"));

            Console.WriteLine("Waiting for data on 230.0.0.1:8899...");

            while (true)
            {

                data = listener.Receive(ref ep);
                uint id = BitConverter.ToUInt32(data, 0);


                Console.WriteLine("Received " + data.Length + " bytes. Packet ID: " + id + "");


                //System.Threading.Thread.Sleep(30);
            }
        }
    }
}
