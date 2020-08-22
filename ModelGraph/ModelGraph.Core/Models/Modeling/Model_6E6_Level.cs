using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6E6_Level : List2ModelOf<Level, Path>
    {
        internal Model_6E6_Level(Model_6E5_LevelList owner, Level item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6E6_Level;

        protected override int GetTotalCount() => Item.Count;
        protected override IList<Path> GetChildItems() => Item.Paths;

        protected override void CreateChildModel(Path childItem) => new Model_6E7_Path(this, childItem);
    }
}
