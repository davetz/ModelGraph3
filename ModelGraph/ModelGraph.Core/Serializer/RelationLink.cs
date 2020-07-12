using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    class RelationLink : LinkSerializer, ISerializer
    {
        static Guid _serializerGuid => new Guid("6E4E6626-98BC-483E-AC9B-C7799511ECF2");

        internal RelationLink(Root root, RelationRoot relationStore) : base(relationStore)
        {
            root.RegisterLinkSerializer((_serializerGuid, this));
        }

        public void PopulateItemIndex(Dictionary<Item, int> itemIndex)
        {
            var relationArray = _relationStore.GetRelationArray();
            foreach (var rel in relationArray)
            {
                if (rel.HasLinks)
                {
                    itemIndex[rel] = 0;
                    var len = rel.GetLinks(out List<Item> parents, out List<Item> children);
                    for (int i = 0; i < len; i++)
                    {
                        itemIndex[parents[i]] = 0;
                        itemIndex[children[i]] = 0;
                    }
                }
            }
        }
        public int GetSerializerItemCount() => 0;
        public void RegisterInternal(Dictionary<int, Item> internalItem) { }
    }
}
