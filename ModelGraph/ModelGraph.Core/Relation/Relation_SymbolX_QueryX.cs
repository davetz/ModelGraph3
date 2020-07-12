
namespace ModelGraph.Core
{
    public class Relation_SymbolX_QueryX : RelationOf<SymbolX,QueryX>
    {
        internal override IdKey IdKey => IdKey.SymbolX_QueryX;

        internal Relation_SymbolX_QueryX(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
