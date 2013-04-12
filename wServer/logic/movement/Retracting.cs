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
    class Retracting : Behavior
    {
        float speed;
        float radius;
        short? objType;
        private Retracting(float speed, float radius, short? objType)
        {
            this.speed = speed;
            this.radius = radius;
            this.objType = objType;
        }
        static readonly Dictionary<Tuple<float, float, short?>, Retracting> instances = new Dictionary<Tuple<float, float, short?>, Retracting>();
        public static Retracting Instance(float speed, float radius, short? objType)
        {
            var key = new Tuple<float, float, short?>(speed, radius, objType);
            Retracting ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new Retracting(speed, radius, objType);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return true;
            var speed = this.speed * GetSpeedMultiplier(Host.Self);

            float dist = radius;
            Entity entity = GetNearestEntity(ref dist, objType);
            Character chr = Host as Character;
            if (entity != null && (entity.X != Host.Self.X || entity.Y != Host.Self.Y))
            {
                var x = Host.Self.X;
                var y = Host.Self.Y;
                Vector2 vect = new Vector2(entity.X, entity.Y) - new Vector2(Host.Self.X, Host.Self.Y);
                vect.Normalize();
                vect *= -1 * (speed / 1.5f) * (time.thisTickTimes / 1000f);
                ValidateAndMove(Host.Self.X + vect.X, Host.Self.Y + vect.Y);
                Host.Self.UpdateCount++;

                return true;
            }
            else return false;
        }
    }
}
