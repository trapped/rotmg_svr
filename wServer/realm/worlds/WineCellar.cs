using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace wServer.realm.worlds
{
    public class WineCellar : World
    {
        public WineCellar()
        {
            Id = WINECELLAR_ID;
            Name = "Wine Cellar";
            Background = 0;
            base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.WineCellar.wmap"));
        }
    }
}
