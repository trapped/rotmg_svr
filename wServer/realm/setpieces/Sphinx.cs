using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.entities;
using wServer.logic.loot;

namespace wServer.realm.setpieces
{
    class Sphinx : ISetPiece
    {
        public int Size
        {
            get { return 81; }
        }

        static readonly byte[,] Center = new byte[,]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
        };

        static readonly byte Floor = (byte)XmlDatas.IdToType["Gold Sand"];
        static readonly byte Central = (byte)XmlDatas.IdToType["Sand Tile"];
        static readonly short Pillar = XmlDatas.IdToType["Tomb Wall"];

        Random rand = new Random();
        public void RenderSetPiece(World world, IntPoint pos)
        {
            int[,] t = new int[81, 81];
            for (int x = 0; x < Size; x++)                      //Flooring
                for (int y = 0; y < Size; y++)
                {
                    double dx = x - (Size / 2.0);
                    double dy = y - (Size / 2.0);
                    double r = Math.Sqrt(dx * dx + dy * dy) + rand.NextDouble() * 4 - 2;
                    if (r <= 35)
                        t[x, y] = 1;
                }

            for (int x = 0; x < 17; x++)                        //Center
                for (int y = 0; y < 17; y++)
                {
                    if (Center[x, y] != 0)
                        t[32 + x, 32 + y] = 2;
                }

            t[36, 36] = t[44, 36] = t[36, 44] = t[44, 44] = 3;  //Pillars
            t[30, 30] = t[50, 30] = t[30, 50] = t[50, 50] = 4;

            t[40, 26] = t[40, 27] = t[39, 27] = t[41, 27] = 4;
            t[40, 54] = t[40, 53] = t[39, 53] = t[41, 53] = 4;
            t[26, 40] = t[27, 40] = t[27, 39] = t[27, 41] = 4;
            t[54, 40] = t[53, 40] = t[53, 39] = t[53, 41] = 4;


            for (int x = 0; x < Size; x++)                      //Rendering
                for (int y = 0; y < Size; y++)
                    if (t[x, y] == 1)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Floor; tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 2)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Central; tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 3)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Central;
                        tile.ObjType = Pillar;
                        tile.Name = ConnectionComputer.GetConnString((_x, _y) => t[x + _x, y + _y] == 3);
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Obstacles[x + pos.X, y + pos.Y] = 2;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 4)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Floor;
                        tile.ObjType = Pillar;
                        tile.Name = ConnectionComputer.GetConnString((_x, _y) => t[x + _x, y + _y] == 4);
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Obstacles[x + pos.X, y + pos.Y] = 2;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }

            Entity sphinx = Entity.Resolve(0x0d54);
            sphinx.Move(pos.X + 40.5f, pos.Y + 40.5f);
            world.EnterWorld(sphinx);
        }
    }
}
