﻿
namespace ModelGraph.Core
{
    public class Relation_Store_ChildRelation : RelationOf<Root, Store, Relation>
    {
        internal override IdKey IdKey => IdKey.Relation_Store_ChildRelation;

        internal Relation_Store_ChildRelation(Root owner)
        {
            Owner = owner;
            Pairing = Pairing.ManyToMany;
            IsRequired = true;
            Initialize(25, 25);
        }
        public override string GetNameId()
        {
            var myName = Name;
            if (string.IsNullOrWhiteSpace(myName) || myName.StartsWith("?")) myName = string.Empty;

            return $"({myName})    {typeof(Store).Name} --> {typeof(Relation).Name}";

        }
    }
}
