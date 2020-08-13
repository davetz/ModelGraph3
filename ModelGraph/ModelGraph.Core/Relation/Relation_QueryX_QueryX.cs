
namespace ModelGraph.Core
{
    public class Relation_QueryX_QueryX : RelationOf<RelationManager, QueryX, QueryX>
    {
        internal override IdKey IdKey => IdKey.Relation_QueryX_QueryX;

        internal Relation_QueryX_QueryX(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
