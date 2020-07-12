
namespace ModelGraph.Core
{
    public class Relation_ComputeX_QueryX : RelationOf<ComputeX,QueryX>
    {
        internal override IdKey IdKey => IdKey.ComputeX_QueryX;

        internal Relation_ComputeX_QueryX(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToOne;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
