using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.logic.attack
{
    class RingAttack : Behavior
    {
        int count;
        float radius;
        float offset;
        int projectileIndex;
        private RingAttack(int count, float radius, float offset, int projectileIndex)
        {
            this.count = count;
            this.radius = radius;
            this.offset = offset;
            this.projectileIndex = projectileIndex;
        }
        static readonly Dictionary<Tuple<int, float, float, int>, RingAttack> instances = new Dictionary<Tuple<int, float, float, int>, RingAttack>();
        public static RingAttack Instance(int count, float radius = 0, float offset = 0, int projectileIndex = 0)
        {
            var key = new Tuple<int, float, float, int>(count, radius, offset, projectileIndex);
            RingAttack ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new RingAttack(count, radius, offset, projectileIndex);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Stunned)) return false;
            float dist = radius;
            Entity entity = radius == 0 ? null : GetNearestEntity(ref dist, null);
            if (entity != null || radius == 0)
            {
                var chr = Host as Character;
                if (chr.Owner == null) return false;
                var angle = entity == null ? offset : Math.Atan2(entity.Y - chr.Y, entity.X - chr.X) + offset;
                var angleInc = (2 * Math.PI) / this.count;
                var desc = chr.ObjectDesc.Projectiles[projectileIndex];

                var count = this.count;
                if (Host.Self.HasConditionEffect(ConditionEffects.Dazed))
                    count = Math.Max(1, count / 2);

                byte prjId = 0;
                Position prjPos = new Position() { X = chr.X, Y = chr.Y };
                var dmg = chr.Random.Next(desc.MinDamage, desc.MaxDamage);
                for (int i = 0; i < count; i++)
                {
                    var prj = chr.CreateProjectile(
                        desc, chr.ObjectType, dmg, time.tickTimes,
                        prjPos, (float)(angle + angleInc * i));
                    chr.Owner.EnterWorld(prj);
                    if (i == 0)
                        prjId = prj.ProjectileId;
                }
                chr.Owner.BroadcastPacket(new MultiShootPacket()
                {
                    BulletId = prjId,
                    OwnerId = Host.Self.Id,
                    BulletType = (byte)desc.BulletType,
                    Position = prjPos,
                    Angle = (float)angle,
                    Damage = (short)dmg,
                    NumShots = (byte)count,
                    AngleIncrement = (float)angleInc,
                }, null);
                return true;
            }
            return false;
        }
    }
}
