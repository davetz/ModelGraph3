using System.Numerics;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public abstract class DrawModel : ChildOf<PageModel>, IDrawModel, ILeadModel
    {
        public PageModel PageModel => Owner;

        #region Constructor  ==================================================
        internal DrawModel(PageModel owner)
        {
            Owner = owner;
            owner.Add(this);
        }
        #endregion

        #region PointerData  ==================================================
        public string ToolTip_Text1 { get; set; }
        public string ToolTip_Text2 { get; set; }

        public Vector2 GridPoint1 { get; set; }
        public Vector2 GridPoint2 { get; set; }

        public Vector2 DrawPoint1 { get; set; }
        public Vector2 DrawPoint2 { get; set; }

        public Vector2 NodePoint1 { get; protected set; }
        public Vector2 NodePoint2 { get; protected set; }

        public Vector2 RegionPoint1 { get; set; }
        public Vector2 RegionPoint2 { get; set; }
        public Vector2 GridPointDelta(bool reset = false)
        {
            var delta = GridPoint2 - GridPoint1;
            if (reset) GridPoint1 = GridPoint2;
            return delta;
        }
        public Vector2 DrawPointDelta(bool reset = false)
        {
            var delta = DrawPoint2 - DrawPoint1;
            if (reset) DrawPoint1 = DrawPoint2;
            return delta;
        }
        #endregion

        #region PointerAction  ================================================
        virtual public bool MoveNode() => false;
        virtual public bool MoveRegion() => false;
        virtual public bool CreateNode() => false;

        #endregion


        #region HitTest  ======================================================
        virtual public bool TapHitTest() => false;
        virtual public bool EndHitTest() => false;
        virtual public bool SkimHitTest() => false;
        virtual public bool DragHitTest() => false;

        public bool AnyHit => _hit != 0;
        public bool PinHit => (_hit & Hit.Pin) != 0;
        public bool NodeHit => (_hit & Hit.Node) != 0;
        public bool EdgeHit => (_hit & Hit.Edge) != 0;
        public bool RegionHit => (_hit & Hit.Region) != 0;
        private Hit _hit;

        protected void ClearHit()
        {
            ToolTip_Text1 = ToolTip_Text2 = string.Empty;
            _hit = Hit.ZZZ;
        }
        protected void SetHitPin() => _hit |= Hit.Pin;
        protected void SetHitNode() => _hit |= Hit.Node;
        protected void SetHitEdge() => _hit |= Hit.Edge;
        protected void SetHitRegion() => _hit |= Hit.Region;

        virtual public bool IsValidRegion() => false;
        virtual public void ClearRegion() { }

        #endregion


        #region Resize  =======================================================
        virtual public void ResizeTop() { }
        virtual public void ResizeLeft() { }
        virtual public void ResizeRight() { }
        virtual public void ResizeBottom() { }
        virtual public void ResizeTopLeft() { }
        virtual public void ResizeTopRight() { }
        virtual public void ResizeBottomLeft() { }
        virtual public void ResizeBottomRight() { }
        virtual public void ResizePropagate() { }
        #endregion


        #region ITreeModel  ===================================================
        public ITreeModel FlyTreeModel { get; protected set; }
        public ITreeModel SideTreeModel { get; protected set; }
        #endregion


        #region Pickers  ======================================================
        public int Picker1Index { get; set; }
        public int Picker2Index { get; set; }

        virtual public int Picker1Width => 0;
        virtual public int Picker2Width => 0;

        virtual public void Picker1Select(int YCord, bool add = false) { }
        virtual public void Picker2Select(int YCord) { }
        virtual public void Picker2Paste() { }
        #endregion

        #region ColorARGB/Apply/Reload  =======================================
        public (byte, byte, byte, byte) ColorARGB { get; set; } = (255, 255, 255, 255);
        public virtual void ColorARGBChanged() { }

        public virtual void Apply() { }
        public virtual void Reload() { }
        #endregion

        #region IDrawData  ====================================================
        virtual public void RefreshEditorData() { }
        virtual public string HeaderTitle => "No Title was specified";

        public IDrawData HelperData => Helper;      // editor layer1  
        protected DrawData Helper = new DrawData(); 

        public IDrawData EditorData => Editor;      // editor layer2
        protected DrawData Editor = new DrawData();

        virtual public Extent EditorExtent => new Extent(100, 100);

        public IDrawData Picker1Data => Picker1;
        protected DrawData Picker1 = new DrawData();

        public IDrawData Picker2Data => Picker2;
        protected DrawData Picker2 = new DrawData();
        #endregion

    }
}

