using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Numerics;

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

        #region DrawStateCursor  ==============================================
        private Dictionary<byte, DrawCursor> _state_Cursor = new Dictionary<byte, DrawCursor>();
        public DrawCursor GetDrawStateCursor() => _state_Cursor.TryGetValue(_drawState, out DrawCursor cur) ? cur : DrawCursor.Arrow;
        protected void SetDrawStateCursors(byte state, DrawCursor cursor) => _state_Cursor[state] = cursor;
        #endregion

        #region DrawStateAction  ==============================================
        private byte _drawState = 0; //byte value of the current state, depends on model
        private Dictionary<byte, string> _drawStateNames = new Dictionary<byte, string>();
        private Dictionary<(byte, DrawEvent), Action> _drawStateEvent_Action = new Dictionary<(byte, DrawEvent), Action>();
        public bool TryGetEventAction(DrawEvent evt, out Action act) => _drawStateEvent_Action.TryGetValue((_drawState, evt), out act);

        protected void SetDrawStateValue(byte stateValue, bool execute = false)
        {
            if (stateValue != _drawState)
            {
                _drawState = stateValue;
                if (!_drawStateNames.TryGetValue(stateValue, out string stateName)) stateName = $"#{_drawState}";
                Debug.WriteLine($"New DrawState: {stateName}");
                if (execute && TryGetEventAction((DrawEvent.ExecuteAction), out Action action))
                {
                    Debug.WriteLine("Executing DrawState:");
                    action();
                }
            }
        }

        protected void InitDrawStateName(Type drawStateEnum)
        {
            foreach(int key in Enum.GetValues(drawStateEnum))
            {
                _drawStateNames[(byte)key] = Enum.GetName(drawStateEnum, key);
            }
        }
        #endregion

        #region DrawEventControl  =============================================
        private Dictionary<DrawEvent, string> _drawControlText = new Dictionary<DrawEvent, string>();
        public bool TryGetDrawControlText(DrawEvent key, out string text) => _drawControlText.TryGetValue(key, out text);
        public bool IsDrawControEnabled(DrawEvent key) => _drawStateEvent_Action.ContainsKey((_drawState, key));
        protected void AddDrawControlText(DrawEvent key, string text) => _drawControlText[key] = text;
        #endregion

        #region Layout  =======================================================
        public string ToolTip_Text1 { get; protected set; }
        public string ToolTip_Text2 { get; protected set; }
        public Vector2 FlyOutSize { get; protected set; }
        public Vector2 FlyOutPoint { get; protected set; }
        public DrawItem VisibleDrawItems { get; set; }

        protected void HideDrawItems(DrawItem flags) => VisibleDrawItems &= ~flags;
        protected void ShowDrawItems(DrawItem flags) => VisibleDrawItems |= flags;
        protected void ToolTipChanged()
        {
            if ((VisibleDrawItems & DrawItem.ToolTipChange) == 0)
                VisibleDrawItems |= DrawItem.ToolTipChange;
            else
                VisibleDrawItems &= ~DrawItem.ToolTipChange;
        }
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
        public uint FlyTreeDelta { get; protected set; } = 1;
        public uint SideTreeDelta { get; protected set; } = 1;
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

