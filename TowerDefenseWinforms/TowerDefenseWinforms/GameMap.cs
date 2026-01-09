using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace TowerDefenseWinforms
{
    public enum MapPreset
    {
        PresetA,
        PresetB,
        PresetC,
        PresetD,
        RandomFromPresets
    }

    public class GameMap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TileSize { get; private set; }

        // 단일 경로
        private List<Point> path = new List<Point>();
        private HashSet<int> pathCells = new HashSet<int>();

        public int PathCount { get { return path.Count; } }

        public Point StartCell { get; private set; }
        public Point EndCell { get; private set; }

        public GameMap(int w, int h, int tileSize)
        {
            Width = w;
            Height = h;
            TileSize = tileSize;
        }

        // 외부에서 호출
        public void Build(MapPreset preset, int seed = 0)
        {
            if (preset == MapPreset.PresetA) BuildPresetA();
            else if (preset == MapPreset.PresetB) BuildPresetB();
            else if (preset == MapPreset.PresetC) BuildPresetC();
            else if (preset == MapPreset.PresetD) BuildPresetD();
            else BuildRandomFromPresets(seed);
        }

        // 프리셋
        private void BuildRandomFromPresets(int seed)
        {
            Random rng = new Random(seed);

            int pick = rng.Next(4); // 0~3
            if (pick == 0) BuildPresetA();
            else if (pick == 1) BuildPresetB();
            else if (pick == 2) BuildPresetC();
            else BuildPresetD();
        }

        private void BuildPresetA()
        {
            Clear();

            int yTop = 1;
            int yBot = Height - 2;

            StartCell = new Point(0, yBot);
            EndCell = new Point(Width - 1, yTop);

            AddPathLine(StartCell, new Point(1, yBot));
            AddPathLine(new Point(1, yBot), new Point(1, yTop));
            AddPathLine(new Point(1, yTop), new Point(Width / 2, yTop));
            AddPathLine(new Point(Width / 2, yTop), new Point(Width / 2, yBot));
            AddPathLine(new Point(Width / 2, yBot), new Point(Width - 2, yBot));
            AddPathLine(new Point(Width - 2, yBot), new Point(Width - 2, yTop));
            AddPathLine(new Point(Width - 2, yTop), EndCell);

            FinalizePath();
        }

        private void BuildPresetB()
        {
            Clear();

            int yTop = 1;
            int yBot = Height - 2;
            int xL = 2;
            int xR = Width - 3;

            StartCell = new Point(0, yTop);
            EndCell = new Point(Width - 1, yBot);

            Point a1 = new Point(xL, yTop);
            Point a2 = new Point(xL, yBot);
            Point a3 = new Point(Width / 2 - 2, yBot);
            Point a4 = new Point(Width / 2 - 2, yTop);

            AddPathLine(StartCell, a1);
            AddPathLine(a1, a2);
            AddPathLine(a2, a3);
            AddPathLine(a3, a4);

            Point mid = new Point(Width / 2 + 2, Height / 2);
            AddPathLine(a4, new Point(a4.X, mid.Y));
            AddPathLine(new Point(a4.X, mid.Y), mid);

            Point b1 = new Point(xR, mid.Y);
            Point b2 = new Point(xR, yTop);
            Point b3 = new Point(Width - 2, yTop);
            Point b4 = new Point(Width - 2, yBot);

            AddPathLine(mid, b1);
            AddPathLine(b1, b2);
            AddPathLine(b2, b3);
            AddPathLine(b3, b4);
            AddPathLine(b4, EndCell);

            FinalizePath();
        }

        private void BuildPresetC()
        {
            Clear();

            int yTop = 1;
            int yBot = Height - 2;

            StartCell = new Point(0, yTop);
            EndCell = new Point(Width - 1, yTop);

            int x1 = Width / 3;
            int x2 = (Width * 2) / 3;

            AddPathLine(StartCell, new Point(x1, yTop));
            AddPathLine(new Point(x1, yTop), new Point(x1, yBot));
            AddPathLine(new Point(x1, yBot), new Point(x2, yBot));
            AddPathLine(new Point(x2, yBot), new Point(x2, yTop));
            AddPathLine(new Point(x2, yTop), EndCell);

            FinalizePath();
        }

        private void BuildPresetD()
        {
            Clear();

            int yTop = 1;
            int yBot = Height - 2;

            StartCell = new Point(0, yBot);
            EndCell = new Point(Width - 1, Height / 2);

            int xMid = Width / 2;
            int xRingL = xMid + 1;
            int xRingR = Width - 3;
            int yRingT = 2;
            int yRingB = Height - 3;

            AddPathLine(StartCell, new Point(2, yBot));
            AddPathLine(new Point(2, yBot), new Point(2, yTop));
            AddPathLine(new Point(2, yTop), new Point(xMid, yTop));
            AddPathLine(new Point(xMid, yTop), new Point(xMid, Height / 2));

            Point enter = new Point(xRingL, Height / 2);
            AddPathLine(new Point(xMid, Height / 2), enter);

            AddPathLine(enter, new Point(xRingL, yRingT));
            AddPathLine(new Point(xRingL, yRingT), new Point(xRingR, yRingT));
            AddPathLine(new Point(xRingR, yRingT), new Point(xRingR, yRingB));
            AddPathLine(new Point(xRingR, yRingB), new Point(xRingL, yRingB));
            AddPathLine(new Point(xRingL, yRingB), new Point(xRingL, Height / 2));

            AddPathLine(new Point(xRingL, Height / 2), EndCell);

            FinalizePath();
        }

        private void BuildRandomSinglePath(int seed)
        {
            Clear();
            Random rng = new Random(seed);

            int minY = 1;
            int maxY = Height - 2;

            // 시작/끝을 범위 내 랜덤
            StartCell = new Point(0, rng.Next(minY, maxY + 1));
            EndCell = new Point(Width - 1, rng.Next(minY, maxY + 1));

            int bends = rng.Next(6, 10);
            int x = 0;

            Point cur = StartCell;

            for (int i = 0; i < bends; i++)
            {
                // x는 항상 증가(역주행/루프 방지)
                int remain = (Width - 1) - x;
                if (remain <= 2) break;

                int stepX = rng.Next(1, Math.Min(4, remain)); // 1~3칸 정도씩 전진
                x += stepX;

                int y = rng.Next(minY, maxY + 1);

                // 너무 같은 줄로만 가는 거 방지
                y = cur.Y + (rng.Next(2) == 0 ? -1 : 1);
                if (y < minY) y = minY;
                if (y > maxY) y = maxY;

                Point bend = new Point(x, y);
                AddPathLine(cur, bend);
                cur = bend;
            }

            // 마지막은 EndCell로 연결
            AddPathLine(cur, new Point(EndCell.X, cur.Y));
            AddPathLine(new Point(EndCell.X, cur.Y), EndCell);

            FinalizePath();
        }

        // Path 유틸
        private void Clear()
        {
            path.Clear();
            pathCells.Clear();
            StartCell = new Point(0, 1);
            EndCell = new Point(Width - 1, Height - 2);
        }

        private void AddPathPoint(Point p)
        {
            if (!InBounds(p)) return;
            path.Add(p);
            pathCells.Add(Key(p));
        }

        private void AddPathLine(Point from, Point to)
        {
            Point cur = from;
            AddPathPoint(cur);

            while (cur.X != to.X)
            {
                int dx = Math.Sign(to.X - cur.X);
                cur = new Point(cur.X + dx, cur.Y);
                AddPathPoint(cur);
            }

            while (cur.Y != to.Y)
            {
                int dy = Math.Sign(to.Y - cur.Y);
                cur = new Point(cur.X, cur.Y + dy);
                AddPathPoint(cur);
            }
        }

        private void FinalizePath()
        {
            // 중복 제거
            List<Point> newPath = new List<Point>();
            HashSet<int> seen = new HashSet<int>();

            for (int i = 0; i < path.Count; i++)
            {
                int k = Key(path[i]);
                if (seen.Contains(k)) continue;
                seen.Add(k);
                newPath.Add(path[i]);
            }

            path = newPath;
            pathCells = seen;

            // Start/End 보정
            if (path.Count >= 1) StartCell = path[0];
            if (path.Count >= 1) EndCell = path[path.Count - 1];
        }

        private int Key(Point p) { return p.Y * 1000 + p.X; }

        private static int Clamp(int v, int lo, int hi)
        {
            if (v < lo) return lo;
            if (v > hi) return hi;
            return v;
        }

        // 좌표/체크
        public bool InBounds(Point c)
        {
            return c.X >= 0 && c.Y >= 0 && c.X < Width && c.Y < Height;
        }

        public bool IsPathCell(Point c)
        {
            return pathCells.Contains(Key(c));
        }

        public Point WorldToCell(Point world)
        {
            return new Point(world.X / TileSize, world.Y / TileSize);
        }

        public PointF CellToWorldCenter(Point c)
        {
            return new PointF((c.X + 0.5f) * TileSize, (c.Y + 0.5f) * TileSize);
        }

        public PointF GetPathWorld(int index)
        {
            if (path.Count == 0) return new PointF(0, 0);
            if (index < 0) index = 0;
            if (index >= path.Count) index = path.Count - 1;
            return CellToWorldCenter(path[index]);
        }

        public PointF GetStartWorld() { return CellToWorldCenter(StartCell); }
        public PointF GetEndWorld() { return CellToWorldCenter(EndCell); }

        // Draw: Grid + Path + Start/End + 화살표
        public void Draw(Graphics g)
        {
            DrawGrid(g);
            DrawPath(g);
            DrawStartEnd(g);
            DrawStartArrow(g);
        }

        public void DrawGrid(Graphics g)
        {
            using (Pen pen = new Pen(Color.FromArgb(40, 255, 255, 255)))
            {
                for (int x = 0; x <= Width; x++)
                    g.DrawLine(pen, x * TileSize, 0, x * TileSize, Height * TileSize);

                for (int y = 0; y <= Height; y++)
                    g.DrawLine(pen, 0, y * TileSize, Width * TileSize, y * TileSize);
            }
        }

        public void DrawPath(Graphics g)
        {
            using (Brush b = new SolidBrush(Color.FromArgb(60, 255, 215, 0)))
            {
                for (int i = 0; i < path.Count; i++)
                {
                    Point c = path[i];
                    Rectangle r = new Rectangle(c.X * TileSize, c.Y * TileSize, TileSize, TileSize);
                    g.FillRectangle(b, r);
                }
            }
        }

        private void DrawStartEnd(Graphics g)
        {
            float r = TileSize * 0.25f;
            PointF s = GetStartWorld();
            PointF e = GetEndWorld();

            using (Brush bs = new SolidBrush(Color.Lime))
            using (Brush be = new SolidBrush(Color.Red))
            {
                g.FillEllipse(bs, s.X - r, s.Y - r, r * 2, r * 2);
                g.FillEllipse(be, e.X - r, e.Y - r, r * 2, r * 2);
            }

            using (Font f = new Font("Arial", 10, FontStyle.Bold))
            {
                g.DrawString("START", f, Brushes.Lime, s.X + r, s.Y - r);
                g.DrawString("END", f, Brushes.Red, e.X + r, e.Y - r);
            }
        }

        private void DrawStartArrow(Graphics g)
        {
            if (path.Count < 2) return;

            PointF p0 = GetPathWorld(0);
            PointF p1 = GetPathWorld(1);

            using (Pen p = new Pen(Color.Lime, 2))
            {
                p.CustomEndCap = new AdjustableArrowCap(4, 6);
                g.DrawLine(p, p0, p1);
            }
        }
    }
}
