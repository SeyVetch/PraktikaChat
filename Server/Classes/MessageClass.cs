using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.EF;

namespace Server.Classes
{
    class MessageClass
    {
        private int idMessage;
        public bool isFromSender;
        public string text;
        public DateTime sendTime;
        public byte[]? image;
        public MessageClass(Message message, int idSender)
        {
            idMessage = message.IdMessage;
            isFromSender = idSender == message.IdUserSender;
            text = message.Text;
            sendTime = (DateTime)message.SendTime;
            image = message.Photo;
        }
        public bool HasImage { get { return !(image == null); } }
        public static string? GetUrlOfMessageImage(Message m)
        {
            bool hasImage = m.Photo != null;
            return hasImage ? "msgimg_" + m.IdMessage.ToString() + "_" + m.SendTime.ToString() : null;
        }
        public string? ImageId { get { return HasImage ? "msgimg_" + idMessage.ToString() + "_" + sendTime.ToString() : null; } }
        public byte[]? GetImage(string imgId) { return imgId == ImageId ? image : null; }
        public static List<MessageClass> transformList(List<Message> M, int idSender)
        {
            List<MessageClass> res = new List<MessageClass>();
            foreach (Message m in M)
            {
                res.Add(new MessageClass(m, idSender));
            }
            return res;
        }
    }
}
