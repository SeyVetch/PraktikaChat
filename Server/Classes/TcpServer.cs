using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server.EF;

namespace Server.Classes
{
    class TcpServer
    {
        private TcpListener _server;
        private Boolean _isRunning;
        private int count = 0;

        public TcpServer(IPEndPoint port)
        {
            _server = new TcpListener(port);
            _server.Start();

            _isRunning = true;

            LoopClients();
        }

        public void LoopClients()
        {
            while (_isRunning)
            {
                // wait for client connection
                if (_server.Pending())
                {
                    TcpClient newClient = _server.AcceptTcpClient();

                    // client found.
                    // create a thread to handle communication
                    Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                    t.Start(newClient);
                }
            }
        }

        public void SendImage(byte[] img, TcpClient client)
        {
            Stream stream = client.GetStream();
            StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);

            int bufferSize = 1024 * 2; //2MB
            int imgSize = img.Length;
            int packages = imgSize / bufferSize;
            string imgSpecs = bufferSize + " " + imgSize;
            sWriter.WriteLine(imgSpecs);
            sWriter.Flush();

            int pakageNum = Convert.ToInt32(sReader.ReadLine());
            while(pakageNum < packages)
            {
                stream.Write(img, pakageNum * bufferSize, bufferSize);
                stream.Flush();
                Console.WriteLine(pakageNum + " Package sent");

                pakageNum = Convert.ToInt32(sReader.ReadLine());
            }
            count++;
        }

