using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseWinforms
{
    public class TowerInstance
    {
        public Tower Tower;
        public Point Cell;
        public PointF Pos;
        public float Cooldown;

        public TowerInstance(Tower tower, Point cell, PointF pos)
        {
            Tower = tower;
            Cell = cell;
            Pos = pos;
            Cooldown = 0f;
        }

        public void Draw(Graphics g, int tileSize)
        {
            float r = tileSize * 0.28f;
            RectangleF body = new RectangleF(Pos.X - r, Pos.Y - r, r * 2, r * 2);

            using (Brush b = new SolidBrush(Color.FromArgb(200, 80, 140, 255)))
                g.FillEllipse(b, body);

            using (Pen p = new Pen(Color.FromArgb(180, 0, 0, 0)))
                g.DrawEllipse(p, body);
        }
    }
}
