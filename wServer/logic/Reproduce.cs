using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic
{
    class Reproduce : Behavior
    {
        short objType;
        int maxCount;
        int cooldown;
        float radius;
        int min, max;
        private Reproduce(short objType, int maxCount, int cooldown, float radius)
        {
            this.objType = objType;
            this.maxCount = maxCount;
            this.cooldown = cooldown;
            this.radius = radius;
            min = (int)(cooldown * 0.95);
            max = (int)(cooldown * 1.05);
        }
        static readonly Dictionary<Tuple<short, int, int, float>, Reproduce> instances = new Dictionary<Tuple<short, int, int, float>, Reproduce>();
        public static Reproduce Instance(short objType, int maxCount, int cooldown = 10000, float radius = 5)
        {
            var key = new Tuple<short, int, int, float>(objType, maxCount, cooldown, radius);
            Reproduce ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new Reproduce(objType, maxCount, cooldown, radius);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            int remainingTick;
            object o;
            if (!Host.StateStorage.TryGetValue(Key, out o))
                remainingTick = rand.Next(min, max);
            else
                remainingTick = (int)o;

            remainingTick -= time.thisTickTimes;
            bool ret;
            if (remainingTick <= 0)
            {
                if (CountEntity(radius, objType) < maxCount)
                {
                    Entity entity = Entity.Resolve(objType);

                    double targetX = Host.Self.X;
                    double targetY = Host.Self.Y;
                    if (radius != 5)
                    {
                        int i = 0;
                        do
                        {
                            var angle = rand.NextDouble() * 2 * Math.PI;
                            targetX = Host.Self.X + radius * 0.5 * Math.Cos(angle);
                            targetY = Host.Self.Y + radius * 0.5 * Math.Sin(angle);
                            i++;
                        } while (targetX < Host.Self.Owner.Map.Width &&
                                 targetY < Host.Self.Owner.Map.Height &&
                                 targetX > 0 && targetY > 0 &&
                                 Host.Self.Owner.Map[(int)targetX, (int)targetY].Terrain !=
                                 Host.Self.Owner.Map[(int)Host.Self.X, (int)Host.Self.Y].Terrain &&
                            i < 10);
                    }

                    entity.Move((float)targetX, (float)targetY);
                    (entity as Enemy).Terrain = (Host as Enemy).Terrain;
                    Host.Self.Owner.EnterWorld(entity);
                }

                remainingTick = rand.Next(min, max);
                ret = true;
            }
            else
                ret = false;
            Host.StateStorage[Key] = remainingTick;
            return ret;
        }
    }
}
