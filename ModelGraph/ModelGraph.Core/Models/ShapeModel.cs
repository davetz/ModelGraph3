using System;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public class ShapeModel : DrawModel
    {
        internal readonly SymbolX Symbol;
        private const float EditRadius = 256;   //width, height of shape in the editor
        private const float EditMargin = 32;    //size of empty space arround the shape editor 
        private const float EditExtent = EditRadius + EditMargin;
        private const int EditSize = (int)(2 * EditExtent);
        private readonly float AbsoluteSize;

        #region DrawMode, DrawState  ==========================================
        // Name the drawing modes and states for this model
        public override List<IdKey> GetModeIdKeys() => new List<IdKey>()
        {
            IdKey.ViewMode,
            IdKey.EditMode,
            IdKey.MoveMode,
            IdKey.CopyMode,
            IdKey.PasteMode,
            IdKey.DeleteMode,
            IdKey.ReshapeMode,
            IdKey.TerminalMode,
            IdKey.FlipRotateMode,
        };
        private enum DrawMode : byte
        {
            View,
            Edit,
            Move,
            Copy,
            Paste,
            Delete,
            Reshape,
            Terminal,
            FlipRotate,
        }
        private DrawMode Mode { get => (DrawMode)_drawMode; set => ModeIndex = (byte)value; }
        private enum DrawState : byte
        {
            OnVoid, //pointer is on an empty space

            OnShape,
            OnLine,
            OnPin,
        }
        #endregion

        #region SetModeStateActions  ==========================================
        private void SetModeStateEventActions()
        {
            SetEventAction(DrawEvent.Apply, ApplyChange);
            SetEventAction(DrawEvent.Revert, Revert);
            SetEventAction(DrawEvent.Picker1Tap, () => { Picker1Tap(); });
            SetEventAction(DrawEvent.Picker2Tap, () => { Picker2Tap(); });
            SetEventAction(DrawEvent.Picker1CtrlTap, () => { Picker1Tap(true); });
            SetModeStateEventAction((byte)DrawMode.View, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Edit, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Copy, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Paste, (byte)DrawState.OnVoid, DrawEvent.Tap, CloneAction);
            SetModeStateEventAction((byte)DrawMode.Delete, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Reshape, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Terminal, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.FlipRotate, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
        }
        #endregion

        #region SetModeStateCursors  ==========================================
        private void SetModeStateCursors()
        {
            SetModeStateCursor((byte)DrawMode.Move, (byte)DrawState.OnVoid, DrawCursor.SizeAll);
            SetModeStateCursor((byte)DrawMode.Paste, (byte)DrawState.OnVoid, DrawCursor.New);
            SetModeStateCursor((byte)DrawMode.Reshape, (byte)DrawState.OnPin, DrawCursor.SizeAll);
        }
        #endregion

        #region Constructor  ==================================================
        internal ShapeModel(PageModel owner, Root root, SymbolX symbol) : base(owner)
        {
            Symbol = symbol;
            AbsoluteSize = symbol.AbsoluteSize;

            Overview = Editor;
            OverviewWidth = 40;

            EditorWidth = EditSize;
            BackLay = new DrawData();

            Picker1 = new DrawData();
            Picker1Width = 40;

            Picker2 = new DrawData();
            Picker2Width = 40;

            SideTreeModel = new TreeModel(PageModel, (m) => { new Model_601_Shape(m, this); });
            SideTreeWidth = 100;

            SetModeNames(typeof(DrawMode));
            SetStateNames(typeof(DrawState));
            SetModeStateEventActions();
            SetModeStateCursors();

            HasApplyRevert = true;
            HasColorPicker = true;

            Editor.GetExtent = () => new Extent(-EditExtent, -EditExtent, EditExtent, EditExtent);
            Picker1.GetExtent = () => new Extent(-16, 0, 16, 0);
            Picker2.GetExtent = () => new Extent(-16, 0, 16, 0);

            foreach (var s in _picker2Shapes) { _templateShapes.Add(s.Clone()); }

            Picker1IsVisible = Picker2IsVisible = OverviewIsVisible = SideTreeIsVisible = true;

            SetViewMode();
            InititalizeBackData();
            Refresh();
        }
        #endregion

        #region ModelCommands  ================================================
        internal override void Cut()
        {
            _shapeClipboard.Clear();
            _shapeClipboard.AddRange(SelectedShapes);
            var shapes = Symbol.GetShapes();
            foreach (var s in SelectedShapes)
            {
                shapes.Remove(s);
            }
            Refresh();
        }
        internal override void Copy()
        {
            _shapeClipboard.Clear();
            _shapeClipboard.AddRange(SelectedShapes);
            Refresh();
        }
        private List<Shape> _shapeClipboard = new List<Shape>(10);
        #endregion

        #region ShapeProperty  ================================================
        private void GetProperties()
        {
            _propertyFlags = Shape.GetProperties(this, SelectedShapes);

            SideTreeDelta++;
            (SideTreeModel as TreeModel).Validate();
            Refresh();
        }
        internal bool IsAugmentedPropertyList;
        internal ShapeProperty PropertyFlags { get => _propertyFlags; }
        private ShapeProperty _propertyFlags;

        internal CapStyle EndCap { get => _endCap; set => Set(ref _endCap, value, ShapeProperty.EndCap); }
        internal void InitEndCap(CapStyle value) => _endCap = value;
        private CapStyle _endCap;

        internal CapStyle DashCap { get => _dashCap; set => Set(ref _dashCap, value, ShapeProperty.DashCap); }
        internal void InitDashCap(CapStyle value) => _dashCap = value;
        private CapStyle _dashCap;

        internal CapStyle StartCap { get => _startCap; set => Set(ref _startCap, value, ShapeProperty.StartCap); }
        internal void InitStartCap(CapStyle value) => _startCap = value;
        private CapStyle _startCap;

        internal StrokeStyle StrokeStyle { get => _strokeStyle; set => Set(ref _strokeStyle, value, ShapeProperty.StrokeStyle); }
        internal void InitStrokeStyle(StrokeStyle value) => _strokeStyle = value;
        private StrokeStyle _strokeStyle;

        internal byte StrokeWidth { get => _strokeWidth; set => Set(ref _strokeWidth, value, ShapeProperty.StrokeWidth); }
        internal void InitStrokeWidth(byte value) => _strokeWidth = value;
        private byte _strokeWidth = 1;


        internal byte Factor1 { get => _factor1; set => Set(ref _factor1, value, ShapeProperty.Factor1); }
        internal void InitFactor1(byte value) => _factor1 = value;
        private byte _factor1 = 25;

        internal byte Radius1 { get => _radius1; set => Set(ref _radius1, value, ShapeProperty.Radius1); }
        internal void InitRadius1(byte value) => _radius1 = value;
        private byte _radius1 = 25;

        internal byte Radius2 { get => _radius2; set => Set(ref _radius2, value, ShapeProperty.Radius2); }
        internal void InitRadius2(byte value) => _radius2 = value;
        private byte _radius2 = 25;

        internal byte Rotation1 { get => _rotation1; set => Set(ref _rotation1, value, ShapeProperty.Rotation1); }
        internal void InitRotation1(byte value) => _rotation1 = value;
        private byte _rotation1 = 0;

        internal byte Rotation2 { get => _rotation2; set => Set(ref _rotation2, value, ShapeProperty.Rotation2); }
        internal void InitRotation2(byte value) => _rotation2 = value;
        private byte _rotation2 = 0;

        internal byte Dimension { get => _dimension; set => Set(ref _dimension, value, ShapeProperty.Dimension); }
        internal void InitDimension(byte value) => _dimension = value;
        private byte _dimension = 3;

        internal bool IsImpaired { get => _isImpaired; set => Set(ref _isImpaired, value, ShapeProperty.IsImpaired); }
        internal void InitImpared(bool value) => _isImpaired = value;
        private bool _isImpaired = false;

        private short SCLAMP(short v) => v < LNS ? LNS : v > LPS ? LPS : v;
        private short SCLAMP(int v) => v < LNS ? LNS : v > LPS ? LPS : (short)v;
        const short LNS = -128;
        const short LPS = 128;

        private ushort USCLAMP(ushort v) => v > LPUS ? LPUS : v;
        private ushort USCLAMP(int v) => v < LNUS ? LNUS : v > LPUS ? LPUS : (ushort)v;
        const ushort LNUS = 0;
        const ushort LPUS = 256;

        internal ushort SizeX { get => _sizeX; set => Set(ref _prevSizeX, ref _sizeX, USCLAMP(value), ShapeProperty.SizeX); }
        internal void InitSizeX(ushort value) => _sizeX = value;
        internal float DeltaSizeX => USCLAMP(_centerX - _prevCenterX) / 256f;
        private ushort _sizeX = 0;
        private ushort _prevSizeX = 0;

        internal ushort SizeY { get => _sizeY; set => Set(ref _prevSizeY, ref _sizeY, USCLAMP(value), ShapeProperty.SizeY); }
        internal void InitSizeY(ushort value) => _sizeY = value;
        internal float DeltaSizeY => USCLAMP(_sizeY - _prevSizeY) / 256f;
        private ushort _sizeY = 0;
        private ushort _prevSizeY = 0;

        internal short CenterX { get => _centerX; set => Set(ref _prevCenterX, ref _centerX, SCLAMP(value), ShapeProperty.CenterX); }
        internal void InitCenterX(short value) => _centerX = value;
        internal float DeltaCenterX => SCLAMP(_centerX - _prevCenterX) / 128f;
        private short _centerX = 0;
        private short _prevCenterX = 0;

        internal short CenterY { get => _centerY; set => Set(ref _prevCenterY, ref _centerY, SCLAMP(value), ShapeProperty.CenterY); }
        internal void InitCenterY(short value) => _centerY = value;
        internal float DeltaCenterY => SCLAMP(_centerY - _prevCenterY) / 128f;
        private short _centerY = 0;
        private short _prevCenterY = 0;

        internal short ExtentEast { get => _extentEast; set => Set(ref _prevExtentEast, ref _extentEast, SCLAMP(value), ShapeProperty.ExtentEast); }
        internal void InitExtentEast(short value) => _extentEast = value;
        internal float DeltaExtentEast => SCLAMP(_extentEast - _prevExtentEast) / 128f;
        private short _extentEast = 0;
        private short _prevExtentEast = 0;

        internal short ExtentWest { get => _extentWest; set => Set(ref _prevExtentWest, ref _extentWest, SCLAMP(value), ShapeProperty.ExtentWest); }
        internal void InitExtentWest(short value) => _extentWest = value;
        internal float DeltaExtentWest => SCLAMP(_extentWest - _prevExtentWest) / 128f;
        private short _extentWest = 0;
        private short _prevExtentWest = 0;

        internal short ExtentNorth { get => _extentNorth; set => Set(ref _prevExtentNorth, ref _extentNorth, SCLAMP(value), ShapeProperty.ExtentNorth); }
        internal void InitExtentNorth(short value) => _extentNorth = value;
        internal float DeltaExtentNorth => SCLAMP(_extentNorth - _prevExtentNorth) / 128f;
        private short _extentNorth = 0;
        private short _prevExtentNorth = 0;

        internal short ExtentSouth { get => _extentSouth; set => Set(ref _prevExtentSouth, ref _extentSouth, SCLAMP(value), ShapeProperty.ExtentSouth); }
        internal void InitExtentSouth(short value) => _extentSouth = value;
        internal float DeltaExtentSouth => SCLAMP(_extentSouth - _prevExtentSouth) / 128f; 
        private short _extentSouth = 0;
        private short _prevExtentSouth = 0;

        private void Set<T>(ref T previous, ref T storage, T value, ShapeProperty sp)
        {
            previous = storage;
            Set(ref storage, value, sp);
        }
        private void Set<T>(ref T storage, T value, ShapeProperty sp)
        {
            if (Equals(storage, value)) return;
            storage = value;
            Shape.SetProperty(this, sp, SelectedShapes);
            GetProperties();
        }
        #endregion

        #region Refresh  ======================================================
        private void Refresh(bool triggerUIRefresh = true)
        {
            RefreshEditorData();
            RefreshPicker1Data();
            RefreshPicker2Data();
            if (triggerUIRefresh) PageModel.TriggerUIRefresh();
        }
        #endregion

        #region Picker1,Picker2,Editor  =======================================
        #region Picker1  ======================================================
        void RefreshPicker1Data()
        {
            var w = Picker1Data.Extent.Width;
            var r = w / 2;
            var c = new Vector2(0, 0);
            var a = (float)Symbol.AbsoluteSize; //needed to calculate the stroke width
            var d = -r;
            Picker1.Clear();
            Picker1Delta++;
            foreach (var s in Symbol.GetShapes())
            {
                s.AddDrawData(Picker1, a, r, c);
                if (SelectedShapes.Contains(s))
                {
                    var points = new Vector2[] { c, new Vector2(r, r) };
                    Picker1.AddParms((points, (ShapeType.CenterRect, StrokeType.Filled, 0), (90, 255, 255, 255)));
                }
                c = new Vector2(0, c.Y + w);
                DrawSeperator();
            }
            void DrawSeperator()
            {
                d += w;
                var p = new Vector2[] { new Vector2(-r, d), new Vector2(r, d) };
                Picker1.AddParms((p, (ShapeType.Line, StrokeType.Simple, 1), (90, 255, 255, 255)));
            }
        }
        private void Picker1Tap(bool add = false)
        {
            _picker2Index = -1;
            _picker1Index = (int)(0.5f + Picker1Data.Point1.Y / Picker1Data.Extent.Width);
            if (Picker1IsValid)
            {
                var s = Symbol.GetShapes()[_picker1Index];
                if (add)
                {
                    if (SelectedShapes.Contains(s))
                    {
                        SelectedShapes.Remove(s);
                        if (SelectedShapes.Count == 0) SetViewMode();
                    }
                    else
                        SelectedShapes.Add(s);
                }
                else
                {
                    SelectedShapes.Clear();
                    SelectedShapes.Add(s);
                }
            }
            else
            {
                SelectedShapes.Clear();
            }
            IsAugmentedPropertyList = SelectedShapes.Count > 0;
            GetProperties();
            if (Mode == DrawMode.Reshape) return;
            if (SelectedShapes.Count == 0) SetViewMode();
            Mode = DrawMode.Edit;
            Refresh(true);
        }
        private bool Picker1IsValid => _picker1Index > -1 && _picker1Index < Symbol.GetShapes().Count;
        private int _picker1Index;
        #endregion

        #region Picker2  ======================================================
        void RefreshPicker2Data()
        {
            var w = Picker2Data.Extent.Width;
            var r = w / 2;
            var c = new Vector2(0, 0);
            var a = (float)Symbol.AbsoluteSize; //needed to calculate the stroke width
            var d = -r;
            Picker2.Clear();
            Picker2Delta++;

            foreach (var s in _shapeClipboard)
            {
                s.AddDrawData(Picker2, a, r, c);
                if (_picker2Index == 0)
                {
                    var points = new Vector2[] { c, new Vector2(r, r) };
                    Picker2.AddParms((points, (ShapeType.CenterRect, StrokeType.Filled, 0), (90, 255, 255, 255)));
                }
            }
            DrawSeperator();

            c = new Vector2(0, c.Y + w);
            foreach (var s in _picker2Shapes)
            {
                s.AddDrawData(Picker2, a, r, c);
                DrawSeperator();
                if (Picker2IsValid && s == _picker2Shapes[_picker2Index - 1])
                {
                    var points = new Vector2[] { c, new Vector2(r, r) };
                    Picker2.AddParms((points, (ShapeType.CenterRect, StrokeType.Filled, 0), (90, 255, 255, 255)));
                }
                c = new Vector2(0, c.Y + w);
                
            }
            void DrawSeperator()
            {
                d += w;
                var p = new Vector2[] { new Vector2(-r, d), new Vector2(r, d) };
                Picker2.AddParms((p, (ShapeType.Line, StrokeType.Simple, 1), (90, 255, 255, 255)));
            }
        }
        private void Picker2Tap()
        {
            _picker1Index = -1;
            SelectedShapes.Clear();
            IsAugmentedPropertyList = false;

            _picker2Index = (int)(0.5f + Picker2Data.Point1.Y / Picker2Data.Extent.Width);
            if (Picker2IsValid)
            {
                SelectedShapes.Add(_templateShapes[_picker2Index - 1]);
            }
            GetProperties();

            if ((_picker2Index == 0 && _shapeClipboard.Count > 0) || Picker2IsValid)
                Mode = DrawMode.Paste;
            else
                Mode = DrawMode.View;
        }
        private bool Picker2IsValid => _picker2Index > 0 && _picker2Index <= _picker2Shapes.Length;
        private int _picker2Index;
        private readonly List<Shape> _templateShapes = new List<Shape>(_picker2Shapes.Length);
        private static readonly Shape[] _picker2Shapes =
        {
            new Line(),
        };
        #endregion

        #region Editor  =======================================================
        void RefreshEditorData()
        {
            var r = EditRadius;
            var c = new Vector2();
            var a = AbsoluteSize;

            Editor.Clear();
            EditorDelta++;
            var shapes = Symbol.GetShapes();
            var coloring = Shape.Coloring.Normal;
            if (Mode == DrawMode.Reshape && SelectedShapes.Count == 1)
            {
                coloring = Shape.Coloring.Light;
                Shape.AddDrawTargets(SelectedShapes[0], _pinTargets, Editor, r, c);
            }

            foreach (var s in shapes) { s.AddDrawData(Editor, a, r, c, coloring); }

            if (Mode == DrawMode.Reshape && SelectedShapes.Count == 1)
            {
                Shape.AddDrawTargets(SelectedShapes[0], _pinTargets, Editor, r, c);
            }
            else if (Mode == DrawMode.Edit && _hitSelecteShapes)
            {
                Editor.AddParms((Shape.GetHitExtent(r, c, SelectedShapes), (ShapeType.CenterRect, StrokeType.Filled, 0), (80, 255, 200, 255)));
            }
        }
        #endregion

        #region BackData  =====================================================
        private void InititalizeBackData()
        {
            var r = EditRadius;
            var d = (float)(r * Math.Sin(Math.PI / 8)); //22.5 degree displacement
            float L = -r, R = r, T = -r, B = r;         //Left, Right, Top, Bottom

            var points1 = new List<Vector2>()   // init with major tick marks 
            {
                new Vector2(L, 0), new Vector2(R, 0),   //X,Y axis
                new Vector2(0, T), new Vector2(0, B),
                new Vector2(L, T), new Vector2(R, B),   //diagonnals
                new Vector2(L, B), new Vector2(R, T)
            };
            var points2 = new List<Vector2>()   // init with minor tick marks 
            {
                new Vector2(L, -d), new Vector2(R, d),  //diagonnals
                new Vector2(L, d), new Vector2(R, -d),
                new Vector2(-d, T), new Vector2(d, B),
                new Vector2(d, T), new Vector2(-d, B)
            };

            for (int i = 0; i < 8; i++)
            {
                var points = (i == 0 || i == 4) ? points1 : points2;
                AddTickMarks(r - i * r / 8f);

                void AddTickMarks(float v)
                {
                    //- - - - - - - - - - - - - - - -Vertical
                    points.Add(new Vector2(-v, T));
                    points.Add(new Vector2(-v, B));

                    points.Add(new Vector2(v, T));
                    points.Add(new Vector2(v, B));

                    //- - - - - - - - - - - - - - - -Horizontal
                    points.Add(new Vector2(L, -v));
                    points.Add(new Vector2(R, -v));

                    points.Add(new Vector2(L, v));
                    points.Add(new Vector2(R, v));
                }
            }

            (byte, byte, byte, byte) color1 = (0xff, 0xff, 0xff, 0xff);
            (byte, byte, byte, byte) color2 = (0x80, 0xff, 0xff, 0x00);

            BackLay.Clear();
            BackLay.AddParms((points1.ToArray(), (ShapeType.Line, StrokeType.Simple, 1), color1));
            BackLay.AddParms((points2.ToArray(), (ShapeType.Line, StrokeType.Simple, 1), color2));
            BackLay.AddParms((new Vector2[] { new Vector2(0, 0), new Vector2(r / 2, r / 2) }, (ShapeType.Circle, StrokeType.Simple, 1), color2));
            BackLay.AddParms((new Vector2[] { new Vector2(0, 0), new Vector2(r, r) }, (ShapeType.Circle, StrokeType.Simple, 1), color1));

            var yN = -r - 32;
            var x1 = -r - 16;
            var x2 = -r / 2 - 8;
            var x3 = -6f;
            var x4 = r / 2 - 8;
            var x5 = r - 16;
            BackLay.AddText(((new Vector2(x1, yN), "nwc"), color2));
            BackLay.AddText(((new Vector2(x2, yN), "nw"), color2));
            BackLay.AddText(((new Vector2(x3, yN), "N"), color1));
            BackLay.AddText(((new Vector2(x4, yN), "ne"), color2));
            BackLay.AddText(((new Vector2(x5, yN), "nec"), color2));

            var yS = r + 2;
            BackLay.AddText(((new Vector2(x1, yS), "swc"), color2));
            BackLay.AddText(((new Vector2(x2, yS), "sw"), color2));
            BackLay.AddText(((new Vector2(x3, yS), "S"), color1));
            BackLay.AddText(((new Vector2(x4, yS), "se"), color2));
            BackLay.AddText(((new Vector2(x5, yS), "sec"), color2));

            var y2 = -r / 2 - 18;
            var y3 = -16f;
            var y4 = r / 2 - 16;
            var x6 = -r - 30;
            var x7 = -r - 24;
            BackLay.AddText(((new Vector2(x6, y2), "wn"), color2));
            BackLay.AddText(((new Vector2(x7, y3), "W"), color1));
            BackLay.AddText(((new Vector2(x6, y4), "ws"), color2));

            var x8 = r + 4;
            var x9 = r + 6;
            BackLay.AddText(((new Vector2(x8, y2), "en"), color2));
            BackLay.AddText(((new Vector2(x9, y3), "E"), color1));
            BackLay.AddText(((new Vector2(x8, y4), "es"), color2));
        }



        #endregion

        internal List<Shape> SelectedShapes = new List<Shape>();

        private void OverviewTap()
        {
            var shapes = Symbol.GetShapes();
            if (SelectedShapes.Count == shapes.Count)
                SetViewMode();
            _picker1Index = 0; //will make Picker1Valid true
            SelectedShapes.Clear();
            SelectedShapes.AddRange(shapes);
            GetProperties();
            Mode = DrawMode.Edit;
        }
        #endregion

        #region ColorARGB/Apply/Revert  =======================================
        protected override void ColorARGBChanged()
        {
            if (SelectedShapes.Count > 0)
            {
                foreach (var s in SelectedShapes)
                {
                    s.SetColor(ColorARGB);
                }
                Refresh(false);
            }
        }
        private void ApplyChange()
        {
            Symbol.SaveShapes();
            RestoreDrawState();
        }
        private void Revert()
        {
            Symbol.ReloadShapes();
            Refresh();
            RestoreDrawState();
            PageModel.TriggerUIRefresh();
        }
        private void RestoreDrawState()
        {
            //if ((PreviousDrawState & DrawState.ModeMask) == DrawState.LinkMode) SetViewMode();
            //if ((PreviousDrawState & DrawState.ModeMask) == DrawState.OperateMode) SetViewMode();
            SetViewMode();
        }
        #endregion

        #region SkimHitTest  ==================================================
        private void SkimHitTest()
        {
            var dp = EditData.Point2;
            //var mode = DrawState & DrawState.ModeMask;
            //if (mode == DrawState.PinsMode)
            //{
            //    var ds = 4; //hit zone
            //    var ex = new Extent(dp.X - ds, dp.Y - ds, dp.X + ds, dp.Y + ds);
            //    for (int i = 0; i < _pinTargets.Count; i++)
            //    {
            //        var p = _pinTargets[i];
            //        if (ex.Contains(p))
            //        {
            //            _pinIndex = i;
            //            ModifyDrawState(DrawState.NowOnPin, DrawState.NowMask | DrawState.EventMask);
            //            return;
            //        }
            //    }
            //}
            //else if (Picker1IsValid)
            //{
            //    var r = EditRadius;
            //    var c = new Vector2();
            //    _hitSelecteShapes = Shape.HitShapes(Editor.Point2, r, c, SelectedShapes);
            //    if (_hitSelecteShapes)
            //    {
            //        ModifyDrawState(DrawState.NowOnNode, DrawState.NowMask | DrawState.EventMask);
            //    }
            //}
            //ModifyDrawState(DrawState.NowOnVoid, DrawState.NowMask | DrawState.EventMask);
        }
        #endregion

        #region ViewMode == ViewSymbol  =======================================
        public void SetViewMode()
        {
            //SetDrawState(DrawState.ViewMode);
            //_picker1Index = _picker2Index = -1;
            //SelectedShapes.Clear();
            //SetProperties();
        }
        private void PasteAction()
        {
            var shapes = Symbol.GetShapes();
            foreach (var s in _shapeClipboard)
            {
                var ns = s.Clone();

                shapes.Add(ns);
                Refresh();
            }
        }
        #endregion

        #region EditMode == ModifySymbol  =====================================
        private void NudgeUp()
        {
            Shape.MoveCenter(SelectedShapes, new Vector2(0, -0.01f));
            RefreshEditorData();
            PageModel.TriggerUIRefresh();
        }
        private void NudgeDown()
        {
            Shape.MoveCenter(SelectedShapes, new Vector2(0, 0.01f));
            RefreshEditorData();
            PageModel.TriggerUIRefresh();
        }
        private void NudgeLeft()
        {
            Shape.MoveCenter(SelectedShapes, new Vector2(-0.01f, 0));
            RefreshEditorData();
            PageModel.TriggerUIRefresh();
        }
        private void NudgeRight()
        {
            Shape.MoveCenter(SelectedShapes, new Vector2(0.01f, 0));
            RefreshEditorData();
            PageModel.TriggerUIRefresh();
        }
        private bool _hitSelecteShapes;
        private void CenterAction()
        {
            Shape.SetCenter(SelectedShapes, new Vector2());
            Refresh();
        }
        #endregion

        #region MoveMode  =====================================================
        private void DragShapeAction()
        {
            var delta = EditData.PointDelta(true) / EditRadius;
            Shape.MoveCenter(SelectedShapes, delta);
            RefreshEditorData(); //fast responce
        }
        private void EndDragAction()
        {
            Refresh();
        }
        #endregion

        #region PinsMode  =====================================================
        private void DragPinAction()
        {
            var delta = EditData.PointDelta(true) / EditRadius;
            Shape.MovePoint(SelectedShapes[0], _pinIndex, delta, EditRadius);
            RefreshEditorData(); //fast responce
        }
        private void NudgePinUp()
        {
            Shape.MovePoint(SelectedShapes[0], _pinIndex, new Vector2(0, -0.01f), 1);
            RefreshEditorData();
            PageModel.TriggerUIRefresh();
        }
        private void NudgePinDown()
        {
            Shape.MovePoint(SelectedShapes[0], _pinIndex, new Vector2(0, 0.01f), 1);
            RefreshEditorData();
            PageModel.TriggerUIRefresh();
        }
        private void NudgePinLeft()
        {
            Shape.MovePoint(SelectedShapes[0], _pinIndex, new Vector2(-0.01f, 0), 1);
            RefreshEditorData();
            PageModel.TriggerUIRefresh();
        }
        private void NudgePinRight()
        {
            Shape.MovePoint(SelectedShapes[0], _pinIndex, new Vector2(0.01f, 0), 1);
            RefreshEditorData();
            PageModel.TriggerUIRefresh();
        }
        int _pinIndex;
        #endregion

        #region CreateMode == AddSymbolShape  =================================
        private void CloneAction()
        {
            var cp = EditData.Point1 / EditRadius;
            if (_picker2Index == 0 && _shapeClipboard.Count > 0)
            {
                var newshapes = Shape.Clone(_shapeClipboard, cp);
                
                foreach (var ns in newshapes)
                {
                    Symbol.GetShapes().Add(ns);
                }
                Refresh();
            }
            else if (Picker2IsValid)
            {
                var ns = _templateShapes[_picker2Index - 1].Clone(cp);
                Symbol.GetShapes().Add(ns);
                Refresh();
            }
            else
                SetViewMode();
        }
        #endregion

        #region LinkMode == DefineTerminals  ==================================
        List<Vector2> _pinTargets = new List<Vector2>();
        #endregion

        #region OperateMode == AutoFlipRotate  ================================
        #endregion
    }
}

