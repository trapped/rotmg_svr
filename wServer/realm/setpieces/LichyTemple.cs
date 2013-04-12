using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.realm.setpieces
{
    class LichyTemple : ISetPiece
    {
        public int Size { get { return 26; } }

        protected static readonly byte Floor = (byte)XmlDatas.IdToType["Blue Floor"];
        protected static readonly short WallA = XmlDatas.IdToType["Blue Wall"];
        protected static readonly short WallB = XmlDatas.IdToType["Destructible Blue Wall"];
        protected static readonly short PillarA = XmlDatas.IdToType["Blue Pillar"];
        protected static readonly short PillarB = XmlDatas.IdToType["Broken Blue Pillar"];

        Random rand = new Random();
        public void RenderSetPiece(World world, IntPoint pos)
        {
            int[,] t = new int[25, 26];

            for (int x = 2; x < 23; x++)    //Floor
                for (int y = 1; y < 24; y++)
                    t[x, y] = rand.Next() % 10 == 0 ? 0 : 1;

            for (int y = 1; y < 24; y++)    //Perimeters
                t[2, y] = t[22, y] = 2;
            for (int x = 2; x < 23; x++)
                t[x, 23] = 2;
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                    t[x + 1, y] = t[x + 21, y] = 2;
            for (int x = 0; x < 5; x++)
                for (int y = 0; y < 5; y++)
                {
                    if ((x == 0 && y == 0) ||
                        (x == 0 && y == 4) ||
                        (x == 4 && y == 0) ||
                        (x == 4 && y == 4)) continue;
                    t[x, y + 21] = t[x + 20, y + 21] = 2;
                }

            for (int y = 0; y < 6; y++)     //Pillars
                t[9, 4 + 3 * y] = t[15, 4 + 3 * y] = 4;

            for (int x = 0; x < 25; x++)    //Corruption
                for (int y = 0; y < 26; y++)
                {
                    if (t[x, y] == 1 || t[x, y] == 0) continue;
                    double p = rand.NextDouble();
                    if (p < 0.1)
                        t[x, y] = 1;
                    else if (p < 0.4)
                        t[x, y]++;
                }

            int r = rand.Next(0, 4);
            for (int i = 0; i < r; i++)     //Rotation
                t = SetPieces.rotateCW(t);
            int w = t.GetLength(0), h = t.GetLength(1);
            
            for (int x = 0; x < w; x++)    //Rendering
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
                        tile.TileId = Floor;
                        tile.ObjType = WallA;
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Obstacles[x + pos.X, y + pos.Y] = 2;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 3)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Floor;
                        world.Obstacles[x + pos.X, y + pos.Y] = 2;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                        Entity wall = Entity.Resolve(WallB);
                        wall.Move(x + pos.X + 0.5f, y + pos.Y + 0.5f);
                        world.EnterWorld(wall);
                    }
                    else if (t[x, y] == 4)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Floor; 
                        tile.ObjType = PillarA;
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Obstacles[x + pos.X, y + pos.Y] = 2;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 5)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Floor; 
                        tile.ObjType = PillarB;
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Obstacles[x + pos.X, y + pos.Y] = 2;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                }

            //Boss
            Entity lich = Entity.Resolve(0x091b);
            lich.Move(pos.X + Size / 2, pos.Y + Size / 2);
            world.EnterWorld(lich);
        }
    }
}
