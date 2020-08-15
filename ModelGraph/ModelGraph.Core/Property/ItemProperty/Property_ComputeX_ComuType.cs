
namespace ModelGraph.Core
{
    public class Property_ComputeX_CompuType : EnumPropertyOf<ComputeX>
    {
        internal override IdKey IdKey => IdKey.ComputeXCompuTypeProperty;

        internal Property_ComputeX_CompuType(PropertyManager owner) : base(owner, owner.GetRoot().Get<Enum_CompuType>()) { }

        internal override string GetTargetName(Item item) => item.GetParent().GetNameId();

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).CompuType;

        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).SetCompuType((CompuType)key);
    }
}
