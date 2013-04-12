using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;
using Mono.Game;

namespace wServer.logic.movement
{
    class Planewalk : Behavior
    {
        float radius;
        short? objType;
        private Planewalk(float radius, short? objType)
        {
            this.radius = radius;
            this.objType = objType;
        }
        static readonly Dictionary<Tuple<float, short?>, Planewalk> instances = new Dictionary<Tuple<float, short?>, Planewalk>();
        public static Planewalk Instance(float radius, short? objType)
        {
            var key = new Tuple<float, short?>(radius, objType);
            Planewalk ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new Planewalk(radius, objType);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return true;

            float dist = radius;
            Entity entity = GetNearestEntity(ref dist, objType);
            if (entity != null)
                ValidateAndMove(entity.X, entity.Y);
            return true;
        }
    }
}
