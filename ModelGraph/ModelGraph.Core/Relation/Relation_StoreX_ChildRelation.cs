
namespace ModelGraph.Core
{
    public class Relation_StoreX_ChildRelation : RelationOf<Store,Relation>
    {
        internal override IdKey IdKey => IdKey.StoreX_ChildRelation;

        internal Relation_StoreX_ChildRelation(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);
            owner.Add(this);
        }
        public override string GetNameId(Root root)
        {
            var myName = Name;
            if (string.IsNullOrWhiteSpace(myName) || myName.StartsWith("?")) myName = string.Empty;

            return $"({myName})    {typeof(Store).Name} --> {typeof(Relation).Name}";

        }
    }
}
