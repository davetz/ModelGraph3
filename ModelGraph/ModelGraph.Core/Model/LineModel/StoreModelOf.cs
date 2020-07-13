
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;

namespace ModelGraph.Core
{
    public abstract class StoreModelOf<T1> : LineModel where T1 : Item 
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal StoreModelOf(LineModel owner, StoreOf<T1> item) : base(owner, item) { }
        protected StoreOf<T1> ST => Item as StoreOf<T1>;

        public override bool CanExpandLeft => TotalCount > 0;
        public override bool CanFilter => TotalCount > 1;
        public override bool CanSort => TotalCount > 1;
        public override int TotalCount => ST.Count;

        internal abstract void CreateChildModel(LineModel parentModel, T1 childItem);
        
        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;

            IsExpandedLeft = true;

            foreach (var itm in ST.Items)
            {
                CreateChildModel(this, itm);
            }

            return true;
        }
        internal override bool Validate(Root root, Dictionary<Item, LineModel> prev)
        {
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
                        prev[child.Item] = child;
                    }
                    CovertClear();

                    foreach (var itm in ST.Items)
                    {
                        if (prev.TryGetValue(itm, out LineModel m))
                        {
                            CovertAdd(m);
                            prev.Remove(m.Item);
                        }
                        else
                        {
                            CreateChildModel(this, itm);
                            viewListChanged = true;
                        }
                    }

                    if (prev.Count > 0)
                    {
                        viewListChanged = true;
                        foreach (var model in prev.Values) { model.Discard(); }
                    }
                }
            }
            return viewListChanged || base.Validate(root, prev);
        }

    }
}
