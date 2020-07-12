using System;
using System.Linq;

namespace ModelGraph.Core
{
    internal class AscendDateTime : ValueOfDateTimeArray
    {
        internal AscendDateTime(ComputeStep step) { _step = step; }

        internal override string Text => "Ascend";

        protected override DateTime[] GetVal()
        {
            var v = _step.Input[0].Evaluate.AsDateTimeArray();
            return v.OrderBy((s) => s).ToArray();
        }
    }
}
