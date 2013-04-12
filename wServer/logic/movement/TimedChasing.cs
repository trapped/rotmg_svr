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
    class TimedChasing : Behavior
    {
        float speed;
        float radius;
        float targetRadius;
        int time;
        short? objType;
        private TimedChasing(float speed, float radius, float targetRadius, int time, short? objType)
        {
            this.speed = speed;
            this.radius = radius;
            this.targetRadius = targetRadius;
            this.time = time;
            this.objType = objType;
        }
        static readonly Dictionary<Tuple<float, float, float, int, short?>, TimedChasing> instances = new Dictionary<Tuple<float, float, float, int, short?>, TimedChasing>();
        public static TimedChasing Instance(float speed, float radius, float targetRadius, int time, short? objType)
        {
            var key = new Tuple<float, float, float, int, short?>(speed, radius, targetRadius, time, objType);
            TimedChasing ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new TimedChasing(speed, radius, targetRadius, time, objType);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return true;
            var speed = this.speed * GetSpeedMultiplier(Host.Self);

            float dist = radius;
            Entity entity = GetNearestEntity(ref dist, objType);
            object obj;
            int t;
            if (Host.StateStorage.TryGetValue(Key, out obj))
                t = (int)obj;
            else
                t = this.time;

            if (entity != null && dist > targetRadius &&
                t > 0)
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
                t -= time.thisTickTimes;
                Host.StateStorage[Key] = t;
                return false;
            }
            else
            {
                t = this.time;
                Host.StateStorage[Key] = t;
                return true;
            }
        }
    }
}
