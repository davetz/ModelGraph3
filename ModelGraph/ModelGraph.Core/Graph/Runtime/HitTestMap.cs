using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class HitTestMap
    {
        private readonly Dictionary<(int,int), List<IHitTestable>> XY_HitSegment;
        private readonly uint _hitSegmentMask;   // size of hit testable segment
        private readonly ushort _hitSegmentSize;   // size of hit testable segment
        internal byte ModelDelta = 252;            //used to keep hitTestMap aligned with the graph

        #region Constructor  ==================================================
        internal HitTestMap(int segmentSize)
        {
            var S = segmentSize;
            if (S > MinSegmentSize)
            {
                S >>= MinPowerOfTwo;
                var N = MinPowerOfTwo;
                for (int i = 0; i < MaxPowerOfTwo; i++)
                {
                    N++;
                    S >>= 1;
                    if (S == 0) break;
                }
                _hitSegmentSize = (ushort)(1 << N);
                _hitSegmentMask = 0xFFFFFFFF << N;
            }
            else
            {
                _hitSegmentSize = MinSegmentSize; //smallest hit testable segment
                _hitSegmentMask = MinSegmentMask; //smallest hit testable segment
            }
            XY_HitSegment = new Dictionary<(int,int), List<IHitTestable>>();
        }
        private const int MaxPowerOfTwo = 12; //for MaxSegmentSize of 2048
        private const int MinPowerOfTwo = 5;
        private const int MinSegmentSize = 32;
        private const uint MinSegmentMask = 0xFFFFFFFF << MinPowerOfTwo;
        private const byte HitMargin = 2;
        #endregion

        #region HitTest  ======================================================
        internal bool HitTest(Vector2 p, out List<IHitTestable> targets)
        {
            var x = (int)((int)p.X & _hitSegmentMask);
            var y = (int)((int)p.Y & _hitSegmentMask);
            return XY_HitSegment.TryGetValue((x, y), out targets);
        }
        #endregion

        #region Insert/Remove  ================================================
        internal void Clear() => XY_HitSegment.Clear();
        internal void Insert(IEnumerable<IHitTestable> items)
        {
            var xyHash = new HashSet<(int,int)>();
            foreach (var item in items)
            {
                xyHash.Clear();
                item.GetHitSegments(xyHash, _hitSegmentMask, _hitSegmentSize, HitMargin);
                foreach (var seg in xyHash)
                {
                    if (!XY_HitSegment.TryGetValue(seg, out List<IHitTestable> targets))
                    {
                        targets = new List<IHitTestable>(2); //most hit sectors will only have one or two HitTestable items
                        XY_HitSegment.Add(seg, targets);
                    }
                    if (!targets.Contains(item)) targets.Add(item);
                }
            }
        }
        internal void Remove(IEnumerable<IHitTestable> items)
        {
            var xyHash = new HashSet<(int,int)>();
            foreach (var item in items)
            {
                xyHash.Clear();
                item.GetHitSegments(xyHash, _hitSegmentMask, _hitSegmentSize, HitMargin);
                foreach (var seg in xyHash)
                {
                    if (XY_HitSegment.TryGetValue(seg, out List<IHitTestable> targets))
                    {
                        targets.Remove(item);
                        if (targets.Count == 0) XY_HitSegment.Remove(seg);
                    }
                }
            }
        }
        #endregion

        #region AddDrawParms  =================================================
        internal void AddDrawParms(DrawData data)
        {
            var points = new Vector2[XY_HitSegment.Count + 1];
            points[0] = new Vector2(_hitSegmentSize, _hitSegmentSize);
            var i = 1;
            foreach (var (x,y) in XY_HitSegment.Keys)
            {
                points[i++] = new Vector2(x, y);
            }        
            data.AddParms((points, (ShapeType.EqualRect, StrokeType.Simple, 1), (200, 100, 100, 0)));
        }
        #endregion
    }

}
