using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.cliPackets;
using wServer.svrPackets;

namespace wServer.realm.entities
{
    partial class Player
    {
        public void Buy(RealmTime time, BuyPacket pkt)
        {
            SellableObject obj = Owner.GetEntity(pkt.ObjectId) as SellableObject;
            if (obj != null)
                obj.Buy(this);
        }

        public void CheckCredits(RealmTime time, CheckCreditsPacket pkt)
        {
            psr.Database.ReadStats(psr.Account);
            Credits = psr.Account.Credits;
            UpdateCount++;
        }
    }
}
