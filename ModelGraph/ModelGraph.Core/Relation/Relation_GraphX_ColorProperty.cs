
namespace ModelGraph.Core
{
    public class Relation_GraphX_ColorProperty : RelationOf<RelationManager, GraphX,Property>
    {
        internal override IdKey IdKey => IdKey.Relation_GraphX_ColorProperty;

        internal Relation_GraphX_ColorProperty(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.ManyToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
