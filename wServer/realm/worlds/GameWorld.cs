using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using wServer.realm.setpieces;
using wServer.realm.entities;

namespace wServer.realm.worlds
{
    class GameWorld : World
    {
        public GameWorld(int mapId, string name, bool oryxPresent)
        {
            Name = name;
            Background = 0;
            base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.world" + mapId + ".wmap"));
            SetPieces.ApplySetPieces(this);
            if (oryxPresent)
            {
                Overseer = new Oryx(this);
                Overseer.Init();
            }
            else
                Overseer = null;
        }

        public Oryx Overseer { get; private set; }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);
            if (Overseer != null)
                Overseer.Tick(time);
        }

        public void EnemyKilled(Enemy enemy, Player killer)
        {
            if (Overseer != null)
                Overseer.OnEnemyKilled(enemy, killer);
        }

        public override int EnterWorld(Entity entity)
        {
            var ret = base.EnterWorld(entity);
            if (entity is Player)
                Overseer.OnPlayerEntered(entity as Player);
            return ret;
        }
    }
}
