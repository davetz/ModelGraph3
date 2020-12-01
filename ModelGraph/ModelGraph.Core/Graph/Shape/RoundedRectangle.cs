using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class RoundedRectangle : Central
    {
        protected override ShapeType ShapeType => ShapeType.RoundedRect;
        internal RoundedRectangle(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.5f;
            Radius2 = 0.3f;
            DXY = new List<Vector2>() { new Vector2(0, 0) };
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

        internal override void AddDrawData(DrawData drawData, float size, float scale, Vector2 offset, Coloring coloring = Coloring.Normal)
        {
            var (cp, r1, r2) = GetCenterRadius(scale, offset);
            var points = new Vector2[] { cp, new Vector2(r1, r2) };
            drawData.AddParms((points, ShapeStrokeWidth(scale / size), ShapeColor(coloring)));
        }
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 offset, FlipState flip)
        {
            var (cp, r1, r2) = GetCenterRadius(flip, scale, offset);
            var points = new Vector2[] { cp, new Vector2(r1, r2) };
            drawData.AddParms((points, ShapeStrokeWidth(), ShapeColor()));
        }
        protected override ShapeProperty PropertyFlags => ShapeProperty.Rad1 | ShapeProperty.Rad2;
        #endregion
    }
}
