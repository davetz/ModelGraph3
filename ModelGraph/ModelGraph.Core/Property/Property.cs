
namespace ModelGraph.Core
{
    public abstract class Property : Item
    {
        internal Value Value = Root.ValuesUnknown;

        internal virtual bool HasParentName => false;
        internal virtual string GetParentName(Root root, Item itm) => default;

        internal virtual bool IsReadonly => false;
        internal virtual bool IsMultiline => false;

        internal virtual string[] GetlListValue(Root root) => new string[0];
        internal virtual int GetIndexValue(Item item) => 0;
        internal virtual void SetIndexValue(Item item, int val) { }

    }
}
