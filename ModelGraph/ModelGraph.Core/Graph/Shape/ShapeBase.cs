using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Devices.Bluetooth;
using Windows.System.Power.Diagnostics;

namespace ModelGraph.Core
{
    internal abstract partial class Shape
    {
        protected abstract ShapeType ShapeType { get; }
        internal Shape() { }

        #region Abstract/Virtual  =============================================
        internal abstract Shape Clone();
        internal abstract Shape Clone(Vector2 Center);

        internal abstract void AddDrawData(DrawData drawData, float size, float scale, Vector2 center, Coloring coloring = Coloring.Normal);
        internal abstract void AddDrawData(DrawData drawData, float scale, Vector2 center, FlipState flip);

        protected abstract (float dx1, float dy1, float dx2, float dy2) GetExtent();
        protected abstract void Scale(Vector2 scale);

        protected virtual void CreatePoints() { }
        protected virtual (int min, int max) MinMaxDimension => (1, 100);
        protected abstract ShapeProperty PropertyFlags { get; }

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
        protected virtual (float, float) GetCenter() => (0, 0);
        static private (float cdx, float cdy) GetCenter(IEnumerable<Shape> shapes)
        {
            var points = new List<(float, float)>();
            foreach (var s in shapes)
            {
                points.Add(s.GetCenter());
            }
            if (points.Count == 0) return (0, 0);
            if (points.Count == 1) return points[0];

            var x1 = 1f;
            var y1 = 1f;
            var x2 = -1f;
            var y2 = -1f;

            foreach (var (px, py) in points)
            {
                if (px < x1) x1 = px;
                if (py < y1) y1 = py;

                if (px > x2) x2 = px;
                if (py > y2) y2 = py;
            }
            return (((x1 + x2) / 2, (y1 + y2) / 2));
        }
        #endregion

        #region SetCenter  ====================================================
        static internal void SetCenter(IEnumerable<Shape> shapes, Vector2 p)
        {
            var (cx, cy) = (p.X, p.Y);
            var (vx, vy) = GetCenter(shapes);

            var (dx, dy) = (cx - vx, cy - vy);

            foreach (var shape in shapes) { shape.MoveCenter(dx, dy); }
        }
        protected void SetCenter(float cx, float cy)
        {
            var (vx, vy) = GetCenter();
            var (dx, dy) = (cx - vx, cy - vy);
            MoveCenter(dx, dy);
        }
        #endregion

        #region MoveCenter  ===================================================
        static internal void MoveCenter(IEnumerable<Shape> shapes, Vector2 ds)
        {
            foreach (var shape in shapes) { shape.MoveCenter(ds.X, ds.Y); }
        }
        protected void MoveCenter(float dx, float dy)
        {
            for (int i = 0; i < DXY.Count; i++)
            {
                var (tx, ty) = DXY[i];
                DXY[i] = Limit(tx + dx, ty + dy);
            }
        }
        #endregion

