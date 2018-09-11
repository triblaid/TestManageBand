using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TestTask.AlgorithmPoligon
{
    public class Polygon
    {
        private Point[] points; //вершины многоугольника
        private GraphicsPath path; //служит для отрисовки многоугольника
        private int minX, maxX, minY, maxY;

        public Polygon(int[] points)
        {
            if (points.Length < 6 || points.Length % 2 == 1)
                throw new Exception();

            minX = minY = int.MaxValue;
            maxX = maxY = int.MinValue;

            this.points = new Point[points.Length / 2];
            for (int i = 0; i < points.Length; i += 2)
            {
                minX = Math.Min(minX, points[i]);
                maxX = Math.Max(maxX, points[i]);
                minY = Math.Min(minY, points[i + 1]);
                maxY = Math.Max(maxY, points[i + 1]);

                this.points[i / 2] = new Point(points[i], points[i + 1]);
            }

            path = new GraphicsPath();
        }

        public Point[] getPoints()
        {
            return points;
        }

        public void fill(Graphics g, SolidBrush brush, float translateX, float translateY, float scale) //отрисовка многоугольника в точке (translateX, translateY) и масштабом scale
        {
            path.Reset();

            float lastX = (points[0].X - minX) * scale + translateX;
            float lastY = (points[0].Y - minY) * scale + translateY;

            for (int i = 1; i < points.Length; i++)
            {
                float x = (points[i].X - minX) * scale + translateX;
                float y = (points[i].Y - minY) * scale + translateY;

                path.AddLine(lastX, lastY, x, y);

                lastX = x;
                lastY = y;
            }

            g.FillPath(brush, path);
        }

        private PointOverEdge classify(Point p, Point v, Point w) //положение точки p относительно отрезка vw
        {
            //коэффициенты уравнения прямой
            int a = v.Y - w.Y;
            int b = w.X - v.X;
            int c = v.X * w.Y - w.X * v.Y;

            //подставим точку в уравнение прямой
            int f = a * p.X + b * p.Y + c;
            if (f > 0)
                return PointOverEdge.RIGHT; //точка лежит справа от отрезка
            if (f < 0)
                return PointOverEdge.LEFT; //слева от отрезка

            int minX = Math.Min(v.X, w.X);
            int maxX = Math.Max(v.X, w.X);
            int minY = Math.Min(v.Y, w.Y);
            int maxY = Math.Max(v.Y, w.Y);

            if (minX <= p.X && p.X <= maxX && minY <= p.Y && p.Y <= maxY)
                return PointOverEdge.BETWEEN; //точка лежит на отрезке
            return PointOverEdge.OUTSIDE; //точка лежит на прямой, но не на отрезке
        }

        private EdgeType edgeType(Point a, Point v, Point w) //тип ребра vw для точки a
        {
            switch (classify(a, v, w))
            {
                case PointOverEdge.LEFT:
                    return ((v.Y < a.Y) && (a.Y <= w.Y)) ? EdgeType.CROSSING : EdgeType.INESSENTIAL;
                case PointOverEdge.RIGHT:
                    return ((w.Y < a.Y) && (a.Y <= v.Y)) ? EdgeType.CROSSING : EdgeType.INESSENTIAL;
                case PointOverEdge.BETWEEN:
                    return EdgeType.TOUCHING;
                default:
                    return EdgeType.INESSENTIAL;
            }
        }

        public PointInPolygon pointInPolygon(Point a) //положение точки в многоугольнике
        {
            bool parity = true;
            for (int i = 0; i < points.Length; i++)
            {
                Point v = points[i];
                Point w = points[(i + 1) % points.Length];

                switch (edgeType(a, v, w))
                {
                    case EdgeType.TOUCHING:
                        return PointInPolygon.BOUNDARY;
                    case EdgeType.CROSSING:
                        parity = !parity;
                        break;
                }
            }

            return parity ? PointInPolygon.OUTSIDE : PointInPolygon.INSIDE;
        }

        public int MinX()
        {
            return minX;
        }

        public int MaxX()
        {
            return maxX;
        }

        public int MinY()
        {
            return minY;
        }

        public int MaxY()
        {
            return maxY;
        }

        public int width() //ширина
        {
            return maxX - minX;
        }

        public int height() //высота
        {
            return maxY - minY;
        }

        public enum PointInPolygon { INSIDE, OUTSIDE, BOUNDARY } //положение точки в многоугольнике

        private enum EdgeType { TOUCHING, CROSSING, INESSENTIAL } //положение ребра

        private enum PointOverEdge { LEFT, RIGHT, BETWEEN, OUTSIDE } //положение точки относительно отрезка

    }
}