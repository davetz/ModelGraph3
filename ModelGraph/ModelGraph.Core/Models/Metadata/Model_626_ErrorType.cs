
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_626_ErrorType : List1ModelOf<Error>
    {
        internal Model_626_ErrorType(Model_621_ErrorRoot owner, Error item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_626_ErrorType;

        public override int TotalCount => Item.Count;
        public override bool CanExpandLeft => true;
        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            for (int i = 0; i < Item.Count; i++)
            {
                new Model_627_ErrorText(this, Item, i);
            }
            return true;
        }
    }
}
