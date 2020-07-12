
namespace ModelGraph.Core
{
    public class Property_SymbolX_Attatch : EnumPropertyOf<SymbolX>
    {
        internal override IdKey IdKey => IdKey.SymbolXAttatchProperty;

        internal Property_SymbolX_Attatch(PropertyRoot owner) : base(owner, owner.DataRoot.Get<Enum_Attach>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).Attach;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).Attach = (Attach)key;
    }
}
