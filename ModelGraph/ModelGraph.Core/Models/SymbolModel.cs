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
            var c = new Vector2(50,80);

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
            Helper.AddText(((new Vector2(x1, yN), "nec"), color2));
            Helper.AddText(((new Vector2(x2, yN), "ne"), color2));
            Helper.AddText(((new Vector2(x3, yN), "N"), color1));
            Helper.AddText(((new Vector2(x4, yN), "nw"), color2));
            Helper.AddText(((new Vector2(x5, yN), "nwc"), color2));

            var yS = r + 2;
            Helper.AddText(((new Vector2(x1, yS), "sec"), color2));
            Helper.AddText(((new Vector2(x2, yS), "se"), color2));
            Helper.AddText(((new Vector2(x3, yS), "S"), color1));
            Helper.AddText(((new Vector2(x4, yS), "sw"), color2));
            Helper.AddText(((new Vector2(x5, yS), "sew"), color2));

            var y2 = -r / 2 - 18;
            var y3 = -16f;
            var y4 = r / 2 - 16;
            var x6 = -r - 28;
            var x7 = -r - 20;
            Helper.AddText(((new Vector2(x6, y2), "en"), color2));
            Helper.AddText(((new Vector2(x7, y3), "E"), color1));
            Helper.AddText(((new Vector2(x6, y4), "es"), color2));

            var x8 = r + 4;
            var x9 = r + 6;
            Helper.AddText(((new Vector2(x8, y2), "wn"), color2));
            Helper.AddText(((new Vector2(x9, y3), "W"), color1));
            Helper.AddText(((new Vector2(x8, y4), "ws"), color2));
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

