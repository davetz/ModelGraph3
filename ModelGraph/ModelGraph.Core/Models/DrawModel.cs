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
        public DrawState DrawState { get; private set; } = DrawState.Unknown;
        protected DrawState PreviousDrawState;
        public DrawCursor DrawCursor { get; protected set; } = DrawCursor.Arrow;

        internal void SetNowOn(DrawState st)
        {
            var state = (DrawState &= ~DrawState.NowMask) | (st & DrawState.NowMask);
            if (state == DrawState) return;   //no change, so nothing to do
            PreviousDrawState = DrawState;
            DrawState = state;
            Debug.WriteLine($"New DrawState: {DrawState}");
        }
        public bool TrySetState(DrawState state, bool reset = false)
        {
            if (state == DrawState.NoChange || state == DrawState) return false;   //no change, so nothing to do
            PreviousDrawState = DrawState;
            DrawState = state;
            Debug.WriteLine($"New DrawState: {DrawState}");

            if (reset)
            {
                DrawEvent_Action.Clear();
                IsToolTipVisible = false;
                IsResizerGridVisible = false;
                DrawCursor = DrawCursor.Arrow;
            }

            if (DrawStateAction.TryGetValue(state, out Action action)) action();

            return true;    // let the caller know that we have changed state and have cleared the Event_Action dictionary
        }
        private Dictionary<DrawState, Action> DrawStateAction = new Dictionary<DrawState, Action>(5);
        protected void SetDrawStateAction(DrawState state, Action action) => DrawStateAction[state] = action;
        protected void ClearDrawStateAction(DrawState state) => DrawStateAction.Remove(state);

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
        public Vector2 ToolTipTarget { get; set; }
        public Extent ResizerExtent { get; set; } //in drawPoint coordinates
        #endregion

        #region PointerData  ==================================================
        protected Vector2 RegionPoint1 { get; set; }
        protected Vector2 RegionPoint2 { get; set; }
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


        #region IDrawData  ====================================================
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

        #region ITreeModel  ===================================================
        public ITreeModel FlyTreeModel { get; protected set; }
        public ITreeModel SideTreeModel { get; protected set; }
        #endregion

        #region ColorARGB  ====================================================
        public (byte, byte, byte, byte) ColorARGB { get => _colorARGB; set => SetColor(value); }
        protected (byte, byte, byte, byte) _colorARGB = (255, 255, 255, 255);
        private void SetColor((byte, byte, byte, byte) colorARGB)
        {
            if (colorARGB != ColorARGB)
            {
                _colorARGB = colorARGB;
                ColorARGBChanged();
            }
        }
        virtual protected void ColorARGBChanged() { }
        #endregion
    }
}

