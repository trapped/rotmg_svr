using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.logic.cond
{
    class GrayBlob : ConditionalBehavior
    {
        public override BehaviorCondition Condition
        {
            get { return BehaviorCondition.Other; }
        }

        protected override bool ConditionMeetCore()
        {
            return true;
        }

        protected override void BehaveCore(BehaviorCondition cond, realm.RealmTime? time, object state)
        {
            float dist = 2;
            var entity = GetNearestEntity(ref dist, null);
            if (entity != null)
            {
                var chr = Host as Character;
                var angleInc = (2 * Math.PI) / 12;
                var desc = chr.ObjectDesc.Projectiles[0];

                byte prjId = 0;
                Position prjPos = new Position() { X = chr.X, Y = chr.Y };
                var dmg = chr.Random.Next(desc.MinDamage, desc.MaxDamage);
                for (int i = 0; i < 12; i++)
                {
                    var prj = chr.CreateProjectile(
                        desc, chr.ObjectType, dmg, time.Value.tickTimes,
                        prjPos, (float)(angleInc * i));
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
                    Angle = 0,
                    Damage = (short)dmg,
                    NumShots = 12,
                    AngleIncrement = (float)angleInc,
                }, null);
                chr.Owner.LeaveWorld(chr);
            }
        }
    }
}
