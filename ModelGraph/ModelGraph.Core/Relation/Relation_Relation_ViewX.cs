
namespace ModelGraph.Core
{
    public class Relation_Relation_ViewX : RelationOf<Relation,ViewX>
    {
        internal override IdKey IdKey => IdKey.Relation_ViewX;

        internal Relation_Relation_ViewX(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
