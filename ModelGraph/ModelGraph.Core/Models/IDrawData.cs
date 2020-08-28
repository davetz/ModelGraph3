using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public interface IDrawData
    {
        List<((Vector2, string) topLeftText, (byte, byte, byte, byte) ARGB)> Text { get; }
        List<(Vector2[] Points, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> Lines { get; }
        List<(Vector2[] Points, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> Splines { get; }
        List<((Vector2, Vector2) centerRadius, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> Rects { get; }
        List<((Vector2, float) centerRadius, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> Circles { get; }
    }
}
