using System;

namespace ModelGraph.Core
{
    internal class LiteralInt32Array : ValueOfInt32Array
    {
        private string _text;
        private int[] _value;

        internal LiteralInt32Array(ComputeStep step, int[] value, string text)
        {
            _step = step;
            _text = text;
            _value = value;
        }
        internal override string Text => _text;

        protected override int[] GetVal()
        {
            if (_step.IsNegated)
            {
                var N = _value.Length;
                var negate = new int[N];
                for (int i = 0; i < N; i++) { negate[i] = -_value[i]; }
                return negate;
            }
            else
                return _value;
        }
    }
}