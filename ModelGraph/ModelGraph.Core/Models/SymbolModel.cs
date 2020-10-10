using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Windows.Security.Cryptography.Core;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Core
{
    public class SymbolModel : DrawModel
    {
        internal readonly SymbolX Symbol;
        private const float EditRadius = 256;   //width, height of shape in the editor
        private const float EditMargin = 32;    //size of empty space arround the shape editor 
        private const float EditExtent = EditRadius + EditMargin;
        private readonly float AbsoluteSize;

        #region Constructor  ==================================================
        internal SymbolModel(PageModel owner, Root root, SymbolX symbol) : base(owner)
        {

            Symbol = symbol;
            AbsoluteSize = symbol.AbsoluteSize;
            Editor.GetExtent = () => new Extent(-EditExtent, -EditExtent, EditExtent, EditExtent);
            Picker1.GetExtent = () => new Extent(-16, 0, 16, 0);
            Picker2.GetExtent = () => new Extent(-16, 0, 16, 0);

            RefreshDrawData();
            RefreshHelperData();
            SideTreeModel = new TreeModel(PageModel, (m) => { new Model_601_Shape(m, this); });

            SetDrawStateAction(DrawState.Apply, ApplyChange);
            SetDrawStateAction(DrawState.Revert, Revert);

            SetViewMode();
            RefreshDrawData();
        }
        #endregion

        #region ShapeProperty  ================================================

        private void SetProperties()
        {
            _propertyFlags = ShapeProperty.None;
            if (_selectPicker1Shapes.Count > 0)
            {
                Shape.GetStrokeProperty(_selectPicker1Shapes, ref _propertyFlags, ref _lineWidth, ref _lineStyle, ref _startCap, ref _dashCap, ref _endCap, ref _colorARGB);
                Shape.GetSizerProperty(_selectPicker1Shapes, ref _polyLocked, ref _min, ref _max, ref _dimension, ref _auxAxis, ref _majorAxis, ref _minorAxis, ref _centAxis, ref _vertAxis, ref _horzAxis);
            }
            else if (_selectPicker2Shape != null)
                _propertyFlags = Shape.GetPropertyFlags(_selectPicker2Shape);

            (SideTreeModel as TreeModel).Validate();
            RefreshDrawData();
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

        internal byte CentAxis { get => _centAxis; set => Set(ref _centAxis, value, ShapeProperty.Cent); }
        private byte _centAxis = 25;

        internal byte VertAxis { get => _vertAxis; set => Set(ref _vertAxis, value, ShapeProperty.Vert); }
        private byte _vertAxis = 25;

        internal byte HorzAxis { get => _horzAxis; set => Set(ref _horzAxis, value, ShapeProperty.Horz); }
        private byte _horzAxis = 25;

        internal byte MajorAxis { get => _majorAxis; set => Set(ref _majorAxis, value, ShapeProperty.Major); }
        private byte _majorAxis = 25;

        internal byte MinorAxis { get => _minorAxis; set => Set(ref _minorAxis, value, ShapeProperty.Minor); }
        private byte _minorAxis = 25;

        internal byte Dimension { get => _dimension; set => Set(ref _dimension, value, ShapeProperty.Dim); }
        private byte _dimension = 3;
        internal bool PolyLocked { get => _polyLocked; set => Set(ref _polyLocked, value, ShapeProperty.PolyLocked); }
        private bool _polyLocked = false;

        private void Set<T>(ref T storage, T value, ShapeProperty sp)
        {
            if (Equals(storage, value)) return;

            storage = value;
            Shape.SetProperty(this, sp, _selectPicker1Shapes);
            RefreshDrawData();
        }
        #endregion

        #region RefreshDrawData  ==================================================
        private void RefreshDrawData()
        {
            RefreshEditorData();
            RefreshPicker1Data();
            RefreshPicker2Data();

            PageModel.TriggerUIRefresh();

            void RefreshEditorData()
            {
                var r = EditRadius;
                var c = new Vector2();
                var a = AbsoluteSize;

                Editor.Clear();
                var shapes = Symbol.GetShapes();
                foreach (var s in shapes)
                {
                    s.AddDrawData(Editor, a, r, c, Shape.Coloring.Normal);
                }
            }
            void RefreshPicker1Data()
            {
                var r = Picker1.Extent.Width / 2;
                var c = new Vector2(0, 0);
                var a = (float)Symbol.AbsoluteSize;
                Picker1.Clear();
                foreach (var s in Symbol.GetShapes())
                {
                    s.AddDrawData(Picker1, a, r, c);
                    if (_selectPicker1Shapes.Contains(s))
                        Picker1.AddShape(((c, new Vector2(r, r)), (ShapeType.Rectangle, StrokeType.Filled, 0), (90, 255, 255, 255)));
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
                    if (s == _selectPicker2Shape)
                        Picker2.AddShape(((c, new Vector2(r, r)), (ShapeType.Rectangle, StrokeType.Filled, 0), (90, 255, 255, 255)));
                    c = new Vector2(0, c.Y + Picker2.Extent.Width);
                }
            }
        }

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
            Helper.AddLine((points1.ToArray(), (ShapeType.MultipleLines, StrokeType.Simple, 1), color1));
            Helper.AddLine((points2.ToArray(), (ShapeType.MultipleLines, StrokeType.Simple, 1), color2));
            Helper.AddShape(((new Vector2(0, 0), new Vector2(r / 2, r / 2)), (ShapeType.Circle, StrokeType.Simple, 1), color2));
            Helper.AddShape(((new Vector2(0, 0), new Vector2(r, r)), (ShapeType.Circle, StrokeType.Simple, 1), color1));

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

        #region PickerSelect  ===========================================
        private void Picker1Select(bool add = false)
        {
            var i = (int)(0.5f + Picker1.Point1.Y / Picker1.Extent.Width);

            if (!add) _selectPicker1Shapes.Clear();
            var shapes = Symbol.GetShapes();
            if (i >= 0 && i < shapes.Count)
            {
                var s = shapes[i];
                if (add)
                {
                    if (_selectPicker1Shapes.Contains(s))
                        _selectPicker1Shapes.Remove(s);
                    else
                        _selectPicker1Shapes.Add(s);
                }
                else
                    _selectPicker1Shapes.Add(s);
            }
            else
                _selectPicker1Shapes.Clear();

            _selectPicker2Shape = null;

            SetProperties();

            if (_selectPicker1Shapes.Count == 0)
                SetViewMode();
            else
                SetEditMode();

        }
        private List<Shape> _selectPicker1Shapes = new List<Shape>(10);

        private void Picker2Select()
        {
            var i = (int)(0.5f + Picker2.Point1.Y / Picker2.Extent.Width);
            _selectPicker2Shape = (i >= 0 && i < _picker2Shapes.Length) ? _picker2Shapes[i] : null;

            _selectPicker1Shapes.Clear();
            SetProperties();

            if (_selectPicker2Shape is null)
                SetViewMode();
            else
                SetCreateMode();
        }
        Shape _selectPicker2Shape;
        static Shape[] _picker2Shapes =
        {
            new Line(),
            new Circle(),
            //new Ellipes(),
            new Rectangle(),
            //new RoundedRectangle(),
            //new PolySide(),
        };
        #endregion

        #region ColorARGB/Apply/Revert  =======================================
        protected override void ColorARGBChanged()
        {
            if (_selectPicker1Shapes.Count > 0)
            {
                foreach (var s in _selectPicker1Shapes)
                {
                    s.SetColor(ColorARGB);
                }
                SetProperties();
                RefreshDrawData();
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
            RefreshDrawData();
            RestoreDrawState();
            PageModel.TriggerUIRefresh();
        }
        private void RestoreDrawState()
        {
            if ((PreviousDrawState & DrawState.ModeMask) == DrawState.LinkMode) SetViewMode();
            if ((PreviousDrawState & DrawState.ModeMask) == DrawState.OperateMode) SetViewMode();
            SetViewMode();
        }
        #endregion

        #region DrawStateControl  =============================================

        #region ViewMode == ViewSymbol  =======================================
        public void SetViewMode()
        {
            if (TrySetState(DrawState.ViewMode))
            {
                _selectPicker2Shape = null;
                _selectPicker1Shapes.Clear();
                SetEventAction(DrawEvent.Picker1Tap, () => { Picker1Select(false); });
                SetEventAction(DrawEvent.Picker1CtrlTap, () => { Picker1Select(true); });
                SetEventAction(DrawEvent.Picker2Tap, () => { Picker2Select(); });
                if (IsPasteActionEnabled) SetEventAction(DrawEvent.Paste, PasteAction);
                SetProperties();
            }
        }
        private void PasteAction()
        {
            var shapes = Symbol.GetShapes();
            foreach (var s in _shapeClipboard)
            {
                var ns = s.Clone();

                shapes.Add(ns);
                RefreshDrawData();
            }

        }
        #endregion

        #region EditMode == ModifySymbol  =====================================
        public void SetEditMode()
        {
            if (TrySetState(DrawState.EditMode))
            {
                SetEventAction(DrawEvent.Tap, SetViewMode);
                SetEventAction(DrawEvent.Picker1Tap, () => { Picker1Select(false); });
                SetEventAction(DrawEvent.Picker1CtrlTap, () => { Picker1Select(true); });
                SetEventAction(DrawEvent.Picker2Tap, () => { Picker2Select(); });
                SetEventAction(DrawEvent.Cut, CutAction);
                SetEventAction(DrawEvent.Copy, CopyAction);
                SetEventAction(DrawEvent.Center, CenterAction);
                SetEventAction(DrawEvent.RotateLeft, RotateLeft);
                SetEventAction(DrawEvent.RotateRight, RotateRight);
                SetEventAction(DrawEvent.SetDegree22, SetDegree22);
                SetEventAction(DrawEvent.SetDegree30, SetDegree30);
                SetEventAction(DrawEvent.VerticalFlip, VerticalFlip);
                SetEventAction(DrawEvent.HorizontalFlip, HorizontalFlip);
                SetEventAction(DrawEvent.OverviewTap, OverviewTap);
            }
        }

        private void CutAction()
        {
            _shapeClipboard.Clear();
            _shapeClipboard.AddRange(_selectPicker1Shapes);
            IsPasteActionEnabled = true;
            var shapes = Symbol.GetShapes();
            foreach (var s in _selectPicker1Shapes)
            {
                shapes.Remove(s);
            }
            SetViewMode();
        }
        private List<Shape> _shapeClipboard = new List<Shape>(10);
        private void CopyAction()
        {
            _shapeClipboard.Clear();
            _shapeClipboard.AddRange(_selectPicker1Shapes);
            IsPasteActionEnabled = true;
            SetViewMode();
        }
        private void CenterAction()
        {
            Shape.SetCenter(_selectPicker1Shapes, new Vector2());
            RefreshDrawData();
        }
        private void RotateLeft()
        {
            Shape.RotateLeft(_selectPicker1Shapes, _useAlternate);
            RefreshDrawData();
        }
        private void RotateRight()
        {
            Shape.RotateRight(_selectPicker1Shapes, _useAlternate);
            RefreshDrawData();
        }
        private void VerticalFlip()
        {
            Shape.VerticalFlip(_selectPicker1Shapes);
            RefreshDrawData();
        }
        private void HorizontalFlip()
        {
            Shape.HorizontalFlip(_selectPicker1Shapes);
            RefreshDrawData();
        }
        private void OverviewTap()
        {
            var n = _selectPicker1Shapes.Count;
            _selectPicker1Shapes.Clear();

            var shapes = Symbol.GetShapes();
            if (n != shapes.Count)
                _selectPicker1Shapes.AddRange(shapes);
            _selectPicker2Shape = null;

            SetProperties();

            if (_selectPicker1Shapes.Count == 0)
                SetViewMode();
            else
                SetEditMode();
        }
        private void SetDegree22() => _useAlternate = false;
        private void SetDegree30() => _useAlternate = true;
        private bool _useAlternate;
        #endregion

        #region CreateMode == AddSymbolShape  =================================
        public void SetCreateMode()
        {
            if (TrySetState(DrawState.CreateMode))
            {
                SetEventAction(DrawEvent.Picker2Tap, () => { Picker2Select(); });
                SetEventAction(DrawEvent.Tap, CloneAction);
                SetProperties();
                RefreshDrawData();
            }
        }
        private void CloneAction()
        {
            if (_selectPicker2Shape is null) SetViewMode();

            var cp = Editor.Point1 / EditRadius;
            var ns = _selectPicker2Shape.Clone(cp);

            ns.SetColor(ColorARGB);
            ns.SetStokeStyle(_lineStyle);
            ns.SetStrokeWidth(_lineWidth);
            ns.SetStartCap(_startCap);
            ns.SetDashCap(_dashCap);
            ns.SetEndCap(_endCap);

            Symbol.GetShapes().Add(ns);
            RefreshDrawData();
        }
        #endregion

        #region LinkMode == DefineTerminals  ==================================
        public void LinkModeMode()
        {
            if (TrySetState(DrawState.LinkMode))
            {
            }
        }
        #endregion

        #region OperateMode == AutoFlipRotate  ================================
        public void SetOperateMode()
        {
            if (TrySetState(DrawState.OperateMode))
            {
            }
        }
        #endregion

        #endregion
    }
}

