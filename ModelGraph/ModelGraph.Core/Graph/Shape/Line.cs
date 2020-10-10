using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class Line : Polyline
    {
        protected override ShapeType ShapeType => ShapeType.Line;
        internal Line(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            DXY = new List<(float dx, float dy)>() {(-0.25f, 0), (0.25f, 0)};
        }

        #region PrivateConstructor  ===========================================
        private Line(Shape shape)
        {
            CopyData(shape);
        }
        private Line(Shape shape, Vector2 center)
        {
            CopyData(shape);
            SetCenter( new Shape[] { this }, center);
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override Shape Clone() => new Line(this);
        internal override Shape Clone(Vector2 center) => new Line(this, center);
        protected override ShapeProperty PropertyFlags => ShapeProperty.Cent;
        #endregion
    }
}
