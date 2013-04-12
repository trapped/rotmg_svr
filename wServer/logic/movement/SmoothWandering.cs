using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.logic.movement
{
    class SmoothWandering : Behavior
    {
        class WanderingState
        {
            public Position beginPos;
            public float angle;
            public float remainingDist;
        }

        float radius;
        float speed;
        private SmoothWandering(float radius, float speed)
        {
            this.radius = radius;
            this.speed = speed;
        }
        static readonly Dictionary<Tuple<float, float>, SmoothWandering> instances = new Dictionary<Tuple<float, float>, SmoothWandering>();
        public static SmoothWandering Instance(float radius, float speed)
        {
            var key = new Tuple<float, float>(radius, speed);
            SmoothWandering ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new SmoothWandering(radius, speed);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return true;
            var speed = this.speed * GetSpeedMultiplier(Host.Self);

            WanderingState state;
            object o;
            if (!Host.StateStorage.TryGetValue(Key, out o))
                Host.StateStorage[Key] = state = new WanderingState() { beginPos = new Position() { X = Host.Self.X, Y = Host.Self.Y } };
            else
            {
                state = (WanderingState)o;

                double dist = (speed / 1.5f) * (time.thisTickTimes / 1000.0);
                double x = Math.Cos(state.angle) * dist;
                double y = Math.Sin(state.angle) * dist;
                state.remainingDist -= (float)dist;
                ValidateAndMove(Host.Self.X + (float)x, Host.Self.Y + (float)y);
                Host.Self.UpdateCount++;
            }

            bool ret;
            if (state.remainingDist <= 0)
            {
                double randAngle = Math.PI * 2 * rand.NextDouble();
                double randRadius = radius * rand.NextDouble();
                Position newPos = new Position()
                {
                    X = (float)(Math.Cos(randAngle) * randRadius) + state.beginPos.X,
                    Y = (float)(Math.Sin(randAngle) * randRadius) + state.beginPos.Y,
                };
                var dx = newPos.X - Host.Self.X;
                var dy = newPos.Y - Host.Self.Y;
                state.angle = (float)Math.Atan2(dy, dx);
                state.remainingDist = (float)Math.Sqrt(dx * dx + dy * dy);
                ret = true;
            }
            else
                ret = false;
            return ret;
        }
    }
}
