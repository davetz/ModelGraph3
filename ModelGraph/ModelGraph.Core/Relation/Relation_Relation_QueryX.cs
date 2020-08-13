
namespace ModelGraph.Core
{
    public class Relation_Relation_QueryX : RelationOf<RelationManager, Relation, QueryX>
    {
        internal override IdKey IdKey => IdKey.Relation_Relation_QueryX;

        internal Relation_Relation_QueryX(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
