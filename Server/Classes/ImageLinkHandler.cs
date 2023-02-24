using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.EF;

namespace Server.Classes
{
    class ImageLinkHandler
    {
        public static byte[]? MakeImageRequest(string imageUrl)
        {
            return HandleImageRequest(imageUrl);
        }
        public static byte[]? HandleImageRequest(string url)
        {
            string[] args = url.Split('_'); //[typeimage, other parameters]
            if (args[0] == "msgimg")
            {
                string[] mArgs = { args[1], args[2] };
                return HandleMessageImage(mArgs);
            }
            if (args[0] == "usrimg")
            {
                string arg = args[1];
                return HandleUSerImage(arg);
            }
            return null;
        }

        public static byte[]? HandleUSerImage(string arg)
        {
            User u = AppData.context.User.FirstOrDefault(i => i.IdUser.ToString() == arg);
            if (u.IdUser.ToString() == arg)
            {
                return u.Photo;
            }
            return null;
        }

        public static byte[]? HandleMessageImage(string[] args) //[IdMessage, SendTime]
        {
            string id = args[0];
            string time = args[1];
            Message m = AppData.context.Message.FirstOrDefault(i => i.IdMessage.ToString() == id);
            if (m.IdMessage.ToString() == id)
            {
                if (m.SendTime.ToString() == time)
                {
                    return m.Photo;
                }
            }
            return null;
        }
    }
}
