
namespace ModelGraph.Core
{
    public class Relation_QueryX_ViewX : RelationOf<QueryX,ViewX>
    {
        internal override IdKey IdKey => IdKey.QueryX_ViewX;

        internal Relation_QueryX_ViewX(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
