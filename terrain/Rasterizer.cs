using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using GeoAPI.Geometries;

namespace terrain
{
    unsafe class Rasterizer<T>
    {
        int w;
        int h;

        public T[,] Buffer { get; private set; }
        public Rasterizer(int w, int h)
        {
            this.w = w;
            this.h = h;
            Buffer = new T[w, h];
        }

        public T this[int x, int y]
        {
            get { return Buffer[x, y]; }
            set { Buffer[x, y] = value; }
        }
        public int Height { get { return this.h; } }
        public int Width { get { return this.w; } }

        public void Plot(double x, double y, T val)
        {
            Buffer[(int)x, (int)y] = val;
        }
        public void PlotSqr(double x, double y, T val, int w)
        {
            switch (w)
            {
                case 0: return;
                case 1:
                    Buffer[(int)x, (int)y] = val; break;
                case 2:
                    Buffer[(int)x, (int)y] = val;
                    Buffer[(int)x + 1, (int)y] = val;
                    Buffer[(int)x, (int)y + 1] = val;
                    Buffer[(int)x + 1, (int)y + 1] = val; break;
                default:
                    for (int _x = 0; _x < w; _x++)
                        for (int _y = 0; _y < w; _y++)
                        {
                            Buffer[(int)x + _x, (int)y + _y] = val;
                        }
                    break;
            }
        }

        public void Clear(T val)
        {
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    Buffer[x, y] = val;
        }

        public void FillPolygon(double[] points, T val)
        {
            // Use refs for faster access (really important!) speeds up a lot!
            int w = Width;
            int h = Height;
            var pixels = Buffer;
            int pn = points.Length;
            int pnh = points.Length >> 1;
            double[] intersectionsX = new double[pnh];

            // Find y min and max (slightly faster than scanning from 0 to height)
            int yMin = h;
            int yMax = 0;
            for (int i = 1; i < pn; i += 2)
            {
                double py = points[i];
                if (py < yMin) yMin = (int)Math.Floor(py);
                if (py > yMax) yMax = (int)Math.Ceiling(py);
            }
            if (yMin < 0) yMin = 0;
            if (yMax >= h) yMax = h - 1;


            // Scan line from min to max
            for (int y = yMin; y <= yMax; y++)
            {
                // Initial point x, y
                double vxi = points[0];
                double vyi = points[1];

                // Find all intersections
                // Based on http://alienryderflex.com/polygon_fill/
                int intersectionCount = 0;
                for (int i = 2; i < pn; i += 2)
                {
                    // Next point x, y
                    double vxj = points[i];
                    double vyj = points[i + 1];

                    // Is the scanline between the two points
                    if (vyi < y && vyj >= y
                     || vyj < y && vyi >= y)
                    {
                        // Compute the intersection of the scanline with the edge (line between two points)
                        intersectionsX[intersectionCount++] = vxi + (y - vyi) / (vyj - vyi) * (vxj - vxi);
                    }
                    vxi = vxj;
                    vyi = vyj;
                }

                // Sort the intersections from left to right using Insertion sort 
                // It's faster than Array.Sort for this small data set
                double t;
                int j;
                for (int i = 1; i < intersectionCount; i++)
                {
                    t = intersectionsX[i];
                    j = i;
                    while (j > 0 && intersectionsX[j - 1] > t)
                    {
                        intersectionsX[j] = intersectionsX[j - 1];
                        j = j - 1;
                    }
                    intersectionsX[j] = t;
                }

                // Fill the pixels between the intersections
                for (int i = 0; i < intersectionCount - 1; i += 2)
                {
                    int x0 = (int)Math.Floor(intersectionsX[i]);
                    int x1 = (int)Math.Ceiling(intersectionsX[i + 1]);

                    // Check boundary
                    if (x1 < 0) x1 = 0;
                    if (x1 >= w) x1 = w - 1;
                    if (x0 < 0) x0 = 0;
                    if (x0 >= w) x0 = w - 1;
                    // Fill the pixels
                    for (int x = x0; x <= x1; x++)
                    {
                        Plot(x, y, val);
                    }
                }
            }
        }

