using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_644_GraphList : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal Model_644_GraphList(Model_623_MetadataRoot owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_644_GraphList;

        internal override bool Validate(Root root, Dictionary<Item, LineModel> prev)
        {
            var viewListChange = false;
            if (!IsExpanded) return false;
            if (ChildDelta == Item.ChildDelta) return false;
            ChildDelta = Item.ChildDelta;

            prev.Clear();
            foreach (var child in Items)
            {
                prev[child.Item] = child;
            }
            CovertClear();

            var st = Item as GraphXRoot;
            foreach (var gx in st.Items)
            {
                if (prev.TryGetValue(gx, out LineModel m))
                {
                    CovertAdd(m);
                    prev.Remove(m);
                }
                else
                {
                    viewListChange = true;
                    new Model_655_Graph(this, gx);
                }
            }

            if (prev.Count > 0)
            {
                viewListChange = true;
                foreach (var model in prev.Values) { model.Discard(); }
            }

            return viewListChange | base.Validate(root, prev);
        }
    }
}
