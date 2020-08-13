
namespace ModelGraph.Core
{
    public class Relation_SymbolX_QueryX : RelationOf<RelationManager, SymbolX, QueryX>
    {
        internal override IdKey IdKey => IdKey.Relation_SymbolX_QueryX;

        internal Relation_SymbolX_QueryX(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
