﻿
namespace ModelGraph.Core
{
    internal class ValueCircular : ValueEmpty
    {
        internal ValueCircular()
        {
            _idString = "@@@@@@";
            _valueType = ValType.IsCircular;
        }
    }
}
