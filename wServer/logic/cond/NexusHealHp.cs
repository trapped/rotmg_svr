using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.logic.cond
{
    class NexusHealHp : Behavior
    {
        protected override bool TickCore(RealmTime time)
        {
            float dist = 5;
            Player entity = GetNearestEntity(ref dist, null) as Player;
            while (entity != null)
            {
                int hp = entity.HP;
                int maxHp = entity.Stats[0] + entity.Boost[0];
                hp = Math.Min(hp + 100, maxHp);
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

                    return true;
                }
                entity = GetNearestEntity(ref dist, null) as Player;
            } 
            return false;
        }
    }
}
