using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using db;
using wServer.realm.entities;
using System.Collections.Concurrent;

namespace wServer.realm.worlds
{
    public class GuildHall : World
    {
        public string Guild { get; set; }

        public GuildHall(string guild)
        {
            Id = GHALL_ID;
            Guild = guild;
            Name = "Guild Hall";
            Background = 0;
            AllowTeleport = true;
            base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.ghall3.wmap"));
        }
        public override World GetInstance(ClientProcessor psr)
        {
            return RealmManager.AddWorld(new GuildHall(Guild));
        }
    }
}
