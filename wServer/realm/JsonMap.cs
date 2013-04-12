using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Ionic.Zlib;
using System.IO;
using db;
using wServer.realm.entities;

namespace wServer.realm
{
    public class MapTileDesc
    {
        public Tile Tile { get; set; }
        public TileRegion Region { get; set; }
        public short ObjType { get; set; }
        public string Name { get; set; }
    }

    public enum TileRegion : byte
    {
        None,
        Spawn,
        Realm_Portals,
        Store_1,
        Store_2,
        Store_3,
        Store_4,
        Store_5,
        Store_6,
        Store_7,
        Store_8,
        Store_9,
        Vault,
        Loot,
        Defender,
        Hallway,
        Hallway_1,
        Hallway_2,
        Hallway_3,
        Enemy,
    }
    public struct MapTile
    {
        public MapTileDesc Desc;
        public int ObjId;

        public ObjectDef ToDef(int x, int y)
        {
            List<KeyValuePair<StatsType, object>> stats = new List<KeyValuePair<StatsType, object>>();
            if (Desc.Name != null)
                foreach (var item in Desc.Name.Split(';'))
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
                ObjectType = Desc.ObjType,
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
    }
    public class JsonMap
    {
        public int Width { get; set; }
        public int Height { get; set; }

        MapTile[,] tiles;
        public MapTile this[int x, int y]
        {
            get { return tiles[x, y]; }
        }

