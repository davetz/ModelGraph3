
namespace ModelGraph.Core
{
    public class Relation_GraphX_ToolTipProperty : RelationOf<RelationManager, GraphX, Property>
    {
        internal override IdKey IdKey => IdKey.Relation_GraphX_ToolTipProperty;

        internal Relation_GraphX_ToolTipProperty(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.ManyToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
