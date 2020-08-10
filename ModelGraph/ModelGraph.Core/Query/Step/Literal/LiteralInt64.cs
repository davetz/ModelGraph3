using System;

namespace ModelGraph.Core
{
    internal class LiteralInt64 : ValueOfInt64
    {
        private string _text;
        private long _value;

        internal LiteralInt64(ComputeStep step, long value, string text)
        {
            _step = step;
            _text = text;
            _value = value;
        }
        internal override string Text => _text;

        protected override long GetVal() => _step.IsNegated ? -_value : _value;
    }
}