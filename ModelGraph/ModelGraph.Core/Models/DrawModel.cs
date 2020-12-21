using System.Numerics;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Windows.UI.ApplicationSettings;

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

            SetEventAction(DrawEvent.Drag, () => { AugmentDrawState(DrawState.Dragging, DrawState.EventMask); });
            SetEventAction(DrawEvent.CtrlDrag, () => { AugmentDrawState(DrawState.CtrlDraging, DrawState.EventMask); });
            SetEventAction(DrawEvent.ShiftDrag, () => { AugmentDrawState(DrawState.ShiftDraging, DrawState.EventMask); });
            SetEventAction(DrawEvent.KeyUpArrow, () => { AugmentDrawState(DrawState.UpArrow, DrawState.EventMask); });
            SetEventAction(DrawEvent.KeyLeftArrow, () => { AugmentDrawState(DrawState.LeftArrow, DrawState.EventMask); });
            SetEventAction(DrawEvent.KeyDownArrow, () => { AugmentDrawState(DrawState.DownArrow, DrawState.EventMask); });
            SetEventAction(DrawEvent.KeyRightArrow, () => { AugmentDrawState(DrawState.RightArrow, DrawState.EventMask); });
            SetEventAction(DrawEvent.ContextMenu, () => { AugmentDrawState(DrawState.ContextMenu, DrawState.EventMask); });

            SetEventAction(DrawEvent.SetAddMode, () => { AugmentDrawState(DrawState.AddMode, DrawState.ModeMask); PageModel.TriggerUIRefresh(); });
            SetEventAction(DrawEvent.SetViewMode, () => { AugmentDrawState(DrawState.ViewMode, DrawState.ModeMask); PageModel.TriggerUIRefresh(); });
            SetEventAction(DrawEvent.SetEditMode, () => { AugmentDrawState(DrawState.EditMode, DrawState.ModeMask); PageModel.TriggerUIRefresh(); });
            SetEventAction(DrawEvent.SetMoveMode, () => { AugmentDrawState(DrawState.MoveMode, DrawState.ModeMask); PageModel.TriggerUIRefresh(); });
            SetEventAction(DrawEvent.SetCopyMode, () => { AugmentDrawState(DrawState.CopyMode, DrawState.ModeMask); PageModel.TriggerUIRefresh(); });
            SetEventAction(DrawEvent.SetPinsMode, () => { AugmentDrawState(DrawState.PinsMode, DrawState.ModeMask); PageModel.TriggerUIRefresh(); });
            SetEventAction(DrawEvent.SetLinkMode, () => { AugmentDrawState(DrawState.LinkMode, DrawState.ModeMask); PageModel.TriggerUIRefresh(); });
            SetEventAction(DrawEvent.SetUnlinkMode, () => { AugmentDrawState(DrawState.UnlinkMode, DrawState.ModeMask); PageModel.TriggerUIRefresh(); });
            SetEventAction(DrawEvent.SetCreateMode, () => { AugmentDrawState(DrawState.CreateMode, DrawState.ModeMask); PageModel.TriggerUIRefresh(); });
            SetEventAction(DrawEvent.SetDeleteMode, () => { AugmentDrawState(DrawState.DeleteMode, DrawState.ModeMask); PageModel.TriggerUIRefresh(); });
            SetEventAction(DrawEvent.SetGravityMode, () => { AugmentDrawState(DrawState.GravityMode, DrawState.ModeMask); PageModel.TriggerUIRefresh(); });
            SetEventAction(DrawEvent.SetOperateMode, () => { AugmentDrawState(DrawState.OperateMode, DrawState.ModeMask); PageModel.TriggerUIRefresh(); });
        }
        public virtual void Release() { }
        #endregion

        #region DrawCursor  ===================================================
        private Dictionary<DrawState, DrawCursor> _drawState_Cursor = new Dictionary<DrawState, DrawCursor>();
        public DrawCursor GetDrawStateCursor() => _drawState_Cursor.TryGetValue(DrawState, out DrawCursor cur) ? cur : DrawCursor.Arrow;
        protected void ClearDrawStateCursors() => _drawState_Cursor.Clear();
        protected void SetDrawStateCursors(DrawState state, DrawCursor cursor) => _drawState_Cursor[state] = cursor;
        #endregion

        #region DrawState  ====================================================
        public DrawState DrawState { get; private set; } = DrawState.Unknown;
        protected DrawState PreviousDrawState;
        protected void AugmentDrawState(DrawState state, DrawState mask)
        {
            var mayNotRepeat = (state & DrawState.MayRepeat) == 0;

            state = (DrawState & ~mask) | (state & mask);
            if (state == DrawState && mayNotRepeat) return;   //no change, so nothing to do
            SetDrawState(state);
        }
        protected void SetDrawState(DrawState state)
        {
            PreviousDrawState = DrawState;
            DrawState = state;
            if (PreviousDrawState != DrawState)
            {//-----------------------------------write drawState debug message only when state changes
                var e = "";
                var u = "Unknown";
                var nowOn = state & DrawState.NowMask;
                var nowOnStr = nowOn == 0 ? e : nowOn.ToString();
                var isTarg = state & DrawState.IsTarget;
                var isTartStr = isTarg == 0 ? e : isTarg.ToString();
                var startOn = state & DrawState.StartOnMask;
                var startOnStr = startOn == 0 ? e : startOn.ToString();
                var mode = state & DrawState.ModeMask;
                var modeStr = mode == 0 ? u : mode.ToString().Replace("Mode", ""); 
                var evnt = state & DrawState.EventMask;
                var evntStr = evnt == 0 ? e : evnt.ToString();
                Debug.WriteLine($"New DrawState: {modeStr} {startOnStr} {nowOnStr} {evntStr} {isTartStr}");
            }
            if (_drawState_Action.TryGetValue(state, out Action action)) action();
        }
        protected void SetDrawStateAction(DrawState state, Action action) => _drawState_Action[state] = action;
        protected void SetEventAction(DrawEvent evt, Action act) => _drawEvent_Action[evt] = act;

        public bool TryGetDrawEventAction(DrawEvent evt, out Action act) => _drawEvent_Action.TryGetValue(evt, out act);

        private Dictionary<DrawEvent, Action> _drawEvent_Action = new Dictionary<DrawEvent, Action>();
        private Dictionary<DrawState, Action> _drawState_Action = new Dictionary<DrawState, Action>();
        #endregion

        #region Layout  =======================================================
        public string ToolTip_Text1 { get; protected set; }
        public string ToolTip_Text2 { get; protected set; }
        public Vector2 FlyOutSize { get; protected set; }
        public Vector2 FlyOutPoint { get; protected set; }
        public Extent ResizerExtent { get; protected set; } //in drawPoint coordinates
        public DrawItem VisibleDrawItems { get; set; }
        public bool IsPasteActionEnabled { get; protected set; }

        protected void HideDrawItems(DrawItem flags) => VisibleDrawItems &= ~flags;
        protected void ShowDrawItems(DrawItem flags) => VisibleDrawItems |= flags;
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

