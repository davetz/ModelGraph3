
namespace ModelGraph.Core
{
    public class Relation_QueryX_Property : RelationOf<QueryX,Property>
    {
        internal override IdKey IdKey => IdKey.QueryX_Property;

        internal Relation_QueryX_Property(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
