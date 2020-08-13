﻿
namespace ModelGraph.Core
{
    public class Relation_ViewX_QueryX : RelationOf<RelationManager, ViewX, QueryX>
    {
        internal override IdKey IdKey => IdKey.Relation_ViewX_QueryX;

        internal Relation_ViewX_QueryX(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToOne;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
