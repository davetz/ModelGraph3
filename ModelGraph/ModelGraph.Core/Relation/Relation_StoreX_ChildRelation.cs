
namespace ModelGraph.Core
{
    public class Relation_StoreX_ChildRelation : RelationOf<RelationManager, Store, Relation>
    {
        internal override IdKey IdKey => IdKey.Relation_StoreX_ChildRelation;

        internal Relation_StoreX_ChildRelation(RelationManager owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);
            owner.Add(this);
        }
        public override string GetNameId()
        {
            var myName = Name;
            if (string.IsNullOrWhiteSpace(myName) || myName.StartsWith("?")) myName = string.Empty;

            return $"({myName})    {typeof(Store).Name} --> {typeof(Relation).Name}";

        }
    }
}
