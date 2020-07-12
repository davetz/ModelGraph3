
namespace ModelGraph.Core
{
    internal abstract class ValueOfStep<T> : EvaluateStep
    {
        abstract protected T GetVal();
    }
}
