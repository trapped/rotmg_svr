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
    class MaintainDist : Behavior
    {
        float speed;
        float radius;
        float targetRadius;
        short? objType;
        private MaintainDist(float speed, float radius, float targetRadius, short? objType)
        {
            this.speed = speed;
            this.radius = radius;
            this.targetRadius = targetRadius;
            this.objType = objType;
        }
        static readonly Dictionary<Tuple<float, float, float, short?>, MaintainDist> instances = new Dictionary<Tuple<float, float, float, short?>, MaintainDist>();
        public static MaintainDist Instance(float speed, float radius, float targetRadius, short? objType)
        {
            var key = new Tuple<float, float, float, short?>(speed, radius, targetRadius, objType);
            MaintainDist ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new MaintainDist(speed, radius, targetRadius, objType);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return true;
            var speed = this.speed * GetSpeedMultiplier(Host.Self);

            float dist = radius;
            Entity entity = GetNearestEntity(ref dist, objType);
            if (entity != null)
            {
                if (dist > targetRadius)
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
                else if (dist < targetRadius)
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
                        ValidateAndMove(Host.Self.X - vect.X, Host.Self.Y - vect.Y);
                        Host.Self.UpdateCount++;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
