
namespace ModelGraph.Core
{
    public class Relation_Store_NameProperty : RelationOf<RelationManager, Store, Property>
    {
        internal override IdKey IdKey => IdKey.Relation_Store_NameProperty;

        internal Relation_Store_NameProperty(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToOne;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
