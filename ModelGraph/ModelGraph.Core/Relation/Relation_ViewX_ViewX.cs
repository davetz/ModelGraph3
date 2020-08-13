
namespace ModelGraph.Core
{
    public class Relation_ViewX_ViewX : RelationOf<RelationManager, ViewX, ViewX>
    {
        internal override IdKey IdKey => IdKey.Relation_ViewX_ViewX;

        internal Relation_ViewX_ViewX(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
