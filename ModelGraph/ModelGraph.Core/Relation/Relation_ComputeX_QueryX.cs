
namespace ModelGraph.Core
{
    public class Relation_ComputeX_QueryX : RelationOf<RelationManager, ComputeX, QueryX>
    {
        internal override IdKey IdKey => IdKey.Relation_ComputeX_QueryX;

        internal Relation_ComputeX_QueryX(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToOne;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
