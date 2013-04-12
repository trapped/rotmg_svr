using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.logic
{
    class SpawnMinion : Behavior
    {
        short objType;
        float radius;
        int maxCount;
        int minTick;
        int maxTick;
        private SpawnMinion(short objType, float radius, int maxCount, int minTick, int maxTick)
        {
            this.objType = objType;
            this.radius = radius;
            this.maxCount = maxCount;
            this.minTick = minTick;
            this.maxTick = maxTick;
        }
        static readonly Dictionary<Tuple<short, float, int, int, int>, SpawnMinion> instances = new Dictionary<Tuple<short, float, int, int, int>, SpawnMinion>();
        public static SpawnMinion Instance(short objType, float radius, int maxCount, int minTick, int maxTick)
        {
            var key = new Tuple<short, float, int, int, int>(objType, radius, maxCount, minTick, maxTick);
            SpawnMinion ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new SpawnMinion(objType, radius, maxCount, minTick, maxTick);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            int remainingTick;
            object o;
            if (!Host.StateStorage.TryGetValue(Key, out o))
                remainingTick = 0;
            else
                remainingTick = (int)o;

            remainingTick -= time.thisTickTimes;
            bool ret;
            if (remainingTick <= 0)
            {
                if (CountEntity(radius, objType) < maxCount)
                {
                    Entity entity = Entity.Resolve(objType);
                    entity.Move(Host.Self.X, Host.Self.Y);
                    (entity as Enemy).Terrain = (Host as Enemy).Terrain;
                    Host.Self.Owner.EnterWorld(entity);
                }

                remainingTick = rand.Next(minTick, maxTick);
                ret = true;
            }
            else
                ret = false;
            Host.StateStorage[Key] = remainingTick;
            return ret;
        }
    }
}
