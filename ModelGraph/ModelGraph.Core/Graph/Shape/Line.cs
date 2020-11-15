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

            DXY = new List<Vector2>() {new Vector2(-0.25f, 0), new Vector2(0.25f, 0)};
        }

        #region PrivateConstructor  ===========================================
        private Line(Shape shape)
        {
            CopyData(shape);
        }
        private Line(Shape shape, Vector2 p)
        {
            CopyData(shape);
            SetCenter( p);
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override Shape Clone() => new Line(this);
        internal override Shape Clone(Vector2 center) => new Line(this, center);
        protected override ShapeProperty PropertyFlags => ShapeProperty.Size;
        protected override (float dx1, float dy1, float dx2, float dy2) GetExtent()
        {
            var (dx1, dy1, dx2, dy2) = base.GetExtent();
            if (dx1 == dx2) { dx1 -= .05f; dx2 += 0.5f; }
            if (dy1 == dy2) { dy1 -= .05f; dy2 += 0.5f; }
            return (dx1, dy1, dx2, dy2);
        }
        #endregion
    }
}
