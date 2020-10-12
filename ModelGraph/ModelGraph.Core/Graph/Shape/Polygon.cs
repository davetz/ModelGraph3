using System.Numerics;

namespace ModelGraph.Core
{
    internal abstract class Polygon : Polyline
    {
        protected override ShapeProperty ValidLineProperty => ShapeProperty.LineStyle | ShapeProperty.DashCap | ShapeProperty.LineWidth;

        protected override (int min, int max) MinMaxDimension => (2, 8);
        protected override ShapeProperty PropertyFlags => ShapeProperty.Major | ShapeProperty.Minor | ShapeProperty.Dim;
    }
}
