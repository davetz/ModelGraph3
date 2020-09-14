using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Security.Cryptography.Core;

namespace ModelGraph.Core
{
    public class SymbolModel : CanvasModel, IPageModel
    {
        internal readonly SymbolX Symbol;
        override public Extent EditorExtent => new Extent(-EditExtent, -EditExtent, EditExtent, EditExtent);
        private const float EditRadius = 256;   //width, height of shape in the editor
        private const float EditMargin = 32;    //size of empty space arround the shape editor 
        private const float EditExtent = EditRadius + EditMargin;


        #region Constructor  ==================================================
        internal SymbolModel(Root root, SymbolX symbol) : base(root)
        {
            Symbol = symbol;
            RefreshDrawData();
            RefreshHelperData();
            PageControl?.Refresh();
        }
        #endregion

        #region IPageModel  ===================================================
        public string TitleName => "TestModelCanvasControl";
        public string TitleSummary => string.Empty;
        public ControlType ControlType => ControlType.SymbolEditor;
        public IPageControl PageControl { get; set; }
        public void Release()
        {
            Owner.Remove(this);
            Discard(); //discard myself and recursivly discard all my children
        }

        public void TriggerUIRefresh()
        {
            if (Symbol.ModelDelta != ModelDelta)
            {
                ModelDelta = Symbol.ModelDelta;
                PageControl?.Refresh();
            }
        }
        #endregion


        #region RefreshDrawData  ==============================================
        override public void RefreshDrawData()
        {
            var r = EditRadius;
            var c = new Vector2(0,0);

            Editor.Clear();
            var shapes = Symbol.GetShapes();
            if (shapes.Count == 0) shapes.Add(new Circle());
            foreach (var s in shapes)
            {
                s.AddDrawData(Editor, r, c);
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
                AddTickMarks(r - i * r / 8f );

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
            (byte, byte, byte, byte) color2 = (0xff, 0xff, 0xff, 0x80);
            (byte, byte, byte, byte) color3 = (0x80, 0xff, 0xff, 0x00);
            (byte, byte, byte, byte) color4 = (0x40, 0xff, 0xff, 0xff);

            Helper.Clear();
            Helper.AddLine((points1.ToArray(), (ShapeType.MultipleLines, StrokeType.Simple, 1), color1));
            Helper.AddLine((points2.ToArray(), (ShapeType.MultipleLines, StrokeType.Simple, 1), color3));
            Helper.AddShape(((new Vector2(0, 0), new Vector2(r / 2, r / 2)), (ShapeType.Circle, StrokeType.Simple, 1), color3));
            Helper.AddShape(((new Vector2(0, 0), new Vector2(r, r)), (ShapeType.Circle, StrokeType.Simple, 1), color1));

           
            //var xC = - 6;
            //var yN = -2;
            //var yS = b - 3;
            //var xe = b - r/4 - 10;
            //var xw = a + r/4 - 10;
            //var xec = b - 10;
            //var xwc = a - 16;

            Helper.AddText(((new Vector2((-r - 16), (-r - 32)), "nec"), color3));
            Helper.AddText(((new Vector2((-r/2 - 8), (-r - 32)), "ne"), color3));
            Helper.AddText(((new Vector2((- 8), (-r - 32)), "N"), color1));
            Helper.AddText(((new Vector2((r/2 - 8), (-r - 32)), "nw"), color3));
            Helper.AddText(((new Vector2((r - 16), (-r - 32)), "new"), color3));
            //ds.DrawText(, xec, yN, color3);
            //ds.DrawText("ne", xe, yN, color3);
            //ds.DrawText("N", xC, yN, color1);
            //ds.DrawText("nw", xw, yN, color3);
            //ds.DrawText("nwc", xwc, yN, color3);

            //ds.DrawText("sec", xec, yS, color3);
            //ds.DrawText("se", xe, yS, color3);
            //ds.DrawText("S", xC, yS, color1);
            //ds.DrawText("sw", xw, yS, color3);
            //ds.DrawText("swc", xwc, yS, color3);


            //var xE = b + 3;
            //var xW = 8;
            //var yC = c - 14;
            //var yn = a + _workAxis - 14;
            //var ys = b - _workAxis - 14;

            //ds.DrawText("en", xE, yn, color3);
            //ds.DrawText("E", xE, yC, color1);
            //ds.DrawText("es", xE, ys, color3);

            //ds.DrawText("wn", xW - 4, yn, color3);
            //ds.DrawText("W", xW, yC, color1);
            //ds.DrawText("ws", xW - 4, ys, color3);
        }



        #endregion

        #endregion

        #region PickerEvents  =================================================
        override public int Picker1Width => 30;
        override public int Picker2Width => 30;

        override public void Picker1Select(int y)
        {
            var w = Picker1Width;
            var x = w / 2;
            var z = (y / w) * w + x;
            Picker1.Clear();
            Picker1.AddShape(((new Vector2(x, z), new Vector2(x, x)), (ShapeType.Rectangle, StrokeType.Filled, 0), (63, 255, 255, 255)));
        }
        override public void Picker2Select(int y)
        {
            var w = Picker2Width;
            var x = w / 2;
            var z = (y / w) * w + x;
            Picker2.Clear();
            Picker2.AddShape(((new Vector2(x, z), new Vector2(x, x)), (ShapeType.Rectangle, StrokeType.Filled, 0), (63, 255, 255, 255)));
        }
        #endregion


        #region HitTest  ======================================================

        override public bool SkimHitTest()
        {
            ClearHit();
            return false;
        }

        override public bool TapHitTest()
        {
            ClearHit();
            return false;
        }

        override public void ClearRegion() => _regionNodes.Clear();

        private Node _hitNode;
        private List<Node> _regionNodes = new List<Node>();
        #endregion

        #region ITreeCanvasModel  =============================================

        override public void RefreshViewList(int viewSize, ItemModel leading, ItemModel selected, ChangeType change = ChangeType.None) { }
        override public (List<ItemModel>, ItemModel, bool, bool) GetCurrentView(int viewSize, ItemModel leading, ItemModel selected)
        {
            if (_hitNode is null) return (new List<ItemModel>(0), null, false, false);
            if (_itemModel != null)
            {
                if (_itemModel.GetItem() != _hitNode)
                {
                    _itemModel.Discard();
                    _itemModel = new Model_6DA_HitNode(_hitNode);
                }
            }
            else
                _itemModel = new Model_6DA_HitNode(_hitNode);

            _itemModel.ExpandRight(Owner);

            if (selected is null || !_itemModel.Items.Contains(selected))
                selected = _itemModel.Count == 0 ? null : _itemModel.Items[0];

            return (_itemModel.Items, selected, true, true);
        }
        private ItemModel _itemModel;
        override public string HeaderTitle => _itemModel.GetItem().GetNameId();

        #endregion

    }
}

