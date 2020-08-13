
namespace ModelGraph.Core
{
    public class Relation_Store_SummaryProperty : RelationOf<RelationManager, Store, Property>
    {
        internal override IdKey IdKey => IdKey.Relation_Store_SummaryProperty;

        internal Relation_Store_SummaryProperty(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToOne;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
