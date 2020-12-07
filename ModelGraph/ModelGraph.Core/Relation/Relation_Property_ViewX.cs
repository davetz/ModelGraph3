
namespace ModelGraph.Core
{
    public class Relation_Property_ViewX : RelationOf<RelationRoot, Property, ViewX>
    {
        internal override IdKey IdKey => IdKey.Relation_Property_ViewX;

        internal Relation_Property_ViewX(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
