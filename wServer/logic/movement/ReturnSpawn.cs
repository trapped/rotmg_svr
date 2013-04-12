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
    class ReturnSpawn : Behavior
    {
        float speed;
        private ReturnSpawn(float speed)
        {
            this.speed = speed;
        }
        static readonly Dictionary<float, ReturnSpawn> instances = new Dictionary<float, ReturnSpawn>();
        public static ReturnSpawn Instance(float speed)
        {
            ReturnSpawn ret;
            if (!instances.TryGetValue(speed, out ret))
                ret = instances[speed] = new ReturnSpawn(speed);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return true;
            var speed = this.speed * GetSpeedMultiplier(Host.Self);

            Position pos = (Host as Enemy).SpawnPoint;
            var tx = pos.X;
            var ty = pos.Y;
            if (Math.Abs(tx - Host.Self.X) > 1 || Math.Abs(ty - Host.Self.Y) > 1)
            {
                var x = Host.Self.X;
                var y = Host.Self.Y;
                Vector2 vect = new Vector2(tx, ty) - new Vector2(Host.Self.X, Host.Self.Y);
                vect.Normalize();
                vect *= (speed / 1.5f) * (time.thisTickTimes / 1000f);
                ValidateAndMove(Host.Self.X + vect.X, Host.Self.Y + vect.Y);
                Host.Self.UpdateCount++;
                return true;
            }
            return false;
        }
    }
}
