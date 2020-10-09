﻿using System.Numerics;

namespace ModelGraph.Core
{
    internal abstract class Polygon : Polyline
    {
        internal override void AddDrawData(DrawData drawData, float size, float scale, Vector2 center, Coloring coloring = Coloring.Normal)
        {
            //var color = GetColor(coloring);
            //var points = GetDrawingPoints(center, scale);

            //using (var geo = CanvasGeometry.CreatePolygon(cc, points))
            //{
            //    if (FillStroke == Fill_Stroke.Filled)
            //        ds.FillGeometry(geo, color);
            //    else
            //        ds.DrawGeometry(geo, color, strokeWidth, StrokeStyle());
            //}
        }
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 center, FlipState flip)
        {

        }

        protected override (int min, int max) MinMaxDimension => (2, 8);
        protected override ShapeProperty PropertyFlags => ShapeProperty.Major | ShapeProperty.Minor | ShapeProperty.Dim | LinePropertyFlags(StrokeType);
    }
}
