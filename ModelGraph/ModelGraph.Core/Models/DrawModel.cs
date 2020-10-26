﻿using System.Numerics;
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
        }
        public virtual void Release() { }
        #endregion

        #region DrawState  ====================================================
        public DrawState DrawState { get; private set; } = DrawState.Unknown;
        protected DrawState PreviousDrawState;
        public DrawCursor DrawCursor { get; protected set; } = DrawCursor.Arrow;

        internal void AugmentDrawState(DrawState state, DrawState mask)
        {
            var notDraging = state != DrawState.Draging;

            state = (DrawState & ~mask) | (state & mask);
            if (state == DrawState && notDraging) return;   //no change, so nothing to do
            SetDrawState(state);
        }
        public bool TrySetState(DrawState state)
        {
            if (state == DrawState.NoChange || state == DrawState) return false;   //no change, so nothing to do
            SetDrawState(state);
            return true;    // let the caller know that we have changed state
        }
        private void SetDrawState(DrawState state)
        {
            PreviousDrawState = DrawState;
            DrawState = state;
            if (PreviousDrawState != DrawState)
                Debug.WriteLine($"New DrawState: {DrawState}");
            if (_drawState_Action.TryGetValue(state, out Action action)) action();
        }
        protected void SetDrawStateAction(DrawState state, Action action) => _drawState_Action[state] = action;
        protected void ClearDrawStateAction(DrawState state) => _drawState_Action.Remove(state);

        public void SetEventAction(DrawEvent evt, Action act) => _drawEvent_Action[evt] = act;
        public void ClearEventAction(DrawEvent evt) => _drawEvent_Action.Remove(evt);
        protected void ClearAllEventActions() => _drawEvent_Action.Clear();

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
        public DrawItem VisibleDrawItems { get; protected set; }
        public DrawItem EnabledDrawItems { get; protected set; }
        public bool IsPasteActionEnabled { get; protected set; }

        protected void HideDrawItems(DrawItem flags) => VisibleDrawItems &= ~flags;
        protected void ShowDrawItems(DrawItem flags) => VisibleDrawItems |= flags;
        protected void EnableDrawItems(DrawItem flags) => EnabledDrawItems |= flags;
        protected void DisableDrawItems(DrawItem flags) => EnabledDrawItems &= ~flags;
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

