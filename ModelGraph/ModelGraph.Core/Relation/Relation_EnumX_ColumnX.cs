﻿
namespace ModelGraph.Core
{
    public class Relation_EnumX_ColumnX : RelationOf<RelationRoot, EnumX, ColumnX>
    {
        internal override IdKey IdKey => IdKey.Relation_EnumX_ColumnX;

        internal Relation_EnumX_ColumnX(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