        public void HandleClient(object obj)
        {
            // retrieve client from parameter passed to thread
            TcpClient client = (TcpClient)obj;

            // sets two streams
            StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
            // you could use the NetworkStream to read and write, 
            // but there is no forcing flush, even when requested

            Boolean bClientConnected = true;
            String sData = null;

            bClientConnected = client.Connected;
            // reads from stream
            sData = sReader.ReadLine();

            // shows content on the console.
            Console.WriteLine("Client > " + sData);
            string[] args = sData.Substring(1).Split('/');
            string json = "";
            if (args[0].ToLower() == "login")
            {
                string[] loginData = args[1].Split('&');
                string l = loginData[0].Split('=')[1];
                string p = loginData[1].Split('=')[1];
                User u = AppData.context.User.Where(i => i.Name == l && i.Password == p).First();
                json = new UserClass(u).ToJson();
                sWriter.WriteLine(json);
                Console.WriteLine("Response > " + json);
            }
            if (args[0].ToLower() == "trylogin")
            {
                string[] loginData = args[1].Split('&');
                string l = loginData[0].Split('=')[1];
                string p = loginData[1].Split('=')[1];
                User u = AppData.context.User.Where(i => i.Name == l && i.Password == p).FirstOrDefault();
                json = "{\"Success\":\"" + (u.Name == l) + "\"}";
                sWriter.WriteLine(json);
                Console.WriteLine("Response > " + json);
            }
            if (args[0].ToLower() == "loginexists")
            {
                string[] loginData = args[1].Split('&');
                string l = loginData[0].Split('=')[1];
                User u = AppData.context.User.FirstOrDefault(i => i.Name == l);
                json = "{\"Success\":\"" + (u.Name == l) + "\"}";
                sWriter.WriteLine(json);
                Console.WriteLine("Response > " + json);
            }
            if (args[0].ToLower() == "register")
            {
                string[] loginData = args[1].Split('&');
                string l = loginData[0].Split('=')[1];
                string p = loginData[1].Split('=')[1];
                if (AppData.context.User.Where(i => i.Name == l).ToArray().Length == 0)
                {
                    bool isImageAttached = Convert.ToBoolean(loginData[2].Split('=')[1]);
                    User newUser = new User
                    {
                        Name = l,
                        Password = p
                    };
                    if (isImageAttached)
                    {
                        Stream stream = client.GetStream();
                        Console.WriteLine("GetImage");
                        string imgSpecs = sReader.ReadLine();
                        Console.WriteLine(imgSpecs);

                        string[] imgargs = imgSpecs.Split();
                        int imgSize = Convert.ToInt32(imgargs[1]);
                        int bufferSize = Convert.ToInt32(imgargs[0]);
                        byte[] res = new byte[imgSize];
                        int pakages = imgSize / bufferSize;
                        int pakageNum = 0;

                        while (pakageNum < pakages)
                        {
                            sWriter.WriteLine(pakageNum);
                            sWriter.Flush();

                            int read = stream.Read(res, pakageNum * bufferSize, bufferSize);
                            if (read == bufferSize)
                            {
                                pakageNum++;
                            }
                            Console.WriteLine(pakageNum + " Package recieved");
                        }
                        sWriter.WriteLine(pakageNum);
                        sWriter.Flush();
                        newUser.Photo = res;
                    }
                    AppData.context.User.Add(newUser);
                    AppData.context.SaveChanges();
                    json = "{\"Success\":\"true\"}";
                    count++;
                }
                else
                {
                    json = "{\"Success\":\"false\"}";
                }
                sWriter.WriteLine(json);
                sWriter.Flush();
                Console.WriteLine("Response > " + json);
            }
            if (args[0].ToLower() == "getmsgs")
            {
                string[] loginData = args[1].Split('&');
                string l = loginData[0].Split('=')[1];
                string p = loginData[1].Split('=')[1];
                int id1 = Convert.ToInt32(loginData[2].Split('=')[1]);
                User u = AppData.context.User.Where(i => i.Name == l && i.Password == p).First();
                int id2 = u.IdUser;
                List<Message> messages = AppData.context.Message.Where(i => (i.IdUserReciever == id1 && i.IdUserSender == id2) 
                || (i.IdUserReciever == id2 && i.IdUserSender == id1)).ToList();
                json = MessageJson.ListToJson(MessageClass.transformList(messages, id2));
                sWriter.WriteLine(json);
                Console.WriteLine("Response > " + json);
            }
            if (args[0].ToLower() == "sendmsg")
            {
                string[] loginData = args[1].Split('&');
                int s = Convert.ToInt32(loginData[0].Split('=')[1]);
                int r = Convert.ToInt32(loginData[1].Split('=')[1]);
                string t = loginData[2].Split('=')[1];
                bool isImageAttached = Convert.ToBoolean(loginData[3].Split('=')[1]);
                Message newMessage = new Message
                {
                    IdUserSender = s,
                    IdUserReciever = r,
                    Text = t,
                    SendTime = DateTime.Now
                };
                if (isImageAttached)
                {
                    Stream stream = client.GetStream();
                    Console.WriteLine("GetImage");
                    string imgSpecs = sReader.ReadLine();
                    Console.WriteLine(imgSpecs);

                    string[] imgargs = imgSpecs.Split();
                    int imgSize = Convert.ToInt32(imgargs[1]);
                    int bufferSize = Convert.ToInt32(imgargs[0]);
                    byte[] res = new byte[imgSize];
                    int pakages = imgSize / bufferSize;
                    int pakageNum = 0;

                    while (pakageNum < pakages)
                    {
                        sWriter.WriteLine(pakageNum);
                        sWriter.Flush();

                        int read = stream.Read(res, pakageNum * bufferSize, bufferSize);
                        if (read == bufferSize)
                        {
                            pakageNum++;
                        }
                        Console.WriteLine(pakageNum + " Package recieved");
                    }
                    sWriter.WriteLine(pakageNum);
                    sWriter.Flush();
                    newMessage.Photo = res;
                    count++;
                }
                AppData.context.Message.Add(newMessage);
                AppData.context.SaveChanges();
                json = "{\"Success\":\"true\"}";
                sWriter.WriteLine(json);
                sWriter.Flush();
                Console.WriteLine("Response > " + json);
            }
            if (args[0].ToLower() == "users")
            {
                List<UserClass> u = UserClass.transformList(AppData.context.User.ToList());
                json = UserClass.ListToJson(u);
                sWriter.WriteLine(json);
                sWriter.Flush();
                Console.WriteLine("Response > " + json);
            }
            if (args[0].ToLower() == "image")
            {
                byte[]? img = ImageLinkHandler.HandleImageRequest(args[1]);
                if (img == null)
                {
                    sWriter.WriteLine("null");
                    sWriter.Flush();
                    Console.WriteLine("Response > " + "null");
                }
                else
                {
                    SendImage(img, client);
                    Console.WriteLine("Response > " + img);
                    sWriter.Flush();
                }
            }
            count++;
            
            if (count >= 50)
            {
                Console.Clear();
                count = 0;
            }

            sWriter.Flush();
            client.Close();
        }//https://mikeadev.net/2012/07/multi-threaded-tcp-server-in-csharp/
    }
}
