using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;
using Mono.Game;

namespace wServer.logic
{
    class Flashing : Behavior
    {
        int time;
        uint color;
        private Flashing(int time, uint color)
        {
            this.time = time;
            this.color = color;
        }
        static readonly Dictionary<Tuple<int, uint>, Flashing> instances = new Dictionary<Tuple<int, uint>, Flashing>();
        public static Flashing Instance(int time, uint color)
        {
            var key = new Tuple<int, uint>(time, color);
            Flashing ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new Flashing(time, color);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            object obj;
            int t;
            if (Host.StateStorage.TryGetValue(Key, out obj))
                t = (int)obj;
            else
                t = this.time;


            bool ret;
            if (t == this.time)
            {
                Host.Self.Owner.BroadcastPacket(new ShowEffectPacket()
                {
                    EffectType = EffectType.Flashing,
                    PosA = new Position() { X = this.time / 1000f, Y = 1 },
                    TargetId = Host.Self.Id,
                    Color = new ARGB(color)
                }, null);
                ret = false;
                t -= time.thisTickTimes;
            }
            else if (t < 0)
            {
                ret = true;
                t = this.time;
            }
            else
            {
                ret = false;
                t -= time.thisTickTimes;
            }
            Host.StateStorage[Key] = t;
            return ret;
        }
    }
}
