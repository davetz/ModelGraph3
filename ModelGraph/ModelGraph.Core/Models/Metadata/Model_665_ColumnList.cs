﻿using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_665_ColumnList : List2ModelOf<EnumX, ColumnX>
    {
        internal Model_665_ColumnList(Model_653_Enum owner, EnumX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_665_ColumnList;
        public override string GetNameId() => Item.Owner.Owner.GetNameId(IdKey);

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => DataRoot.Get<Relation_EnumX_ColumnX>().ChildCount(Item);
        protected override IList<ColumnX> GetChildItems() => DataRoot.Get<Relation_EnumX_ColumnX>().TryGetChildren(Item, out IList<ColumnX> list) ? list : new ColumnX[0];
        protected override void CreateChildModel(ColumnX childItem)
        {
            new Model_667_Column(this, childItem);
        }
        #endregion

        //public override void GetMenuCommands(Root root, List<LineCommand> list)
        //{
        //    list.Clear();
        //    list.Add(new RemoveCommand(this, () => ItemUnLinked.Record(root, root.Get<Relation_EnumX_ColumnX>(), )));
        //}
    }
}