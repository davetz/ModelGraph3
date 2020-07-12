
namespace ModelGraph.Core
{
    public class Property_Relation_Pairing : EnumPropertyOf<Relation>
    {
        internal override IdKey IdKey => IdKey.RelationPairingProperty;

        internal Property_Relation_Pairing(PropertyRoot owner) : base(owner, owner.DataRoot.Get<Enum_Pairing>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).Pairing;

        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).TrySetPairing((Pairing)key);
    }
}
