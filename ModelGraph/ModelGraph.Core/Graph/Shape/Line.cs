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
        private Line(Shape shape, Vector2 p)
        {
            CopyData(shape);
            SetCenter( p.X, p.Y);
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override Shape Clone() => new Line(this);
        internal override Shape Clone(Vector2 center) => new Line(this, center);
        protected override ShapeProperty PropertyFlags => ShapeProperty.Cent;
        protected override (float dx1, float dy1, float dx2, float dy2) GetExtent()
        {
            var (dx1, dy1, dx2, dy2) = base.GetExtent();
            var dw = SW / 100f;
            if (SW == 0) dw = .01f;
            return (dx1 - dw, dy1 - dw, dx2 + dw, dy2 + dw);
        }
        #endregion
    }
}
