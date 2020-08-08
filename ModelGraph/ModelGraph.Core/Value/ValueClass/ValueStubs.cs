
namespace ModelGraph.Core
{
    internal abstract partial class Value
    {
        internal static ValueUnknown ValuesUnknown = new ValueUnknown();
        internal static ValueInvalid ValuesInvalid = new ValueInvalid();
        internal static ValueCircular ValuesCircular = new ValueCircular();
        internal static ValueUnresolved ValuesUnresolved = new ValueUnresolved();
        internal static LiteralUnresolved LiteralUnresolved = new LiteralUnresolved();
    }
}