        public void Init(int w, int h)
        {
            Width = w; Height = h;
            tiles = new MapTile[w, h];
            descs = new Tuple<Position, obj>[0];
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

        Tuple<Position, obj>[] descs;
        public IEnumerable<Entity> InstantiateEntities()
        {
            foreach (var i in descs)
            {
                var entity = Entity.Resolve(Objects.id2type[i.Item2.id]);
                entity.Move(i.Item1.X + 0.5f, i.Item1.Y + 0.5f);
                if (i.Item2.name != null)
                    foreach (var item in i.Item2.name.Split(';'))
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
                                (entity as ConnectedObject).Connection = Utils.FromString(kv[1]); break;
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
        //public void SaveEntities(IEnumerable<Entity> entities)
        //{
        //    List<Tuple<Position, obj>> list = new List<Tuple<Position, obj>>();
        //    foreach (var i in entities)
        //    {
        //        var o = new obj() { id = Objects.type2id[(short)i.ObjectType] };
        //        string s = "";
        //        if (!string.IsNullOrEmpty(i.Name))
        //            s += ";name:" + i.Name;
        //        if (i.Size != 100)
        //            s += ";size:" + i.Size;
        //        if (i.ConditionEffects != 0)
        //            s += ";eff:0x" + ((int)i.ConditionEffects).ToString("X8");
        //        if (i is ConnectedObject)
        //            s += ";conn:0x" + (i as ConnectedObject).Connection.ToString("X8");
        //        //if (en.Stats.ContainsKey(StatsType.MerchantMerchandiseType))
        //        //    s += ";mtype:" + en.Stats[StatsType.MerchantMerchandiseType];
        //        //if (en.Stats.ContainsKey(StatsType.MerchantRemainingCount))
        //        //    s += ";mcount:" + en.Stats[StatsType.MerchantRemainingCount];
        //        //if (en.Stats.ContainsKey(StatsType.MerchantRemainingMinute))
        //        //    s += ";mtime:" + en.Stats[StatsType.MerchantRemainingMinute];
        //        //if (en.Stats.ContainsKey(StatsType.NameChangerStar))
        //        //    s += ";nstar:" + en.Stats[StatsType.NameChangerStar];
        //        o.name = s.Trim(';');
        //        list.Add(new Tuple<Position, obj>(new Position() { X = (int)(i.X - 0.5), Y = (int)(i.Y - 0.5) }, o));
        //    }
        //    descs = list.ToArray();
        //}

        MapTileDesc loc2Tile(loc loc, List<obj> objs)
        {
            MapTileDesc ret = new MapTileDesc();

            if (loc.ground != null)
                ret.Tile = (Tile)Grounds.id2type[loc.ground];
            else
                ret.Tile = (Tile)0xff;

            if (loc.objs != null)
            {
                foreach (var z in loc.objs)
                {
                    ObjectDesc desc;
                    if (Objects.objectDescs.TryGetValue(Objects.id2type[z.id], out desc))
                    {
                        if (desc.Static && !desc.Enemy)
                        {
                            ret.ObjType = desc.ObjectType;
                            ret.Name = z.name;
                        }
                        else
                            objs.Add(z);
                    }
                    else
                        objs.Add(z);
                }
            }

            if (loc.regions != null)
                ret.Region = (TileRegion)Enum.Parse(typeof(TileRegion), loc.regions[0].id.Replace(' ', '_'));
            return ret;
        }
        public int FromJson(string json, int idBase)
        {
            if (string.IsNullOrEmpty(json))
            {
                Init(1, 1);
                return 0;
            }
            var obj = JsonConvert.DeserializeObject<json_dat>(json);
            var dat = ZlibStream.UncompressBuffer(obj.data);
            Init(obj.width, obj.height);

            Dictionary<short, MapTileDesc> tileDict = new Dictionary<short, MapTileDesc>();
            Dictionary<short, List<obj>> objDict = new Dictionary<short, List<obj>>();
            for (int i = 0; i < obj.dict.Length; i++)
            {
                var o = new List<obj>();
                tileDict[(short)i] = loc2Tile(obj.dict[i], o);
                if (o.Count > 0)
                    objDict[(short)i] = o;
            }

            var objs = new List<Tuple<Position, obj>>();
            int c = 0;
            using (NReader rdr = new NReader(new MemoryStream(dat)))
                for (int y = 0; y < obj.height; y++)
                    for (int x = 0; x < obj.width; x++)
                    {
                        var k = rdr.ReadInt16();
                        if (tileDict[k].ObjType != 0)
                        {
                            c++;
                            tiles[x, y] = new MapTile()
                            {
                                Desc = tileDict[k],
                                ObjId = idBase + c
                            };
                        }
                        else
                            tiles[x, y] = new MapTile()
                            {
                                Desc = tileDict[k],
                                ObjId = 0
                            };
                        List<obj> o;
                        if (objDict.TryGetValue(k, out o))
                            foreach (var i in o)
                                objs.Add(new Tuple<Position, obj>(new Position()
                                {
                                    X = x,
                                    Y = y
                                }, i));
                    }
            descs = objs.ToArray();
            return c;
        }

        //public string ToJson()
        //{
        //    var obj = new json_dat();
        //    obj.width = Width; obj.height = Height;

        //    Dictionary<Position, TileRegion> regs = new Dictionary<Position, TileRegion>();
        //    foreach (var i in Regions)
        //        foreach (var j in i.Value)
        //            regs.Add(j, i.Key);

        //    List<loc> locs = new List<loc>();
        //    MemoryStream ms = new MemoryStream();
        //    using (NWriter wtr = new NWriter(ms))
        //        for (int y = 0; y < obj.height; y++)
        //            for (int x = 0; x < obj.width; x++)
        //            {
        //                var loc = new loc();
        //                loc.ground = Grounds.type2id[(short)Tiles[x][y]];
        //                loc.objs = descs.Where(_ => _.Item1.X == x && _.Item1.Y == y).Select(_ => _.Item2).ToArray();
        //                TileRegion reg;
        //                if (regs.TryGetValue(new Position() { X = x, Y = y }, out reg))
        //                    loc.regions = new obj[] { new obj() { id = reg.ToString().Replace('_', ' ') } };

        //                int ix = locs.IndexOf(loc);
        //                if (ix == -1)
        //                {
        //                    ix = locs.Count;
        //                    locs.Add(loc);
        //                }
        //                wtr.Write((short)ix);
        //            }
        //    obj.data = ZlibStream.CompressBuffer(ms.ToArray());
        //    obj.dict = locs.ToArray();
        //    return JsonConvert.SerializeObject(obj);
        //}
    }
}
