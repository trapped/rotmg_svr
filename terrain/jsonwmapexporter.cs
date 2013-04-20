using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using terrain;
using Ionic.Zlib;
using Newtonsoft.Json;
using System.IO;

namespace terrain
{
    class jsonwmapexporter
    {
        static String filepath = null;
        static void Main()
        {
            byte[] dis = Convert(File.ReadAllText("C:/map.jm"));
            File.WriteAllBytes("C:/Users/Ospite/Desktop/map.wmap", dis);
        }
        private struct obj
        {
            public string name;
            public string id;
        }
        private struct loc
        {
            public string ground;
            public obj[] objs;
            public obj[] regions;
        }
        private struct json_dat
        {
            public byte[] data;
            public int width;
            public int height;
            public loc[] dict;
        }
        void Export(TerrainTile[,] tiles, string path)
        {
            File.WriteAllBytes(path, Export(tiles));
        }
        public static byte[] Convert(string json)
        {
            var obj = JsonConvert.DeserializeObject<json_dat>(json);
            var dat = ZlibStream.UncompressBuffer(obj.data);

            Dictionary<short, TerrainTile> tileDict = new Dictionary<short, TerrainTile>();
            for (int i = 0; i < obj.dict.Length; i++)
            {
                var o = obj.dict[i];
                tileDict[(short)i] = new TerrainTile()
                {
                    TileId = o.ground == null ? (short)0xff : XmlDatas.IdToType[o.ground],
                    TileObj = o.objs == null ? null : o.objs[0].id,
                    Name = o.objs == null ? "" : o.objs[0].name ?? "",
                    Terrain = TerrainType.None,
                    Region = o.regions == null ? TileRegion.None : (TileRegion)Enum.Parse(typeof(TileRegion), o.regions[0].id.Replace(' ', '_'))
                };
            }

            var tiles = new TerrainTile[obj.width, obj.height];
            using (NReader rdr = new NReader(new MemoryStream(dat)))
                for (int y = 0; y < obj.height; y++)
                    for (int x = 0; x < obj.width; x++)
                    {
                        tiles[x, y] = tileDict[rdr.ReadInt16()];
                    }
            return Export(tiles);
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
