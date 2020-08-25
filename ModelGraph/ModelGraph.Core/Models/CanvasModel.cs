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
        public bool IsAnyHit => Hit != 0;
        public bool IsHitPin => (Hit & HitType.Pin) != 0;
        public bool IsHitNode => (Hit & HitType.Node) != 0;
        public bool IsHitEdge => (Hit & HitType.Edge) != 0;
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

        public List<((Vector2, Vector2) centerRadius, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> DrawRects => _drawRects;
        protected List<((Vector2, Vector2), (Stroke, byte), (byte, byte, byte, byte))> _drawRects = new List<((Vector2, Vector2), (Stroke, byte), (byte, byte, byte, byte))>();

        public List<((Vector2, float) centerRadius, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> DrawCircles => _drawCircles;
        protected List<((Vector2, float), (Stroke, byte), (byte, byte, byte, byte))> _drawCircles = new List<((Vector2, float), (Stroke, byte), (byte, byte, byte, byte))>();

        public List<(Vector2[] Points, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> DrawLines => _drawLines;
        protected List<(Vector2[], (Stroke, byte), (byte, byte, byte, byte))> _drawLines = new List<(Vector2[], (Stroke, byte), (byte, byte, byte, byte))>();

        public List<(Vector2[] Points, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> DrawSplines => _drawSplines;
        protected List<(Vector2[], (Stroke, byte), (byte, byte, byte, byte))> _drawSplines = new List<(Vector2[], (Stroke, byte), (byte, byte, byte, byte))>();

        public List<((Vector2, string) topLeftText, (byte, byte, byte, byte) ARGB)> DrawText => _drawText;
        protected List<((Vector2, string), (byte, byte, byte, byte))> _drawText = new List<((Vector2, string), (byte, byte, byte, byte))>();
    }
}

