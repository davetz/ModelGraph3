
namespace ModelGraph.Core
{
    public class Relation_Store_ComputeX : RelationOf<RelationManager, Store, ComputeX>
    {
        internal override IdKey IdKey => IdKey.Relation_Store_ComputeX;

        internal Relation_Store_ComputeX(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
