
namespace ModelGraph.Core
{
    public class Relation_ViewX_Property : RelationOf<RelationRoot, ViewX, Property>
    {
        internal override IdKey IdKey => IdKey.Relation_ViewX_Property;

        internal Relation_ViewX_Property(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
