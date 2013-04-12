using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace terrain
{
    enum TerrainType
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
    enum TileRegion : byte
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
    struct TerrainTile : IEquatable<TerrainTile>
    {
        public int PolygonId;
        public float Elevation;
        public float Moisture;
        public string Biome;
        public short TileId;
        public string Name;
        public string TileObj;
        public TerrainType Terrain;
        public TileRegion Region;

        public bool Equals(TerrainTile other)
        {
            return
                this.TileId  == other.TileId &&
                this.TileObj == other.TileObj &&
                this.Name == other.Name &&
                this.Terrain == other.Terrain &&
                this.Region == other.Region;
        }
    }
}
