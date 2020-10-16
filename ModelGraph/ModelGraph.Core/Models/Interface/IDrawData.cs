using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public interface IDrawData
    {
        Extent Extent { get; }
        Vector2 Point1 { get; set; }
        Vector2 Point2 { get; set; }
        Vector2 PointDelta(bool reset = false);

        List<((Vector2, string), (byte, byte, byte, byte))> Text { get; }
        List<(Vector2[], (ShapeType, StrokeType, byte), (byte, byte, byte, byte))> Parms { get; }
    }
}
