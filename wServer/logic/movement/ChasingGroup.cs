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
    class ChasingGroup : Behavior
    {
        float speed;
        float radius;
        float targetRadius;
        string group;
        private ChasingGroup(float speed, float radius, float targetRadius, string group)
        {
            this.speed = speed;
            this.radius = radius;
            this.targetRadius = targetRadius;
            this.group = group;
        }
        static readonly Dictionary<Tuple<float, float, float, string>, ChasingGroup> instances = new Dictionary<Tuple<float, float, float, string>, ChasingGroup>();
        public static ChasingGroup Instance(float speed, float radius, float targetRadius, string group)
        {
            var key = new Tuple<float, float, float, string>(speed, radius, targetRadius, group);
            ChasingGroup ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new ChasingGroup(speed, radius, targetRadius, group);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return true;
            var speed = this.speed * GetSpeedMultiplier(Host.Self);

            float dist = radius;
            Entity entity = GetNearestEntityByGroup(ref dist, group);
            if (entity != null && dist > targetRadius)
            {
                var tx = entity.X + rand.Next(-2, 2) / 2f;
                var ty = entity.Y + rand.Next(-2, 2) / 2f;
                if (tx != Host.Self.X || ty != Host.Self.Y)
                {
                    var x = Host.Self.X;
                    var y = Host.Self.Y;
                    Vector2 vect = new Vector2(tx, ty) - new Vector2(Host.Self.X, Host.Self.Y);
                    vect.Normalize();
                    vect *= (speed / 1.5f) * (time.thisTickTimes / 1000f);
                    ValidateAndMove(Host.Self.X + vect.X, Host.Self.Y + vect.Y);
                    Host.Self.UpdateCount++;
                }
                return true;
            }
            else return false;
        }
    }
}
