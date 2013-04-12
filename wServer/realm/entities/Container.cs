using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace wServer.realm.entities
{
    public interface IContainer
    {
        int[] SlotTypes { get; }
        Item[] Inventory { get; }
    }

    public class Container : StaticObject, IContainer
    {
        public Container(short objType, int? life, bool dying)
            : base(objType, life, false, dying, false)
        {
            Inventory = new Item[12];
            SlotTypes = new int[12];
        }

        public Container(XElement node)
            : base((short)Utils.FromString(node.Attribute("type").Value), null, false, false, false)
        {
            SlotTypes = Utils.FromCommaSepString32(node.Element("SlotTypes").Value);
            XElement eq = node.Element("Equipment");
            if (eq != null)
            {
                var inv = Utils.FromCommaSepString16(eq.Value).Select(_ => _ == -1 ? null : XmlDatas.ItemDescs[_]).ToArray();
                Array.Resize(ref inv, 12);
                Inventory = inv;
            }
        }

        public int[] SlotTypes { get; private set; }
        public Item[] Inventory { get; private set; }
        public int? BagOwner { get; set; }

        protected override void ImportStats(StatsType stats, object val)
        {
            switch (stats)
            {
                case StatsType.Inventory0: Inventory[0] = (int)val == -1 ? null : XmlDatas.ItemDescs[(short)(int)val]; break;
                case StatsType.Inventory1: Inventory[1] = (int)val == -1 ? null : XmlDatas.ItemDescs[(short)(int)val]; break;
                case StatsType.Inventory2: Inventory[2] = (int)val == -1 ? null : XmlDatas.ItemDescs[(short)(int)val]; break;
                case StatsType.Inventory3: Inventory[3] = (int)val == -1 ? null : XmlDatas.ItemDescs[(short)(int)val]; break;
                case StatsType.Inventory4: Inventory[4] = (int)val == -1 ? null : XmlDatas.ItemDescs[(short)(int)val]; break;
                case StatsType.Inventory5: Inventory[5] = (int)val == -1 ? null : XmlDatas.ItemDescs[(short)(int)val]; break;
                case StatsType.Inventory6: Inventory[6] = (int)val == -1 ? null : XmlDatas.ItemDescs[(short)(int)val]; break;
                case StatsType.Inventory7: Inventory[7] = (int)val == -1 ? null : XmlDatas.ItemDescs[(short)(int)val]; break;
                case StatsType.Inventory8: Inventory[8] = (int)val == -1 ? null : XmlDatas.ItemDescs[(short)(int)val]; break;
                case StatsType.Inventory9: Inventory[9] = (int)val == -1 ? null : XmlDatas.ItemDescs[(short)(int)val]; break;
                case StatsType.Inventory10: Inventory[10] = (int)val == -1 ? null : XmlDatas.ItemDescs[(short)(int)val]; break;
                case StatsType.Inventory11: Inventory[11] = (int)val == -1 ? null : XmlDatas.ItemDescs[(short)(int)val]; break;
                case StatsType.OwnerAccountId: BagOwner = (int)val == -1 ? (int?)null : (int)val; break;
            }
            base.ImportStats(stats, val);
        }
        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.Inventory0] = (Inventory[0] != null ? Inventory[0].ObjectType : -1);
            stats[StatsType.Inventory1] = (Inventory[1] != null ? Inventory[1].ObjectType : -1);
            stats[StatsType.Inventory2] = (Inventory[2] != null ? Inventory[2].ObjectType : -1);
            stats[StatsType.Inventory3] = (Inventory[3] != null ? Inventory[3].ObjectType : -1);
            stats[StatsType.Inventory4] = (Inventory[4] != null ? Inventory[4].ObjectType : -1);
            stats[StatsType.Inventory5] = (Inventory[5] != null ? Inventory[5].ObjectType : -1);
            stats[StatsType.Inventory6] = (Inventory[6] != null ? Inventory[6].ObjectType : -1);
            stats[StatsType.Inventory7] = (Inventory[7] != null ? Inventory[7].ObjectType : -1);
            stats[StatsType.Inventory8] = (Inventory[8] != null ? Inventory[8].ObjectType : -1);
            stats[StatsType.Inventory9] = (Inventory[9] != null ? Inventory[9].ObjectType : -1);
            stats[StatsType.Inventory10] = (Inventory[10] != null ? Inventory[10].ObjectType : -1);
            stats[StatsType.Inventory11] = (Inventory[11] != null ? Inventory[11].ObjectType : -1);
            stats[StatsType.OwnerAccountId] = BagOwner ?? -1;
            base.ExportStats(stats);
        }

        public override void Tick(RealmTime time)
        {
            if (ObjectType == 0x504)    //Vault chest
                return;
            bool hasItem = false;
            foreach (var i in Inventory)
                if (i != null)
                {
                    hasItem = true;
                    break;
                }
            if (!hasItem)
                Owner.LeaveWorld(this);
            base.Tick(time);
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            return false;
        }
    }
}
