using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.cliPackets;
using wServer.svrPackets;
using System.Collections.Concurrent;

namespace wServer.realm.entities
{
    partial class Player
    {
        Dictionary<Player, int> potentialTrader = new Dictionary<Player, int>();
        Player tradeTarget;
        bool[] trade;
        bool tradeAccepted;

        public void RequestTrade(RealmTime time, RequestTradePacket pkt)
        {
            if (!NameChosen)
            {
                psr.SendPacket(new TextPacket()
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "",
                    Text = "Unique name is required to trade with other!"
                });
                return;
            }
            if (tradeTarget != null)
            {
                psr.SendPacket(new TextPacket()
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "*Error*",
                    Text = "You're already trading!"
                });
                return;
            }
            Player target = Owner.GetUniqueNamedPlayer(pkt.Name);
            if (target == null)
            {
                psr.SendPacket(new TextPacket()
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "*Error*",
                    Text = "Player not found!"
                });
                return;
            }
            if (target.tradeTarget != null && target.tradeTarget != this)
            {
                psr.SendPacket(new TextPacket()
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "*Error*",
                    Text = target.Name + " is already trading!"
                });
                return;
            }

            if (this.potentialTrader.ContainsKey(target))
            {
                this.tradeTarget = target;
                this.trade = new bool[12];
                this.tradeAccepted = false;
                target.tradeTarget = this;
                target.trade = new bool[12];
                target.tradeAccepted = false;
                this.potentialTrader.Clear();
                target.potentialTrader.Clear();

                TradeItem[] my = new TradeItem[this.Inventory.Length];
                for (int i = 0; i < this.Inventory.Length; i++)
                    my[i] = new TradeItem()
                    {
                        Item = this.Inventory[i] == null ? -1 : this.Inventory[i].ObjectType,
                        SlotType = this.SlotTypes[i],
                        Included = false,
                        Tradeable = (this.Inventory[i] == null || i < 4) ? false : !this.Inventory[i].Soulbound
                    };
                TradeItem[] your = new TradeItem[target.Inventory.Length];
                for (int i = 0; i < target.Inventory.Length; i++)
                    your[i] = new TradeItem()
                    {
                        Item = target.Inventory[i] == null ? -1 : target.Inventory[i].ObjectType,
                        SlotType = target.SlotTypes[i],
                        Included = false,
                        Tradeable = (target.Inventory[i] == null || i < 4) ? false : !target.Inventory[i].Soulbound
                    };

                this.psr.SendPacket(new TradeStartPacket()
                {
                    MyItems = my,
                    YourName = target.Name,
                    YourItems = your
                });
                target.psr.SendPacket(new TradeStartPacket()
                {
                    MyItems = your,
                    YourName = this.Name,
                    YourItems = my
                });
            }
            else
            {
                target.potentialTrader[this] = 1000 * 20;
                target.psr.SendPacket(new TradeRequestedPacket()
                {
                    Name = Name
                });
                psr.SendPacket(new TextPacket()
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "",
                    Text = "You have sent a trade request to " + target.Name + "!"
                });
                return;
            }
        }
        public void ChangeTrade(RealmTime time, ChangeTradePacket pkt)
        {
            this.tradeAccepted = false;
            tradeTarget.tradeAccepted = false;
            this.trade = pkt.Offers;

            tradeTarget.psr.SendPacket(new TradeChangedPacket()
            {
                Offers = this.trade
            });
        }
        public void AcceptTrade(RealmTime time, AcceptTradePacket pkt)
        {
            this.trade = pkt.MyOffers;
            if (tradeTarget.trade.SequenceEqual(pkt.YourOffers))
            {
                tradeTarget.trade = pkt.YourOffers;
                this.tradeAccepted = true;
                tradeTarget.psr.SendPacket(new TradeAcceptedPacket()
                {
                    MyOffers = tradeTarget.trade,
                    YourOffers = this.trade
                });

                if (this.tradeAccepted && tradeTarget.tradeAccepted)
                {
                    DoTrade();
                }
            }
        }
        public void CancelTrade(RealmTime time, CancelTradePacket pkt)
        {
            this.psr.SendPacket(new TradeDonePacket()
            {
                Result = 1,
                Message = "Trade canceled!"
            });
            tradeTarget.psr.SendPacket(new TradeDonePacket()
            {
                Result = 1,
                Message = "Trade canceled!"
            });

            tradeTarget.tradeTarget = null;
            tradeTarget.trade = null;
            tradeTarget.tradeAccepted = false;
            this.tradeTarget = null;
            this.trade = null;
            this.tradeAccepted = false;
            return;
        }
        void CheckTradeTimeout(RealmTime time)
        {
            List<Tuple<Player, int>> newState = new List<Tuple<Player, int>>();
            foreach (var i in potentialTrader)
                newState.Add(new Tuple<Player, int>(i.Key, i.Value - time.thisTickTimes));

            foreach (var i in newState)
            {
                if (i.Item2 < 0)
                {
                    i.Item1.psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Trade to " + Name + " has timed out!"
                    });
                    potentialTrader.Remove(i.Item1);
                }
                else potentialTrader[i.Item1] = i.Item2;
            }
        }

        void DoTrade()
        {
            List<Item> thisItems = new List<Item>();
            for (int i = 0; i < this.trade.Length; i++)
                if (this.trade[i])
                {
                    thisItems.Add(this.Inventory[i]);
                    this.Inventory[i] = null;
                }
            List<Item> targetItems = new List<Item>();
            for (int i = 0; i < tradeTarget.trade.Length; i++)
                if (tradeTarget.trade[i])
                {
                    targetItems.Add(tradeTarget.Inventory[i]);
                    tradeTarget.Inventory[i] = null;
                }

            for (int i = 0; i < this.Inventory.Length; i++) //put items by slotType
                if (this.Inventory[i] == null)
                {
                    if (this.SlotTypes[i] == 0)
                    {
                        this.Inventory[i] = targetItems[0];
                        targetItems.RemoveAt(0);
                    }
                    else
                    {
                        int itmIdx = -1;
                        for (int j = 0; j < targetItems.Count; j++)
                            if (targetItems[j].SlotType == this.SlotTypes[i])
                            {
                                itmIdx = j;
                                break;
                            }
                        if (itmIdx != -1)
                        {
                            this.Inventory[i] = targetItems[itmIdx];
                            targetItems.RemoveAt(itmIdx);
                        }
                    }
                    if (targetItems.Count == 0) break;
                }
            if (targetItems.Count > 0)
                for (int i = 0; i < this.Inventory.Length; i++) //force put item
                    if (this.Inventory[i] == null)
                    {
                        this.Inventory[i] = targetItems[0];
                        targetItems.RemoveAt(0);
                        if (targetItems.Count == 0) break;
                    }


            for (int i = 0; i < tradeTarget.Inventory.Length; i++) //put items by slotType
                if (tradeTarget.Inventory[i] == null)
                {
                    if (tradeTarget.SlotTypes[i] == 0)
                    {
                        tradeTarget.Inventory[i] = thisItems[0];
                        thisItems.RemoveAt(0);
                    }
                    else
                    {
                        int itmIdx = -1;
                        for (int j = 0; j < thisItems.Count; j++)
                            if (thisItems[j].SlotType == tradeTarget.SlotTypes[i])
                            {
                                itmIdx = j;
                                break;
                            }
                        if (itmIdx != -1)
                        {
                            tradeTarget.Inventory[i] = thisItems[itmIdx];
                            thisItems.RemoveAt(itmIdx);
                        }
                    }
                    if (thisItems.Count == 0) break;
                }
            if (thisItems.Count > 0)
                for (int i = 0; i < tradeTarget.Inventory.Length; i++) //force put item
                    if (tradeTarget.Inventory[i] == null)
                    {
                        tradeTarget.Inventory[i] = thisItems[0];
                        thisItems.RemoveAt(0);
                        if (thisItems.Count == 0) break;
                    }

            this.UpdateCount++;
            tradeTarget.UpdateCount++;


            this.psr.SendPacket(new TradeDonePacket()
            {
                Result = 1,
                Message = "Trade done!"
            });
            tradeTarget.psr.SendPacket(new TradeDonePacket()
             {
                 Result = 1,
                 Message = "Trade done!"
             });

            tradeTarget.tradeTarget = null;
            tradeTarget.trade = null;
            tradeTarget.tradeAccepted = false;
            this.tradeTarget = null;
            this.trade = null;
            this.tradeAccepted = false;
        }
    }
}
