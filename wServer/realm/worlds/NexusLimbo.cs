using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace wServer.realm.worlds
{
    public class NexusLimbo : World
    {
        public NexusLimbo()
        {
            Id = NEXUS_LIMBO;
            Name = "Nexus Tutorial";
            Background = 0;
            base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.nexusLimbo.wmap"));
        }
    }
}
