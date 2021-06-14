using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class XRectangle : CCentral
    {
        protected override ShapeType ShapeType => ShapeType.CenterRect;
        internal XRectangle(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = Radius2 = 0.30f;
            DXY = new List<Vector2>() { new Vector2(0, 0) };
        }

        #region PrivateConstructor  ===========================================
        private XRectangle(XShape shape)
        {
            CopyData(shape);
        }
        private XRectangle(XShape shape, Vector2 p)
        {
            CopyData(shape);
            Center = p;
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override XShape Clone() =>new XRectangle(this);
        internal override XShape Clone(Vector2 center) => new XRectangle(this, center);

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
        protected override XShapeProperty PropertyFlags => XShapeProperty.Rad1 | XShapeProperty.Rad2;
        #endregion
    }
}
