using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.logic.loot;
using terrain;

namespace wServer.realm.setpieces
{
    class SkullShrine : ISetPiece
    {
        public int Size { get { return 33; } }

        static readonly byte Grass = (byte)XmlDatas.IdToType["Blue Grass"];
        static readonly byte Tile = (byte)XmlDatas.IdToType["Castle Stone Floor Tile"];
        static readonly byte TileDark = (byte)XmlDatas.IdToType["Castle Stone Floor Tile Dark"];
        static readonly byte Stone = (byte)XmlDatas.IdToType["Cracked Purple Stone"];
        protected static readonly short PillarA = XmlDatas.IdToType["Blue Pillar"];
        protected static readonly short PillarB = XmlDatas.IdToType["Broken Blue Pillar"];

        Random rand = new Random();
        public void RenderSetPiece(World world, IntPoint pos)
        {
            int[,] t = new int[33, 33];

            for (int x = 0; x < 33; x++)                    //Grassing
                for (int y = 0; y < 33; y++)
                {
                    if (Math.Abs(x - Size / 2) / (Size / 2.0) + rand.NextDouble() * 0.3 < 0.95 &&
                        Math.Abs(y - Size / 2) / (Size / 2.0) + rand.NextDouble() * 0.3 < 0.95)
                        t[x, y] = 1;
                }

            for (int x = 12; x < 21; x++)                   //Outer
                for (int y = 4; y < 29; y++)
                    t[x, y] = 2;
            t = SetPieces.rotateCW(t);
            for (int x = 12; x < 21; x++)
                for (int y = 4; y < 29; y++)
                    t[x, y] = 2;

            for (int x = 13; x < 20; x++)                   //Inner
                for (int y = 5; y < 28; y++)
                    t[x, y] = 4;
            t = SetPieces.rotateCW(t);
            for (int x = 13; x < 20; x++)
                for (int y = 5; y < 28; y++)
                    t[x, y] = 4;

            for (int i = 0; i < 4; i++)                     //Ext
            {
                for (int x = 13; x < 20; x++)
                    for (int y = 5; y < 7; y++)
                        t[x, y] = 3;
                t = SetPieces.rotateCW(t);
            }

            for (int i = 0; i < 4; i++)                     //Pillars
            {
                t[13, 7] = rand.Next() % 3 == 0 ? 6 : 5;
                t[19, 7] = rand.Next() % 3 == 0 ? 6 : 5;
                t[13, 10] = rand.Next() % 3 == 0 ? 6 : 5;
                t[19, 10] = rand.Next() % 3 == 0 ? 6 : 5;
                t = SetPieces.rotateCW(t);
            }

            Noise noise = new Noise(Environment.TickCount); //Perlin noise
            for (int x = 0; x < 33; x++)
                for (int y = 0; y < 33; y++)
                    if (noise.GetNoise(x / 33f * 8, y / 33f * 8, .5f) < 0.2)
                        t[x, y] = 0;

            for (int x = 0; x < 33; x++)                    //Rendering
                for (int y = 0; y < 33; y++)
                {
                    if (t[x, y] == 1)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Grass; tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 2)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = TileDark; tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 3)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Tile; tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 4)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Stone; tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 5)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Stone; tile.ObjType = PillarA;
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Obstacles[x + pos.X, y + pos.Y] = 2;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 6)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Stone; tile.ObjType = PillarB;
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Obstacles[x + pos.X, y + pos.Y] = 2;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                }

            Entity skull = Entity.Resolve(0x0d56);          //Skulls!
            skull.Move(pos.X + Size / 2f, pos.Y + Size / 2f);
            world.EnterWorld(skull);
        }
    }
}
