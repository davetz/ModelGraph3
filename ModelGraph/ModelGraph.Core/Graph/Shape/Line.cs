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
        protected override void CreatePoints() { }
        #endregion
    }
}
