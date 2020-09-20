
namespace ModelGraph.Core
{
    public class Property_Shape_Color : PropertyOf<SymbolModel, string>
    {
        internal override IdKey IdKey => IdKey.ShapeStrokeColorProperty;

        internal Property_Shape_Color(PropertyManager owner) : base(owner)
        {
            Value = new StringValue(this);
        }

        internal override string GetValue(Item item) => Cast(item).ColorARGB;
        internal override void SetValue(Item item, string val) => Cast(item).ColorARGB = val;
    }
}
