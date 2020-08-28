using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public abstract class CanvasModel
    {
        public string ToolTip_Text1 { get; set; }
        public string ToolTip_Text2 { get; set; }

        public Vector2 GridPoint1 { get; set; }
        public Vector2 GridPoint2 { get; set; }

        public Vector2 DrawPoint1 { get; set; }
        public Vector2 DrawPoint2 { get; set; }

        public Vector2 NodePoint1 { get; protected set; }
        public Vector2 NodePoint2 { get; protected set; }

        public Vector2 RegionPoint1 { get; set; }
        public Vector2 RegionPoint2 { get; set; }
        public Vector2 GridPointDelta(bool reset = false)
        {
            var delta = GridPoint2 - GridPoint1;
            if (reset) GridPoint1 = GridPoint2;
            return delta;
        }
        public Vector2 DrawPointDelta(bool reset = false)
        {
            var delta = DrawPoint2 - DrawPoint1;
            if (reset) DrawPoint1 = DrawPoint2;
            return delta;
        }
        public bool AnyHit => _hit != 0;
        public bool PinHit => (_hit & Hit.Pin) != 0;
        public bool NodeHit => (_hit & Hit.Node) != 0;
        public bool EdgeHit => (_hit & Hit.Edge) != 0;
        public bool RegionHit => (_hit & Hit.Region) != 0;

        public int Picker1Index { get; set; }
        public int Picker2Index { get; set; }


        protected void ClearHit()
        {
            ToolTip_Text1 = ToolTip_Text2 = string.Empty;
            _hit = Hit.ZZZ;
        }
        protected void SetHitPin() => _hit |= Hit.Pin;
        protected void SetHitNode() => _hit |= Hit.Node;
        protected void SetHitEdge() => _hit |= Hit.Edge;
        protected void SetHitRegion() => _hit |= Hit.Region;
        private Hit _hit;

        public IDrawData EditorData => Editor;
        public IDrawData Picker1Data => Picker1;
        public IDrawData Picker2Data => Picker2;

        internal DrawData Editor = new DrawData();
        internal DrawData Picker1 = null;
        internal DrawData Picker2 = new DrawData();
    }
}

