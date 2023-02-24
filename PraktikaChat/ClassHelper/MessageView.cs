using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PraktikaChat.ClassHelper
{
    class MessageView
    {
        public bool isFromSender;
        public DateTime SendTime { get; set; }
        public string? imageId;
        public string Text { get; set; }
        public byte[] Photo { get; set; }
        public string Sender { get; set; }
        public Brush brush { get; set; }
        public MessageView(MessageJson m)
        {
            isFromSender = m.isFromSender;
            SendTime = m.sendTime;
            imageId = m.imageId;
            Text = m.text;
            if (imageId != null)
            {
                Photo = FetchImage;
            }
            else
            {
                Photo = null;
            }
            if (isFromSender)
            {
                brush = Brushes.LightBlue;
            }
            else
            {
                brush = Brushes.LightGray;
            }
        }
        public static List<MessageView> TransfromList(List<MessageJson> MJ)
        {
            List<MessageView> res = new List<MessageView>();
            if (MJ == null)
            {
                return null;
            }
            foreach(MessageJson mj in MJ)
            {
                res.Add(new MessageView(mj));
            }
            return res;
        }
        public byte[]? FetchImage
        {
            get
            {
                return LinkHandler.GetImage(imageId);
            }
        }
    }
}