        #region SetProperty  ==================================================
        internal static void SetProperty(SymbolModel sm, ShapeProperty pf, Shape s)
        {
            SetStrokeProperty(sm, pf, s);
            if ((pf & ShapeProperty.Aux) != 0) s.AuxAxis = sm.AuxAxis;
            if ((pf & ShapeProperty.Cent) != 0) s.MinorAxis = sm.CentAxis;
            if ((pf & ShapeProperty.Vert) != 0) s.MajorAxis = sm.VertAxis;
            if ((pf & ShapeProperty.Horz) != 0) s.MinorAxis = sm.HorzAxis;
            if ((pf & ShapeProperty.Rad1) != 0) s.MajorAxis = sm.MajorAxis;
            if ((pf & ShapeProperty.Rad2) != 0) s.MinorAxis = sm.MinorAxis;
            if ((pf & ShapeProperty.Dim) != 0) s.Dimension = sm.Dimension;
        }
        internal static void SetProperty(SymbolModel sm, ShapeProperty pf, IEnumerable<Shape> shapes)
        {
            SetStrokeProperty(sm, pf, shapes);
            if ((pf & ShapeProperty.Aux) != 0) ResizeAuxAxis(shapes, sm.AuxAxis);
            if ((pf & ShapeProperty.Cent) != 0) ResizeCentral(shapes, sm.CentAxis);
            if ((pf & ShapeProperty.Vert) != 0) ResizeVertical(shapes, sm.VertAxis);
            if ((pf & ShapeProperty.Horz) != 0) ResizeHorizontal(shapes, sm.HorzAxis);
            if ((pf & ShapeProperty.Rad1) != 0) ResizeMajorAxis(shapes, sm.MajorAxis);
            if ((pf & ShapeProperty.Rad2) != 0) ResizeMinorAxis(shapes, sm.MinorAxis);
            if ((pf & ShapeProperty.Dim) != 0) SetDimension(shapes, sm.Dimension);
        }
        internal static void SetStrokeProperty(SymbolModel sm, ShapeProperty pf, IEnumerable<Shape> shapes)
        {
            foreach (var s in shapes) SetStrokeProperty(sm, pf, s);
        }
        internal static void SetStrokeProperty(SymbolModel sm, ShapeProperty pf, Shape s)
        {
            if ((pf & ShapeProperty.LineWidth) != 0) s.SetStrokeWidth(sm.LineWidth);
            if ((pf & ShapeProperty.LineStyle) != 0) s.SetStokeStyle(sm.LineStyle);
            if ((pf & ShapeProperty.StartCap) != 0) s.SetStartCap(sm.StartCap);
            if ((pf & ShapeProperty.DashCap) != 0) s.SetDashCap(sm.DashCap);
            if ((pf & ShapeProperty.EndCap) != 0) s.SetEndCap(sm.EndCap);
        }
        internal static void ResizeCentral(IEnumerable<Shape> shapes, float slider)
        {
            var (_, _, _, _, cdx, cdy, dx, dy) = GetExtent(shapes);

            if (dx + dy > 0)
            {
                var actualSize = (dx > dy) ? dx : dy;
                var desiredSize = ConvertSlider(slider);
                var ratio = 2 * desiredSize / actualSize;
                var scale = new Vector2(ratio, ratio);
                foreach (var shape in shapes)
                {
                    shape.Scale(scale);
                    SetCenter(shapes, new Vector2(cdx, cdy));
                }
            }
        }
        internal static void ResizeVertical(IEnumerable<Shape> shapes, float slider)
        {
            var (_, dy1, _, dy2, cdx, cdy, dx, dy) = GetExtent(shapes);

            if (dx + dy > 0)
            {

                var actualSize = dy2 - dy1;
                var desiredSize = ConvertSlider(slider);
                var ratio = 2 * desiredSize / actualSize;
                var scale = new Vector2(1, ratio);
                foreach (var shape in shapes)
                {
                    shape.Scale(scale);
                    SetCenter(shapes, new Vector2(cdx, cdy));
                }
            }
        }
        internal static void ResizeHorizontal(IEnumerable<Shape> shapes, float slider)
        {
            var (dx1, _, dx2, _, cdx, cdy, dx, dy) = GetExtent(shapes);

            if (dx + dy > 0)
            {
                var actualSize = dx2 - dx1;
                var desiredSize = ConvertSlider(slider);
                var ratio = 2 * desiredSize / actualSize;
                var scale = new Vector2(ratio, 1);
                foreach (var shape in shapes)
                {
                    shape.Scale(scale);
                    SetCenter(shapes, new Vector2(cdx, cdy));
                }
            }
        }
        internal static void ResizeMajorAxis(IEnumerable<Shape> shapes, float slider)
        {
            var (_, _, _, _, cdx, cdy, _, _) = GetExtent(shapes);
            var desiredSize = ConvertSlider(slider);

            foreach (var shape in shapes)
            {
                shape.Radius1 = desiredSize;
                shape.CreatePoints();
            }
            SetCenter(shapes, new Vector2(cdx, cdy));
        }
        internal static void ResizeMinorAxis(IEnumerable<Shape> shapes, float slider)
        {
            var (_, _, _, _, cdx, cdy, _, _) = GetExtent(shapes);
            var desiredSize = ConvertSlider(slider);
            foreach (var shape in shapes)
            {
                shape.Radius2 = desiredSize;
                shape.CreatePoints();
            }
            SetCenter(shapes, new Vector2(cdx, cdy));
        }
        internal static void ResizeAuxAxis(IEnumerable<Shape> shapes, float slider)
        {
            var (_, _, _, _, cdx, cdy, _, _) = GetExtent(shapes);
            var desiredSize = ConvertSlider(slider);
            foreach (var shape in shapes)
            {
                shape.AuxFactor = desiredSize;
                shape.CreatePoints();
            }
            SetCenter(shapes, new Vector2(cdx, cdy));
        }
        internal static void SetDimension(IEnumerable<Shape> shapes, float pd)
        {
            var (min, max, _) = GetDimension(shapes);
            if (pd < min) pd = min;
            if (pd > max) pd = max;
            foreach (var shape in shapes)
            {
                shape.PD = (byte)pd;
                shape.CreatePoints();
            }
        }

        private static float ConvertSlider(float val)
        {
            var v = val < 0 ? 0 : val > 100 ? 100 : val;
            return v / 100;
        }
        #endregion

