
namespace ModelGraph.Core
{
    public class Relation_Store_SummaryProperty : RelationOf<RelationRoot, Store, Property>
    {
        internal override IdKey IdKey => IdKey.Relation_Store_SummaryProperty;

        internal Relation_Store_SummaryProperty(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToOne;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
