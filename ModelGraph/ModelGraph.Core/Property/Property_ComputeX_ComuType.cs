
namespace ModelGraph.Core
{
    public class Property_ComputeX_CompuType : EnumPropertyOf<ComputeX>
    {
        internal override IdKey IdKey => IdKey.ComputeXCompuTypeProperty;

        internal Property_ComputeX_CompuType(PropertyRoot owner) : base(owner, owner.DataRoot.Get<Enum_CompuType>()) { }

        internal override string GetParentName(Root root, Item item) => root.Get<Relation_Store_ComputeX>().TryGetParent(item, out Store p) ? p.GetNameId(root) : InvalidItem;

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).CompuType;

        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).CompuType = (CompuType)key;
    }
}
