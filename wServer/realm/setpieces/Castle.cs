using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.logic.loot;
using wServer.realm.entities;

namespace wServer.realm.setpieces
{
    class Castle : ISetPiece
    {
        public int Size { get { return 40; } }

        protected static readonly byte Floor = (byte)XmlDatas.IdToType["Rock"];
        protected static readonly byte Bridge = (byte)XmlDatas.IdToType["Bridge"];
        protected static readonly byte WaterA = (byte)XmlDatas.IdToType["Shallow Water"];
        protected static readonly byte WaterB = (byte)XmlDatas.IdToType["Dark Water"];
        protected static readonly short WallA = XmlDatas.IdToType["Grey Wall"];
        protected static readonly short WallB = XmlDatas.IdToType["Destructible Grey Wall"];

        protected static readonly LootDef chest = new LootDef(0, 0, 0, 0,
                Tuple.Create(0.2, (ILoot)new TierLoot(6, ItemType.Weapon)),
                Tuple.Create(0.1, (ILoot)new TierLoot(7, ItemType.Weapon)),
                Tuple.Create(0.01, (ILoot)new TierLoot(8, ItemType.Weapon)),

                Tuple.Create(0.2, (ILoot)new TierLoot(5, ItemType.Armor)),
                Tuple.Create(0.1, (ILoot)new TierLoot(6, ItemType.Armor)),
                Tuple.Create(0.05, (ILoot)new TierLoot(7, ItemType.Armor)),

                Tuple.Create(0.2, (ILoot)new TierLoot(2, ItemType.Ability)),
                Tuple.Create(0.1, (ILoot)new TierLoot(3, ItemType.Ability)),
                Tuple.Create(0.01, (ILoot)new TierLoot(4, ItemType.Ability)),

                Tuple.Create(0.15, (ILoot)new TierLoot(2, ItemType.Ring)),
                Tuple.Create(0.05, (ILoot)new TierLoot(3, ItemType.Ring)),

                Tuple.Create(0.1, (ILoot)HpPotionLoot.Instance),
                Tuple.Create(0.1, (ILoot)MpPotionLoot.Instance)
            );

        Random rand = new Random();
        public void RenderSetPiece(World world, IntPoint pos)
        {
            int[,] t = new int[31, 40];

            for (int x = 0; x < 13; x++)    //Moats
                for (int y = 0; y < 13; y++)
                {
                    if ((x == 0 && (y < 3 || y > 9)) ||
                        (y == 0 && (x < 3 || x > 9)) ||
                        (x == 12 && (y < 3 || y > 9)) ||
                        (y == 12 && (x < 3 || x > 9)))
                        continue;
                    t[x + 0, y + 0] = t[x + 18, y + 0] = 2;
                    t[x + 0, y + 27] = t[x + 18, y + 27] = 2;
                }
            for (int x = 3; x < 28; x++)
                for (int y = 3; y < 37; y++)
                {
                    if (x < 6 || x > 24 || y < 6 || y > 33)
                        t[x, y] = 2;
                }

            for (int x = 7; x < 24; x++)    //Floor
                for (int y = 7; y < 33; y++)
                    t[x, y] = rand.Next() % 3 == 0 ? 0 : 1;

            for (int x = 0; x < 7; x++)    //Perimeter
                for (int y = 0; y < 7; y++)
                {
                    if ((x == 0 && y != 3) ||
                        (y == 0 && x != 3) ||
                        (x == 6 && y != 3) ||
                        (y == 6 && x != 3))
                        continue;
                    t[x + 3, y + 3] = t[x + 21, y + 3] = 4;
                    t[x + 3, y + 30] = t[x + 21, y + 30] = 4;
                }
            for (int x = 6; x < 25; x++)
                t[x, 6] = t[x, 33] = 4;
            for (int y = 6; y < 34; y++)
                t[6, y] = t[24, y] = 4;

            for (int x = 13; x < 18; x++)    //Bridge
                for (int y = 3; y < 7; y++)
                    t[x, y] = 6;

            for (int x = 0; x < 31; x++)    //Corruption
                for (int y = 0; y < 40; y++)
                {
                    if (t[x, y] == 1 || t[x, y] == 0) continue;
                    double p = rand.NextDouble();
                    if (t[x, y] == 6)
                    {
                        if (p < 0.4)
                            t[x, y] = 0;
                        continue;
                    }

                    if (p < 0.1)
                        t[x, y] = 1;
                    else if (p < 0.4)
                        t[x, y]++;
                }

            //Boss & Chest
            t[15, 27] = 7;
            t[15, 20] = 8;

            int r = rand.Next(0, 4);
            for (int i = 0; i < r; i++)     //Rotation
                t = SetPieces.rotateCW(t);
            int w = t.GetLength(0), h = t.GetLength(1);

            for (int x = 0; x < w; x++)     //Rendering
                for (int y = 0; y < h; y++)
                {
                    if (t[x, y] == 1)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Floor;
                        tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }

                    else if (t[x, y] == 2)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = WaterA;
                        tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 3)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = WaterB;
                        tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 3;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }

                    else if (t[x, y] == 4)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Floor;
                        tile.ObjType = WallA;
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Obstacles[x + pos.X, y + pos.Y] = 2;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 5)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Floor;
                        world.Obstacles[x + pos.X, y + pos.Y] = 2;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                        Entity wall = Entity.Resolve(WallB);
                        wall.Move(x + pos.X + 0.5f, y + pos.Y + 0.5f);
                        world.EnterWorld(wall);
                    }

                    else if (t[x, y] == 6)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Bridge;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 7)
                    {
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
                        container.Move(pos.X + x + 0.5f, pos.Y + y + 0.5f);
                        world.EnterWorld(container);
                    }
                    else if (t[x, y] == 8)
                    {
                        Entity cyclops = Entity.Resolve(0x67d);
                        cyclops.Move(pos.X + x, pos.Y + y);
                        world.EnterWorld(cyclops);
                    }
                }
        }
    }
}
