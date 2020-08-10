using System;

namespace ModelGraph.Core
{
    internal class LiteralInt64Array : ValueOfInt64Array
    {
        private string _text;
        private long[] _value;

        internal LiteralInt64Array(ComputeStep step, long[] value, string text)
        {
            _step = step;
            _text = text;
            _value = value;
        }
        internal override string Text => _text;

        protected override long[] GetVal()
        {
            if (_step.IsNegated)
            {
                var N = _value.Length;
                var negate = new long[N];
                for (int i = 0; i < N; i++) { negate[i] = -_value[i]; }
                return negate;
            }
            else
                return _value;
        }
    }
}