        public void DrawLineBresenham(double x1, double y1, double x2, double y2, T val, int width)
        {
            // Use refs for faster access (really important!) speeds up a lot!
            int w = Width;
            int h = Height;
            var pixels = Buffer;

            // Distance start and end point
            double dx = x2 - x1;
            double dy = y2 - y1;

            // Determine sign for direction x
            int incx = 0;
            if (dx < 0)
            {
                dx = -dx;
                incx = -1;
            }
            else if (dx > 0)
            {
                incx = 1;
            }

            // Determine sign for direction y
            int incy = 0;
            if (dy < 0)
            {
                dy = -dy;
                incy = -1;
            }
            else if (dy > 0)
            {
                incy = 1;
            }

            // Which gradient is larger
            double pdx, pdy, odx, ody, es, el;
            if (dx > dy)
            {
                pdx = incx;
                pdy = 0;
                odx = incx;
                ody = incy;
                es = dy;
                el = dx;
            }
            else
            {
                pdx = 0;
                pdy = incy;
                odx = incx;
                ody = incy;
                es = dx;
                el = dy;
            }

            // Init start
            double x = x1;
            double y = y1;
            double error = el / 2;
            if (y < h && y >= 0 && x < w && x >= 0)
            {
                Plot(x, y, val);
            }

            // Set limit
            double lx, hx, ly, hy;
            if (x1 < x2)
            {
                lx = x1;
                hx = x2;
            }
            else
            {
                lx = x2;
                hx = x1;
            }
            if (y1 < y2)
            {
                ly = y1;
                hy = y2;
            }
            else
            {
                ly = y2;
                hy = y1;
            }

            // Walk the line!
            for (int i = 0; i < el; i++)
            {
                // Update error term
                error -= es;

                // Decide which coord to use
                if (error < 0)
                {
                    error += el;
                    x += odx;
                    y += ody;
                }
                else
                {
                    x += pdx;
                    y += pdy;
                }
                if (x > hx) x = hx;
                else if (x < lx) x = lx;
                if (y > hy) y = hy;
                else if (y < ly) y = ly;

                // Set pixel
                if (y < h && y >= 0 && x < w && x >= 0)
                {
                    PlotSqr(x, y, val, width);
                }
            }
        }

        private const float StepFactor = 2f;

        void DrawCurveSegment(
            double x1, double y1, double x2, double y2,
            double x3, double y3, double x4, double y4,
            double tension, T val, int width)
        {
            // Determine distances between controls points (bounding rect) to find the optimal stepsize
            var minX = Math.Min(x1, Math.Min(x2, Math.Min(x3, x4)));
            var minY = Math.Min(y1, Math.Min(y2, Math.Min(y3, y4)));
            var maxX = Math.Max(x1, Math.Max(x2, Math.Max(x3, x4)));
            var maxY = Math.Max(y1, Math.Max(y2, Math.Max(y3, y4)));

            // Get slope
            var lenx = maxX - minX;
            var len = maxY - minY;
            if (lenx > len)
            {
                len = lenx;
            }

            // Prevent divison by zero
            if (len != 0)
            {
                // Init vars
                var step = StepFactor / len;
                double tx1 = x2;
                double ty1 = y2;
                double tx2, ty2;

                // Calculate factors
                var sx1 = tension * (x3 - x1);
                var sy1 = tension * (y3 - y1);
                var sx2 = tension * (x4 - x2);
                var sy2 = tension * (y4 - y2);
                var ax = sx1 + sx2 + 2 * x2 - 2 * x3;
                var ay = sy1 + sy2 + 2 * y2 - 2 * y3;
                var bx = -2 * sx1 - sx2 - 3 * x2 + 3 * x3;
                var by = -2 * sy1 - sy2 - 3 * y2 + 3 * y3;

                // Interpolate
                for (var t = step; t <= 1; t += step)
                {
                    var tSq = t * t;

                    tx2 = ax * tSq * t + bx * tSq + sx1 * t + x2;
                    ty2 = ay * tSq * t + by * tSq + sy1 * t + y2;

                    // Draw line
                    DrawLineBresenham(tx1, ty1, tx2, ty2, val, width);
                    tx1 = tx2;
                    ty1 = ty2;
                }

                // Prevent rounding gap
                DrawLineBresenham(tx1, ty1, x3, y3, val, width);
            }
        }


        public void DrawCurve(double[] points, double tension, T val, int width)
        {
            int pn = points.Length;

            // First segment
            DrawCurveSegment(points[0], points[1], points[0], points[1], points[2], points[3], points[4], points[5], tension, val, width);

            // Middle segments
            int i;
            for (i = 2; i < pn - 4; i += 2)
            {
                DrawCurveSegment(points[i - 2], points[i - 1], points[i], points[i + 1], points[i + 2], points[i + 3], points[i + 4], points[i + 5], tension, val, width);
            }

            // Last segment
            DrawCurveSegment(points[i - 2], points[i - 1], points[i], points[i + 1], points[i + 2], points[i + 3], points[i + 2], points[i + 3], tension, val, width);
        }
        public void DrawClosedCurve(double[] points, double tension, T val, int width)
        {
            int pn = points.Length;

            // First segment
            DrawCurveSegment(points[pn - 2], points[pn - 1], points[0], points[1], points[2], points[3], points[4], points[5], tension, val, width);

            // Middle segments
            int i;
            for (i = 2; i < pn - 4; i += 2)
            {
                DrawCurveSegment(points[i - 2], points[i - 1], points[i], points[i + 1], points[i + 2], points[i + 3], points[i + 4], points[i + 5], tension, val, width);
            }

            // Last segment
            DrawCurveSegment(points[i - 2], points[i - 1], points[i], points[i + 1], points[i + 2], points[i + 3], points[0], points[1], tension, val, width);

            // Last-to-First segment
            DrawCurveSegment(points[i], points[i + 1], points[i + 2], points[i + 3], points[0], points[1], points[2], points[3], tension, val, width);
        }
    }
}
