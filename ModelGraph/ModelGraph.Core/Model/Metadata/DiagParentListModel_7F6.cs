
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class DiagParentListModel_7F6 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal DiagParentListModel_7F6(DiagRelationModel_7F4 owner, Relation item) : base(owner, item) { }
        private Relation RX => Item as Relation;
        internal override IdKey IdKey => IdKey.DiagParentListModel_7F6;

        public override bool CanExpandLeft => TotalCount > 0;
        public override bool CanFilter => TotalCount > 1;
        public override bool CanSort => TotalCount > 1;
        public override int TotalCount => RX.GetParentLinkPairCount();


        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            if (TotalCount > 0)
            {
                IsExpandedLeft = true;

                foreach (var pair in RX.GetParentLinkPairList())
                {
                    new DiagChildParentModel_7F8(this, RX, pair);
                }
            }
            return true;
        }

        internal override bool Validate(Root root, Dictionary<Item, LineModel> prev)
        {
            var prev2 = new Dictionary<(Item, Item), LineModel>(Count);

            var viewListChanged = false;
            if (IsExpanded || AutoExpandLeft)
            {
                AutoExpandLeft = false;
                IsExpandedLeft = true;

                if (ChildDelta != Item.ChildDelta)
                {
                    ChildDelta = Item.ChildDelta;

                    prev.Clear();
                    foreach (var child in Items)
                    {
                        var pc = child as DiagChildParentModel_7F8;
                        prev2[pc.ItemPair] = pc;
                    }
                    CovertClear();

                    if (TotalCount > 0)
                    {
                        foreach (var pair in RX.GetParentLinkPairList())
                        {

                            if (prev2.TryGetValue(pair, out LineModel m))
                            {
                                CovertAdd(m);
                                prev.Remove(m.Item);
                            }
                            else
                            {
                                new DiagChildParentModel_7F8(this, RX, pair);
                                viewListChanged = true;
                            }
                        }
                    }

                    if (prev2.Count > 0)
                    {
                        viewListChanged = true;
                        foreach (var model in prev2.Values) { model.Discard(); }
                    }
                }
            }
            return viewListChanged || base.Validate(root, prev);
        }
    }
}
