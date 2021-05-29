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
        public void PostRefresh() => PageModel.Root.PostRefresh();
        #endregion

        #region DrawStateCursor  ==============================================
        private Dictionary<(byte, byte), DrawCursor> _modeState_Cursor = new Dictionary<(byte, byte), DrawCursor>();
        public DrawCursor GetModeStateCursor() => _modeState_Cursor.TryGetValue((_drawMode, _drawState), out DrawCursor cur) ? cur : DrawCursor.Arrow;
        protected void SetModeStateCursor(byte mode, byte state, DrawCursor cursor) => _modeState_Cursor[(mode, state)] = cursor;
        #endregion

        #region DrawStateAction  ==============================================
        private byte _drawMode = 0; //byte value of the current draw mode, model dependant
        private byte _drawState = 0; //byte value of the current draw state, model dependant
        private readonly Dictionary<byte, string> _drawModeNames = new Dictionary<byte, string>();
        private readonly Dictionary<byte, string> _drawStateNames = new Dictionary<byte, string>();
        private readonly Dictionary<(byte, byte, DrawEvent), Action> _modeStateEvent_Action = new Dictionary<(byte, byte, DrawEvent), Action>();

        public byte ModeIndex { get => _drawMode; set => TransitionModeState(value); }
        private void TransitionModeState(byte mode)
        {
            var prevState = _drawState;
            SetModeState(mode, 0, true);
            SetModeState(mode, prevState, true);
        }
        public bool TryGetEventAction(DrawEvent evt, out Action act) => _modeStateEvent_Action.TryGetValue((_drawMode, _drawState, evt), out act);
        virtual public List<IdKey> GetModeIdKeys() => new List<IdKey>(0);
        protected void SetDrawState(byte state) => SetModeState(_drawMode, state, true);

        private void SetModeState(byte mode, byte state, bool execute = false)
        {
            if (mode != _drawMode || state != _drawState)
            {
                _drawMode = mode;
                _drawState = state;
                var modeName = _drawModeNames.TryGetValue(mode, out string vMode) ? vMode : $"#{_drawMode}";
                var stateName = _drawStateNames.TryGetValue(state, out string vState) ? vState : $"#{_drawState}";
                var executing = "no action";
                if (execute && TryGetEventAction((DrawEvent.Pseudo), out Action action))
                {
                    executing = "executing action";
                    action();
                }
                Debug.WriteLine($"Drawing mode state:  {modeName} {stateName} - {executing}");
            }
        }

        protected void SetModeStateEventAction(byte mode, byte state, DrawEvent evt, Action act) => _modeStateEvent_Action[(mode, state, evt)] = act;

        protected void SetModeNames(Type drawModeEnum)
        {
            foreach (byte key in Enum.GetValues(drawModeEnum))
            {
                _drawModeNames[key] = Enum.GetName(drawModeEnum, key);
            }
        }
        protected void SetStateNames(Type drawStateEnum)
        {
            foreach (byte key in Enum.GetValues(drawStateEnum))
            {
                _drawStateNames[key] = Enum.GetName(drawStateEnum, key);
            }
        }
        #endregion

        #region Layout  =======================================================
        public Vector2 FlyOutSize { get; protected set; }
        public Vector2 FlyOutPoint { get; protected set; }
        public string ToolTip_Text1 { get; protected set; }
        public string ToolTip_Text2 { get; protected set; }
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

        public ushort EditorWidth {get; protected set;}

        public ushort Picker1Width { get; protected set; }

        public ushort Picker2Width { get; protected set; }

        public ushort OverviewWidth { get; protected set; }

        public ushort SideTreeWidth { get; protected set; }


        #endregion

        #region IDrawData  ====================================================
        virtual public string HeaderTitle => "No Title was specified";

        public IDrawData EditData => Editor;
        public IDrawData BackData => BackLay;
        public IDrawData Picker1Data => Picker1;
        public IDrawData Picker2Data => Picker2;
        public IDrawData OverviewData => Overview;

        protected DrawData Editor = new DrawData(); // editor interactive layer - required
        protected DrawData BackLay { get; set; }    // editor background layer - optional 
        protected DrawData Picker1 { get; set; }  // optional
        protected DrawData Picker2{ get; set; }   // optional
        protected DrawData Overview { get; set; } // optional, but usually points to Editor

        public ITreeModel FlyTreeModel { get; protected set; }
        public ITreeModel SideTreeModel { get; protected set; }

        public ushort UndoCount { get; protected set; }
        public ushort RedoCount { get; protected set; }

        public bool HasUndoRedo { get; protected set; }
        public bool HasApplyRevert { get; protected set; }

        public byte EditorDelta { get; protected set; } = 1;
        public byte Picker1Delta { get; protected set; } = 1;
        public byte Picker2Delta { get; protected set; } = 1;
        public byte ToolTipDelta { get; protected set; } = 1;
        public byte FlyTreeDelta { get; protected set; } = 1;
        public byte SideTreeDelta { get; protected set; } = 1;
        public byte OverviewDelta { get; protected set; } = 1;
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

