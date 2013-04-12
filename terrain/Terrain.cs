using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetTopologySuite.Triangulate;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System.Drawing;
using NetTopologySuite.GeometriesGraph;
using NetTopologySuite.Operation.Overlay;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Simplify;
using NetTopologySuite.Operation.Polygonize;
using NetTopologySuite.Operation.Union;

namespace terrain
{
    class Terrain
    {
        public const int Size = 2048;

        static void Show(IEnumerable<MapPolygon> polys, IEnumerable<MapNode> plot)
        {
            Bitmap map = new Bitmap(Size, Size);
            using (Graphics g = Graphics.FromImage(map))
            {
                foreach (var poly in polys)
                {
                    g.FillPolygon(new SolidBrush(Color.FromArgb(poly.DistanceToCoast == 0 ? 128 : (int)(poly.DistanceToCoast * 255), Color.Blue)),
                        poly.Nodes.Select(_ => new PointF((float)(_.X + 1) / 2 * Size, (float)(_.Y + 1) / 2 * Size)).ToArray());
                    for (int j = 0; j < poly.Nodes.Length; j++)
                    {
                        MapNode curr = poly.Nodes[j];
                        MapNode prev = j == 0 ? poly.Nodes[poly.Nodes.Length - 1] : poly.Nodes[j - 1];
                        g.DrawLine(Pens.White,
                            (float)(prev.X + 1) / 2 * Size, (float)(prev.Y + 1) / 2 * Size,
                            (float)(curr.X + 1) / 2 * Size, (float)(curr.Y + 1) / 2 * Size);
                    }
                }
                if (plot != null)
                    foreach (var i in plot)
                        g.FillRectangle(Brushes.Black, (float)(i.X + 1) / 2 * Size - 2, (float)(i.Y + 1) / 2 * Size - 2, 4, 4);
            }
            Test.Show(map);
        }

        static int MinDistToMapEdge(PlanarGraph graph, Node n, int limit)
        {
            if (n.Coordinate.X == 0 || n.Coordinate.X == Size ||
                n.Coordinate.Y == 0 || n.Coordinate.Y == Size)
                return 0;

            int ret = int.MaxValue;
            Stack<Tuple<int, Node>> stack = new Stack<Tuple<int, Node>>();
            HashSet<Node> visited = new HashSet<Node>();
            stack.Push(new Tuple<int, Node>(0, n));
            do
            {
                var state = stack.Pop();
                if (state.Item2.Coordinate.X == 0 || state.Item2.Coordinate.X == Size ||
                    state.Item2.Coordinate.Y == 0 || state.Item2.Coordinate.Y == Size)
                {
                    if (state.Item1 < ret)
                        ret = state.Item1;
                    if (ret == 0) return 0;

                    continue;
                }
                visited.Add(state.Item2);

                if (state.Item1 > limit) continue;
                foreach (var i in state.Item2.Edges)
                {
                    Node node = graph.Find(i.DirectedCoordinate);
                    if (!visited.Contains(node))
                        stack.Push(new Tuple<int, Node>(state.Item1 + 1, node));
                }

            } while (stack.Count > 0);
            return ret;
        }

        static Bitmap RenderColorBmp(TerrainTile[,] tiles)
        {
            int w = tiles.GetLength(0);
            int h = tiles.GetLength(1);
            Bitmap bmp = new Bitmap(w, h);
            BitmapBuffer buff = new BitmapBuffer(bmp);
            buff.Lock();
            for (int y = 0; y < w; y++)
                for (int x = 0; x < h; x++)
                    buff[x, y] = TileTypes.color[tiles[x, y].TileId];
            buff.Unlock();
            return bmp;
        }
        static Bitmap RenderTerrainBmp(TerrainTile[,] tiles)
        {
            int w = tiles.GetLength(0);
            int h = tiles.GetLength(1);
            Bitmap bmp = new Bitmap(w, h);
            BitmapBuffer buff = new BitmapBuffer(bmp);
            buff.Lock();
            for (int y = 0; y < w; y++)
                for (int x = 0; x < h; x++)
                {
                    buff[x, y] = TileTypes.terrainColor[tiles[x, y].Terrain];
                }
            buff.Unlock();
            return bmp;
        }
        static Bitmap RenderMoistBmp(TerrainTile[,] tiles)
        {
            int w = tiles.GetLength(0);
            int h = tiles.GetLength(1);
            Bitmap bmp = new Bitmap(w, h);
            BitmapBuffer buff = new BitmapBuffer(bmp);
            buff.Lock();
            for (int y = 0; y < w; y++)
                for (int x = 0; x < h; x++)
                {
                    uint color = 0x00ffffff;
                    color |= (uint)(tiles[x, y].Moisture * 255) << 24;
                    buff[x, y] = color;
                }
            buff.Unlock();
            return bmp;
        }
        static Bitmap RenderEvalBmp(TerrainTile[,] tiles)
        {
            int w = tiles.GetLength(0);
            int h = tiles.GetLength(1);
            Bitmap bmp = new Bitmap(w, h);
            BitmapBuffer buff = new BitmapBuffer(bmp);
            buff.Lock();
            for (int y = 0; y < w; y++)
                for (int x = 0; x < h; x++)
                {
                    uint color = 0x00ffffff;
                    color |= (uint)(tiles[x, y].Elevation * 255) << 24;
                    buff[x, y] = color;
                }
            buff.Unlock();
            return bmp;
        }
        static Bitmap RenderNoiseBmp(int w, int h)
        {
            Bitmap bmp = new Bitmap(w, h);
            BitmapBuffer buff = new BitmapBuffer(bmp);
            buff.Lock();
            var noise = new Noise(Environment.TickCount);
            for (int y = 0; y < w; y++)
                for (int x = 0; x < h; x++)
                {
                    uint color = 0x00ffffff;
                    color |= (uint)(noise.GetNoise(x / (double)w * 2, y / (double)h * 2, 0) * 255) << 24;
                    buff[x, y] = color;
                }
            buff.Unlock();
            return bmp;
        }

