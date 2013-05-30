using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace wServer.realm.worlds
{
    public class Beachzone : World
    {
        public Beachzone()
        {
            Id = BEACHZONE_ID;
            Name = "Beachzone";
            Background = 0;
            base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.Beachzone.wmap"));
        }
    }
}
