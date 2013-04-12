using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.svrPackets;
using wServer.cliPackets;

namespace wServer.realm.entities
{
    public partial class Player
    {
        long l = 0;
        void HandleGround(RealmTime time)
        {
            if (time.tickTimes - l > 500)
            {
                if (HasConditionEffect(ConditionEffects.Paused) ||
                    HasConditionEffect(ConditionEffects.Invincible))
                    return;

                WmapTile tile = Owner.Map[(int)X, (int)Y];
                ObjectDesc objDesc = tile.ObjType == 0 ? null : XmlDatas.ObjectDescs[tile.ObjType];
                TileDesc tileDesc = XmlDatas.TileDescs[tile.TileId];
                if (tileDesc.Damaging && (objDesc == null || !objDesc.ProtectFromGroundDamage))
                {
                    int dmg = Random.Next(tileDesc.MinDamage, tileDesc.MaxDamage);
                    dmg = (int)statsMgr.GetDefenseDamage(dmg, true);

                    HP -= dmg;
                    UpdateCount++;
                    if (HP <= 0)
                        Death("lava");

                    l = time.tickTimes;
                }
            }
        }
        public void GroundDamage(RealmTime time, GroundDamagePacket pkt)
        {
        }
    }
}
