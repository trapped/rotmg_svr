using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace terrain
{
    class TileTypes
    {
        public const short DeepWater = 0xbc;
        public const short Water = 0x73;
        public const short MovingWater = 0x72;
        public const short Beach = 0xbe;
        public const short Sand = 0xbd;
        public const short Grass = 0x48;
        public const short BrightGrass = 0x23;
        public const short LightGrass = 0x46;
        public const short YellowGrass = 0x47;
        public const short DarkGrass = 0x56;
        public const short BlueGrass = 0x57;
        public const short Snow = 0x6b;
        public const short SnowRock = 0x6a;
        public const short Rock = 0x60;

        public const short Road = 0xd0;

        public static readonly Dictionary<short, string> id = new Dictionary<short, string>()
        {
            { 0xbc, "Dark Water" },
            { 0x72, "Water" },
            { 0x73, "Shallow Water" },
            { 0xbe, "Dark Sand" },
            { 0xbd, "Light Sand" },
            { 0x48, "Grass" },
            { 0x23, "Bright Grass" },
            { 0x46, "Light Grass" },
            { 0x47, "Yellow Grass" },
            { 0x56, "Dark Grass" },
            { 0x57, "Blue Grass" },
            { 0x6b, "Ice" },
            { 0x6a, "Snowy Rock" },
            { 0x60, "Rock" },
            { 0xd0, "Road" },
        };

        public static readonly Dictionary<short, uint> color = new Dictionary<short, uint>()
        {
            { 0xbc, 0xFF000080 },
            { 0x72, 0xFF0000FF },
            { 0x73, 0xFF0000FF },
            { 0xbe, 0xFF969648 },
            { 0xbd, 0xFFF7F7BA },
            { 0x48, 0xFF4FCF00 },
            { 0x23, 0xFF6FEF20 },
            { 0x46, 0xFFC3E612 },
            { 0x47, 0xFFB3D602 },
            { 0x56, 0xFF107D1D },
            { 0x57, 0xFF10517D },
            { 0x6b, 0xFFFFFFFF },
            { 0x6a, 0xFFCCCCCC },
            { 0x60, 0xFF5E5E5E },
            { 0xd0, 0xFF9C5000 },
        };

        public static readonly Dictionary<TerrainType, uint> terrainColor = new Dictionary<TerrainType, uint>()
        {
            { TerrainType.None, 0xFF000000 },
            { TerrainType.Mountains, 0xFF5E5E5E },
            { TerrainType.ShorePlains, 0xFF0000FF },
            { TerrainType.HighForest, 0xFF969648 },
            { TerrainType.HighSand, 0xFFF7F7BA },
            { TerrainType.HighPlains, 0xFF4FCF00 },
            { TerrainType.LowPlains, 0xFF6FEF20 },
            { TerrainType.LowForest, 0xFFC3E612 },
            { TerrainType.LowSand, 0xFFB3D602 },
            { TerrainType.MidForest, 0xFF107D1D },
            { TerrainType.MidSand, 0xFF10517D },
            { TerrainType.MidPlains, 0xFFFFFFFF },
            { TerrainType.ShoreSand, 0xFFCCCCCC },
        };
    }
}
