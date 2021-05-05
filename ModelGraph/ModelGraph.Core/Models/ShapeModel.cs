﻿using System;
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
        internal ShapeSelector Selector;

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
        private enum DrawState : byte
        {
            OnVoid, //pointer is on an empty space on the drawing

            OnShape,
            OnLine,
            OnLinePoint,
        }
        #endregion

        #region SetModeStateActions  ==========================================
        private void SetModeStateEventActions()
        {
            SetModeStateEventAction((byte)DrawMode.View, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Edit, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Copy, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Paste, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Delete, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Reshape, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Terminal, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.FlipRotate, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
        }
        #endregion

        #region SetModeStateCursors  ==========================================
        private void SetModeStateCursors()
        {
            SetModeStateCursor((byte)DrawMode.Edit, (byte)DrawState.OnVoid, DrawCursor.Aim);
            SetModeStateCursor((byte)DrawMode.Move, (byte)DrawState.OnVoid, DrawCursor.Aim);
            SetModeStateCursor((byte)DrawMode.Copy, (byte)DrawState.OnVoid, DrawCursor.Aim);
            SetModeStateCursor((byte)DrawMode.Paste, (byte)DrawState.OnVoid, DrawCursor.New);
            SetModeStateCursor((byte)DrawMode.Reshape, (byte)DrawState.OnVoid, DrawCursor.Aim);
        }
        #endregion

        #region Constructor  ==================================================
        internal ShapeModel(PageModel owner, Root root, SymbolX symbol) : base(owner)
        {
            Symbol = symbol;
            AbsoluteSize = symbol.AbsoluteSize;
            Selector = new ShapeSelector(this);

            SetDrawConfig(DrawItem.Editor, EditSize, SizeType.Fixed);
            SetDrawConfig(DrawItem.Picker1, 40, SizeType.Fixed);
            SetDrawConfig(DrawItem.Picker2, 40, SizeType.Fixed);
            SetDrawConfig(DrawItem.Overview, 40, SizeType.Fixed);
            SetDrawConfig(DrawItem.SideTree, 100, SizeType.Variable);

            SetModeNames(typeof(DrawMode));
            SetStateNames(typeof(DrawState));
            SetModeStateEventActions();
            SetModeStateCursors();

            Editor.GetExtent = () => new Extent(-EditExtent, -EditExtent, EditExtent, EditExtent);
            Picker1.GetExtent = () => new Extent(-16, 0, 16, 0);
            Picker2.GetExtent = () => new Extent(-16, 0, 16, 0);
            SideTreeModel = new TreeModel(PageModel, (m) => { new Model_601_Shape(m, this); });

            foreach (var s in _picker2Shapes) { _templateShapes.Add(s.Clone()); }

            VisibleDrawItems = DrawItem.Picker1 | DrawItem.Picker2 | DrawItem.Overview | DrawItem.ColorPicker | DrawItem.SideTree;

            SetViewMode();
            RefreshHelperData();
            Refresh();
        }
        #endregion

        #region ShapeProperty  ================================================

        private void SetProperties()
        {
            Shape.GetStrokeProperty(SelectedShapes, ref _propertyFlags, ref _lineWidth, ref _lineStyle, ref _startCap, ref _dashCap, ref _endCap, ref _colorARGB);
            Shape.GetSizerProperty(SelectedShapes, ref _polyLocked, ref _min, ref _max, ref _dimension, ref _auxAxis, ref _radius1, ref _radius2, ref _size, ref _vSize, ref _hSize);
            SideTreeDelta++;
            (SideTreeModel as TreeModel).Validate();
            Refresh();
        }
        private byte _min;
        private byte _max;

        internal ShapeProperty PropertyFlags { get => _propertyFlags; }
        private ShapeProperty _propertyFlags;

        internal CapStyle EndCap { get => _endCap; set => Set(ref _endCap, value, ShapeProperty.EndCap); }
        private CapStyle _endCap;

        internal CapStyle DashCap { get => _dashCap; set => Set(ref _dashCap, value, ShapeProperty.DashCap); }
        private CapStyle _dashCap;

        internal CapStyle StartCap { get => _startCap; set => Set(ref _startCap, value, ShapeProperty.StartCap); }
        private CapStyle _startCap;

        internal StrokeStyle LineStyle { get => _lineStyle; set => Set(ref _lineStyle, value, ShapeProperty.LineStyle); }
        private StrokeStyle _lineStyle;

        internal byte LineWidth { get => _lineWidth; set => Set(ref _lineWidth, value, ShapeProperty.LineWidth); }
        private byte _lineWidth = 2;

        internal byte AuxAxis { get => _auxAxis; set => Set(ref _auxAxis, value, ShapeProperty.Aux); }
        private byte _auxAxis = 25;

        internal byte Size { get => _size; set => Set(ref _size, value, ShapeProperty.Size); }
        private byte _size = 25;

        internal byte VSize { get => _vSize; set => Set(ref _vSize, value, ShapeProperty.Vert); }
        private byte _vSize = 25;

        internal byte HSize { get => _hSize; set => Set(ref _hSize, value, ShapeProperty.Horz); }
        private byte _hSize = 25;

        internal byte Radius1 { get => _radius1; set => Set(ref _radius1, value, ShapeProperty.Rad1); }
        private byte _radius1 = 25;

        internal byte Radius2 { get => _radius2; set => Set(ref _radius2, value, ShapeProperty.Rad2); }
        private byte _radius2 = 25;

        internal byte Dimension { get => _dimension; set => Set(ref _dimension, value, ShapeProperty.Dim); }
        private byte _dimension = 3;
        internal bool PolyLocked { get => _polyLocked; set => Set(ref _polyLocked, value, ShapeProperty.PolyLocked); }
        private bool _polyLocked = false;

        private void Set<T>(ref T storage, T value, ShapeProperty sp)
        {
            if (Equals(storage, value)) return;
            storage = value;
            Shape.SetProperty(this, sp, SelectedShapes);
            SetProperties();
        }
        #endregion

        #region Refresh  ======================================================
        private void Refresh(bool triggerUIRefresh = true)
        {
            RefreshEditorData();
            RefreshPicker1Data();
            RefreshPicker2Data();
            if (triggerUIRefresh) PageModel.TriggerUIRefresh();

            #region InternalMethods  ==========================================
            void RefreshPicker1Data()
            {
                var r = Picker1.Extent.Width / 2;
                var c = new Vector2(0, 0);
                var a = (float)Symbol.AbsoluteSize;
                Picker1.Clear();
                foreach (var s in Symbol.GetShapes())
                {
                    s.AddDrawData(Picker1, a, r, c);
                    if (SelectedShapes.Contains(s))
                    {
                        var points = new Vector2[] { c, new Vector2(r, r) };
                        Picker1.AddParms((points, (ShapeType.CenterRect, StrokeType.Filled, 0), (90, 255, 255, 255)));
                    }
                    c = new Vector2(0, c.Y + Picker1.Extent.Width);
                }
            }
            void RefreshPicker2Data()
            {
                var r = Picker2.Extent.Width / 2;
                var c = new Vector2(0, 0);
                var a = (float)Symbol.AbsoluteSize; //needed to calculate the stroke width

                Picker2.Clear();
                foreach (var s in _picker2Shapes)
                {
                    s.AddDrawData(Picker2, a, r, c);
                    if (Picker2IsValid && s == _picker2Shapes[_picker2Index])
                    {
                        var points = new Vector2[] { c, new Vector2(r, r) };
                        Picker2.AddParms((points, (ShapeType.CenterRect, StrokeType.Filled, 0), (90, 255, 255, 255)));
                    }
                    c = new Vector2(0, c.Y + Picker2.Extent.Width);
                }
            }
            #endregion
        }

        #region RefreshEditorData  ============================================
        void RefreshEditorData()
        {
            var r = EditRadius;
            var c = new Vector2();
            var a = AbsoluteSize;

            Editor.Clear();
            var shapes = Symbol.GetShapes();
            var coloring = Shape.Coloring.Normal;
            //if (DrawState == DrawState.PinsMode && SelectedShapes.Count == 1)
            //{
            //    coloring = Shape.Coloring.Light;
            //    Shape.AddDrawTargets(SelectedShapes[0], _pinTargets, Editor, r, c);
            //}

            //foreach (var s in shapes) { s.AddDrawData(Editor, a, r, c, coloring); }

            //if (DrawState == DrawState.PinsMode && SelectedShapes.Count == 1)
            //{
            //    Shape.AddDrawTargets(SelectedShapes[0], _pinTargets, Editor, r, c);
            //}
            //else if (DrawState == DrawState.EditMode && _hitSelecteShapes)
            //{
            //    Editor.AddParms((Shape.GetHitExtent(r, c, SelectedShapes), (ShapeType.CenterRect, StrokeType.Filled, 0), (80, 255, 200, 255)));
            //}
        }
        #endregion

        #region RefreshHelperData  ============================================
        private void RefreshHelperData()
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

            Helper.Clear();
            Helper.AddParms((points1.ToArray(), (ShapeType.Line, StrokeType.Simple, 1), color1));
            Helper.AddParms((points2.ToArray(), (ShapeType.Line, StrokeType.Simple, 1), color2));
            Helper.AddParms((new Vector2[] { new Vector2(0, 0), new Vector2(r / 2, r / 2) }, (ShapeType.Circle, StrokeType.Simple, 1), color2));
            Helper.AddParms((new Vector2[] { new Vector2(0, 0), new Vector2(r, r) }, (ShapeType.Circle, StrokeType.Simple, 1), color1));

            var yN = -r - 32;
            var x1 = -r - 16;
            var x2 = -r / 2 - 8;
            var x3 = -6f;
            var x4 = r / 2 - 8;
            var x5 = r - 16;
            Helper.AddText(((new Vector2(x1, yN), "nwc"), color2));
            Helper.AddText(((new Vector2(x2, yN), "nw"), color2));
            Helper.AddText(((new Vector2(x3, yN), "N"), color1));
            Helper.AddText(((new Vector2(x4, yN), "ne"), color2));
            Helper.AddText(((new Vector2(x5, yN), "nec"), color2));

            var yS = r + 2;
            Helper.AddText(((new Vector2(x1, yS), "swc"), color2));
            Helper.AddText(((new Vector2(x2, yS), "sw"), color2));
            Helper.AddText(((new Vector2(x3, yS), "S"), color1));
            Helper.AddText(((new Vector2(x4, yS), "se"), color2));
            Helper.AddText(((new Vector2(x5, yS), "sec"), color2));

            var y2 = -r / 2 - 18;
            var y3 = -16f;
            var y4 = r / 2 - 16;
            var x6 = -r - 30;
            var x7 = -r - 24;
            Helper.AddText(((new Vector2(x6, y2), "wn"), color2));
            Helper.AddText(((new Vector2(x7, y3), "W"), color1));
            Helper.AddText(((new Vector2(x6, y4), "ws"), color2));

            var x8 = r + 4;
            var x9 = r + 6;
            Helper.AddText(((new Vector2(x8, y2), "en"), color2));
            Helper.AddText(((new Vector2(x9, y3), "E"), color1));
            Helper.AddText(((new Vector2(x8, y4), "es"), color2));
        }



        #endregion
        #endregion

        #region Picker1,Overview,Picker2  =====================================
        private void Picker1Tap(bool add = false)
        {
            _picker2Index = -1;
            _picker1Index = (int)(0.5f + Picker1.Point1.Y / Picker1.Extent.Width);
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
            SetProperties();
            //if (DrawState == DrawState.PinsMode) return;
            //if (SelectedShapes.Count == 0) SetViewMode();
            //SetDrawState(DrawState.EditMode);
        }
        private void OverviewTap()
        {
            var shapes = Symbol.GetShapes();
            if (SelectedShapes.Count == shapes.Count)
                SetViewMode();
            _picker1Index = 0; //will make Picker1Valid true
            SelectedShapes.Clear();
            SelectedShapes.AddRange(shapes);
            SetProperties();
            //SetDrawState(DrawState.EditMode);
        }

        private void Picker2Tap()
        {
            _picker1Index = -1;
            SelectedShapes.Clear();
 
            _picker2Index = (int)(0.5f + Picker2.Point1.Y / Picker2.Extent.Width);
            if (Picker2IsValid)
            {
                SelectedShapes.Add(_templateShapes[_picker2Index]);
            }
            SetProperties();

            //if (Picker2IsValid)
            //    SetDrawState(DrawState.CreateMode);
            //else
            //   SetViewMode();
        }
        private bool Picker1IsValid => _picker1Index > -1 && _picker1Index < Symbol.GetShapes().Count;
        private bool Picker2IsValid => _picker2Index > -1 && _picker2Index < _picker2Shapes.Length;
        private int _picker1Index;
        private int _picker2Index;
        internal List<Shape> SelectedShapes = new List<Shape>();
        private List<Shape> _templateShapes = new List<Shape>(_picker2Shapes.Length);
        private static Shape[] _picker2Shapes =
        {
            new Line(),
            new Circle(),
            new Ellipes(),
            new Rectangle(),
            new RoundedRectangle(),
            new PolySide(),
            new PolyStar(),
            new PolyGear(),
            new PolyPulse(),
            new PolySpike(),
            new PolyWave(),
            new PolySpring(),
        };
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
            var dp = Editor.Point2;
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
        private void CutAction()
        {
            _shapeClipboard.Clear();
            _shapeClipboard.AddRange(SelectedShapes);
            var shapes = Symbol.GetShapes();
            foreach (var s in SelectedShapes)
            {
                shapes.Remove(s);
            }
            SetViewMode();
        }
        private List<Shape> _shapeClipboard = new List<Shape>(10);
        private void CopyAction()
        {
            _shapeClipboard.Clear();
            _shapeClipboard.AddRange(SelectedShapes);
            SetViewMode();
        }
        private void CenterAction()
        {
            Shape.SetCenter(SelectedShapes, new Vector2());
            Refresh();
        }
        private void RotateLeft()
        {
            Shape.RotateLeft(SelectedShapes, _useAlternate);
            Refresh();
        }
        private void RotateRight()
        {
            Shape.RotateRight(SelectedShapes, _useAlternate);
            Refresh();
        }
        private void VerticalFlip()
        {
            Shape.VerticalFlip(SelectedShapes);
            Refresh();
        }
        private void HorizontalFlip()
        {
            Shape.HorizontalFlip(SelectedShapes);
            Refresh();
        }
        private void SetDegree22() => _useAlternate = false;
        private void SetDegree30() => _useAlternate = true;
        private bool _useAlternate;
        #endregion

        #region MoveMode  =====================================================
        private void DragShapeAction()
        {
            var delta = Editor.PointDelta(true) / EditRadius;
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
            var delta = Editor.PointDelta(true) / EditRadius;
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
            if (Picker2IsValid)
            {
                var cp = Editor.Point1 / EditRadius;
                var ns = _templateShapes[_picker2Index].Clone(cp);
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
