
namespace ModelGraph.Core
{
    public class Relation_GraphX_QueryX : RelationOf<GraphX,QueryX>
    {
        internal override IdKey IdKey => IdKey.GraphX_QueryX;

        internal Relation_GraphX_QueryX(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
