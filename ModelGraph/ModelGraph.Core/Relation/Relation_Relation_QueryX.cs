
namespace ModelGraph.Core
{
    public class Relation_Relation_QueryX : RelationOf<Relation,QueryX>
    {
        internal override IdKey IdKey => IdKey.Relation_QueryX;

        internal Relation_Relation_QueryX(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
