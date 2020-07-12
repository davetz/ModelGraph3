
namespace ModelGraph.Core
{
    internal class ValueInvalid : ValueEmpty
    {
        internal ValueInvalid()
        {
            _idString = "######";
            _valueType = ValType.IsInvalid;
        }
    }
}
