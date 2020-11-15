using System.Numerics;

namespace ModelGraph.Core
{
    internal abstract class PolySpline : Polyline
    {
        #region OverideAbstract  ==============================================
        internal override void AddDrawData(DrawData drawData, float size, float scale, Vector2 offset, Coloring c = Coloring.Normal)
        {
            var points = GetDrawingPoints(scale, offset);

            if (points.Length > 2)
            {
                drawData.AddParms((points, ShapeStrokeWidth(scale / size), ShapeColor(c)));
            }
        }
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 offset, FlipState flip)
        {
            var points = GetDrawingPoints(flip, scale, offset);

            if (points.Length > 2)
            {
                drawData.AddParms((points, ShapeStrokeWidth(scale), ShapeColor()));
            }
        }
        protected override Vector2 GetCenter()
        {
            var (x1, y1, x2, y2) = GetExtent();
            return new Vector2((x1 + x2), (y1 + y2)) / 2;
        }
        #endregion
    }
}
