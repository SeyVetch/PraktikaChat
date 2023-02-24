using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace PraktikaChat.ClassHelper
{
    class LinkHandler
    {
        static string adr = "http://127.0.0.1:9999";
        static IPAddress address = IPAddress.Parse("127.0.0.1");
        static IPEndPoint port = new IPEndPoint(address, 9999);

        static public Dictionary<string, byte[]?> images = new Dictionary<string, byte[]>();

        static ClientDemo client;
        private static bool connect(IPEndPoint p)
        {
            client = new ClientDemo(p);
            return client._isConnected;
        }
        public static byte[] GetImage(string url)
        {
            if (images.Keys.Contains(url))
            {
                return images[url];
            }
            bool connected = connect(port);
            if (!connected)
            {
                return null;
            }
            byte[] response = client.GetImage("/Image/" + url);
            images.Add(url,response);
            return response;
        }
        public static UserClass[] GetUsers()
        {
            bool connected = connect(port);
            if (!connected)
            {
                return null;
            }
            string link = "/Users";
            string response = client.GetJson(link);
            if (response == "")
            {
                return null;
            }
            UserClass[] res = UserClass.arrayFromJson(response);
            return res;
        }
        public static UserClass? LogIn(string log, string pas)
        {
            bool connected = connect(port);
            if (!connected)
            {
                return null;
            }
            string parameters = "/log=" + log + "&pas=" + pas;
            string link = "/Login" + parameters;
            string response = client.GetJson(link);
            if (response == "")
            {
                return null;
            }
            UserClass res = new UserClass(response);
            return res;
        }
        public static bool Register(string log, string pas, byte[]? img = null)
        {
            bool connected = connect(port);
            if (!connected)
            {
                return false;
            }
            bool response = client.Register(log, pas, img);
            return response;
        }
        public static bool SendMessage(int idSender, int idReciever, string text, byte[]? img = null)
        {
            bool connected = connect(port);
            if (!connected)
            {
                return false;
            }
            bool response = client.SendMessage(idSender, idReciever, text, img);
            return response;
        }
        public static List<MessageView> GetMessages(string log, string pas, int idOtherUser)
        {
            bool connected = connect(port);
            if (!connected)
            {
                return null;
            }
            string parameters = "/log=" + log + "&pas=" + pas + "&idUser=" + idOtherUser;
            string link = "/GetMsgs" + parameters;
            string response = client.GetJson(link);
            if (response == "")
            {
                return null;
            }
            List<MessageView> res = MessageView.TransfromList(MessageJson.getList(response));
            return res;
        }
        public static bool TryLogIn(string log, string pas)
        {
            bool connected = connect(port);
            string parameters = "/log=" + log + "&pas=" + pas;
            string link = "/TryLogin" + parameters;
            string res = client.GetJson(link);
            if (res == "")
            {
                return false;
            }
            res = res.Substring(1, res.Length - 2).Split(':')[1];
            return Convert.ToBoolean(res.Substring(1, res.Length - 2));
        }
        public static bool LoginExists(string log)
        {
            bool connected = connect(port);
            string parameters = "/log=" + log;
            string link = "/loginexists" + parameters;
            string res = client.GetJson(link);
            if (res == "")
            {
                return false;
            }
            res = res.Substring(1, res.Length - 2).Split(':')[1];
            return Convert.ToBoolean(res.Substring(1, res.Length - 2));
        }
    }
}
