
namespace ModelGraph.Core
{
    public class Relation_Relation_ViewX : RelationOf<RelationManager, Relation, ViewX>
    {
        internal override IdKey IdKey => IdKey.Relation_Relation_ViewX;

        internal Relation_Relation_ViewX(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
