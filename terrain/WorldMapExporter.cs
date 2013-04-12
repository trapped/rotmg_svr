using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zlib;

namespace terrain
{
    class WorldMapExporter
    {
        public static void Export(TerrainTile[,] tiles, string path)
        {
            File.WriteAllBytes(path, Export(tiles));
        }
        public static byte[] Export(TerrainTile[,] tiles)
        {
            List<TerrainTile> dict = new List<TerrainTile>();

            int w = tiles.GetLength(0);
            int h = tiles.GetLength(1);
            byte[] dat = new byte[w * h * 2];
            int idx = 0;
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    TerrainTile tile = tiles[x, y];
                    short i = (short)dict.IndexOf(tile);
                    if (i == -1)
                    {
                        i = (short)dict.Count;
                        dict.Add(tile);
                    }
                    dat[idx] = (byte)(i & 0xff);
                    dat[idx + 1] = (byte)(i >> 8);
                    idx += 2;
                }

            MemoryStream ms = new MemoryStream();
            using (BinaryWriter wtr = new BinaryWriter(ms))
            {
                wtr.Write((short)dict.Count);
                foreach (var i in dict)
                {
                    wtr.Write(i.TileId);
                    wtr.Write(i.TileObj ?? "");
                    wtr.Write(i.Name ?? "");
                    wtr.Write((byte)i.Terrain);
                    wtr.Write((byte)i.Region);
                }
                wtr.Write(w);
                wtr.Write(h);
                wtr.Write(dat);
            }
            return ZlibStream.CompressBuffer(ms.ToArray());
        }
    }
}
