using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using wServer.realm;

namespace svrMonitor
{
    class MapArea : Control
    {
        public World World { get; set; }
        public Monitor.State State { get; set; }
        public PointF Offset { get; set; }
        public int Sca { get; set; }

        Label foc = new Label() { AutoSize = false, Size = new Size(0, 0) };
        public MapArea()
        {
            Controls.Add(foc);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Selectable |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            Sca = 5;
        }

        protected override void OnClick(EventArgs e)
        {
            foc.Focus();
            base.OnClick(e);
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            PointF center = new PointF(Width / 2f, Height / 2f);
            center.X = center.X / Sca + Offset.X;
            center.Y = center.Y / Sca + Offset.Y;

            PointF mouse = new PointF(e.X, e.Y);
            mouse.X = mouse.X / Sca + Offset.X;
            mouse.Y = mouse.Y / Sca + Offset.Y;

            Offset = new PointF(Offset.X - (center.X - mouse.X), Offset.Y - (center.Y - mouse.Y));
            Invalidate();

            base.OnMouseEnter(e);
            base.OnMouseClick(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta < 0) Sca--;
            if (e.Delta > 0) Sca++;
            Invalidate();
            base.OnMouseWheel(e);
        }

        void Plot(Graphics g, float x, float y, Color color)
        {
            x = (x - Offset.X) * Sca;
            y = (y - Offset.Y) * Sca;
            if (x < 0) x = 0;
            else if (x > Width) x = Width;
            if (y < 0) y = 0;
            else if (y > Height) y = Height;

            g.FillRectangle(new SolidBrush(color), new RectangleF(x - 1, y - 1, 2, 2));
        }

        void Rect(Graphics g, float x, float y, float offset, Color color)
        {
            x = (x - offset - Offset.X) * Sca;
            y = (y - offset - Offset.Y) * Sca;
            if (x < 0) x = 0;
            else if (x > Width) x = Width;
            if (y < 0) y = 0;
            else if (y > Height) y = Height;

            g.FillRectangle(new SolidBrush(color), new RectangleF(x - 0.5f * Sca, y - 0.5f * Sca, Sca, Sca));
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            if (State == null) return;

            foreach (var i in State.Tiles)
                Rect(e.Graphics, i.Item1, i.Item2, -.5f, Color.Brown);

            foreach (var i in State.Players)
                Rect(e.Graphics, i.Item2, i.Item3, 0, Color.Yellow);
            foreach (var i in State.Enemies)
                Rect(e.Graphics, i.Item2, i.Item3, 0, Color.Red);
            foreach (var i in State.StaticObjs)
                Rect(e.Graphics, i.Item2, i.Item3, 0, Color.White);
            foreach (var i in State.Projectiles)
                Plot(e.Graphics, i.Item1, i.Item2, Color.Blue);
        }
    }
}
