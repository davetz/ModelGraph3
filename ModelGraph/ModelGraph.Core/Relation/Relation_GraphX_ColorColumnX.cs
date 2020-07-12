
namespace ModelGraph.Core
{
    public class Relation_GraphX_ColorColumnX : RelationOf<GraphX,ColumnX>
    {
        internal override IdKey IdKey => IdKey.GraphX_ColorColumnX;

        internal Relation_GraphX_ColorColumnX(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToOne;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
