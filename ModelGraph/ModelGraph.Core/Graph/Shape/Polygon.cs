using System.Numerics;

namespace ModelGraph.Core
{
    internal abstract class Polygon : Polyline
    {
        protected override (int min, int max) MinMaxDimension => (3, 16);
        protected override ShapeProperty PropertyFlags => ShapeProperty.Rad1 | ShapeProperty.Rad2 | ShapeProperty.Dim;
        protected override ShapeProperty ValidLineProperty => ShapeProperty.LineStyle | ShapeProperty.DashCap | ShapeProperty.LineWidth;
    }
}
