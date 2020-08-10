using System;

namespace ModelGraph.Core
{
    internal class Count : ValueOfInt32
    {
        internal Count(ComputeStep step) { _step = step; }

        internal override string Text => "Count";

        protected override int GetVal()
        {
            return _step.Input[0].Evaluate.AsLength();
        }
    }
}
