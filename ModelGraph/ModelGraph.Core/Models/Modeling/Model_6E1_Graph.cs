﻿using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6E1_Graph : List2ModelOf<GraphX, Graph>
    {
        internal Model_6E1_Graph(ItemModel owner, GraphX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6E1_Graph;

        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            var pageModel = GetPageModel();
            list.Clear();
            list.Add(new CreateCommand(this, () => { IsExpandedLeft = true; pageModel.NewView((p) => { new GraphModel(p, Item.CreateGraph()); }, ControlType.ComplexDraw); } ));
        }

        protected override int GetTotalCount() => Item.Count;
        protected override IList<Graph> GetChildItems() => Item.Items;

        protected override void CreateChildModel(Graph childItem) => new Model_6A5_Graph(this, childItem);
    }
}
