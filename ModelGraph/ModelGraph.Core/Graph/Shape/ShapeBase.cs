using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ModelGraph.Core
{
    internal abstract partial class Shape
    {
        protected abstract ShapeType ShapeType { get; }
        internal Shape() { }

        #region Abstract/Virtual  =============================================
        internal abstract Shape Clone();
        internal abstract Shape Clone(Vector2 Center);
        internal virtual bool HitTest(Vector2 point)
        {
            var ds = Vector2.Abs(point - GetCenter());
            return (ds.X < Radius1 && ds.Y < Radius1);
        }
        internal abstract void AddDrawData(DrawData drawData, float size, float scale, Vector2 offset, Coloring coloring = Coloring.Normal);
        internal abstract void AddDrawData(DrawData drawData, float scale, Vector2 offset, FlipState flip);

        protected abstract (float x1, float y1, float x2, float y2, float cx, float cy) GetExtent();
        protected abstract void Scale(Vector2 scale);
        protected abstract ShapeProperty PropertyFlags { get; }
        protected abstract void CreatePoints();

        protected virtual (int min, int max) MinMaxDimension => (1, 100);


        protected virtual void DeltaSizeX(float delta) { }
        protected virtual void DeltaSizeY(float delta) { }
        protected virtual void DeltaCenterX(float delta) { }
        protected virtual void DeltaCenterY(float delta) { }
        #endregion

        #region Flip/Rotate  ==================================================
        static internal void RotateLeft(IEnumerable<Shape> shapes, bool useAlternate = false)
        {
            foreach (var shape in shapes) { shape.RotateLeft(useAlternate); }
        }
        static internal void RotateRight(IEnumerable<Shape> shapes, bool useAlternate = false)
        {
            foreach (var shape in shapes) { shape.RotateRight(useAlternate); }
        }
        static internal void VerticalFlip(IEnumerable<Shape> shapes)
        {
            foreach (var shape in shapes) { shape.VerticalFlip(); }
        }
        static internal void HorizontalFlip(IEnumerable<Shape> shapes)
        {
            foreach (var shape in shapes) { shape.HorizontalFlip(); }
        }
        #endregion

        #region GetCenter  ====================================================
        protected virtual Vector2 GetCenter() => Vector2.Zero;
        static private Vector2 GetCenter(IEnumerable<Shape> shapes)
        {
            var points = new List<Vector2>();
            foreach (var s in shapes)
            {
                points.Add(s.GetCenter());
            }
            if (points.Count == 0) return Vector2.Zero;
            if (points.Count == 1) return points[0];

            var x1 = 1f;
            var y1 = 1f;
            var x2 = -1f;
            var y2 = -1f;

            foreach (var p in points)
            {
                if (p.X < x1) x1 = p.X;
                if (p.Y < y1) y1 = p.Y;

                if (p.X > x2) x2 = p.X;
                if (p.Y > y2) y2 = p.Y;
            }
            return new Vector2((x1 + x2), (y1 + y2)) / 2;
        }
        #endregion

        #region SetCenter  ====================================================
        static internal void SetCenter(IEnumerable<Shape> shapes, Vector2 p)
        {
            foreach (var shape in shapes) { shape.MoveCenter(p - GetCenter(shapes)); }
        }
        protected void SetCenter(Vector2 p) => MoveCenter(p - GetCenter());
        #endregion

        #region MoveCenter  ===================================================
        static internal void MoveCenter(IEnumerable<Shape> shapes, Vector2 ds)
        {
            foreach (var shape in shapes) { shape.MoveCenter(ds); }
        }
        protected void MoveCenter(Vector2 ds)
        {
            for (int i = 0; i < DXY.Count; i++)
            {
                DXY[i] = Limit(ds + DXY[i]);
            }
        }
        #endregion

        #region SetProperty  ==================================================
        internal static void SetProperty(ShapeModel sm, ShapeProperty pf, IEnumerable<Shape> shapes)
        {
            foreach (var s in shapes) { SetProperty(sm, pf, s); }
        }
        private static void SetProperty(ShapeModel sm, ShapeProperty pf, Shape s)
        {
            switch (pf)
            {
                case ShapeProperty.EndCap:
                    s.SetEndCap(sm.EndCap);
                    break;

                case ShapeProperty.DashCap:
                    s.SetDashCap(sm.DashCap);
                    break;

                case ShapeProperty.StartCap:
                    s.SetStartCap(sm.StartCap);
                    break;

                case ShapeProperty.StrokeStyle:
                    s.SetStokeStyle(sm.StrokeStyle);
                    break;

                case ShapeProperty.StrokeWidth:
                    s.SetStrokeWidth(sm.StrokeWidth);
                    break;

                case ShapeProperty.SizeX:
                    s.DeltaCenterX(sm.DeltaSizeX);
                    break;
                case ShapeProperty.SizeY:
                    s.DeltaCenterY(sm.DeltaSizeY);
                    break;
                case ShapeProperty.Radius1:
                    break;
                case ShapeProperty.Radius2:
                    break;
                case ShapeProperty.Factor1:
                    break;
                case ShapeProperty.CenterX:
                    s.DeltaCenterX(sm.DeltaCenterX);
                    break;
                case ShapeProperty.CenterY:
                    s.DeltaCenterY(sm.DeltaCenterY);
                    break;
                case ShapeProperty.Rotation:
                    break;
                case ShapeProperty.Dimension:
                    break;
                case ShapeProperty.IsImpaired:
                    break;
                case ShapeProperty.ExtentEast:
                    break;
                case ShapeProperty.ExtentWest:
                    break;
                case ShapeProperty.ExtentNorth:
                    break;
                case ShapeProperty.ExtentSouth:
                    break;
            }
            s.CreatePoints();
        }
        #endregion

        #region GetProperty  ==================================================
        static internal ShapeProperty GetProperties(ShapeModel sm, IEnumerable<Shape> shapes)
        {
            var first_type = ShapeType.Unknown;
            foreach (var s in shapes)
            {
                var (sh, st, sw) = s.ShapeStrokeWidth();
                first_type = sh;

                var sc = st & StrokeType.SC_Triangle;
                var dc = st & StrokeType.DC_Triangle;
                var ec = st & StrokeType.EC_Triangle;
                var ss = st & StrokeType.Filled;

                sm.InitStrokeWidth(s.SW);
                sm.InitStrokeStyle(ss == StrokeType.Dotted ? StrokeStyle.Dotted : ss == StrokeType.Dashed ? StrokeStyle.Dashed : ss == StrokeType.Filled ? StrokeStyle.Filled : StrokeStyle.Solid);
                sm.InitEndCap(ec == StrokeType.EC_Round ? CapStyle.Round : ec == StrokeType.EC_Square ? CapStyle.Square : ec == StrokeType.EC_Triangle ? CapStyle.Triangle : CapStyle.Flat);
                sm.InitDashCap(dc == StrokeType.DC_Round ? CapStyle.Round : dc == StrokeType.DC_Square ? CapStyle.Square : dc == StrokeType.DC_Triangle ? CapStyle.Triangle : CapStyle.Flat);
                sm.InitStartCap(sc == StrokeType.SC_Round ? CapStyle.Round : sc == StrokeType.SC_Square ? CapStyle.Square : sc == StrokeType.SC_Triangle ? CapStyle.Triangle : CapStyle.Flat);
                sm.ColorARGB  = s.ShapeColor();

                break;
            }
            if (first_type == ShapeType.Unknown) return ShapeProperty.None;

            float x1 = 1f, y1 = 1f, x2 = -1f, y2 = -1f, cx = 0, cy = 0, N = 0;
            foreach (var s in shapes)
            {
                var (sx1, sy1, sx2, sy2, scx, scy) = s.GetExtent();
                s.CenterX = scx;
                s.CenterY = scy;
                if (sx1 < x1) x1 = sx1;
                if (sy1 < y1) y1 = sy1;
                if (sx2 > x2) x2 = sx2;
                if (sy2 > y2) y2 = sy2;
                cx += scx;
                cy += scy;
                N += 1;
            }
            sm.InitSizeX(ToUShort(x2 - x1));
            sm.InitSizeY(ToUShort(y2 - y1));
            sm.InitCenterX(ToShort(cx / N));
            sm.InitCenterY(ToShort(cy / N));
            sm.InitExtentEast(ToShort(x1));
            sm.InitExtentWest(ToShort(x2));
            sm.InitExtentNorth(ToShort(y1));
            sm.InitExtentSouth(ToShort(y2));

            var all_same_type = true;
            foreach (var s in shapes)
            {
                if (first_type == s.ShapeType) continue;
                all_same_type = false;
                break;
            }



            if (all_same_type)
            {

            }
            else
            {

            }

            var flags = ShapeProperty.SizeX | ShapeProperty.SizeY | ShapeProperty.CenterX | ShapeProperty.CenterY | ShapeProperty.ExtentEast | ShapeProperty.ExtentWest | ShapeProperty.ExtentNorth | ShapeProperty.ExtentSouth;
            foreach (var shape in shapes)
            {
                flags |= shape.PropertyFlags | shape.StrokePropertyFlags();
            }
            return flags;
        }
        #endregion

        #region AddDrawTargets  ===============================================
        static internal void AddDrawTargets(Shape s, List<Vector2> targets, DrawData drawData, float scale, Vector2 offset)
        {
            var points = s.GetDrawingPoints(FlipState.None, scale, offset);
            var points2 = new Vector2[points.Length * 2];
            var rd = new Vector2(7, 7);
            targets.Clear();
            var i = 0;
            foreach (var point in points)
            {
                targets.Add(point);
                points2[i++] = point;
                points2[i++] = rd;
            }
            drawData.AddParms((points2, (ShapeType.Circle, StrokeType.Simple, 3), (255, 255, 255, 255)));
        }
        static internal void MovePoint(Shape s, int index, Vector2 ds, float scale)
        {
            if (s is Polyline pl)
                pl.MovePoint(index, ds);
            else
                s.MoveCenter(ds);
        }
        #endregion
    }
}
