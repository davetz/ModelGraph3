
namespace ModelGraph.Core
{
    public class Relation_GraphX_ColorColumnX : RelationOf<RelationManager, GraphX,ColumnX>
    {
        internal override IdKey IdKey => IdKey.Relation_GraphX_ColorColumnX;

        internal Relation_GraphX_ColorColumnX(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToOne;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
