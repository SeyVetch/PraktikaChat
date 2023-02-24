using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using Server.Classes;
using System.Text.Json;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint port = new IPEndPoint(address, 9999); 

            TcpServer server = new TcpServer(port);

            //TcpListener listener = new TcpListener(port);
            //Console.WriteLine(port.ToString());

            //listener.Start();

            //Console.WriteLine("--Server loaded--");

            //while (true) //loop forever
            //{

            //    Console.WriteLine("Waiting for New Client");
            //    Socket sock = listener.AcceptSocket();
            //    byte[] buffer = new byte[32];

            //    string incomingMessage = "";

            //    //read:
            //    while (sock.Available > 0)
            //    {
            //        int gotBytes = sock.Receive(buffer);
            //        incomingMessage += Encoding.ASCII.GetString(buffer, 0, gotBytes);

            //    }

            //    //debugging:
            //    Console.WriteLine(incomingMessage);

            //    //Now check whether its a GET or a POST

            //    if (incomingMessage.ToUpper().Contains("GET") && incomingMessage.ToUpper().Contains("/LOGIN")) //a search has been asked for
            //    {

            //        //sock.Send();

            //        //extracting the post data

            //        string htmlPostData = incomingMessage.Substring(incomingMessage.IndexOf("songName"));

            //        string[] parameters = htmlPostData.Split('&');

            //        string[] inputs = new string[5];

            //        for (int i = 0; i < parameters.Length; i++)
            //        {
            //            inputs[i] = (parameters[i].Split('='))[1];
            //            inputs[i] = inputs[i].Replace('+', ' ');
            //        }
            //    }
            //}//https://stackoverflow.com/questions/9742663/how-do-i-make-http-requests-to-a-tcp-server
        }
    }
}
