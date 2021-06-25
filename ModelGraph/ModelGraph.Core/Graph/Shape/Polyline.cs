using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal abstract class Polyline : Shape
    {

        #region Polyline Methods  =============================================
        internal Vector2[] GetDrawingPoints(float scale, Vector2 offset)
        {
            var list = new List<Vector2>(DXY.Count);
            foreach (var p in DXY)
            {
                list.Add(offset + p * scale);
            }
            return list.ToArray();
        }

        internal void MovePoint(int index, Vector2 ds)
        {
            if (index < 0) return;
            if (index < DXY.Count)
            {
                DXY[index] = Limit(DXY[index] + ds);
            }
        }
        internal bool TryDeletePoint(int index)
        {
            if (index < 0) return false;
            if (index < DXY.Count && DXY.Count > 2)
            {
                DXY.RemoveAt(index);
                return true;
            }
            return false;
        }
        internal bool TryAddPoint(int index, Vector2 p)
        { 
            if (index < 0) return false;
            if (index < DXY.Count)
            {
                DXY.Insert(index + 1, p);
                return true;
            }
            else if (index == DXY.Count)
            {
                DXY.Add(p);
                return true;
            }
            return false;
        }
        #endregion

        #region OverideAbstract  ==============================================
        protected override Vector2 GetCenter()
        {
            var (x1, y1, x2, y2, cx, cy) = GetExtent();
            return new Vector2(cx, cy);
        }
        internal override (float x1, float y1, float x2, float y2, float cx, float cy) GetExtent()
        {
            if (DXY is null) return (0, 0, 0, 0, 0, 0);
            var x1 = 1f;
            var y1 = 1f;
            var x2 = -1f;
            var y2 = -1f;
            foreach (var d in DXY)
            {
                if (d.X < x1) x1 = d.X;
                if (d.Y < y1) y1 = d.Y;

                if (d.X > x2) x2 = d.X;
                if (d.Y > y2) y2 = d.Y;
            }
            var cx = (x2 + x1) / 2;
            var cy = (y2 + y1) / 2;
            return (x1 == 1) ? (0, 0, 0, 0, 0, 0) : (x1, y1, x2, y2, cx, cy);
        }
        protected override void Scale(Vector2 scale) => TransformPoints(Matrix3x2.CreateScale(scale));

        protected override void DeltaCenterX(float delta)
        {
            for (int i = 0; i < DXY.Count; i++)
            {
                var p = DXY[i];
                DXY[i] = new Vector2(p.X + delta, p.Y); 
            }
        }
        protected override void DeltaCenterY(float delta)
        {
            for (int i = 0; i < DXY.Count; i++)
            {
                var p = DXY[i];
                DXY[i] = new Vector2(p.X, p.Y + delta);
            }
        }
        internal override void AddDrawData(DrawData drawData, float size, float scale, Vector2 offset, Coloring c = Coloring.Normal)
        {
            var points = GetDrawingPoints(scale, offset);
            drawData.AddParms((points, ShapeStrokeWidth(scale / size), ShapeColor(c)));
        }
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 offset, FlipState flip)
        {
            var points = GetDrawingPoints(flip, scale, offset);
            drawData.AddParms((points, ShapeStrokeWidth(), ShapeColor()));
        }
        protected override ShapeProperty PropertyFlags => ShapeProperty.Radius1 | ShapeProperty.Radius2;
        #endregion
    }
}
