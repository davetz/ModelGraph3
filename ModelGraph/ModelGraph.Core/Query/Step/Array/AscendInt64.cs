using System;
using System.Linq;

namespace ModelGraph.Core
{
    internal class AscendInt64 : ValueOfInt64Array
    {
        internal AscendInt64(ComputeStep step) { _step = step; }

        internal override string Text => "Ascend";

        protected override Int64[] GetVal()
        {
            var v = _step.Input[0].Evaluate.AsInt64Array();
            return v.OrderBy((s) => s).ToArray();
        }
    }
}
