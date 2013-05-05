using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using terrain;

namespace wServer.realm.worlds
{
    class Zeta : World
    {
        public Zeta()
        {
            Id = ZETA_ID;
            Name = "Zeta";
            Background = 0;
            FromWorldMap(new MemoryStream(Json2Wmap.Convert2(this.GetType().Assembly.GetManifestResourceStream("wServer.realm.worlds.zeta.jm").ToString())));
        }
    }
}
