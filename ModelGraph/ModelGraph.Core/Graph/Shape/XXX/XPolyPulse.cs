using System.Collections.Generic;
using System.Numerics;
using Windows.ApplicationModel.Chat;

namespace ModelGraph.Core
{
    internal class XPolyPulse : XPolyline
    {
        protected override ShapeType ShapeType => ShapeType.PolyPulse;
        internal XPolyPulse(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.75f;
            Radius2 = 0.25f;
            AuxFactor = 0.25f;
            Dimension = 7;
            CreatePoints();
        }

        #region PrivateConstructor  ===========================================
        private XPolyPulse(XShape shape)
        {
            CopyData(shape);
        }
        private XPolyPulse(XShape shape, Vector2 p)
        {
            CopyData(shape);
            SetCenter(p);
        }
        #endregion

        #region CreatePoints  =================================================
        protected override void CreatePoints()
        {
            var cp = GetCenter();
            var D = Dimension;
            var n = 0;
            var N = 1 + D;
            DXY = new List<Vector2>(N);

            var (r1, r2, f1) = GetRadius();

            var d = D / 2;
            var ax = r1 * 2 / d; //ax step size
            var bx = ax * f1;
            var dx = -r1;
            var dy = r2;

            Add(dx, -dy);
            dx += bx;
            for (int i = 0; i < D; i++)
            {
                if (Add(dx + bx, dy)) goto Restore;
                dx += ax;
                if (Add(dx - bx, dy)) goto Restore;
                if (Add(dx + bx, -dy)) goto Restore;
                dx += ax;
                if (Add(dx - bx, -dy)) goto Restore;
            }
            Restore:
            TransformPoints(Matrix3x2.CreateRotation(RadiansStart));
            SetCenter(cp);

            bool Add(float x, float y)
            {
                DXY.Add(Limit(x, y));
                return (++n >= N);
            }
        }
        #endregion

        #region RequiredMethods  ==============================================
        internal override XShape Clone() =>new XPolyPulse(this);
        internal override XShape Clone(Vector2 center) => new XPolyPulse(this, center);
        protected override (int min, int max) MinMaxDimension => (2, 19);
        protected override XShapeProperty PropertyFlags => XShapeProperty.Rad1 | XShapeProperty.Rad2 | XShapeProperty.Aux | XShapeProperty.Dim;
        #endregion
    }
}
