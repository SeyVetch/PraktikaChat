using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PraktikaChat.ClassHelper
{
    public class UserClass
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public byte[]? Photo { get; set; }
        public bool isOnline { get; set; }
        public string ImageId { get; set; }
        public UserClass(string Json)
        {
            string[] args = Json.Substring(1, Json.Length - 2).Split(',');
            string id = args[0].Split(':')[1];
            id = id.Substring(1, id.Length - 2);
            IdUser = Convert.ToInt32(id);
            string name = args[1].Split(':')[1];
            Name = name.Substring(1, name.Length - 2);
            string online = args[2].Split(':')[1];
            online = online.Substring(1, online.Length - 2);
            string link = args[3].Split(':')[1];
            link = link.Substring(1, link.Length - 2);
            ImageId = link == "null" ? null : link;
            isOnline = Convert.ToBoolean(Convert.ToInt32(online));
            if (ImageId != null)
            {
                Photo = LinkHandler.GetImage(ImageId);
            }
        }
        public static UserClass[] arrayFromJson(string Json)
        {
            string[] args = Json.Substring(1, Json.Length - 2).Split(';');
            UserClass[] res = new UserClass[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                res[i] = new UserClass(args[i]);
            }
            return res;
        }
    }
}
