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
    class Swirling : Behavior
    {
        class CirclingState
        {
            public Position center;
            public float angle;
        }

        float radius;
        float angularSpeed;
        float speed;
        private Swirling(float radius, float speed)
        {
            this.radius = radius;
            this.angularSpeed = speed / radius;
            this.speed = speed;
        }
        static readonly Dictionary<Tuple<float, float>, Swirling> instances = new Dictionary<Tuple<float, float>, Swirling>();
        public static Swirling Instance(float radius, float speed)
        {
            var key = new Tuple<float, float>(radius, speed);
            Swirling ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new Swirling(radius, speed);
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
                float dist = radius;
                Host.StateStorage[Key] = state = new CirclingState()
                {
                    center = new Position() { X = Host.Self.X, Y = Host.Self.Y },
                    angle = (float)(2 * Math.PI * rand.NextDouble())
                };
            }
            else
            {
                state = (CirclingState)o;

                state.angle += angularSpeed * (time.thisTickTimes / 1000f);
                double x = state.center.X + Math.Cos(state.angle) * radius;
                double y = state.center.Y + Math.Sin(state.angle) * radius;
                if (x != Host.Self.X && y != Host.Self.Y)
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
