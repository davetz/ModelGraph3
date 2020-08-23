using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public abstract class DrawCanvas
    {
        public string ToolTip_Text1 { get; set; }
        public string ToolTip_Text2 { get; set; }

        public Vector2 GridPoint1 { get; set; }
        public Vector2 GridPoint2 { get; set; }

        public Vector2 DrawPoint1 { get; set; }
        public Vector2 DrawPoint2 { get; set; }

        public Vector2 NodePoint1 { get; protected set; }
        public Vector2 NodePoint2 { get; protected set; }

        public Vector2 RegionPoint1 { get; protected set; }
        public Vector2 RegionPoint2 { get; protected set; }
        public Vector2 GridPointDelta(bool reset = false)
        {
            var delta = GridPoint2 - GridPoint2;
            if (reset) GridPoint1 = GridPoint2;
            return delta;
        }
        public Vector2 DrawPointDelta(bool reset = false)
        {
            var delta = DrawPoint2 - DrawPoint2;
            if (reset) DrawPoint1 = DrawPoint2;
            return delta;
        }
        public bool IsAnyHit => Hit != 0;
        public bool IsHitPin => (Hit & HitType.Pin) != 0;
        public bool IsHitNode => (Hit & HitType.Node) != 0;
        public bool IsHitRegion => (Hit & HitType.Region) != 0;
        internal HitType Hit;

        public float Scale => _scale;
        protected float _scale;

        public Vector2 Offset => _offset;
        protected Vector2 _offset;

        protected void ClearDrawData()
        {
            _drawText.Clear();
            _drawLines.Clear();
            _drawRects.Clear();
            _drawCircles.Clear();
            _drawSplines.Clear();
        }
        public IList<((float, float, float, float) XYXY, Stroke style, byte Width, (byte A, byte R, byte G, byte B) Color)> DrawRects => _drawRects;
        protected IList<((float, float, float, float), Stroke, byte, (byte, byte, byte, byte))> _drawRects = new List<((float, float, float, float), Stroke, byte, (byte, byte, byte, byte))>();

        public IList<((float, float, float) XYR, Stroke style, byte Width, (byte A, byte R, byte G, byte B) Color)> DrawCircles => _drawCircles;
        protected IList<((float, float, float), Stroke, byte, (byte, byte, byte, byte))> _drawCircles = new List<((float, float, float), Stroke, byte, (byte, byte, byte, byte))>();

        public IList<(Vector2[] Points, Stroke style, byte Width, (byte A, byte R, byte G, byte B) Color)> DrawLines => _drawLines;
        protected IList<(Vector2[], Stroke, byte, (byte, byte, byte, byte))> _drawLines = new List<(Vector2[], Stroke, byte, (byte, byte, byte, byte))>();

        public IList<(Vector2[] Points, Stroke style, byte Width, (byte A, byte R, byte G, byte B) Color)> DrawSplines => _drawSplines;
        protected IList<(Vector2[], Stroke, byte, (byte, byte, byte, byte))> _drawSplines = new List<(Vector2[], Stroke, byte, (byte, byte, byte, byte))>();

        public IList<(Vector2 TopLeft, string Text, (byte A, byte R, byte G, byte B) Color)> DrawText => _drawText;
        protected IList<(Vector2, string, (byte A, byte R, byte G, byte B))> _drawText = new List<(Vector2, string, (byte A, byte R, byte G, byte B))>();
    }
}

