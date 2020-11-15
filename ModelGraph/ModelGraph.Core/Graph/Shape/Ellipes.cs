using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class Ellipes : Central
    {
        protected override ShapeType ShapeType => ShapeType.Ellipse;

        internal Ellipes(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.30f;
            Radius2 = 0.20f;
            DXY = new List<Vector2>() { new Vector2(0, 0) };
        }

        #region PrivateConstructor  ===========================================
        private Ellipes(Shape shape)
        {
            CopyData(shape);
        }
        private Ellipes(Shape shape, Vector2 center)
        {
            CopyData(shape);
            Center = center;
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override Shape Clone() =>new Ellipes(this);
        internal override Shape Clone(Vector2 center) => new Ellipes(this, center);

        internal override void AddDrawData(DrawData drawData, float size, float scale, Vector2 offset, Coloring coloring = Coloring.Normal)
        {
            var (cp, r1, r2) = GetCenterRadius(scale, offset);
            var rd = new Vector2(r1, r2);
            var points = new Vector2[] { cp, new Vector2(r1, r2) };
            drawData.AddParms((points, ShapeStrokeWidth(scale / size), ShapeColor(coloring)));

        }
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 offset, FlipState flip)
        {
            var (cp, r1, r2) = GetCenterRadius(flip, scale, offset);
            var rd = new Vector2(r1, r2);
            var points = new Vector2[] { cp, new Vector2(r1, r2) };
            drawData.AddParms((points, ShapeStrokeWidth(), ShapeColor()));
        }
        protected override ShapeProperty PropertyFlags => ShapeProperty.Rad1 | ShapeProperty.Rad2;
        #endregion
    }
}
