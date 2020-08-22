using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6E5_LevelList : List2ModelOf<Graph, Level>
    {
        internal Model_6E5_LevelList(Model_6A5_Graph owner, Graph item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6E5_LevelList;
        public override string GetNameId() => Root.GetNameId(IdKey);

        protected override int GetTotalCount() => Item.Levels.Count;
        protected override IList<Level> GetChildItems() => Item.Levels;

        protected override void CreateChildModel(Level childItem) => new Model_6E6_Level(this, childItem);
    }
}
