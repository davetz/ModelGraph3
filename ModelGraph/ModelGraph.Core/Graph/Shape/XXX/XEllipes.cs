using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class XEllipes : CCentral
    {
        protected override ShapeType ShapeType => ShapeType.Ellipse;

        internal XEllipes(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.30f;
            Radius2 = 0.20f;
            DXY = new List<Vector2>() { new Vector2(0, 0) };
        }

        #region PrivateConstructor  ===========================================
        private XEllipes(XShape shape)
        {
            CopyData(shape);
        }
        private XEllipes(XShape shape, Vector2 center)
        {
            CopyData(shape);
            Center = center;
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override XShape Clone() =>new XEllipes(this);
        internal override XShape Clone(Vector2 center) => new XEllipes(this, center);

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
        protected override XShapeProperty PropertyFlags => XShapeProperty.Rad1 | XShapeProperty.Rad2;
        #endregion
    }
}
