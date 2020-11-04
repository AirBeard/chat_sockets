using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Client
{
    
    class Program
    {
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            Console.Write("Введите ник: ");
            string nickname = Console.ReadLine();
            socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000));
            socket.Send(Encoding.UTF8.GetBytes(nickname));
            Task.Run(Receive);
            while(true)
            {
                string message = Console.ReadLine();
                if (message != "")
                    socket.Send(Encoding.UTF8.GetBytes(message));
            }
        }
        static void Receive()
        {
            byte[] buffer = new byte[4096];
            int count;
            try
            {
                while (true)
                {
                    count = socket.Receive(buffer);
                    Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, count));
                }
            }
            catch (SocketException)
            {
                socket.Close();
            }
        }
    }
}
