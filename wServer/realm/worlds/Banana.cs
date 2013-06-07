using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace wServer.realm.worlds
{
    public class Banana : World
    {
        public Banana()
        {
            Id = -23;
            Name = "Banana";
            Background = 0;
            base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.banana.wmap"));
        }
    }
}
