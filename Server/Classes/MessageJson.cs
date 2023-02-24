using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.EF;

namespace Server.Classes
{
    class MessageJson
    {
        public bool isFromSender;
        public DateTime sendTime;
        public string? imageId;
        public string text;
        public MessageJson(MessageClass mc)
        {
            isFromSender = mc.isFromSender;
            sendTime = mc.sendTime;
            imageId = mc.ImageId;
            text = mc.text;
        }
        public static List<MessageJson> transformList(List<MessageClass> M)
        {
            List<MessageJson> res = new List<MessageJson>();
            foreach (MessageClass m in M)
            {
                res.Add(new MessageJson(m));
            }
            return res;
        }
        public string ToJson()
        {
            string Text = "\"Text\"=\"" + text + "\",";
            string FromSender = "\"isFromSender\"=\"" + Convert.ToInt32(isFromSender) + "\",";
            string link = "\"ImageLink\"=\"" + imageId + "\",";
            string SendTime = "\"SendTime\"=\"" + sendTime.ToString() + "\"";
            return "{" + Text + FromSender + link + SendTime + "}";
        }
        public static string ListToJson(List<MessageClass> M)
        {
            string res = "{";
            List<string> json = new List<string>();
            foreach(MessageClass m in M)
            {
                MessageJson mj = new MessageJson(m);
                json.Add(mj.ToJson());
            }
            res += string.Join(";", json);
            res += "}";
            return res;
        }
    }
}
