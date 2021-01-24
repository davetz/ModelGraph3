using System.Numerics;

namespace ModelGraph.Core
{
    internal static class Vector2Extension
    {
        private const float ds = 1.5f; //hit test margin

        internal static bool ContainsX(this Vector2 v, float x) => x >= (v.X - ds) && x <= (v.X + ds);
        internal static bool ContainsY(this Vector2 v, float y) => y >= (v.Y - ds) && y <= (v.Y + ds);
        internal static bool Contains(this Vector2 v, Vector2 xy) => xy.X >= (v.X - ds) && xy.X <= (v.X + ds) && xy.Y >= (v.Y - ds) && xy.Y <= (v.Y + ds);
    }   
}
