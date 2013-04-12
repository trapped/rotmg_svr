using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace wServer.realm.worlds
{
    public class Kitchen : World
    {
        public Kitchen()
        {
            Name = "Kitchen";
            Background = 0;
            base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.kitchen.wmap"));
        }
    }
}
