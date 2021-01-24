using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ModelGraph.Core
{
    internal class ShapeSelector
    {
        internal Shape HitShape;
        internal Shape RefShape;
        internal Shape PrevShape;
        internal int HitPin;

        internal List<Shape> Shapes = new List<Shape>();
        internal List<int> Pins = new List<int>();

        #region Constructor  ==================================================
        internal ShapeSelector(SymbolModel model)
        {
            _model = model;
        }
        private readonly SymbolModel _model;
        #endregion

        #region HitTestPoint  =================================================
        internal bool HitTestPoint(Vector2 p)
        {
            PrevShape = HitShape;
            // clear previous results
            HitShape = null;
            HitPin = -1;
            foreach (var shape in _model.Symbol.GetShapes())
            {
                if (shape.HitTest(p))
                {
                    HitShape = shape;
                    return true;
                }
            }
            return false;
        }
        #endregion

    }
}
