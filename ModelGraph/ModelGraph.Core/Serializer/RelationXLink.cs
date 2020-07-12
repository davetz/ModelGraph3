using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    class RelationXLink : LinkSerializer, ISerializer
    {
        static Guid _serializerGuid => new Guid("61662F08-F43A-44D9-A9BB-9B0126492B8C");

        internal RelationXLink(Root root, RelationXRoot relationStore) : base(relationStore)
        {
            root.RegisterLinkSerializer((_serializerGuid, this));
        }

        public void PopulateItemIndex(Dictionary<Item, int> itemIndex) { }

        public int GetSerializerItemCount() => 0;
    }
}
