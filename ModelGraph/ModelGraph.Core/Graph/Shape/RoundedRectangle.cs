using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class RoundedRectangle : Central
    {
        protected override ShapeType ShapeType => ShapeType.RoundedRectangle;
        internal RoundedRectangle(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.5f;
            Radius2 = 0.3f;
            DXY = new List<(float dx, float dy)>() { (0, 0) };
        }
        internal float Corner => 0.1f;

        #region PrivateConstructor  ===========================================
        internal RoundedRectangle(Shape shape)
        {
            CopyData(shape);
        }

        internal RoundedRectangle(Shape shape, Vector2 p)
        {
            CopyData(shape);
            Center = p;
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override Shape Clone() =>new RoundedRectangle(this);
        internal override Shape Clone(Vector2 center) => new RoundedRectangle(this, center);

        internal override void AddDrawData(DrawData drawData, float size, float scale, Vector2 center, Coloring coloring = Coloring.Normal)
        {
            var (cp, r1, r2) = GetCenterRadius(center, scale);
            var rd = new Vector2(r1, r2);

            drawData.AddShape(((cp, rd), ShapeStrokeWidth(scale / size), ShapeColor(coloring)));
        }
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 center, FlipState flip)
        {
            var (cp, r1, r2) = GetCenterRadius(flip, center, scale);
            var rd = new Vector2(r1, r2);

            drawData.AddShape(((cp, rd), ShapeStrokeWidth(), ShapeColor()));
        }
        protected override ShapeProperty PropertyFlags => ShapeProperty.Rad1 | ShapeProperty.Rad2;
        #endregion
    }
}
