using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.logic.attack
{
    class HealGroup : Behavior
    {
        float radius;
        int amount;
        string group;
        private HealGroup(float radius, int amount, string group)
        {
            this.radius = radius;
            this.amount = amount;
            this.group = group;
        }
        static readonly Dictionary<Tuple<float, int, string>, HealGroup> instances = new Dictionary<Tuple<float, int, string>, HealGroup>();
        public static HealGroup Instance(float radius, int amount, string group)
        {
            var key = new Tuple<float, int, string>(radius, amount, group);
            HealGroup ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new HealGroup(radius, amount, group);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            bool ret = false;
            foreach (var entity in GetNearestEntitiesByGroup(radius, group).OfType<Enemy>())
            {
                int hp = entity.HP;
                hp = Math.Min(hp + amount, entity.ObjectDesc.MaxHP);
                if (hp != entity.HP)
                {
                    int n = hp - entity.HP;
                    entity.HP = hp;
                    entity.UpdateCount++;
                    entity.Owner.BroadcastPacket(new ShowEffectPacket()
                    {
                        EffectType = EffectType.Potion,
                        TargetId = entity.Id,
                        Color = new ARGB(0xffffffff)
                    }, null);
                    entity.Owner.BroadcastPacket(new ShowEffectPacket()
                    {
                        EffectType = EffectType.Trail,
                        TargetId = Host.Self.Id,
                        PosA = new Position() { X = entity.X, Y = entity.Y },
                        Color = new ARGB(0xffffffff)
                    }, null);
                    entity.Owner.BroadcastPacket(new NotificationPacket()
                    {
                        ObjectId = entity.Id,
                        Text = "+" + n,
                        Color = new ARGB(0xff00ff00)
                    }, null);
                    ret = true;
                }
            }
            return ret;
        }
    }
}
