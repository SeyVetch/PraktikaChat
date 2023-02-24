using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PraktikaChat.ClassHelper
{
    class MessageJson
    {
        public bool isFromSender { get; set; }
        public DateTime sendTime { get; set; }
        public string? imageId { get; set; }
        public string text { get; set; }
        //public MessageJson(MessageClass mc)
        //{
        //    isFromSender = mc.isFromSender;
        //    sendTime = mc.sendTime;
        //    imageId = mc.ImageId;
        //    text = mc.text;
        //}
        //public static List<MessageJson> transformList(List<MessageClass> M)
        //{
        //    List<MessageJson> res = new List<MessageJson>();
        //    foreach (MessageClass m in M)
        //    {
        //        res.Add(new MessageJson(m));
        //    }
        //    return res;
        //}
        //public static List<MessageJson> transformList(List<Message> M, int IdUser)
        //{
        //    List<MessageJson> res = new List<MessageJson>();
        //    foreach (Message m in M)
        //    {
        //        MessageClass ms = new MessageClass(m, IdUser);
        //        res.Add(new MessageJson(ms));
        //    }
        //    return res;
        //}
        public MessageJson(string Json)
        {
            string[] args = Json.Substring(1, Json.Length - 2).Split(',');
            string Text = args[0].Split('=')[1];
            Text = Text.Substring(1, Text.Length - 2);
            text = Text;
            string FromSender = args[1].Split('=')[1];
            FromSender = FromSender.Substring(1, FromSender.Length - 2);
            isFromSender = Convert.ToBoolean(Convert.ToInt32(FromSender));
            string link = args[2].Split('=')[1];
            link = link.Substring(1, link.Length - 2);
            imageId = link;
            string SendTime = args[3].Split('=')[1];
            SendTime = SendTime.Substring(1, SendTime.Length - 2);
            sendTime = Convert.ToDateTime(SendTime);
        }
        public static List<MessageJson> getList(string Json)
        {
            List<MessageJson> res = new List<MessageJson>();
            if (Json == "{}")
            {
                return null;
            }
            string[] args = Json.Substring(1, Json.Length - 2).Split(';');
            foreach (string json in args)
            {
                res.Add(new MessageJson(json));
            }
            return res;
        }
    }
}
