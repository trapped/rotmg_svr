using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.logic.movement
{
    class SimpleWandering : Behavior
    {
        class WanderingState
        {
            public int x;
            public int y;
            public float remainingDist;
        }

        float speed;
        float dist;
        private SimpleWandering(float speed, float dist)
        {
            this.speed = speed;
            this.dist = dist;
        }
        static readonly Dictionary<Tuple<float, float>, SimpleWandering> instances = new Dictionary<Tuple<float, float>, SimpleWandering>();
        public static SimpleWandering Instance(float speed, float dist = 1f)
        {
            Tuple<float, float> key = new Tuple<float, float>(speed, dist);
            SimpleWandering ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new SimpleWandering(speed, dist);
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
                Host.StateStorage[Key] = state = new WanderingState();
            else
            {
                state = (WanderingState)o;

                float dist = (speed / 1.5f) * (time.thisTickTimes / 1000f);
                state.remainingDist -= dist;
                ValidateAndMove(Host.Self.X + state.x * dist, Host.Self.Y + state.y * dist);
                Host.Self.UpdateCount++;
            }

            bool ret;
            if (state.remainingDist <= 0)
            {
                state.x = rand.Next(-1, 2);
                state.y = rand.Next(-1, 2);
                state.remainingDist = dist + dist * (float)(rand.NextDouble() * 0.1 - 0.05);
                ret = true;
            }
            else
                ret = false;
            return ret;
        }
    }
}
