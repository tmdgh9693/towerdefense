using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseWinforms
{
    public class MonsterInstance
    {
        public MonsterDefinition Def;
        public float MaxHP;
        public float HP;
        public PointF Pos;
        public int PathIndex;

        public MonsterInstance(MonsterDefinition def, float maxHp)
        {
            Def = def;
            MaxHP = maxHp;
            HP = maxHp;
            Pos = new PointF(0, 0);
            PathIndex = 0;
        }

        public void Draw(Graphics g, int tileSize)
        {
            float r = tileSize * 0.25f;
            RectangleF body = new RectangleF(Pos.X - r, Pos.Y - r, r * 2, r * 2);

            using (Brush b = new SolidBrush(Color.FromArgb(200, 220, 60, 60)))
                g.FillEllipse(b, body);

            float w = tileSize * 0.6f;
            float h = 4f;
            float x = Pos.X - w / 2f;
            float y = Pos.Y - r - 8f;

            float t = (MaxHP <= 0f) ? 0f : (HP / MaxHP);
            if (t < 0f) t = 0f;
            if (t > 1f) t = 1f;

            using (Brush bg = new SolidBrush(Color.FromArgb(140, 0, 0, 0)))
                g.FillRectangle(bg, x, y, w, h);

            using (Brush fg = new SolidBrush(Color.FromArgb(220, 0, 220, 0)))
                g.FillRectangle(fg, x, y, w * t, h);
        }
    }
}
