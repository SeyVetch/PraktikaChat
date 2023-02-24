using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.EF;

namespace Server.Classes
{
    class UserClass
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public byte[]? Photo { get; set; }
        public bool isOnline { get; set; }
        public string? ImageId { get; set; }
        public UserClass(User user)
        {
            IdUser = user.IdUser;
            Name = user.Name;
            Photo = user.Photo;
            isOnline = user.IsLoggedIn;
            ImageId = GetUrlOfUserImage(user);
        }
        public static List<UserClass> transformList(List<User> U)
        {
            List<UserClass> res = new List<UserClass>();
            foreach (User u in U)
            {
                res.Add(new UserClass(u));
            }
            return res;
        }
        public static string? GetUrlOfUserImage(User u)
        {
            bool hasImage = u.Photo != null;
            return hasImage ? "usrimg_" + u.IdUser.ToString() : null;
        }
        public static string ListToJson(List<UserClass> U)
        {
            string res = "{";
            List<string> usrs = new List<string>();
            foreach(UserClass u in U)
            {
                usrs.Add(u.ToJson());
            }
            res += string.Join(";", usrs);
            res += "}";
            return res;
        }
        public string ToJson()
        {
            string id = "\"IdUser\":\"" + IdUser + "\",";
            string name = "\"Name\":\"" + Name + "\",";
            string online = "\"isOnline\":\"" + Convert.ToInt32(isOnline) + "\",";
            string link = "\"ImageLink\":\"" + ImageId + "\"";
            return "{" + id + name + online + link + "}";
        }
    }
}
