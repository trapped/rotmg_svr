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
    class Charge : Behavior
    {
        float speed;
        float radius;
        short? objType;
        private Charge(float speed, float radius, short? objType)
        {
            this.speed = speed;
            this.radius = radius;
            this.objType = objType;
        }
        static readonly Dictionary<Tuple<float, float, short?>, Charge> instances = new Dictionary<Tuple<float, float, short?>, Charge>();
        public static Charge Instance(float speed, float radius, short? objType)
        {
            var key = new Tuple<float, float, short?>(speed, radius, objType);
            Charge ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new Charge(speed, radius, objType);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return true;
            var speed = this.speed * GetSpeedMultiplier(Host.Self);

            Position target;
            object o;
            if (!Host.StateStorage.TryGetValue(Key, out o))
            {
                float dist = radius;
                var entity = GetNearestEntity(ref dist, objType);
                if (entity == null) return true;
                Host.StateStorage[Key] = target = new Position()
                {
                    X = entity.X,
                    Y = entity.Y
                };
            }
            else
                target = (Position)o;

            if (target.X != Host.Self.X || target.Y != Host.Self.Y)
            {
                Vector2 vect = new Vector2((float)target.X, (float)target.Y) - new Vector2(Host.Self.X, Host.Self.Y);
                vect.Normalize();
                vect *= (speed / 1.5f) * (time.thisTickTimes / 1000f);
                ValidateAndMove(Host.Self.X + vect.X, Host.Self.Y + vect.Y);
                Host.Self.UpdateCount++;
            }

            if (Dist(Host.Self.X, Host.Self.Y, target.X, target.Y) < 1)
            {
                Host.StateStorage.Remove(Key);
                return true;
            }
            else
                return false;
        }
    }
}
