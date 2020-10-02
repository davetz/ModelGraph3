using System;
using System.Collections.Generic;
using System.Numerics;
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
            RefreshDrawData();
            RefreshHelperData();
            SideTreeModel = new TreeModel(PageModel, (m) => { new Model_601_Shape(m, this); });

            SetDrawStateAction(DrawState.Apply, ApplyChange);
            SetDrawStateAction(DrawState.Revert, Revert);

            Editor.GetExtent = () => new Extent(-EditExtent, -EditExtent, EditExtent, EditExtent);
            Picker1.GetExtent = () => new Extent(-16, 0, 16, 0);
            Picker2.GetExtent = () => new Extent(-16, 0, 16, 0);

            SetViewMode();
            RefreshDrawData();
        }
       #endregion

        #region ShapeProperty  ================================================

        #region SetProperties  ================================================
        private void SetProperties()
        {
            Properties = ShowProperty.None;

            if (DrawState == DrawState.EditMode && _selectPicker1Shapes.Count > 0 )
                SetProps(_selectPicker1Shapes[0]);
            else if (DrawState == DrawState.EditMode && _selectPicker2Shape is Shape s)
                SetProps(s);

            (SideTreeModel as TreeModel).HeaderModel.ExpandRight(null);

            void SetProps(Shape s1)
            {
                var (_, st, sw) = s1.ShapeStrokeWidth();
                StrokeWidth = sw;

                var sc = st & StrokeType.SC_Triangle;
                var dc = st & StrokeType.DC_Triangle;
                var ec = st & StrokeType.EC_Triangle;
                var ss = st & StrokeType.Filled;

                _endCapStyle = ec == StrokeType.EC_Round ? CapStyle.Round : ec == StrokeType.EC_Square ? CapStyle.Square : ec == StrokeType.EC_Triangle ? CapStyle.Triangle : CapStyle.Flat;
                _dashCapStyle = dc == StrokeType.DC_Round ? CapStyle.Round : dc == StrokeType.DC_Square ? CapStyle.Square : dc == StrokeType.DC_Triangle ? CapStyle.Triangle : CapStyle.Flat;
                _startCapStyle = sc == StrokeType.SC_Round ? CapStyle.Round : sc == StrokeType.SC_Square ? CapStyle.Square : sc == StrokeType.SC_Triangle ? CapStyle.Triangle : CapStyle.Flat;
                _strokeStyle = ss == StrokeType.Dotted ? StrokeStyle.Dotted : ss == StrokeType.Dashed ? StrokeStyle.Dashed : ss == StrokeType.Filled ? StrokeStyle.Filled : StrokeStyle.Solid;

                ColorARGB = s1.ShapeColor();


                Properties |= ShowProperty.StrokeStyle;
                if (_strokeStyle == StrokeStyle.Filled) return;
                Properties |= ShowProperty.StartCap | ShowProperty.EndCap;
                if (_strokeStyle == StrokeStyle.Solid) return;
                Properties |= ShowProperty.DashCap;
            }
        }

        internal ShowProperty Properties;
        #endregion

        #region EndCapStyle  ==================================================
        internal CapStyle EndCapStyle { get => _endCapStyle; set => SetEndCap(value); }
        private void SetEndCap(CapStyle value)
        {
            _endCapStyle = value;
            foreach (var s in _selectPicker1Shapes)
            {
                s.SetEndCap(value);
            }
            RefreshDrawData();
            PageModel.TriggerUIRefresh();
        }
        private CapStyle _endCapStyle;
        #endregion

        #region DashCapStyle  =================================================
        internal CapStyle DashCapStyle { get => _dashCapStyle; set => SetDashCap(value); }
        private void SetDashCap(CapStyle value)
        {
            _dashCapStyle = value;
            foreach (var s in _selectPicker1Shapes)
            {
                s.SetDashCap(value);
            }
            RefreshDrawData();
            PageModel.TriggerUIRefresh();
        }
        private CapStyle _dashCapStyle;
        #endregion

        #region StartCapStyle  ================================================
        internal CapStyle StartCapStyle { get => _startCapStyle; set => SetStartCap(value); }
        private void SetStartCap(CapStyle value)
        {
            _startCapStyle = value;
            foreach (var s in _selectPicker1Shapes)
            {
                s.SetStartCap(value);
            }
            RefreshDrawData();
            PageModel.TriggerUIRefresh();
        }
        private CapStyle _startCapStyle;
        #endregion

        #region StrokeStyle  ==================================================
        internal StrokeStyle StrokeStyle { get => _strokeStyle; set => SetStrokeStyle(value); }
        private void SetStrokeStyle(StrokeStyle value)
        {
            _strokeStyle = value;
            foreach (var s in _selectPicker1Shapes)
            {
                s.SetStokeStyle(value);
            }
            RefreshDrawData();
            PageModel.TriggerUIRefresh();
        }
        private StrokeStyle _strokeStyle;
        #endregion

        #region StrokeWidth  ==================================================
        internal byte StrokeWidth { get => _strokeWidth; set => SetStrokeWidth(value); }
        private void SetStrokeWidth(byte value)
        {
            _strokeWidth = value;
            foreach (var s in _selectPicker1Shapes)
            {
                s.SetStrokeWidth(_strokeWidth);
            }
            RefreshDrawData();
            PageModel.TriggerUIRefresh();
        }
        private byte _strokeWidth = 2;
        #endregion

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
            RefreshDrawData();
            if (_selectPicker1Shapes.Count == 0)
                SetViewMode();
            else
                SetEditMode();

        }
        private List<Shape> _selectPicker1Shapes = new List<Shape>(10);

        Shape _selectPicker2Shape;
        static Shape[] _picker2Shapes =
        {
            new Line(),
            new Circle(),
            //new Ellipes(),
            //new Rectangle(),
            //new RoundedRectangle(),
            //new PolySide(),
        };

        private void Picker2Select()
        {
            var i = (int)(0.5f + Picker2.Point1.Y / Picker2.Extent.Width);
            _selectPicker2Shape = (i >= 0 && i < _picker2Shapes.Length) ? _picker2Shapes[i] : null;

            _selectPicker1Shapes.Clear();
            RefreshDrawData();

            if (_selectPicker2Shape is null)
                SetViewMode();
            else
                SetCreateMode();
        }
        #endregion

        #region ColorARGB/Apply/Revert  =======================================
        public override void ColorARGBChanged()
        {
            foreach (var s in _selectPicker1Shapes)
            {
                s.SetColor(ColorARGB);
            }
            RefreshDrawData();
            PageModel.TriggerUIRefresh();
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
                SetProperties();
                _selectPicker2Shape = null;
                _selectPicker1Shapes.Clear();
                SetEventAction(DrawEvent.Picker1Tap, () => { Picker1Select(false); });
                SetEventAction(DrawEvent.Picker1CtrlTap, () => { Picker1Select(true); });
                SetEventAction(DrawEvent.Picker2Tap, () => { Picker2Select(); });
                RefreshDrawData();
            }
        }
        #endregion

        #region EditMode == ModifySymbol  =====================================
        public void SetEditMode()
        {
            if (TrySetState(DrawState.EditMode))
            {
                SetProperties();
                SetEventAction(DrawEvent.Tap, SetViewMode);
            }
        }
        #endregion

        #region CreateMode == AddSymbolShape  =================================
        public void SetCreateMode()
        {
            if (TrySetState(DrawState.CreateMode))
            {
                SetProperties();
                SetEventAction(DrawEvent.Picker2Tap, () => { Picker2Select(); });
                SetEventAction(DrawEvent.Tap, CloneShape);
            }
        }
        private void CloneShape()
        {
            if (_selectPicker2Shape is null) SetViewMode();

            var cp = Editor.Point1 / EditRadius;
            Symbol.GetShapes().Add(_selectPicker2Shape.Clone(cp));
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

