using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using wServer.realm;

namespace svrMonitor
{
    public static class Mon
    {
        internal static Monitor mon;
        public static void Show()
        {
            Application.Run(mon = new Monitor());
        }
        public static void Tick(RealmTime time)
        {
            if (mon != null)
                mon.Tick(time);
        }
    }
}
