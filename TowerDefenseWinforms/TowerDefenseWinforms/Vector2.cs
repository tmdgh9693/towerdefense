using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseWinforms
{
    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y) { X = x; Y = y; }

        public float Length()
        {
            return (float)System.Math.Sqrt(X * X + Y * Y);
        }

        public Vector2 Normalized()
        {
            float len = Length();
            if (len <= 0.0001f) return new Vector2(0, 0);
            return new Vector2(X / len, Y / len);
        }

        public static Vector2 From(PointF a, PointF b)
        {
            return new Vector2(b.X - a.X, b.Y - a.Y);
        }

        public static Vector2 operator /(Vector2 v, float d)
        {
            return new Vector2(v.X / d, v.Y / d);
        }
    }
}