        public static void Generate()
        {
            //while (true)
            //    Test.Show(RenderNoiseBmp(500, 500));
            while (true)
            {
                int seed = Environment.TickCount;
                //seed = 15465515;
                seed = 21409625;
                Random rand = new Random(seed);
                PolygonMap map = new PolygonMap(rand.Next());
                map.Generate(Size * 6);

                var dat = CreateTerrain(rand.Next(), map);
                new Biome(rand.Next(), map).ComputeBiomes(dat);
                Test.Show(RenderColorBmp(dat));
                Test.Show(RenderTerrainBmp(dat));
                Test.Show(RenderMoistBmp(dat));
                Test.Show(RenderEvalBmp(dat));

                map = null;
                dat = null;
                GC.WaitForFullGCComplete(-1);
                GC.Collect();
            }
        }

        static TerrainTile[,] CreateTerrain(int seed, PolygonMap map)
        {
            Rasterizer<TerrainTile> rasterizer = new Rasterizer<TerrainTile>(Size, Size);
            //Set all to ocean
            rasterizer.Clear(new TerrainTile()
            {
                PolygonId = -1,
                Elevation = 0,
                Moisture = 1,
                TileId = TileTypes.DeepWater,
                TileObj = null
            });
            //Render lands poly
            foreach (var poly in map.Polygons.Where(_ => !_.IsWater))
            {
                uint color = 0x00ffffff;
                color |= (uint)(poly.DistanceToCoast * 255) << 24;
                rasterizer.FillPolygon(
                    poly.Nodes.SelectMany(_ =>
                    {
                        return new[]{ (_.X + 1) / 2 * Size,
                                      (_.Y + 1) / 2 * Size};
                    }).Concat(new[]{ (poly.Nodes[0].X + 1) / 2 * Size,
                                     (poly.Nodes[0].Y + 1) / 2 * Size}).ToArray(),
                    new TerrainTile()
                    {
                        PolygonId = poly.Id,
                        Elevation = (float)poly.DistanceToCoast,
                        Moisture = -1,
                        TileId = TileTypes.Grass,
                        TileObj = null
                    });
            }
            //Render roads
            MapFeatures fea = new MapFeatures(map, seed);
            var roads = fea.GenerateRoads();
            foreach (var i in roads)
            {
                rasterizer.DrawClosedCurve(i.SelectMany(_ => new[] { 
                    (_.X + 1) / 2 * Size, (_.Y + 1) / 2 * Size }).ToArray(),
                    1, new TerrainTile()
                    {
                        PolygonId = -1,
                        Elevation = -1,
                        Moisture = -1,
                        TileId = TileTypes.Road,
                        TileObj = null
                    }, 3);
            }
            //Render waters poly
            foreach (var poly in map.Polygons.Where(_ => _.IsWater))
            {
                var tile = new TerrainTile()
                {
                    PolygonId = poly.Id,
                    Elevation = (float)poly.DistanceToCoast,
                    TileObj = null
                };
                if (poly.IsOcean && !poly.IsCoast || poly.Neighbour.All(_ => _.IsWater))
                {
                    tile.TileId = TileTypes.DeepWater;
                    tile.Moisture = 0;
                }
                else
                {
                    tile.TileId = TileTypes.MovingWater;
                    tile.Moisture = 1;
                }
                rasterizer.FillPolygon(
                    poly.Nodes.SelectMany(_ =>
                    {
                        return new[]{ (_.X + 1) / 2 * Size,
                                      (_.Y + 1) / 2 * Size};
                    }).Concat(new[]{ (poly.Nodes[0].X + 1) / 2 * Size,
                                     (poly.Nodes[0].Y + 1) / 2 * Size}).ToArray(), tile);
            }
            //Render rivers
            var rivers = fea.GenerateRivers();
            Dictionary<Tuple<MapNode, MapNode>, int> edges = new Dictionary<Tuple<MapNode, MapNode>, int>();
            foreach (var i in rivers)
            {
                for (int j = 1; j < i.Length; j++)
                {
                    Tuple<MapNode, MapNode> edge = new Tuple<MapNode, MapNode>(i[j - 1], i[j]);
                    int count;
                    if (edges.TryGetValue(edge, out count))
                        count++;
                    else
                        count = 1;
                    edges[edge] = count;
                }
            }
            foreach (var i in edges)
            {
                i.Key.Item1.IsWater = true;
                i.Key.Item2.IsWater = true;
                rasterizer.DrawLineBresenham(
                    (i.Key.Item1.X + 1) / 2 * Size, (i.Key.Item1.Y + 1) / 2 * Size,
                    (i.Key.Item2.X + 1) / 2 * Size, (i.Key.Item2.Y + 1) / 2 * Size,
                    new TerrainTile()
                    {
                        PolygonId = -1,
                        Elevation = (float)(i.Key.Item1.DistanceToCoast + i.Key.Item2.DistanceToCoast) / 2,
                        Moisture = 1,
                        TileId = TileTypes.Water,
                        TileObj = null
                    }, 3 * Math.Min(2, i.Value));
            }

            return rasterizer.Buffer;
        }
    }
}