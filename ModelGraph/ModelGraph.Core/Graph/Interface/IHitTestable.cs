using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal interface IHitTestable
    {
        //bool HitTestHit(Vector2 p);
        void GetHitSegments(HashSet<long> hitSegments, uint mask, ushort size, byte margin);
    }
}
