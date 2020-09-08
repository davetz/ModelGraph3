using System.Numerics;

namespace ModelGraph.Core
{
    internal abstract class PolySpline : Polyline
    {
        #region OverideAbstract  ==============================================
        internal override void AddDrawData(IDrawData drawData, float scale, Vector2 center, float strokeWidth, Coloring coloring = Coloring.Normal)
        {
            //var color = GetColor(coloring);
            //var points = GetDrawingPoints(center, scale);

            //using (var pb = new CanvasPathBuilder(ctl))
            //{
            //    pb.BeginFigure(points[0]);
            //    var N = DXY.Count;
            //    for (var i = 0; i < N - 2;)
            //    {
            //        pb.AddCubicBezier(points[i], points[++i], points[++i]);
            //    }
            //    pb.EndFigure(CanvasFigureLoop.Open);

            //    using (var geo = CanvasGeometry.CreatePath(pb))
            //    {
            //        if (FillStroke == Fill_Stroke.Filled)
            //            ds.FillGeometry(geo, color);
            //        else
            //            ds.DrawGeometry(geo, color, strokeWidth, StrokeStyle());
            //    }
            //}
        }
        internal override void AddDrawData(IDrawData drawData, float scale, Vector2 center, FlipState flip)
        {
            //var color = GetColor(Coloring.Normal);
            //var points = GetDrawingPoints(flip, scale, center);

            //using (var pb = new CanvasPathBuilder(cc))
            //{
            //    pb.BeginFigure(points[0]);
            //    var N = DXY.Count;
            //    for (var i = 0; i < N - 2;)
            //    {
            //        pb.AddCubicBezier(points[i], points[++i], points[++i]);
            //    }
            //    pb.EndFigure(CanvasFigureLoop.Open);

            //    using (var geo = CanvasGeometry.CreatePath(pb))
            //    {
            //        if (FillStroke == Fill_Stroke.Filled)
            //            ds.FillGeometry(geo, color);
            //        else
            //            ds.DrawGeometry(geo, color, StrokeWidth, StrokeStyle());
            //    }
            //}
        }
        #endregion
    }
}
