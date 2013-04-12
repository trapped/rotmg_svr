using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace wServer.realm.worlds
{
    public class Tutorial : World
    {
        public Tutorial(bool isLimbo)
        {
            Id = TUT_ID;
            Name = "Tutorial";
            Background = 0;
            if (!(IsLimbo = isLimbo))
            {
                base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.tutorial.wmap"));
            }
        }

        public override World GetInstance(ClientProcessor psr)
        {
            return RealmManager.AddWorld(new Tutorial(false));
        }
    }
}
