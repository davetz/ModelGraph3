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
        public void DrawControlReady() => PageModel.Root.PostRefresh();
        #endregion

        #region ModeStateCursor  ==============================================
        private Dictionary<(byte, byte), DrawCursor> _modeState_Cursor = new Dictionary<(byte, byte), DrawCursor>();
        public DrawCursor GetModeStateCursor() => _modeState_Cursor.TryGetValue((_drawMode, _drawState), out DrawCursor cur) ? cur : DrawCursor.Arrow;
        protected void SetModeStateCursor(byte mode, byte state, DrawCursor cursor) => _modeState_Cursor[(mode, state)] = cursor;
        #endregion

        #region ModeStateNames  ===============================================
        //each target model can define its own modes and states enums
        //under the covers, the mode and state are just treated as numeric values

        protected byte _drawMode = 0; //numeric value of the current mode, 
        protected byte _drawState = 0; //numeric value of the current state

        //the mode and state values expressed as strings for debugging
        private readonly Dictionary<byte, string> _modeNames = new Dictionary<byte, string>();
        private readonly Dictionary<byte, string> _stateNames = new Dictionary<byte, string>();

        protected void SetModeNames(Type drawModeEnum)
        {
            foreach (byte key in Enum.GetValues(drawModeEnum))
            {
                _modeNames[key] = Enum.GetName(drawModeEnum, key);
            }
        }
        protected void SetStateNames(Type drawStateEnum)
        {
            foreach (byte key in Enum.GetValues(drawStateEnum))
            {
                _stateNames[key] = Enum.GetName(drawStateEnum, key);
            }
        }
        #endregion

        #region UI drawMode ===================================================
        //the draw mode index is used to select an idKey for localized strings
        virtual public List<IdKey> GetModeIdKeys() => new List<IdKey>(0); //specified by the target model

        //the UI can get and set the draw mode
        public byte ModeIndex { get => _drawMode; set => TransitionModeState(value); }
        private void TransitionModeState(byte mode)
        {
            var prevState = _drawState;
            SetModeState(mode, 0, true); //fake the effect of a pointer move
            SetModeState(mode, prevState, true);
        }

        // the target model sets the draw state
        protected void SetState(byte state) => SetModeState(_drawMode, state, true);

        private void SetModeState(byte mode, byte state, bool execute = false)
        {
            if (mode != _drawMode || state != _drawState)
            {
                _drawMode = mode;
                _drawState = state;
                var modeName = _modeNames.TryGetValue(mode, out string vMode) ? vMode : $"#{_drawMode}";
                var stateName = _stateNames.TryGetValue(state, out string vState) ? vState : $"#{_drawState}";
                var executing = "no action";
                if (execute && TryGetEventAction((DrawEvent.Pseudo), out Action action))
                {
                    executing = "executing action";
                    action();
                }
                Debug.WriteLine($"Drawing mode state:  {modeName} {stateName} - {executing}");
            }
        }
        #endregion

        #region UI drawEvent  =================================================
        //there are 3 posible ways to envoke an drawEvent -> action
        private enum EventDependancy : byte { Event, ModeEvent, ModeStateEvent };
        private readonly Dictionary<DrawEvent, EventDependancy> _event_Dependancy = new Dictionary<DrawEvent, EventDependancy>();

        private readonly Dictionary<DrawEvent, Action> _event_Action = new Dictionary<DrawEvent, Action>();
        private readonly Dictionary<(byte, DrawEvent), Action> _modeEvent_Action = new Dictionary<(byte, DrawEvent), Action>();
        private readonly Dictionary<(byte, byte, DrawEvent), Action> _modeStateEvent_Action = new Dictionary<(byte, byte, DrawEvent), Action>();

        public bool TryGetEventAction(DrawEvent evt, out Action act)
        {
            if (_event_Dependancy.TryGetValue(evt, out EventDependancy val))
            {
                switch (val)
                {
                    case EventDependancy.ModeStateEvent:
                        return _modeStateEvent_Action.TryGetValue((_drawMode, _drawState, evt), out act);
                    case EventDependancy.ModeEvent:
                        return _modeEvent_Action.TryGetValue((_drawMode, evt), out act);
                    default:
                        return _event_Action.TryGetValue(evt, out act);
                }
            }
            act = null;
            return false;
        }

        //the target model populates the modeStateEvent_Action dictionaries
        protected void SetEventAction(DrawEvent evt, Action act)
        {
            _event_Dependancy[evt] = EventDependancy.Event;
            _event_Action[evt] = act;
        }
        protected void SetModeEventAction(byte mode, DrawEvent evt, Action act)
        {
            _event_Dependancy[evt] = EventDependancy.ModeEvent;
            _modeEvent_Action[(mode, evt)] = act;
        }
        protected void SetModeStateEventAction(byte mode, byte state, DrawEvent evt, Action act)
        {
            _event_Dependancy[evt] = EventDependancy.ModeStateEvent;
            _modeStateEvent_Action[(mode, state, evt)] = act;
        }
        #endregion

        #region IDrawData  ====================================================
        virtual public string HeaderTitle => "No Title was specified";

        public Vector2 FlyOutSize { get; protected set; }
        public Vector2 FlyOutPoint { get; protected set; }
        public string ToolTip_Text1 { get; protected set; }
        public string ToolTip_Text2 { get; protected set; }

        public ushort EditorWidth {get; protected set;}

        public ushort Picker1Width { get; protected set; }

        public ushort Picker2Width { get; protected set; }

        public ushort OverviewWidth { get; protected set; }

        public ushort SideTreeWidth { get; protected set; }

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

        virtual public int UndoCount => 0;
        virtual public int RedoCount => 0;

        public bool HasUndoRedo { get; protected set; }
        public bool HasApplyRevert { get; protected set; }
        public bool HasColorPicker { get; protected set; }

        public byte EditorDelta { get; protected set; } = 1;
        public byte Picker1Delta { get; protected set; } = 1;
        public byte Picker2Delta { get; protected set; } = 1;
        public byte ToolTipDelta { get; protected set; } = 1;
        public byte FlyTreeDelta { get; protected set; } = 1;
        public byte SideTreeDelta { get; protected set; } = 1;
        public byte OverviewDelta { get; protected set; } = 1;

        //ui control visibility
        public bool ToolTipIsVisible { get; protected set; }
        public bool Picker1IsVisible { get; protected set; }
        public bool Picker2IsVisible { get; protected set; }
        public bool SelectorIsVisible { get; protected set; }
        public bool FlyTreeIsVisible { get; protected set; }
        public bool SideTreeIsVisible { get; protected set; }
        public bool OverviewIsVisible { get; set; }
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

