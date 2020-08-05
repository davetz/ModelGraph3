
namespace ModelGraph.Core
{
    public class Relation_QueryX_Property : RelationOf<RelationRoot, QueryX, Property>
    {
        internal override IdKey IdKey => IdKey.Relation_QueryX_Property;

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
