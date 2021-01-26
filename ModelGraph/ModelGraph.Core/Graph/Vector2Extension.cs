using System.Numerics;

namespace ModelGraph.Core
{
    internal static class Vector2Extension
    {
        internal static bool IsClose2X(this Vector2 v, float x) => x >= (v.X - 2) && x <= (v.X + 2);
        internal static bool IsClose2Y(this Vector2 v, float y) => y >= (v.Y - 2) && y <= (v.Y + 2);
        internal static bool IsClose2P(this Vector2 v, Vector2 p) => p.X >= (v.X - 2) && p.X <= (v.X + 2) && p.Y >= (v.Y - 2) && p.Y <= (v.Y + 2);
        internal static bool IsClose4X(this Vector2 v, float x) => x >= (v.X - 4) && x <= (v.X + 4);
        internal static bool IsClose4Y(this Vector2 v, float y) => y >= (v.Y - 4) && y <= (v.Y + 4);
        internal static bool IsClose4P(this Vector2 v, Vector2 p) => p.X >= (v.X - 4) && p.X <= (v.X + 4) && p.Y >= (v.Y - 4) && p.Y <= (v.Y + 4);
    }
}
