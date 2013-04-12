using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;

namespace wServer.logic.cond
{
    class GolemSatelliteSpawn : Behavior
    {
        private GolemSatelliteSpawn() { }
        public static readonly GolemSatelliteSpawn Instance = new GolemSatelliteSpawn();
        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            float dist = 4;
            var entity = GetNearestEntityByGroup(ref dist, "Golem Satellites");
            if (entity == null)
            {
                entity = Entity.Resolve((short)(0x6d2 + rand.Next(0, 3)));
                entity.Move(Host.Self.X, Host.Self.Y);
                Host.Self.Owner.EnterWorld(entity);

                for (int i = 0; i < 3; i++)
                {
                    entity = Entity.Resolve((short)(0x6d5 + i));
                    entity.Move(Host.Self.X, Host.Self.Y);
                    Host.Self.Owner.EnterWorld(entity);
                }
                return true;
            }
            return false;
        }
    }

    class GolemSatellites : Behavior
    {
        private GolemSatellites() { }
        public static readonly GolemSatellites Instance = new GolemSatellites();
        protected override bool TickCore(RealmTime time)
        {
            float dist = 8;
            var entity = GetNearestEntityByGroup(ref dist, "Golem");
            if (entity != null && dist > 4)
            {
                ValidateAndMove(entity.X, entity.Y);
                Host.StateStorage[Key] = 30 * 1000;
                return true;
            }
            else if (entity == null)
            {
                int remainingTick;
                object obj;
                if (!Host.StateStorage.TryGetValue(Key, out obj))
                    remainingTick = 30 * 1000;
                else
                    remainingTick = (int)obj;

                remainingTick -= time.thisTickTimes;
                if (remainingTick <= 0)
                    Host.Self.Owner.LeaveWorld(Host.Self);
                Host.StateStorage[Key] = remainingTick;
                return false;
            }
            else
            {
                Host.StateStorage[Key] = 30 * 1000;
                return true;
            }
        }
    }
}
