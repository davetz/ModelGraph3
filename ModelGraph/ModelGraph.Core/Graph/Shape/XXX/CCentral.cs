using System.Numerics;

namespace ModelGraph.Core
{
    internal abstract class CCentral : XShape
    {
        internal CCentral() { }

        protected Vector2 Center { get { return DXY[0]; } set { DXY[0] = Limit(value.X, value.Y); } }
        protected override XShapeProperty ValidLineProperty => XShapeProperty.LineStyle | XShapeProperty.DashCap | XShapeProperty.LineWidth;


        #region GetCenterRadius  ==============================================
        protected override Vector2 GetCenter() => DXY[0];
        protected (Vector2 cp, float r1, float r2) GetCenterRadius(float scale, Vector2 offset)
        {
            var (r1, r2, _) = GetRadius(scale);
            return (offset + Center * scale, r1, r2);
        }
        protected (Vector2 cp, float r1, float r2) GetCenterRadius(FlipState flip, float scale, Vector2 offset)
        {
            var (r1, r2, _) = GetRadius(scale);

            switch (flip)
            {
                case FlipState.None:
                    return ConvertedPoint();

                case FlipState.VertFlip:
                    var mv = Matrix3x2.CreateScale(1, -1);
                    return TransformedPoint(mv);

                case FlipState.HorzFlip:
                    var mh = Matrix3x2.CreateScale(-1, 1);
                    return TransformedPoint(mh);

                case FlipState.VertHorzFlip:
                    var mb = Matrix3x2.CreateScale(-1, -1);
                    return TransformedPoint(mb);

                case FlipState.LeftRotate:
                    SwapRadius();
                    var ml = Matrix3x2.CreateRotation(FullRadians / 4);
                    return TransformedPoint(ml);

                case FlipState.LeftHorzFlip:
                    SwapRadius();
                    var mlh = Matrix3x2.CreateRotation(FullRadians / 4) * Matrix3x2.CreateScale(-1, 1);
                    return TransformedPoint(mlh);

                case FlipState.RightRotate:
                    SwapRadius();
                    var mr = Matrix3x2.CreateRotation(-FullRadians / 4);
                    return TransformedPoint(mr);

                case FlipState.RightHorzFlip:
                    SwapRadius();
                    var mlr = Matrix3x2.CreateRotation(-FullRadians / 4) * Matrix3x2.CreateScale(-1, 1);
                    return TransformedPoint(mlr);
            }
            return (Vector2.Zero, r1, r2);

            void SwapRadius()
            {
                var t = r1;
                r1 = r2;
                r2 = t;
            }
            (Vector2 cp, float r1, float r2) ConvertedPoint() => (offset + DXY[0] * scale, r1, r2);
            (Vector2 cp, float r1, float r2) TransformedPoint(Matrix3x2 m) => (offset + Vector2.Transform(DXY[0], m) * scale, r1, r2);
        }
        #endregion

        #region OverideAbstract  ==============================================
        protected override (float dx1, float dy1, float dx2, float dy2) GetExtent()
        {
            var r = new Vector2(Radius1, Radius2);
            var p1 = DXY[0] - r;
            var p2 = DXY[0] + r;
            return (p1.X, p1.Y, p2.X, p2.Y);
        }
        protected override void Scale(Vector2 scale)
        {
            Radius1 = Radius1 * scale.X;
            Radius2 = Radius2 * scale.Y;
        }
        #endregion
    }
}
