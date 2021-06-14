using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class XCircle : CCentral
    {
        protected override ShapeType ShapeType => ShapeType.Circle;
        internal XCircle(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = Radius2 = 0.25f;
            DXY = new List<Vector2>() { new Vector2(0, 0) };
        }

        #region PrivateConstructor  ===========================================
        private XCircle(XShape shape)
        {
            CopyData(shape);
        }
        private XCircle(XShape shape, Vector2 center)
        {
            CopyData(shape);
            Center = center;
        }
        #endregion

        #region RequiredMethods  ==============================================
        internal override XShape Clone() =>new XCircle(this);
        internal override XShape Clone(Vector2 center) => new XCircle(this, center);

        internal override void AddDrawData(DrawData drawData, float size, float scale, Vector2 offset, Coloring colr = Coloring.Normal)
        {
            var (cp, r1, _) = GetCenterRadius(scale, offset);
            var points = new Vector2[] { cp, new Vector2(r1, r1) };
            drawData.AddParms((points, ShapeStrokeWidth(scale/size), ShapeColor(colr)));
        }
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 offset, FlipState flip)
        {
            var (cp, r1, _) = GetCenterRadius(scale, offset);
            var points = new Vector2[] { cp, new Vector2(r1, r1) };
            drawData.AddParms((points, ShapeStrokeWidth(), ShapeColor()));
        }
        protected override void Scale(Vector2 scale)
        {
            if (scale.X == 1)
                Radius1 = Radius2 = (Radius1 * scale.Y);
            else
                Radius1 = Radius2 = (Radius1 * scale.X);
        }
        protected override XShapeProperty PropertyFlags => XShapeProperty.Rad1;
        #endregion
    }
}
