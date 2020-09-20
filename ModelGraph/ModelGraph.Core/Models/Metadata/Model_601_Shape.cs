
namespace ModelGraph.Core
{
    public class Model_601_Shape : List1ModelOf<SymbolModel>
    {
        internal Model_601_Shape(TreeModel owner, SymbolModel item) : base(owner, item) {}
        internal override IdKey IdKey => IdKey.Model_601_Shape;

        public override string GetNameId() => "Shape";
        public override string GetSummaryId() => string.Empty;

        public override bool CanExpandRight => true;
        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            root.Get<Property_Shape_Color>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_IsFilled>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_StrokeWidth>().CreatePropertyModel(this, Item);

            return true;
        }

    }
}
