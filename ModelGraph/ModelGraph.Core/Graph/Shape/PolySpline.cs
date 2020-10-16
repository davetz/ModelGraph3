using System.Numerics;

namespace ModelGraph.Core
{
    internal abstract class PolySpline : Polyline
    {
        #region OverideAbstract  ==============================================
        internal override void AddDrawData(DrawData drawData, float size, float scale, Vector2 center, Coloring c = Coloring.Normal)
        {
            var points = GetDrawingPoints(center, scale);

            if (points.Length > 2)
            {
                drawData.AddParms((points, ShapeStrokeWidth(scale / size), ShapeColor(c)));
            }
        }
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 center, FlipState flip)
        {
            var points = GetDrawingPoints(flip, scale, center);

            if (points.Length > 2)
            {
                drawData.AddParms((points, ShapeStrokeWidth(scale), ShapeColor()));
            }
        }
        protected override (float, float) GetCenter()
        {
            var (x1, y1, x2, y2) = GetExtent();
            return ((x1 + x2) / 2, (y1 + y2) / 2);
        }
        #endregion
    }
}
