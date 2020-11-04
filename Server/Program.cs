
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static TcpListener listener= new TcpListener(IPAddress.Parse("127.0.0.1"),6000);
        static Dictionary<Socket, String> users = new Dictionary<Socket, string>();
        static void Main(string[] args)
        {
            listener.Start();
            Console.WriteLine("Сервер запущен!");
            byte[] buffer = new byte[4096];
            int count;
            while (true)
            {
                Socket s = listener.AcceptSocket();
                count = s.Receive(buffer);
                string nickname = Encoding.UTF8.GetString(buffer, 0, count);
                foreach (Socket u in users.Keys)
                    u.Send(Encoding.UTF8.GetBytes(nickname +" подключился к чату"));
                users.Add(s,nickname);
                Console.WriteLine(nickname + " подключился к чату");
                Task.Run(() => Receive(s));
            }
        }
        static void Receive(Socket s) 
        {
            byte[] buffer = new byte[4096];
            int count;
            try
            {
                while (true)
                {
                    count = s.Receive(buffer);
                    string message = users[s] + ">> " + Encoding.UTF8.GetString(buffer, 0, count);
                    foreach (Socket u in users.Keys)
                        u.Send(Encoding.UTF8.GetBytes(message));
                }
            }
            catch (SocketException)
            {
                users.Remove(s);
            }
        }
    }
}
