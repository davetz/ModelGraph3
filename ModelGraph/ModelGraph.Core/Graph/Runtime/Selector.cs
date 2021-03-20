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


        #region Properties  ===================================================
        internal bool IsVoidHit => (HitLocation == HitLocation.Void);
        internal bool IsNodeHit => ((HitLocation & HitLocation.Node) != 0);
        internal bool IsEdgeHit => ((HitLocation & HitLocation.Edge) != 0);
        internal bool IsTopHit => ((HitLocation & HitLocation.Top) != 0);
        internal bool IsLeftHit => ((HitLocation & HitLocation.Left) != 0);
        internal bool IsRightHit => ((HitLocation & HitLocation.Right) != 0);
        internal bool IsBottomHit => ((HitLocation & HitLocation.Bottom) != 0);
        internal bool IsCenterHit => ((HitLocation & HitLocation.Center) != 0);
        internal bool IsSideHit => ((HitLocation & HitLocation.SideOf) != 0);

        internal bool IsEnd1Hit => ((HitLocation & HitLocation.End1) != 0);
        internal bool IsEnd2Hit => ((HitLocation & HitLocation.End2) != 0);
        internal bool IsBendHit => ((HitLocation & HitLocation.Bend) != 0);
        #endregion

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
