using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace wServer.realm.worlds
{
    public class Gee : World //map creating tutorial world
    {
        public Gee()
        {
            Id = GEE_ID;
            Name = "Gee";
            Background = 0;
            base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.ggtutorial.wmap"));
        }
    }
}
