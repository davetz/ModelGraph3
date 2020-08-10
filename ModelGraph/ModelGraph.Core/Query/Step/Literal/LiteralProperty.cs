using System;

namespace ModelGraph.Core
{
    internal class LiteralProperty : EvaluateStep
    {
        private Func<Item> _getItem;

        internal LiteralProperty(ComputeStep step, Property property, Func<Item> getItem)
        {
            Property = property;
            _getItem = getItem;
            _step = step;
        }
        internal Property Property { get; }

        internal override ValType ValType => (Property is ComputeX cx && cx.Value == Value.ValuesUnknown) ? cx.Owner.AllocateValueCache(cx) : Property.Value.ValType;
        internal override string Text => (Property is ColumnX col) ? col.Name : ((Property is ComputeX cx) ? cx.Name : Property.GetNameId());

        internal override bool AsBool()
        {
            if (!Property.Value.GetValue(_getItem(), out bool val))
                val = false;
            return (_step.IsNegated) ? !val : val;
        }
        internal override long AsInt64()
        {
            if (!Property.Value.GetValue(_getItem(), out long val))
                val = 0;
            return (_step.IsNegated) ? ~val : val;
        }
        internal override double AsDouble()
        {
            if (!Property.Value.GetValue(_getItem(), out double val))
                val = 0;
            return (_step.IsNegated) ? -val : val;
        }
        internal override string AsString()
        {
            if (!Property.Value.GetValue(_getItem(), out string val))
                val = string.Empty;
            return val;
        }
        internal override DateTime AsDateTime()
        {
            if (!Property.Value.GetValue(_getItem(), out DateTime val))
                val = default(DateTime);
            return val;
        }

        internal override int AsLength()
        {
            if (!Property.Value.GetLength(_getItem(), out int val))
                val = 0;
            return val;
        }

        internal override bool[] AsBoolArray()
        {
            if (!Property.Value.GetValue(_getItem(), out bool[] val))
                val = new bool[0];
            return val;
        }
        internal override long[] AsInt64Array()
        {
            if (!Property.Value.GetValue(_getItem(), out long[] val))
                val = new long[0];
            return val;
        }
        internal override double[] AsDoubleArray()
        {
            if (!Property.Value.GetValue(_getItem(), out double[] val))
                val = new double[0];
            return val;
        }
        internal override string[] AsStringArray()
        {
            if (!Property.Value.GetValue(_getItem(), out string[] val))
                val = new string[0];
            return val;
        }
        internal override DateTime[] AsDateTimeArray()
        {
            if (!Property.Value.GetValue(_getItem(), out DateTime[] val))
                val = new DateTime[0];
            return val;
        }
    }
}
