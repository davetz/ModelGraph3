
namespace ModelGraph.Core
{
    public class Relation_GraphX_SymbolX : RelationOf<RelationManager, GraphX, SymbolX>
    {
        internal override IdKey IdKey => IdKey.Relation_GraphX_SymbolX;

        internal Relation_GraphX_SymbolX(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
