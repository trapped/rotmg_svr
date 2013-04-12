using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using Mono.Game;

namespace wServer.logic.movement
{
    class Tangential : Behavior
    {
        float speed;
        private Tangential(float speed)
        {
            this.speed = speed;
        }
        static readonly Dictionary<float, Tangential> instances = new Dictionary<float, Tangential>();
        public static Tangential Instance(float speed)
        {
            Tangential ret;
            if (!instances.TryGetValue(speed, out ret))
                ret = instances[speed] = new Tangential(speed);
            return ret;
        }

        Vector2 GetVelocity()
        {
            var ms = 100;
            Position? history = Host.Self.TryGetHistory(ms);
            if (history == null)
                return Vector2.Zero;
            return new Vector2(
                (Host.Self.X - history.Value.X) / (ms / 1000f),
                (Host.Self.Y - history.Value.Y) / (ms / 1000f)
            );
        }

        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return true;
            var speed = this.speed * GetSpeedMultiplier(Host.Self);

            Vector2 velo = GetVelocity();
            if (velo != Vector2.Zero)
                velo.Normalize();
            float dist = (speed / 1.5f) * (time.thisTickTimes / 1000f);
            ValidateAndMove(Host.Self.X + velo.X * dist, Host.Self.Y + velo.Y * dist);
            Host.Self.UpdateCount++;
            return true;
        }
    }
}
