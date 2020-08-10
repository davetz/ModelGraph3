using System;

namespace ModelGraph.Core
{
    internal class LiteralInt32 : ValueOfInt32
    {
        private string _text;
        private int _value;

        internal LiteralInt32(ComputeStep step, int value, string text)
        {
            _step = step;
            _text = text;
            _value = value;
        }
        internal override string Text => _text;

        protected override int GetVal() => _step.IsNegated ? -_value : _value;
    }
}