        #region GetProperty  ==================================================
        static internal ShapeProperty GetPropertyFlags(Shape shape) => (shape is null) ? ShapeProperty.None : GetPropertyFlags(new Shape[] { shape });
        static internal ShapeProperty GetPropertyFlags(IEnumerable<Shape> shapes)
        {
            var type = ShapeType.Unknown;
            var flags = ShapeProperty.None;
            var isNotMixed = true;

            foreach (var shape in shapes)
            {
                flags |= shape.PropertyFlags | shape.LinePropertyFlags();

                if (type == ShapeType.Unknown)
                    type = shape.ShapeType;
                else if (shape.ShapeType != type)
                    isNotMixed = false;
            }

            return isNotMixed ? flags : (flags &= ~ShapeProperty.MultiSizerMask) | ShapeProperty.Vert | ShapeProperty.Horz;
        }
        internal static void GetStrokeProperty(IEnumerable<Shape> shapes, ref ShapeProperty flag, ref byte width, ref StrokeStyle style, ref CapStyle startCap, ref CapStyle dashCap, ref CapStyle endCap, ref (byte,byte,byte,byte) color)
        {
            var first = true;
            foreach (var s in shapes)
            {
                var (_, st, sw) = s.ShapeStrokeWidth();

                var sc = st & StrokeType.SC_Triangle;
                var dc = st & StrokeType.DC_Triangle;
                var ec = st & StrokeType.EC_Triangle;
                var ss = st & StrokeType.Filled;

                if (first)
                {
                    first = false;
                    width = s.SW;
                    style = ss == StrokeType.Dotted ? StrokeStyle.Dotted : ss == StrokeType.Dashed ? StrokeStyle.Dashed : ss == StrokeType.Filled ? StrokeStyle.Filled : StrokeStyle.Solid;
                    endCap = ec == StrokeType.EC_Round ? CapStyle.Round : ec == StrokeType.EC_Square ? CapStyle.Square : ec == StrokeType.EC_Triangle ? CapStyle.Triangle : CapStyle.Flat;
                    dashCap = dc == StrokeType.DC_Round ? CapStyle.Round : dc == StrokeType.DC_Square ? CapStyle.Square : dc == StrokeType.DC_Triangle ? CapStyle.Triangle : CapStyle.Flat;
                    startCap = sc == StrokeType.SC_Round ? CapStyle.Round : sc == StrokeType.SC_Square ? CapStyle.Square : sc == StrokeType.SC_Triangle ? CapStyle.Triangle : CapStyle.Flat;
                    color = s.ShapeColor();
                }
            }
            flag = GetPropertyFlags(shapes);
        }
        internal static void GetSizerProperty(IEnumerable<Shape> shapes, ref bool locked, ref byte min, ref byte max, ref byte dim, ref byte aux, ref byte major, ref byte minor, ref byte cent, ref byte vert, ref byte horz)
        {
            if (shapes.Count() > 0)
            {
                var (dx1, dy1, dx2, dy2, _, _, _, _) = GetExtent(shapes);
                var (r1, r2, f1) = GetMaxRadius(shapes);

                var (smin, smax, sdim) = GetDimension(shapes);
                min = (byte)smin;
                max = (byte)smax;
                dim = (byte)sdim;

                horz = (byte)Limited(dx1, dx2);
                vert = (byte)Limited(dy1, dy2);
                cent = (byte)Larger(vert, horz);
                major = (byte)Factor(r1);
                minor = (byte)Factor(r2);
                aux = (byte)Factor(f1);
            }

            float Larger(float p, float q) => (p > q) ? p : q;
            float Limited(float a, float b) => Larger(Factor(a), Factor(b));
            float Factor(float v) => (float)Math.Round(100 * ((v < 0.01f) ? 0.01 : v));
        }
        #endregion

        #region LockSliders  ==================================================
        internal static void LockSliders(IEnumerable<Shape> shapes, bool isLocked)
        {
            foreach (var shape in shapes)
            {
                shape.IsLocked = isLocked;
            }
        }
        #endregion

        #region AddDrawTargets  ===============================================
        static internal  void AddDrawTargets(IEnumerable<Shape> shapes, List<Vector2> targets, DrawData drawData, float scale, Vector2 center)
        {
            var (_, _, _, _, cdx, cdy, dw, dh) = GetExtent(shapes);
            if (dw + dh > 0)
            {
                DrawTarget(new Vector2(cdx, cdy) * scale + center, true);
                
                if (shapes.Count() == 1  && shapes.First() is Polyline polyline)
                {
                    var points = polyline.GetDrawingPoints( center, scale);
                    foreach (var point in points)
                    {
                        DrawTarget(point);
                    }
                }

                void DrawTarget(Vector2 c, bool highlight = false)
                {
                    targets.Add(c);

                    drawData.AddShape(((c, new Vector2(7, 7)), (ShapeType.Circle, StrokeType.Simple, 3), (255, 255, 255, 255)));
                    if  (highlight)
                        drawData.AddShape(((c, new Vector2(9, 9)), (ShapeType.Circle, StrokeType.Simple, 3), (255, 255, 0, 0)));
                }
            }
        }
        #endregion

        #region AddDrawHighLight  =============================================
        internal static void AddDrawHighLight(DrawData drawData, float width, int index)
        {
            var hw = width / 2;
            var y1 = index * width;
            var y2 = y1 + width;
            drawData.AddShape(((new Vector2(hw, y1), new Vector2(hw, y2)), (ShapeType.Line, StrokeType.Simple, (byte)width),( 255, 80, 80, 80)));
        }
        #endregion
    }
}
