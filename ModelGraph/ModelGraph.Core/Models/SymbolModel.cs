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
        override public Extent EditorExtent => new Extent(-EditExtent, -EditExtent, EditExtent, EditExtent);
        private const float EditRadius = 256;   //width, height of shape in the editor
        private const float EditMargin = 32;    //size of empty space arround the shape editor 
        private const float EditExtent = EditRadius + EditMargin;
        private float AbsoluteSize;

        #region Constructor  ==================================================
        internal SymbolModel(PageModel owner, Root root, SymbolX symbol) : base(owner)
        {

            Symbol = symbol;
            AbsoluteSize = symbol.AbsoluteSize;
            RefreshEditorData();
            RefreshHelperData();
            RefreshPicker1Data();
            RefreshPicker2Data();
            PageModel.TriggerUIRefresh();
            SideTreeModel = new TreeModel(PageModel, (m) => { new Model_601_Shape(m, this); });
        }
        #endregion

        #region ShapeProperty  ================================================
        internal CapStyle EndCapStyle;
        internal CapStyle DashCapStyle;
        internal CapStyle StartCapStyle;
        internal StrokeStyle StrokeStyle;
        internal byte StrokeWidth = 2;
        #endregion

        #region RefreshEditorData  ============================================
        override public void RefreshEditorData()
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
        #endregion

        #region ColorARGB  ====================================================
        public override void ColorARGBChanged()
        {
            foreach (var s in _selectShapes)
            {
                s.SetColor(ColorARGB);
            }
            RefreshEditorData();
            RefreshPicker1Data();
            PageModel.TriggerUIRefresh();
        }
        #endregion

        #region StrokeWidth  ==================================================
        internal void StrokeWidthChanged()
        {
            foreach (var s in _selectShapes)
            {
                s.SetStrokeWidth(StrokeWidth);
            }
            RefreshEditorData();
            RefreshPicker1Data();
            PageModel.TriggerUIRefresh();
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

        #region RefreshPicker1Data  ===========================================
        private void RefreshPicker1Data()
        {
            var r = Picker2Width / 2;
            var c = new Vector2(0, 0);
            var a = (float)Symbol.AbsoluteSize;
            Picker1.Clear();
            foreach (var s in Symbol.GetShapes())
            {
                s.AddDrawData(Picker1, a, r, c);
                if (_selectShapes.Contains(s))
                    Picker1.AddShape(((c, new Vector2(r, r)), (ShapeType.Rectangle, StrokeType.Filled, 0), (90, 255, 255, 255)));
                c = new Vector2(0, c.Y + Picker2Width);
            }
        }
        override public int Picker1Width => 32;
        override public void Picker1Select(int y, bool add = false)
        {
            var w = Picker1Width;
            var i = y / w;

            if (!add) _selectShapes.Clear();
            var shapes = Symbol.GetShapes();
            if (i < shapes.Count)
            {
                var s = shapes[i];
                if (add)
                {
                    if (_selectShapes.Contains(s))
                        _selectShapes.Remove(s);
                    else
                        _selectShapes.Add(s);
                }
                else
                    _selectShapes.Add(s);
            }
            else
                _selectShapes.Clear();

            _selectPicker2Shape = null;
            RefreshEditorData();
            RefreshPicker1Data();
            RefreshPicker2Data();
        }
        private List<Shape> _selectShapes = new List<Shape>(10);
        #endregion

        #region RefreshPicker2Data  ===========================================
        private void RefreshPicker2Data()
        {
            var r = Picker2Width / 2;
            var c = new Vector2(0, 0);
            var a = (float)Symbol.AbsoluteSize;

            Picker2.Clear();
            foreach (var s in _picker2Shapes)
            {
                s.AddDrawData(Picker2, a, r, c);
                if (s == _selectPicker2Shape)
                    Picker2.AddShape(((c, new Vector2(r, r)), (ShapeType.Rectangle, StrokeType.Filled, 0), (90, 255, 255, 255)));
                c = new Vector2(0, c.Y + Picker2Width);
            }
        }
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
        override public int Picker2Width => 32;

        override public void Picker2Select(int y)
        {
            var w = Picker2Width;
            var i = y / w;
            _selectPicker2Shape = (i < _picker2Shapes.Length) ? _picker2Shapes[i] : null;

            _selectShapes.Clear();
            RefreshEditorData();
            RefreshPicker1Data();
            RefreshPicker2Data();
        }
        #endregion


        #region HitTest  ======================================================

        override public bool TapHitTest()
        {
            ClearHit();
            if (_selectPicker2Shape is null)
            {

            }
            else
            {
                var cp = DrawPoint1 / EditRadius;
                Symbol.GetShapes().Add(_selectPicker2Shape.Clone(cp));
                RefreshEditorData();
                PageModel.TriggerUIRefresh();
            }
            return false;
        }
        #endregion
    }
}

