using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ModelGraph.Core
{
    internal class ShapeSelector
    {
        internal Shape HitShape;
        internal Shape PrevShape;

        internal Shape RefShape;
        internal int HitPin;

        internal List<Shape> Shapes = new List<Shape>();
        internal List<int> Pins = new List<int>();

        #region Constructor  ==================================================
        internal ShapeSelector(ShapeModel model)
        {
            _model = model;
        }
        private readonly ShapeModel _model;
        #endregion

        internal void Clear()
        {
            HitShape = PrevShape = RefShape = null;
            Shapes.Clear();
            Pins.Clear();
        }

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
