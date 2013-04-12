using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.svrPackets;
using wServer.logic;

namespace wServer.realm.entities
{
    class Trap : StaticObject
    {
        const int LIFETIME = 10;

        Player player;
        float radius;
        int dmg;
        ConditionEffectIndex effect;
        int duration;
        public Trap(Player player, float radius, int dmg, ConditionEffectIndex eff, float effDuration)
            : base(0x0711, LIFETIME * 1000, true, true, false)
        {
            this.player = player;
            this.radius = radius;
            this.dmg = dmg;
            this.effect = eff;
            this.duration = (int)(effDuration * 1000);
        }

        int t = 0;
        int p = 0;
        public override void Tick(RealmTime time)
        {
            if (t / 500 == p)
            {
                Owner.BroadcastPacket(new ShowEffectPacket()
                {
                    EffectType = EffectType.Trap,
                    Color = new ARGB(0xff9000ff),
                    TargetId = Id,
                    PosA = new Position() { X = radius / 2 }
                }, null);
                p++;
                if (p == LIFETIME * 2)
                {
                    Explode(time);
                    return;
                }
            }
            t += time.thisTickTimes;

            bool monsterNearby = false;
            Behavior.AOE(Owner, this, radius / 2, false, enemy => monsterNearby = true);
            if (monsterNearby)
                Explode(time);

            base.Tick(time);
        }

        void Explode(RealmTime time)
        {
            Owner.BroadcastPacket(new ShowEffectPacket()
            {
                EffectType = EffectType.AreaBlast,
                Color = new ARGB(0xff9000ff),
                TargetId = Id,
                PosA = new Position() { X = radius }
            }, null);
            Behavior.AOE(Owner, this, radius, false, enemy =>
            {
                (enemy as Enemy).Damage(player, time, dmg, false, new ConditionEffect()
                {
                    Effect = effect,
                    DurationMS = duration
                });
            });
            Owner.LeaveWorld(this);
        }
    }
}
