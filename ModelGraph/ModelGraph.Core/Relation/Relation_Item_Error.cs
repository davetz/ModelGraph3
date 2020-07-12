
namespace ModelGraph.Core
{
    public class Relation_Item_Error : RelationOf<Item,Error>
    {
        internal override IdKey IdKey => IdKey.Item_Error;

        internal Relation_Item_Error(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }

        public override string GetNameId(Root root)
        {
            var myName = Name;
            if (string.IsNullOrWhiteSpace(myName) || myName.StartsWith("?")) myName = string.Empty;

            return $"({myName})    {typeof(Item).Name} --> {typeof(Error).Name}";

        }
    }
}
