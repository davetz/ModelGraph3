using System.Numerics;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace ModelGraph.Core
{
    public abstract class DrawModel : ChildOf<PageModel>, IDrawModel, ILeadModel
    {
        public PageModel PageModel => Owner;

        #region Constructor  ==================================================
        internal DrawModel(PageModel owner)
        {
            Owner = owner;
            owner.Add(this);
        }
        #endregion

        #region DrawState  ====================================================
        public DrawState DrawState => _state;
        public DrawCursor DrawCursor { get; protected set; } = DrawCursor.Arrow;

        public bool TrySetState(DrawState state)
        {
            if (state == DrawState.NoChange || state == _state) return false;   //no change, so nothing to do
            _state = state;
            Debug.WriteLine($"State: {_state}");

            DrawEvent_Action.Clear();
            IsToolTipVisible = false;
            IsResizerGridVisible = false;
            DrawCursor = DrawCursor.Arrow;

            return true;    // let the caller know that we have changed state and have cleared the Event_Action dictionary
        }
        private DrawState _state = DrawState.Unknown;

        public void SetEventAction(DrawEvent evt, Action act) => DrawEvent_Action[evt] = act;
        public void ClearEventAction(DrawEvent evt) => DrawEvent_Action.Remove(evt);
        public Dictionary<DrawEvent, Action> DrawEvent_Action { get; } = new Dictionary<DrawEvent, Action>();
        #endregion

        #region Layout  =======================================================
        public string ToolTip_Text1 { get; set; }
        public string ToolTip_Text2 { get; set; }

        public bool IsToolTipVisible { get; protected set; }
        public bool IsResizerGridVisible { get; protected set; }
        public bool IsFlyTreeVisible { get; protected set; }
        public bool IsSideTreeVisible { get; protected set; }
        public bool IsOverviewVisible { get; protected set; }
        public bool IsPicker1Visible { get; protected set; }
        public bool IsPicker2Visible { get; protected set; }
        public bool IsColorPickerEnabled { get; protected set; }

        virtual public int Picker1Width => 0;
        virtual public int Picker2Width => 0;
        public Vector2 ToolTipTarget { get; set; }
        public Extent ResizerExtent { get; set; } //in drawPoint coordinates
        virtual public Extent EditorExtent => new Extent(100, 100);
        #endregion

        #region PointerData  ==================================================
        public Vector2 GridPoint1 { get; set; }
        public Vector2 GridPoint2 { get; set; }

        public Vector2 DrawPoint1 { get; set; }
        public Vector2 DrawPoint2 { get; set; }

        protected Vector2 RegionPoint1 { get; set; }
        protected Vector2 RegionPoint2 { get; set; }
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
        #endregion

        #region PointerAction  ================================================
        virtual public bool MoveNode() => false;
        virtual public bool MoveRegion() => false;
        virtual public bool CreateNode() => false;

        #endregion


        #region HitTest  ======================================================

        protected void ClearHit()
        {
            ToolTip_Text1 = ToolTip_Text2 = string.Empty;
            _hit = Hit.ZZZ;
        }
        private Hit _hit;
        protected void SetHitPin() => _hit |= Hit.Pin;
        protected void SetHitNode() => _hit |= Hit.Node;
        protected void SetHitEdge() => _hit |= Hit.Edge;
        protected void SetHitRegion() => _hit |= Hit.Region;
        #endregion


        #region Resize  =======================================================
        virtual public void ResizeTop() { }
        virtual public void ResizeLeft() { }
        virtual public void ResizeRight() { }
        virtual public void ResizeBottom() { }
        virtual public void ResizeTopLeft() { }
        virtual public void ResizeTopRight() { }
        virtual public void ResizeBottomLeft() { }
        virtual public void ResizeBottomRight() { }
        virtual public void ResizePropagate() { }
        #endregion


        #region ITreeModel  ===================================================
        public ITreeModel FlyTreeModel { get; protected set; }
        public ITreeModel SideTreeModel { get; protected set; }
        #endregion


        #region Pickers  ======================================================
        virtual public void Picker1Select(int YCord, bool add = false) { }
        virtual public void Picker2Select(int YCord) { }
        virtual public void Picker2Paste() { }
        #endregion

        #region ColorARGB/Apply/Reload  =======================================
        public (byte, byte, byte, byte) ColorARGB { get; set; } = (255, 255, 255, 255);
        public virtual void ColorARGBChanged() { }

        public virtual void Apply() { }
        public virtual void Reload() { }
        #endregion

        #region IDrawData  ====================================================
        virtual public void RefreshEditorData() { }
        virtual public string HeaderTitle => "No Title was specified";

        public IDrawData HelperData => Helper;      // editor layer1  
        protected DrawData Helper = new DrawData(); 

        public IDrawData EditorData => Editor;      // editor layer2
        protected DrawData Editor = new DrawData();

        public IDrawData Picker1Data => Picker1;
        protected DrawData Picker1 = new DrawData();

        public IDrawData Picker2Data => Picker2;
        protected DrawData Picker2 = new DrawData();
        #endregion

    }
}

