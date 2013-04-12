using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.logic.attack
{
    class AngleAttack : Behavior
    {
        float angle;
        int projectileIndex;
        private AngleAttack(float angle, int projectileIndex)
        {
            this.angle = angle;
            this.projectileIndex = projectileIndex;
        }
        static readonly Dictionary<Tuple<float, int>, AngleAttack> instances = new Dictionary<Tuple<float, int>, AngleAttack>();
        public static AngleAttack Instance(float angle, int projectileIndex = 0)
        {
            var key = new Tuple<float, int>(angle, projectileIndex);
            AngleAttack ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new AngleAttack(angle, projectileIndex);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Stunned)) return false;

            var chr = Host as Character;
            if (chr.Owner == null) return false;
            var desc = chr.ObjectDesc.Projectiles[projectileIndex];

            var prj = chr.CreateProjectile(
                desc, chr.ObjectType, chr.Random.Next(desc.MinDamage, desc.MaxDamage),
                time.tickTimes, new Position() { X = chr.X, Y = chr.Y }, (float)angle);
            chr.Owner.EnterWorld(prj);
            if (false)//(projectileIndex == 0)
                chr.Owner.BroadcastPacket(new ShootPacket()
                {
                    BulletId = prj.ProjectileId,
                    OwnerId = Host.Self.Id,
                    ContainerType = Host.Self.ObjectType,
                    Position = prj.BeginPos,
                    Angle = prj.Angle,
                    Damage = (short)prj.Damage
                }, null);
            else
                chr.Owner.BroadcastPacket(new MultiShootPacket()
                {
                    BulletId = prj.ProjectileId,
                    OwnerId = Host.Self.Id,
                    Position = prj.BeginPos,
                    Angle = prj.Angle,
                    Damage = (short)prj.Damage,
                    BulletType = (byte)(desc.BulletType),
                    AngleIncrement = 0,
                    NumShots = 1,
                }, null);
            return true;
        }
    }
}
