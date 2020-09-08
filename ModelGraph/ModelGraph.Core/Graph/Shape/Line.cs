using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class Line : Polyline
    {
        internal Line(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            DXY = new List<(float dx, float dy)>() {(-0.25f, 0), (0.25f, 0)};
        }

        #region PrivateConstructor  ===========================================
        private Line(ShapeBase shape)
        {
            CopyData(shape);
        }
        private Line(ShapeBase shape, Vector2 center)
        {
            CopyData(shape);
            SetCenter( new ShapeBase[] { this }, center);
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override ShapeBase Clone() => new Line(this);
        internal override ShapeBase Clone(Vector2 center) => new Line(this, center);
        internal override HasSlider Sliders => HasSlider.Horz | HasSlider.Vert;
        protected override byte TypeCode => (byte)Shape.Line;
        #endregion
    }
}
