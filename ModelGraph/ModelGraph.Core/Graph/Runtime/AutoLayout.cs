namespace ModelGraph.Core
{/*
 */
    public partial class Graph
    {
        internal void CheckLayout()
        {
            var cp = 0;
            foreach (var node in Nodes)
            {
                if (node.TryInitialize(cp)) cp += 8;
            }
            AdjustGraph();
        }
    }
}
