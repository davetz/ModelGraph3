﻿
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_621_ErrorRoot : List2ModelOf<ErrorRoot, Error>
    {
        internal Model_621_ErrorRoot(Model_612_Root owner, ErrorRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_621_ErrorRoot;

        protected override int GetTotalCount() => Item.Count;
        protected override IList<Error> GetChildItems() => Item.Items;
        protected override void CreateChildModel(Error childItem)
        {
            new Model_626_ErrorType(this, childItem);
        }
    }
}
