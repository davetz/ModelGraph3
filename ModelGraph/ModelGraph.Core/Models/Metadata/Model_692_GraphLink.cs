using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class Model_692_GraphLink : List2ModelOf<QueryX, QueryX>
    {
        internal GraphX Aux1 { get; private set; }
        internal override IdKey IdKey => _idKey;
        private IdKey _idKey = IdKey.Model_692_GraphLink;

        internal Model_692_GraphLink(LineModel owner, QueryX item, GraphX aux1, IdKey idKey) : base(owner, item) { Aux1 = aux1; _idKey = idKey; }
        public override string GetKindId() => Item.Owner.Owner.GetKindId(IdKey);

        protected override int GetTotalCount() => Aux1.Owner.GetTotalCount(this);
        protected override IList<QueryX> GetChildItems() => Aux1.Owner.GetChildItems(this);
        protected override void CreateChildModel(QueryX childItem) => new Model_692_GraphLink(this, childItem, Aux1, GetChildIdKey());
        private IdKey GetChildIdKey() 
        {
            if (IdKey == IdKey.Model_693_GraphPathHead) return IdKey.Model_694_GraphPathLink;
            if (IdKey == IdKey.Model_694_GraphPathLink) return IdKey.Model_694_GraphPathLink;
            if (IdKey == IdKey.Model_695_GraphGroupHead) return IdKey.Model_696_GraphGroupLink;
            if (IdKey == IdKey.Model_696_GraphGroupLink) return IdKey.Model_696_GraphGroupLink;
            if (IdKey == IdKey.Model_697_GraphEgressHead) return IdKey.Model_698_GraphEgressLink;
            if (IdKey == IdKey.Model_698_GraphEgressLink) return IdKey.Model_698_GraphEgressLink;

            return IdKey.Model_692_GraphLink;
        }

        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeManager>().RemoveItem(Item)));

            if (Aux1.Owner.IsRadialSequence(this))
            {
                if (IdKey == IdKey.Model_692_GraphLink)
                {
                    list.Add(new MakePathHeadCommand(this, MakePathSequence));
                    list.Add(new MakeGroupHeadCommand(this, MakeGroupSequence));
                    list.Add(new MakeEgressHeadCommand(this, MakeEgressSequence));
                }
                else if (IdKey == IdKey.Model_693_GraphPathHead)
                {
                    list.Add(new MakeGraphLinkCommand(this, MakeLinkSequence));
                    list.Add(new MakeGroupHeadCommand(this, MakeGroupSequence));
                    list.Add(new MakeEgressHeadCommand(this, MakeEgressSequence));
                }
                else if (IdKey == IdKey.Model_695_GraphGroupHead)
                {
                    list.Add(new MakeGraphLinkCommand(this, MakeLinkSequence));
                    list.Add(new MakePathHeadCommand(this, MakePathSequence));
                    list.Add(new MakeEgressHeadCommand(this, MakeEgressSequence));
                }
                else if (IdKey == IdKey.Model_697_GraphEgressHead)
                {
                    list.Add(new MakeGraphLinkCommand(this, MakeLinkSequence));
                    list.Add(new MakePathHeadCommand(this, MakePathSequence));
                    list.Add(new MakeGroupHeadCommand(this, MakeGroupSequence));
                }
            }
        }

        private void MakeLinkSequence()
        {
            CollapseRefresh();
            _idKey = IdKey.Model_692_GraphLink;
            Aux1.Owner.ConvertQuerySequence(this, QueryType.Graph);
        }
        private void MakePathSequence()
        {
            CollapseRefresh();
            _idKey = IdKey.Model_693_GraphPathHead;
            Aux1.Owner.ConvertQuerySequence(this, QueryType.Path);
        }
        private void MakeGroupSequence()
        {
            CollapseRefresh();
            _idKey = IdKey.Model_695_GraphGroupHead;
            Aux1.Owner.ConvertQuerySequence(this, QueryType.Group);
        }
        private void MakeEgressSequence()
        {
            CollapseRefresh();
            _idKey = IdKey.Model_697_GraphEgressHead;
            Aux1.Owner.ConvertQuerySequence(this, QueryType.Egress);
        }
        private void CollapseRefresh()
        {
            CollapseLeft();
            ChildDelta -= 3;
            ModelDelta -= 3;
            Owner.ChildDelta -= 3;
        }

        internal override DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop) => Aux1.Owner.ModelDrop(this, dropModel, doDrop) ? DropAction.Link : DropAction.None;

        public override bool CanExpandRight => true;
        internal override bool ExpandRight(Root root)
        {
            if (!IsExpandedRight)
            {
                IsExpandedRight = true;

                root.Get<Property_QueryX_Relation>().CreatePropertyModel(this, Item);
                root.Get<Property_QueryX_IsReversed>().CreatePropertyModel(this, Item);
                if (Item.QueryKind == QueryType.Path)
                {
                    root.Get<Property_QueryX_IsBreakPoint>().CreatePropertyModel(this, Item);
                }
                if (IdKey == IdKey.Model_693_GraphPathHead)
                {
                    root.Get<Property_QueryX_ExclusiveKey>().CreatePropertyModel(this, Item);
                    root.Get<Property_QueryX_Where>().CreatePropertyModel(this, Item);
                    root.Get<Property_QueryX_LineColor>().CreatePropertyModel(this, Item);
                    root.Get<Property_QueryX_LineStyle>().CreatePropertyModel(this, Item);
                    root.Get<Property_QueryX_DashStyle>().CreatePropertyModel(this, Item);
                    root.Get<Property_QueryX_Facet1>().CreatePropertyModel(this, Item);
                    root.Get<Property_QueryX_Connect1>().CreatePropertyModel(this, Item);
                    root.Get<Property_QueryX_Facet2>().CreatePropertyModel(this, Item);
                    root.Get<Property_QueryX_Connect2>().CreatePropertyModel(this, Item);
                }
                else
                {
                    root.Get<Property_QueryX_Where>().CreatePropertyModel(this, Item);
                }
                return true;
            }
            return false;
        }

    }
}
