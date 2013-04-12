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
    class Circling : Behavior
    {
        class CirclingState
        {
            public WeakReference<Entity> target;
            public float angle;
        }

        float radius;
        float sight;
        float angularSpeed;
        float speed;
        short? objType;
        private Circling(float radius, float sight, float speed, short? objType)
        {
            this.radius = radius;
            this.sight = sight;
            this.angularSpeed = speed / radius;
            this.speed = speed;
            this.objType = objType;
        }
        static readonly Dictionary<Tuple<float, float, float, short?>, Circling> instances = new Dictionary<Tuple<float, float, float, short?>, Circling>();
        public static Circling Instance(float radius, float sight, float speed, short? objType)
        {
            var key = new Tuple<float, float, float, short?>(radius, sight, speed, objType);
            Circling ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new Circling(radius, sight, speed, objType);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return true;
            var speed = this.speed * GetSpeedMultiplier(Host.Self);

            CirclingState state;
            object o;
            if (!Host.StateStorage.TryGetValue(Key, out o))
            {
                float dist = sight;
                Host.StateStorage[Key] = state = new CirclingState()
                {
                    target = WeakReference<Entity>.Create(GetNearestEntity(ref dist, objType)),
                    angle = (float)(2 * Math.PI * rand.NextDouble())
                };
            }
            else
            {
                state = (CirclingState)o;

                state.angle += angularSpeed * (time.thisTickTimes / 1000f);
                if (!state.target.IsAlive)
                {
                    Host.StateStorage.Remove(Key);
                    return false;
                }
                var target = state.target.Target;
                if (target == null || target.Owner == null)
                {
                    Host.StateStorage.Remove(Key);
                    return false;
                }

                double x = target.X + Math.Cos(state.angle) * radius;
                double y = target.Y + Math.Sin(state.angle) * radius;
                if (x != Host.Self.X || y != Host.Self.Y)
                {
                    Vector2 vect = new Vector2((float)x, (float)y) - new Vector2(Host.Self.X, Host.Self.Y);
                    vect.Normalize();
                    vect *= (speed / 1.5f) * (time.thisTickTimes / 1000f);
                    ValidateAndMove(Host.Self.X + vect.X, Host.Self.Y + vect.Y);
                    Host.Self.UpdateCount++;
                }
            }

            if (state.angle >= Math.PI * 2)
                state.angle -= (float)(Math.PI * 2);
            return true;
        }
    }
}
