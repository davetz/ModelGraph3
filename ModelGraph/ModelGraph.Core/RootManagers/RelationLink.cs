using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    class RelationLink : LinkSerializer, ISerializer
    {
        static Guid _serializerGuid => new Guid("6E4E6626-98BC-483E-AC9B-C7799511ECF2");

        internal RelationLink(Root root, RelationManager relationStore) : base(relationStore)
        {
            root.RegisterLinkSerializer((_serializerGuid, this));
        }

        public void PopulateItemIndex(Dictionary<Item, int> itemIndex) { }
        public int GetSerializerItemCount() => 0;
    }
}
