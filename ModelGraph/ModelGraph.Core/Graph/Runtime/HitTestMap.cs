using System.Collections.Generic;

namespace ModelGraph.Core
{
    internal class HitTestMap
    {
        private readonly Dictionary<long, HashSet<IHitTestable>> XY_HitSegment;
        private readonly uint _hitSegmentMask;   // size of hit testable segment
        private readonly ushort _hitSegmentSize;   // size of hit testable segment

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
            XY_HitSegment = new Dictionary<long, HashSet<IHitTestable>>();
        }
        private const int MaxPowerOfTwo = 12; //for MaxSegmentSize of 2048
        private const int MinPowerOfTwo = 5;
        private const int MinSegmentSize = 32;
        private const uint MinSegmentMask = 0xFFFFFFFF << MinPowerOfTwo;
        private const byte HitMargin = 4;
        #endregion

        #region Insert/Remove  ================================================
        internal void Insert(IEnumerable<IHitTestable> items)
        {
            var xyHash = new HashSet<long>();
            foreach (var item in items)
            {
                item.GetHitSegments(xyHash, _hitSegmentMask, _hitSegmentSize, HitMargin);
                foreach (var seg in xyHash)
                {
                    if (!XY_HitSegment.TryGetValue(seg, out HashSet<IHitTestable> targets))
                    {
                        targets = new HashSet<IHitTestable>();
                        XY_HitSegment.Add(seg, targets);
                    }
                    targets.Add(item);
                }
            }
        }
        internal void Remove(IEnumerable<IHitTestable> items)
        {
            var xyHash = new HashSet<long>();
            foreach (var item in items)
            {
                item.GetHitSegments(xyHash, _hitSegmentMask, _hitSegmentSize, HitMargin);
                foreach (var seg in xyHash)
                {
                    if (XY_HitSegment.TryGetValue(seg, out HashSet<IHitTestable> targets))
                    {
                        targets.Remove(item);
                        if (targets.Count == 0) XY_HitSegment.Remove(seg);
                    }
                }
            }
        }
        #endregion
    }

}
