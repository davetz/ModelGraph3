using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6E7_Path : List2ModelOf<Path, Path>
    {
        internal Model_6E7_Path(ItemModel owner, Path item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6E7_Path;
        public override string GetNameId() => $"{Item.Head.GetFullNameId()} --> {Item.Tail.GetFullNameId()}";
        public override string GetKindId() => $"{Item.GetNameId()}{GetKind}";
        string GetKind => Item.IsRadial ? Root.GetKindId(IdKey.RadialPath) : Root.GetKindId(IdKey.LinkPath);
        protected override int GetTotalCount() => (Item.Paths is null) ? 0 : Item.Paths.Length;
        protected override IList<Path> GetChildItems() => (Item.Paths is null) ? new Path[0] : Item.Paths;

        protected override void CreateChildModel(Path childItem) => new Model_6E7_Path(this, childItem);
    }
}
