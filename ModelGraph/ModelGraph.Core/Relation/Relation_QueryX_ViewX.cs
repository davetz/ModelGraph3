
namespace ModelGraph.Core
{
    public class Relation_QueryX_ViewX : RelationOf<RelationManager, QueryX, ViewX>
    {
        internal override IdKey IdKey => IdKey.Relation_QueryX_ViewX;

        internal Relation_QueryX_ViewX(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
