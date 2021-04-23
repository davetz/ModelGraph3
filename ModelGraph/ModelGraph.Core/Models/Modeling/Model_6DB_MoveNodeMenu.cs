using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6DB_MoveNodeMenu : List1ModelOf<GraphModel>
    {
        internal Model_6DB_MoveNodeMenu(TreeModel owner, GraphModel item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6DB_MoveNodeMenu;
        public override string GetNameId() => Root.GetNameId(IdKey);

        public override bool CanExpandRight => true;
        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            new Model_6F1_VertAlign(this, Item.Selector);
            new Model_6F2_HorzAlign(this, Item.Selector);
            new Model_6F3_Flip(this, Item.Selector);
            new Model_6F4_Rotate(this, Item.Selector, Angles.LR9045);
            return true;
        }
    }
}
