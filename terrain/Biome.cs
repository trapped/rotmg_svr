using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace terrain
{
    class Biome
    {
        Random rand;
        PolygonMap map;
        public Biome(int seed, PolygonMap map)
        {
            rand = new Random(seed);
            this.map = map;
        }


        double[] elevationThreshold;
        double[] moistureThreshold;
        HashSet<MapPolygon> beaches;
        public void ComputeBiomes(TerrainTile[,] buff)
        {
            var nodeMoist = ComputeMoisture();
            var polyMoist = RedistributeMoisture(nodeMoist);

            var elevs = map.Polygons
                .SelectMany(_ => _.Nodes)
                .Select(_ => _.DistanceToCoast.Value)
                .OrderBy(_ => _)
                .Distinct().ToArray();
            elevationThreshold = new double[]
            {
                0 / 20.0,
                3 / 20.0,
                7 / 20.0,
                10 / 20.0,
            };
            beaches = new HashSet<MapPolygon>(map.Polygons.Where(_ =>
                !_.IsWater && _.Neighbour.Any(__ => __.IsCoast)));

            var moists = nodeMoist
                .Select(_ => _.Value)
                .OrderBy(_ => _)
                .Distinct().ToArray();
            moistureThreshold = new double[]
            {
                1 / 7.0,
                2 / 7.0,
                3 / 7.0,
                4 / 7.0,
                5 / 7.0,
                6 / 7.0,
            };

            AddNoiseAndBiome(buff, polyMoist);
            Randomize(buff);
            ComputeSpawnTerrains(buff);
        }

        Dictionary<MapNode, double> ComputeMoisture()
        {
            Dictionary<MapNode, double> moisture = new Dictionary<MapNode, double>();
            int totalCount = map.Polygons
                .SelectMany(_ => _.Nodes)
                .Distinct().Count();
            var waterNodes = map.Polygons
                .Where(_ => !_.IsOcean)   //no ocean
                .Where(_ => !_.Neighbour.Any(__ => __.IsOcean))    //no beaches
                .SelectMany(_ => _.Nodes)
                .Where(_ => _.IsWater && !_.IsOcean)
                .Distinct().ToArray();

            Queue<MapNode> q = new Queue<MapNode>();
            foreach (var i in waterNodes)
            {
                q.Enqueue(i);
                moisture[i] = rand.Next(0, 6);
            }

            do
            {
                var node = q.Dequeue();
                double dist = moisture[node] + 1;
                foreach (var i in node.Edges)
                {
                    var target = i.To;
                    double targetDist;
                    if (!moisture.TryGetValue(target, out targetDist))
                        targetDist = int.MaxValue;
                    if (targetDist > dist)
                    {
                        moisture[target] = dist;
                        q.Enqueue(target);
                    }
                }
            } while (q.Count > 0);
            return moisture;
        }
        Dictionary<MapPolygon, double> RedistributeMoisture(Dictionary<MapNode, double> nodes)
        {
            List<double> sorted = new List<double>(nodes.Values.Distinct());
            sorted.Sort();
            Dictionary<double, double> dict = new Dictionary<double, double>();
            for (int i = 0; i < sorted.Count; i++)
            {
                double y = (double)(sorted.Count - i) / sorted.Count;
                //double x = (Math.Sqrt(1.0) - Math.Sqrt(1.0 * (1 - y)));
                double x = y * 0.8;
                dict[sorted[i]] = (x > 1 ? 1 : x);
            }
            foreach (var i in nodes.Keys.ToArray())
            {
                nodes[i] = dict[nodes[i]] * (1 - i.DistanceToCoast.Value * 0);
            }

            Dictionary<MapPolygon, double> ret = new Dictionary<MapPolygon, double>();
            foreach (var i in map.Polygons)
            {
                ret[i] = i.Nodes.Average(_ => nodes[_]);
            }
            return ret;
        }

        short GetBiomeGround(string biome)
        {
            switch (biome)
            {
                case "beach":
                    return TileTypes.Beach;

                case "snowy":
                    return TileTypes.SnowRock;
                case "mountain":
                    return TileTypes.Rock;

                case "taiga":
                    return TileTypes.BrightGrass;
                case "shrub":
                    return TileTypes.LightGrass;

                case "rainforest":
                    return TileTypes.BlueGrass;
                case "forest":
                    return TileTypes.DarkGrass;
                case "grassland":
                    return TileTypes.Grass;

                case "dryland":
                    return TileTypes.YellowGrass;
                case "desert":
                    return TileTypes.Sand;
            }
            return 0;
        }
        TerrainType GetBiomeTerrain(TerrainTile tile)
        {
            if (tile.PolygonId == -1) return TerrainType.None;
            MapPolygon poly = map.Polygons[tile.PolygonId];

            if (!poly.IsWater && beaches.Contains(poly))
                return TerrainType.ShoreSand;
            else if (poly.IsWater)
                return TerrainType.None;
            else
            {
                if (tile.Elevation >= elevationThreshold[3])
                    return TerrainType.Mountains;
                else if (tile.Elevation > elevationThreshold[2])
                {
                    if (tile.Moisture > moistureThreshold[4])
                        return TerrainType.HighPlains;
                    else if (tile.Moisture > moistureThreshold[2])
                        return TerrainType.HighForest;
                    else
                        return TerrainType.HighSand;
                }
                else if (tile.Elevation > elevationThreshold[1])
                {
                    if (tile.Moisture > moistureThreshold[4])
                        return TerrainType.MidForest;
                    else if (tile.Moisture > moistureThreshold[2])
                        return TerrainType.MidPlains;
                    else
                        return TerrainType.MidSand;
                }
                else
                {
                    if (poly.Neighbour.Any(_ => beaches.Contains(_)))
                    {
                        if (tile.Moisture > moistureThreshold[2])
                            return TerrainType.ShorePlains;
                    }

                    if (tile.Moisture > moistureThreshold[3])
                        return TerrainType.LowForest;
                    else if (tile.Moisture > moistureThreshold[2])
                        return TerrainType.LowPlains;
                    else
                        return TerrainType.LowSand;
                }
            }
            return TerrainType.None;
        }

        string GetBiome(TerrainTile tile)
        {
            if (tile.PolygonId == -1) return "unknown";
            MapPolygon poly = map.Polygons[tile.PolygonId];

            if (beaches.Contains(poly))
                return "beach";
            else if (poly.IsWater)
            {
                if (poly.IsCoast) return "coast";
                else if (poly.IsOcean) return "ocean";
                else return "water";
            }
            else
            {
                if (tile.Elevation >= elevationThreshold[3])
                {
                    if (tile.Moisture > moistureThreshold[4])
                        return "snowy";
                    else
                        return "mountain";
                }
                else if (tile.Elevation > elevationThreshold[2])
                {
                    if (tile.Moisture > moistureThreshold[4])
                        return "dryland";
                    else if (tile.Moisture > moistureThreshold[2])
                        return "taiga";
                    else
                        return "desert";
                }
                else if (tile.Elevation > elevationThreshold[1])
                {
                    if (tile.Moisture > moistureThreshold[4])
                        return "forest";
                    else if (tile.Moisture > moistureThreshold[2])
                        return "shrub";
                    else
                        return "desert";
                }
                else
                {
                    if (tile.Moisture > moistureThreshold[4])
                        return "rainforest";
                    else if (tile.Moisture > moistureThreshold[3])
                        return "forest";
                    else if (tile.Moisture > moistureThreshold[2])
                        return "grassland";
                    else
                        return "desert";
                }
            }
            return "unknown";
        }
        void AddNoiseAndBiome(TerrainTile[,] buff, Dictionary<MapPolygon, double> moist)
        {
            int w = buff.GetLength(0);
            int h = buff.GetLength(1);
            var elevationNoise = new Noise(rand.Next());
            var moistureNoise = new Noise(rand.Next());
            //var elevationNoise = PerlinNoise.GetPerlinNoise(rand.Next(), 256, 256, 2);
            //var moistureNoise = PerlinNoise.GetPerlinNoise(rand.Next(), 256, 256, 2);
            for (int y = 0; y < w; y++)
                for (int x = 0; x < h; x++)
                {
                    var tile = buff[x, y];
                    if (tile.PolygonId != -1)
                    {
                        var poly = map.Polygons[tile.PolygonId];

                        tile.Elevation = (float)(poly.DistanceToCoast + poly.DistanceToCoast *
                            elevationNoise.GetNoise(x * 128.0 / w, y * 128.0 / h, 0.3) * 0.01f);
                        if (tile.Elevation > 1) tile.Elevation = 1;
                        else if (tile.Elevation < 0) tile.Elevation = 0;

                        tile.Moisture = (float)(moist[poly] + moist[poly] *
                            moistureNoise.GetNoise(x * 128.0 / w, y * 128.0 / h, 0.3) * 0.01f);
                        if (tile.Moisture > 1) tile.Moisture = 1;
                        else if (tile.Moisture < 0) tile.Moisture = 0;
                    }

                    tile.Biome = GetBiome(tile);
                    var biomeGround = GetBiomeGround(tile.Biome);
                    if (biomeGround != 0)
                        tile.TileId = biomeGround;

                    buff[x, y] = tile;
                }
        }
        void Randomize(TerrainTile[,] buff)
        {
            int w = buff.GetLength(0);
            int h = buff.GetLength(1);
            TerrainTile[,] tmp = (TerrainTile[,])buff.Clone();
            for (int y = 10; y < h - 10; y++)
                for (int x = 10; x < w - 10; x++)
                {
                    var tile = buff[x, y];

                    if (tile.TileId == TileTypes.Water && tile.Elevation >= elevationThreshold[3])
                        tile.TileId = TileTypes.SnowRock;
                    else if (tile.TileId != TileTypes.Water && tile.TileId != TileTypes.Road)
                    {
                        var id = buff[x + rand.Next(-2, 3), y + rand.Next(-2, 3)].TileId;
                        while (id == TileTypes.Water || id == TileTypes.Road)
                            id = buff[x + rand.Next(-2, 3), y + rand.Next(-2, 3)].TileId;
                        tile.TileId = id;
                    }

                    if (tile.TileId == TileTypes.Beach)
                        tile.Region = TileRegion.Spawn;

                    string biome = tile.Biome;
                    if (tile.TileId == TileTypes.Beach) biome = "beach";
                    else if (tile.TileId == TileTypes.MovingWater) biome = "coast";

                    var biomeObj = Decoration.GetDecor(biome, rand);
                    if (biomeObj != null)
                    {
                        tile.TileObj = biomeObj;
                        var size = Decoration.GetSize(biomeObj, rand);
                        if (size != null)
                            tile.Name = "size:" + size;
                    }

                    buff[x, y] = tile;
                }
        }
        void ComputeSpawnTerrains(TerrainTile[,] buff)
        {
            int w = buff.GetLength(0);
            int h = buff.GetLength(1);
            for (int y = 0; y < w; y++)
                for (int x = 0; x < h; x++)
                {
                    var tile = buff[x, y];
                    tile.Terrain = GetBiomeTerrain(tile);
                    buff[x, y] = tile;
                }
        }
    }
}
