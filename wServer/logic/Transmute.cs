using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic
{
    class Transmute : Behavior
    {
        short objType;
        int minCount;
        int maxCount;
        public Transmute(short objType, int minCount = 1, int maxCount = 1)
        {
            this.objType = objType;
            this.minCount = minCount;
            this.maxCount = maxCount;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            int c = rand.Next(minCount, maxCount + 1);
            Entity parent = Host as Entity;
            for (int i = 0; i < c; i++)
            {
                Entity entity = Entity.Resolve(objType);
                entity.Move(parent.X, parent.Y);
                (entity as Enemy).Terrain = (Host as Enemy).Terrain;
                parent.Owner.EnterWorld(entity);
            }
            parent.Owner.LeaveWorld(parent);
            return true;
        }
    }
}
