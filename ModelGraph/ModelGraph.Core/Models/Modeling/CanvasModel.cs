using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace ModelGraph.Core
{
    public abstract class CanvasModel : Item, ISelector
    {
        abstract public bool MoveNode();
        abstract public bool MoveRegion();
        abstract public bool CreateNode();

        abstract public bool TapHitTest();
        abstract public bool EndHitTest();
        abstract public bool SkimHitTest();
        abstract public bool DragHitTest();

        abstract public bool RegionNodeHitTest();

        abstract public void ShowPropertyPanel();
        abstract public void HidePropertyPanel();

        abstract public void ResizeTop();
        abstract public void ResizeLeft();
        abstract public void ResizeRight();
        abstract public void ResizeBottom();
        abstract public void ResizeTopLeft();
        abstract public void ResizeTopRight();
        abstract public void ResizeBottomLeft();
        abstract public void ResizeBottomRight();
        abstract public void ResizePropagate();

        abstract public void RefreshCanvasDrawData();

        #region HitTest  ======================================================
        internal HitType Hit;

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
        #endregion

        #region CanvasDraw  ===================================================
        public IList<(Extent Extent, (byte A, byte R, byte G, byte B) Color)> DrawRects => _drawRects;
        protected IList<(Extent Extent, (byte A, byte R, byte G, byte B) Color)> _drawRects = new List<(Extent, (byte, byte, byte, byte))>();

        public IList<(Vector2[] Points, bool IsDotted, byte Width, (byte A, byte R, byte G, byte B) Color)> DrawLines => _drawLines;
        protected IList<(Vector2[] Points, bool IsDotted, byte Width, (byte A, byte R, byte G, byte B) Color)> _drawLines = new List<(Vector2[] Points, bool IsDotted, byte Width, (byte A, byte R, byte G, byte B) Color)>();

        public IList<(Vector2[] Points, bool IsDotted, byte Width, (byte A, byte R, byte G, byte B) Color)> DrawSplines => _drawSplines;
        protected IList<(Vector2[] Points, bool IsDotted, byte Width, (byte A, byte R, byte G, byte B) Color)> _drawSplines = new List<(Vector2[] Points, bool IsDotted, byte Width, (byte A, byte R, byte G, byte B) Color)>();

        public IList<(Vector2 TopLeft, string Text, (byte A, byte R, byte G, byte B) Color)> DrawText => _drawText;
        protected IList<(Vector2 TopLeft, string Text, (byte A, byte R, byte G, byte B) Color)> _drawText = new List<(Vector2 TopLeft, string Text, (byte A, byte R, byte G, byte B) Color)>();
        #endregion
    }
}

