﻿using System;

namespace ModelGraph.Core
{
    internal class Length : ValueOfInt32
    {
        internal Length(ComputeStep step) { _step = step; }

        internal override string Text => "Length";

        protected override int GetVal()
        {
            return _step.Input[0].Evaluate.AsLength();
        }
    }
}
