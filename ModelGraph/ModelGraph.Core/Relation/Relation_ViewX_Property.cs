
namespace ModelGraph.Core
{
    public class Relation_ViewX_Property : RelationOf<RelationManager, ViewX, Property>
    {
        internal override IdKey IdKey => IdKey.Relation_ViewX_Property;

        internal Relation_ViewX_Property(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
