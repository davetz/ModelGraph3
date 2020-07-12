
namespace ModelGraph.Core
{
    public class Relation_Store_NameProperty : RelationOf<Store,Property>
    {
        internal override IdKey IdKey => IdKey.Store_NameProperty;

        internal Relation_Store_NameProperty(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToOne;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
