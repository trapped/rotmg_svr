using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.entities;
using wServer.logic.loot;

namespace wServer.realm.setpieces
{
    class Pyre : ISetPiece
    {
        public int Size
        {
            get { return 30; }
        }

        static readonly byte Floor = (byte)XmlDatas.IdToType["Scorch Blend"];

        static readonly LootDef chest = new LootDef(0, 0, 0, 0,
                Tuple.Create(0.2, (ILoot)new TierLoot(5, ItemType.Weapon)),
                Tuple.Create(0.1, (ILoot)new TierLoot(6, ItemType.Weapon)),
                Tuple.Create(0.01, (ILoot)new TierLoot(7, ItemType.Weapon)),

                Tuple.Create(0.2, (ILoot)new TierLoot(4, ItemType.Armor)),
                Tuple.Create(0.1, (ILoot)new TierLoot(5, ItemType.Armor)),
                Tuple.Create(0.05, (ILoot)new TierLoot(6, ItemType.Armor)),

                Tuple.Create(0.2, (ILoot)new TierLoot(2, ItemType.Ability)),
                Tuple.Create(0.01, (ILoot)new TierLoot(3, ItemType.Ability)),

                Tuple.Create(0.15, (ILoot)new TierLoot(1, ItemType.Ring)),
                Tuple.Create(0.05, (ILoot)new TierLoot(2, ItemType.Ring)),

                Tuple.Create(0.1, (ILoot)HpPotionLoot.Instance),
                Tuple.Create(0.1, (ILoot)MpPotionLoot.Instance)
            );

        Random rand = new Random();
        public void RenderSetPiece(World world, IntPoint pos)
        {
            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                {
                    double dx = x - (Size / 2.0);
                    double dy = y - (Size / 2.0);
                    double r = Math.Sqrt(dx * dx + dy * dy) + rand.NextDouble() * 4 - 2;
                    if (r <= 10)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Floor; tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                }

            Entity lord = Entity.Resolve(0x675);
            lord.Move(pos.X + 15.5f, pos.Y + 15.5f);
            world.EnterWorld(lord);

            Container container = new Container(0x0501, null, false);
            int count = rand.Next(5, 8);
            List<Item> items = new List<Item>();
            while (items.Count < count)
            {
                Item item = chest.GetRandomLoot(rand);
                if (item != null) items.Add(item);
            }
            for (int i = 0; i < items.Count; i++)
                container.Inventory[i] = items[i];
            container.Move(pos.X + 15.5f, pos.Y + 15.5f);
            world.EnterWorld(container);
        }
    }
}
