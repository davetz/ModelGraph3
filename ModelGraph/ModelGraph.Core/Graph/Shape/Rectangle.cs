using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class Rectangle : Central
    {
        protected override ShapeType ShapeType => ShapeType.Rectangle;
        internal Rectangle(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = Radius2 = 0.30f;
            DXY = new List<(float dx, float dy)>() { (0, 0) };
        }

        #region PrivateConstructor  ===========================================
        private Rectangle(Shape shape)
        {
            CopyData(shape);
        }
        private Rectangle(Shape shape, Vector2 center)
        {
            CopyData(shape);
            Center = center;
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override Shape Clone() =>new Rectangle(this);
        internal override Shape Clone(Vector2 center) => new Rectangle(this, center);

        internal override void AddDrawData(DrawData drawData, float size, float scale, Vector2 center, Coloring coloring = Coloring.Normal)
        {
            var (cp, r1, r2) = GetCenterRadius(center, scale);
            var rd = new Vector2(r1,r2);

            drawData.AddShape(((cp, rd), ShapeStrokeWidth(scale / size), ShapeColor(coloring)));
        }
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 center, FlipState flip)
        {
            var (cp, r1, r2) = GetCenterRadius(flip, center, scale);
            var rd = new Vector2(r1, r2);

            drawData.AddShape(((cp, rd), ShapeStrokeWidth(), ShapeColor()));
        }
        protected override ShapeProperty PropertyFlags => ShapeProperty.Vert | ShapeProperty.Horz;
        #endregion
    }
}
