
namespace ModelGraph.Core
{
    public class Relation_Store_ColumnX : RelationOf<RelationRoot, Store, ColumnX>
    {
        internal override IdKey IdKey => IdKey.Relation_Store_ColumnX;

        internal Relation_Store_ColumnX(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
