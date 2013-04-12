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
        bool AuditItem(IContainer container, Item item, int slot)
        {
            return item == null || container.SlotTypes[slot] == 0 || item.SlotType == container.SlotTypes[slot];
        }

        Random invRand = new Random();
        public void InventorySwap(RealmTime time, InvSwapPacket pkt)
        {
            Entity en1 = Owner.GetEntity(pkt.Obj1.ObjectId);
            Entity en2 = Owner.GetEntity(pkt.Obj2.ObjectId);
            IContainer con1 = en1 as IContainer;
            IContainer con2 = en2 as IContainer;

            //TODO: locker
            Item item1 = con1.Inventory[pkt.Obj1.SlotId];
            Item item2 = con2.Inventory[pkt.Obj2.SlotId];
            if (!AuditItem(con2, item1, pkt.Obj2.SlotId) ||
                !AuditItem(con1, item2, pkt.Obj1.SlotId))
                (en1 as Player).Client.SendPacket(new InvResultPacket() { Result = 1 });
            else
            {
                con1.Inventory[pkt.Obj1.SlotId] = item2;
                con2.Inventory[pkt.Obj2.SlotId] = item1;
                en1.UpdateCount++;
                en2.UpdateCount++;

                if (en1 is Player)
                {
                    (en1 as Player).CalcBoost();
                    (en1 as Player).Client.SendPacket(new InvResultPacket() { Result = 0 });
                }
                if (en2 is Player)
                {
                    (en2 as Player).CalcBoost();
                    (en2 as Player).Client.SendPacket(new InvResultPacket() { Result = 0 });
                }
            }
        }
        public void InventoryDrop(RealmTime time, InvDropPacket pkt)
        {
            //TODO: locker again
            const short NORM_BAG = 0x0500;
            const short SOUL_BAG = 0x0503;

            Entity entity = Owner.GetEntity(pkt.Slot.ObjectId);
            IContainer con = entity as IContainer;
            if (con.Inventory[pkt.Slot.SlotId] == null) return;

            Item item = con.Inventory[pkt.Slot.SlotId];
            con.Inventory[pkt.Slot.SlotId] = null;
            entity.UpdateCount++;

            Container container;
            if (item.Soulbound)
            {
                container = new Container(SOUL_BAG, 1000 * 60, true);
                container.BagOwner = AccountId;
            }
            else
                container = new Container(NORM_BAG, 1000 * 60, true);
            container.Inventory[0] = item;
            container.Move(entity.X + (float)((invRand.NextDouble() * 2 - 1) * 0.5), entity.Y + (float)((invRand.NextDouble() * 2 - 1) * 0.5));
            container.Size = 75;
            Owner.EnterWorld(container);

            if (entity is Player)
            {
                (entity as Player).CalcBoost();
                (entity as Player).Client.SendPacket(new InvResultPacket()
                {
                    Result = 0
                });
            }
        }
    }
}
