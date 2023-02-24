using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.EF;

namespace Server.Classes
{
    public class AppData
    {
        public static Entities context { get; } = new Entities();
    }
}
