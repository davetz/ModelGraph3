﻿
namespace ModelGraph.Core
{
    public class Relation_QueryX_QueryX : RelationOf<RelationRoot, QueryX, QueryX>
    {
        internal override IdKey IdKey => IdKey.Relation_QueryX_QueryX;

        internal Relation_QueryX_QueryX(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
