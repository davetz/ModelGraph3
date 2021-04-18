using System;
using System.Numerics;

namespace ModelGraph.Core
{
    internal abstract class Selector
    {
        internal Vector2 HitPoint;  //refined hit point location
        internal Vector2 RefPoint;  //saved point reference

        internal HitLocation HitLocation;  //hit location details
        internal HitLocation RefLocation;  //saved hit reference

        internal Extent Extent = new Extent(); // selector rectangle
        protected bool _enableSnapshot = true;

        #region RequiredMethods  ==============================================
        internal abstract void HitTestPoint(Vector2 p);
        internal abstract void HitTestRegion(bool toggleMode, Vector2 p1, Vector2 p2);
        internal abstract void SaveHitReference();
        internal abstract void Clear();
        internal abstract void Move(Vector2 delta);
        internal abstract void FlipVert();
        internal abstract void FlipHorz();
        internal abstract void AlignVertLeft();
        internal abstract void AlignVertRight();
        internal abstract void AlignVertCenter();
        internal abstract void AlignHorzTop();
        internal abstract void AlignHorzCenter();
        internal abstract void AlignHorzBottom();
        protected abstract void Rotate(Matrix3x2 mx, Vector2 atPoint);
        internal void Rotate(float degree, Vector2 atPoint) => Rotate(Matrix3x2.CreateRotation((float)(degree * Math.PI / 180)), atPoint);
        #endregion
    }
}
