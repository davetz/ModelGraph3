using System;
using System.Linq;

namespace ModelGraph.Core
{
    internal class DescendDateTime : ValueOfDateTimeArray
    {
        internal DescendDateTime(ComputeStep step) { _step = step; }

        internal override string Text => "Descend";

        protected override DateTime[] GetVal()
        {
            var v = _step.Input[0].Evaluate.AsDateTimeArray();
            return v.OrderByDescending((s) => s).ToArray();
        }
    }
}
