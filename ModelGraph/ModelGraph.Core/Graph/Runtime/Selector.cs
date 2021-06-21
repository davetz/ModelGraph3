using System;
using System.Numerics;

namespace ModelGraph.Core
{
    public abstract class Selector : Item
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
        protected abstract void Rotate(Matrix3x2 mx);
        internal void Rotate(float radians) => Rotate(Matrix3x2.CreateRotation((float)(radians), HitPoint));
        #endregion
    }
}
