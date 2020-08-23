using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public class GraphModel : Item, IDataModel
    {
        internal readonly Root Owner;
        internal override Item GetOwner() => Owner;
        internal readonly Graph Graph;
        internal readonly GraphCanvas GraphCanvas;

        #region Constructor  ==================================================
        internal GraphModel(Root root, Graph graph)
        {
            Owner = root;
            Graph = graph;
            GraphCanvas = new GraphCanvas(this, graph);
            root.Add(this);
        }        
        #endregion

        #region IDataModel  ===================================================
        public string TitleName => "TestModelCanvasControl";
        public string TitleSummary => string.Empty;
        public ControlType ControlType => ControlType.GraphDisplay;
        public IPageControl PageControl { get; set; }
        public void Release()
        {
            Owner.Remove(this);
            Discard(); //discard myself and recursivly discard all my children
        }
        public IDrawCanvas GetDrawCanvas(CanvasId id) => GraphCanvas;

        public void TriggerUIRefresh()
        {
            if (Graph.ChildDelta != ChildDelta)
            {
                ChildDelta = Graph.ChildDelta;
                PageControl?.Refresh();
            }
        }
        #endregion
    }
}

