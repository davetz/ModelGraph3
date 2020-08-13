
namespace ModelGraph.Core
{
    public class Relation_GraphX_QueryX : RelationOf<RelationManager, GraphX, QueryX>
    {
        internal override IdKey IdKey => IdKey.Relation_GraphX_QueryX;

        internal Relation_GraphX_QueryX(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
