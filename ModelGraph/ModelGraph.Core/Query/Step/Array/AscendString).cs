﻿using System.Linq;

namespace ModelGraph.Core
{
    internal class AscendString : ValueOfStringArray
    {
        internal AscendString(ComputeStep step) { _step = step; }

        internal override string Text => "Ascend";

        protected override string[] GetVal()
        {
            var v = _step.Input[0].Evaluate.AsStringArray();
            return v.OrderBy((s) => s).ToArray();
        }
    }
}
