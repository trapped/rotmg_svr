using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zlib;
using wServer.realm.entities;

namespace wServer.realm
{
    public enum TileRegion : byte
    {
        None,
        Spawn,
        Realm_Portals,
        //Store_1,
        //Store_2,
        //Store_3,
        //Store_4,
        //Store_5,
        //Store_6,
        //Store_7,
        //Store_8,
        //Store_9,
        Vault,
        Loot,
        Defender,
        Hallway,
        Hallway_1,
        Hallway_2,
        Hallway_3,
        Enemy,
    }
    public enum WmapTerrain : byte
    {
        None,
        Mountains,
        HighSand,
        HighPlains,
        HighForest,
        MidSand,
        MidPlains,
        MidForest,
        LowSand,
        LowPlains,
        LowForest,
        ShoreSand,
        ShorePlains,
    }
    public struct WmapTile
    {
        public byte UpdateCount;
        public byte TileId;
        public string Name;
        public short ObjType;
        public WmapTerrain Terrain;
        public TileRegion Region;
        public int ObjId;

        public ObjectDef ToDef(int x, int y)
        {
            List<KeyValuePair<StatsType, object>> stats = new List<KeyValuePair<StatsType, object>>();
            if (!string.IsNullOrEmpty(Name))
                foreach (var item in Name.Split(';'))
                {
                    string[] kv = item.Split(':');
                    switch (kv[0])
                    {
                        case "name":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.Name, kv[1])); break;
                        case "size":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.Size, Utils.FromString(kv[1]))); break;
                        case "eff":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.Effects, Utils.FromString(kv[1]))); break;
                        case "conn":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.ObjectConnection, Utils.FromString(kv[1]))); break;
                        //case "mtype":
                        //    entity.Stats[StatsType.MerchantMerchandiseType] = Utils.FromString(kv[1]); break;
                        //case "mcount":
                        //    entity.Stats[StatsType.MerchantRemainingCount] = Utils.FromString(kv[1]); break;
                        //case "mtime":
                        //    entity.Stats[StatsType.MerchantRemainingMinute] = Utils.FromString(kv[1]); break;
                        //case "nstar":
                        //    entity.Stats[StatsType.NameChangerStar] = Utils.FromString(kv[1]); break;
                    }
                }
            return new ObjectDef()
            {
                ObjectType = ObjType,
                Stats = new ObjectStats()
                {
                    Id = ObjId,
                    Position = new Position()
                    {
                        X = x + 0.5f,
                        Y = y + 0.5f
                    },
                    Stats = stats.ToArray()
                }
            };
        }

        public WmapTile Clone()
        {
            return new WmapTile()
            {
                UpdateCount = (byte)(UpdateCount + 1),
                TileId = TileId,
                Name = Name,
                ObjType = ObjType,
                Terrain = Terrain,
                Region = Region,
                ObjId = ObjId,
            };
        }
    }
    public class Wmap
    {
        public int Width { get; set; }
        public int Height { get; set; }

        WmapTile[,] tiles;
        public WmapTile this[int x, int y]
        {
            get { return tiles[x, y]; }
            set { tiles[x, y] = value; }
        }

        Tuple<IntPoint, short, string>[] entities;
        public int Load(Stream stream, int idBase)
        {
            List<WmapTile> dict = new List<WmapTile>();
            using (BinaryReader rdr = new BinaryReader(new ZlibStream(stream, CompressionMode.Decompress)))
            {
                short c = rdr.ReadInt16();
                for (short i = 0; i < c; i++)
                {
                    WmapTile tile = new WmapTile();
                    tile.TileId = (byte)rdr.ReadInt16();
                    string obj = rdr.ReadString();
                    tile.ObjType = string.IsNullOrEmpty(obj) ? (short)0 : XmlDatas.IdToType[obj];
                    tile.Name = rdr.ReadString();
                    tile.Terrain = (WmapTerrain)rdr.ReadByte();
                    tile.Region = (TileRegion)rdr.ReadByte();
                    dict.Add(tile);
                }
                Width = rdr.ReadInt32();
                Height = rdr.ReadInt32();
                tiles = new WmapTile[Width, Height];
                int enCount = 0;
                List<Tuple<IntPoint, short, string>> entities = new List<Tuple<IntPoint, short, string>>();
                for (int y = 0; y < Height; y++)
                    for (int x = 0; x < Width; x++)
                    {
                        WmapTile tile = dict[rdr.ReadInt16()];
                        tile.UpdateCount = 1;

                        ObjectDesc desc;
                        if (tile.ObjType != 0 &&
                            (!XmlDatas.ObjectDescs.TryGetValue(tile.ObjType, out desc) ||
                            !desc.Static || desc.Enemy))
                        {
                            entities.Add(new Tuple<IntPoint, short, string>(new IntPoint(x, y), tile.ObjType, tile.Name));
                            tile.ObjType = 0;
                        }

                        if (tile.ObjType != 0)
                        {
                            enCount++;
                            tile.ObjId = idBase + enCount;
                        }


                        tiles[x, y] = tile;
                    }
                this.entities = entities.ToArray();
                return enCount;
            }
        }

        public IEnumerable<Entity> InstantiateEntities()
        {
            foreach (var i in entities)
            {
                var entity = Entity.Resolve(i.Item2);
                entity.Move(i.Item1.X + 0.5f, i.Item1.Y + 0.5f);
                if (i.Item3 != null)
                    foreach (var item in i.Item3.Split(';'))
                    {
                        string[] kv = item.Split(':');
                        switch (kv[0])
                        {
                            case "name":
                                entity.Name = kv[1]; break;
                            case "size":
                                entity.Size = Utils.FromString(kv[1]); break;
                            case "eff":
                                entity.ConditionEffects = (ConditionEffects)Utils.FromString(kv[1]); break;
                            case "conn":
                                (entity as ConnectedObject).Connection = ConnectionInfo.Infos[(uint)Utils.FromString(kv[1])]; break;
                            //case "mtype":
                            //    entity.Stats[StatsType.MerchantMerchandiseType] = Utils.FromString(kv[1]); break;
                            //case "mcount":
                            //    entity.Stats[StatsType.MerchantRemainingCount] = Utils.FromString(kv[1]); break;
                            //case "mtime":
                            //    entity.Stats[StatsType.MerchantRemainingMinute] = Utils.FromString(kv[1]); break;
                            //case "nstar":
                            //    entity.Stats[StatsType.NameChangerStar] = Utils.FromString(kv[1]); break;
                        }
                    }
                yield return entity;
            }
        }
    }
}
