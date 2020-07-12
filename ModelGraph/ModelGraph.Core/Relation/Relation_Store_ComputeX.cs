
namespace ModelGraph.Core
{
    public class Relation_Store_ComputeX : RelationOf<Store,ComputeX>
    {
        internal override IdKey IdKey => IdKey.Store_ComputeX;

        internal Relation_Store_ComputeX(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
