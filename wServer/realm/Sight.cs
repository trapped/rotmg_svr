using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.realm
{
    static class Sight
    {
        static Dictionary<int, IntPoint[]> points = new Dictionary<int, IntPoint[]>();
        public static IntPoint[] GetSightCircle(int radius)
        {
            IntPoint[] ret;
            if (!points.TryGetValue(radius, out ret))
            {
                var pts = new List<IntPoint>();
                for (var y = -radius; y <= radius; y++)
                    for (var x = -radius; x <= radius; x++)
                    {
                        if (x * x + y * y <= radius * radius)
                            pts.Add(new IntPoint(x, y));
                    }
                ret = points[radius] = pts.ToArray();
            }
            return ret;
        }
    }
}
