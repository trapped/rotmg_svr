using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using wServer.realm;

namespace svrMonitor
{
    partial class Monitor : Form
    {
        private TableLayoutPanel tableLayoutPanel1;
        private ComboBox gameIds;
        private SplitContainer splitContainer1;
        private CheckBox checkBox1;
        private NumericUpDown numericUpDown1;


        MapArea paint;
        public Monitor()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            paint = new MapArea() { Dock = DockStyle.Fill };
            splitContainer1.Panel1.Controls.Add(paint);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Mon.mon.Opacity = 1;
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            Mon.mon.Opacity = .7;
            base.OnMouseEnter(e);
        }

        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gameIds = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(392, 273);
            this.splitContainer1.SplitterDistance = 225;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.gameIds, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDown1, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(392, 44);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // gameIds
            // 
            this.gameIds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gameIds.FormattingEnabled = true;
            this.gameIds.Location = new System.Drawing.Point(3, 3);
            this.gameIds.Name = "gameIds";
            this.gameIds.Size = new System.Drawing.Size(121, 19);
            this.gameIds.Sorted = true;
            this.gameIds.TabIndex = 0;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.Color.White;
            this.checkBox1.Location = new System.Drawing.Point(130, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(54, 15);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Paused";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(190, 3);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            555620615,
            287,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(55, 21);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // Monitor
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(392, 273);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Monitor";
            this.Opacity = 0.7D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Monitor";
            this.TopMost = true;
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        public class State
        {
            public List<Tuple<int, int>> Tiles = new List<Tuple<int, int>>();
            public List<Tuple<int, float, float>> Players = new List<Tuple<int, float, float>>();
            public List<Tuple<int, float, float>> Enemies = new List<Tuple<int, float, float>>();
            public List<Tuple<int, float, float>> StaticObjs = new List<Tuple<int, float, float>>();
            public List<Tuple<float, float>> Projectiles = new List<Tuple<float, float>>();
        }

        public Dictionary<long, State> states = new Dictionary<long, State>();

        public void Tick(RealmTime time)
        {
            this.BringToFront();
            try
            {
                int id = int.Parse((gameIds.SelectedItem ?? 0).ToString());
                foreach (var i in RealmManager.Worlds)
                {
                    if (!gameIds.Items.Contains(i.Value.Id.ToString()))
                        gameIds.Items.Add(i.Value.Id.ToString());
                    if (i.Value.Id == id)
                    {
                        if (paint.World != i.Value)
                        {
                            paint.Sca = 5;
                            paint.Offset = PointF.Empty;
                            states.Clear();
                        }
                        paint.World = i.Value;
                    }
                }
            }
            catch { gameIds.SelectedIndex = 0; }


            if (paint.World != null)
            {
                State s = new State();
                for (int y = 0; y < paint.World.Map.Height; y++)
                    for (int x = 0; x < paint.World.Map.Width; x++)
                        if (paint.World.Map[x,y].TileId != 0xff)
                            s.Tiles.Add(new Tuple<int, int>(x, y));

                foreach (var i in paint.World.Players)
                    s.Players.Add(new Tuple<int, float, float>(i.Value.Id, i.Value.X, i.Value.Y));
                foreach (var i in paint.World.Enemies)
                    s.Enemies.Add(new Tuple<int, float, float>(i.Value.Id, i.Value.X, i.Value.Y));
                foreach (var i in paint.World.StaticObjects)
                    s.StaticObjs.Add(new Tuple<int, float, float>(i.Value.Id, i.Value.X, i.Value.Y));
                foreach (var i in paint.World.Projectiles)
                    s.Projectiles.Add(new Tuple<float, float>(i.Value.X, i.Value.Y));
                states.Add(time.tickCount, s);

                if (!checkBox1.Checked)
                {
                    paint.State = s;
                    numericUpDown1.Value = time.tickCount;
                }
            }

            //if (time.tickCount % 20 == 0)
            //    paint.Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            State state;
            if (checkBox1.Checked && states.TryGetValue((long)numericUpDown1.Value, out state))
            {
                paint.State = state;
                paint.Refresh();
            }
        }
    }
}
