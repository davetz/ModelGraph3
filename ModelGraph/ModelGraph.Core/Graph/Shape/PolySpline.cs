using System.Numerics;

namespace ModelGraph.Core
{
    internal abstract class PolySpline : Polyline
    {
        #region OverideAbstract  ==============================================
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 center, float strokeWidth, Coloring coloring = Coloring.Normal)
        {
            var points = GetDrawingPoints(center, scale);

            if (points.Length > 2)
            {
                drawData.AddLine((points, ShapeStrokeWidth, ShapeColor));
            }
        }
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 center, FlipState flip)
        {
            var points = GetDrawingPoints(flip, scale, center);

            if (points.Length > 2)
            {
                drawData.AddLine((points, ShapeStrokeWidth, ShapeColor));
            }
        }
        #endregion
    }
}
