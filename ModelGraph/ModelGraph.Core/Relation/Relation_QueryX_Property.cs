
namespace ModelGraph.Core
{
    public class Relation_QueryX_Property : RelationOf<RelationManager, QueryX, Property>
    {
        internal override IdKey IdKey => IdKey.Relation_QueryX_Property;

        internal Relation_QueryX_Property(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.ManyToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
