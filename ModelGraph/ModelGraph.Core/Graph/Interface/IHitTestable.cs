using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal interface IHitTestable
    {
        //bool HitTestHit(Vector2 p);
        void GetHitSegments(HashSet<(int,int)> hitSegments, uint mask, ushort size, byte margin);
    }
}
