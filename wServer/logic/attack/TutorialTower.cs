using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.logic.attack
{
    class TutorialTower : Behavior
    {
        private TutorialTower() { }
        public static readonly TutorialTower Instance = new TutorialTower();

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            int remainingTick;
            object o;
            if (!Host.StateStorage.TryGetValue(Key, out o))
                remainingTick = 0;
            else
                remainingTick = (int)o;

            remainingTick -= time.thisTickTimes;
            bool ret;
            if (remainingTick <= 0)
            {
                double angle = 0;
                switch (Host.Self.ObjectType)
                {
                    case 0x0802: angle = Math.PI / 2; break;
                    case 0x0803: angle = Math.PI * 3 / 2; break;
                    case 0x0804: angle = 0; break;
                    case 0x0805: angle = Math.PI; break;
                }

                ProjectileDesc desc = Host.Self.ObjectDesc.Projectiles[0];
                Projectile proj = Host.Self.CreateProjectile(desc, Host.Self.ObjectType,
                    5, time.tickTimes, new Position() { X = Host.Self.X, Y = Host.Self.Y },
                    (float)angle);
                Host.Self.Owner.EnterWorld(proj);
                Host.Self.Owner.BroadcastPacket(new ShootPacket()
                {
                    BulletId = proj.ProjectileId,
                    OwnerId = Host.Self.Id,
                    ContainerType = Host.Self.ObjectType,
                    Position = proj.BeginPos,
                    Angle = proj.Angle,
                    Damage = (short)proj.Damage
                }, null);

                remainingTick = rand.Next(2000, 3500);
                ret = true;
            }
            else
                ret = false;
            Host.StateStorage[Key] = remainingTick;
            return ret;
        }
    }
}
