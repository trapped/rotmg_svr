using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.logic.attack
{
    class ThrowAttack : Behavior
    {
        float bombRadius;
        float sightRadius;
        int damage;
        private ThrowAttack(float bombRadius, float sightRadius, int damage)
        {
            this.bombRadius = bombRadius;
            this.sightRadius = sightRadius;
            this.damage = damage;
        }
        static readonly Dictionary<Tuple<float, float, int>, ThrowAttack> instances = new Dictionary<Tuple<float, float, int>, ThrowAttack>();
        public static ThrowAttack Instance(float bombRadius, float sightRadius, int damage)
        {
            var key = new Tuple<float, float, int>(bombRadius, sightRadius, damage);
            ThrowAttack ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new ThrowAttack(bombRadius, sightRadius, damage);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Stunned)) return false;
            float dist = sightRadius;
            Entity player = GetNearestEntity(ref dist, null);
            if (player != null)
            {
                var chr = Host as Character;
                Position target = new Position()
                {
                    X = player.X,
                    Y = player.Y
                };
                chr.Owner.BroadcastPacket(new ShowEffectPacket()
                {
                    EffectType = EffectType.Throw,
                    Color = new ARGB(0xffff0000),
                    TargetId = Host.Self.Id,
                    PosA = target
                }, null);
                chr.Owner.Timers.Add(new WorldTimer(1500, (world, t) =>
                {
                    world.BroadcastPacket(new AOEPacket()
                    {
                        Position = target,
                        Radius = bombRadius,
                        Damage = (ushort)damage,
                        EffectDuration = 0,
                        Effects = 0,
                        OriginType = Host.Self.ObjectType
                    }, null);
                    AOE(world, target, bombRadius, true, p =>
                    {
                        (p as IPlayer).Damage(damage, Host.Self as Character);
                    });
                }));

                return true;
            }
            return false;
        }
    }
}
