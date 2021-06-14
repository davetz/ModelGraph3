using System.Numerics;

namespace ModelGraph.Core
{
    internal abstract class XPolygon : XPolyline
    {
        protected override (int min, int max) MinMaxDimension => (3, 16);
        protected override XShapeProperty PropertyFlags => XShapeProperty.Rad1 | XShapeProperty.Rad2 | XShapeProperty.Dim;
        protected override XShapeProperty ValidLineProperty => XShapeProperty.LineStyle | XShapeProperty.DashCap | XShapeProperty.LineWidth;
    }
}
