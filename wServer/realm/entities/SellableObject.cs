using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.worlds;
using wServer.svrPackets;

namespace wServer.realm.entities
{
    public class SellableObject : StaticObject
    {
        const int BUY_NO_GOLD = 3;
        const int BUY_NO_FAME = 6;

        public SellableObject(short objType)
            : base(objType, null, true, false, false)
        {
            if (objType == 0x0505)  //Vault chest
            {
                Price = 500;
                Currency = CurrencyType.Gold;
                RankReq = 0;
            }
        }

        public int Price { get; set; }
        public CurrencyType Currency { get; set; }
        public int RankReq { get; set; }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.SellablePrice] = Price;
            stats[StatsType.SellablePriceCurrency] = (int)Currency;
            stats[StatsType.SellableRankRequirement] = RankReq;
            base.ExportStats(stats);
        }
        protected override void ImportStats(StatsType stats, object val)
        {
            if (stats == StatsType.SellablePrice) Price = (int)val;
            else if (stats == StatsType.SellablePriceCurrency) Currency = (CurrencyType)(int)val;
            else if (stats == StatsType.SellableRankRequirement) RankReq = (int)val;
            base.ImportStats(stats, val);
        }

        protected bool TryDeduct(Player player)
        {
            var acc = player.Client.Account;
            player.Client.Database.ReadStats(acc);
            if (!player.NameChosen) return false;

            if (Currency == CurrencyType.Fame)
            {
                if (acc.Stats.Fame < Price) return false;
                player.CurrentFame = acc.Stats.Fame = player.Client.Database.UpdateFame(acc, -Price);
                player.UpdateCount++;
                return true;
            }
            else
            {
                if (acc.Credits < Price) return false;
                player.Credits = acc.Credits = player.Client.Database.UpdateCredit(acc, -Price);
                player.UpdateCount++;
                return true;
            }
        }

        public virtual void Buy(Player player)
        {
            if (ObjectType == 0x0505)   //Vault chest
            {
                if (TryDeduct(player))
                {
                    var chest = player.Client.Database.CreateChest(player.Client.Account);
                    (Owner as Vault).AddChest(chest, this);
                    player.Client.SendPacket(new BuyResultPacket()
                    {
                        Result = 0,
                        Message = "Vault chest purchased!"
                    });
                }
                else
                    player.Client.SendPacket(new BuyResultPacket()
                    {
                        Result = BUY_NO_GOLD,
                        Message = "Not enough gold!"
                    });
            }
        }
    }
}
