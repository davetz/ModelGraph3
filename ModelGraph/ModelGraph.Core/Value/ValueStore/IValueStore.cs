
namespace ModelGraph.Core
{
    internal interface IValueStore<T>
    {
        int Count { get; }
        bool IsSpecific(Item key);
        void Clear();
        void Remove(Item key);
        void SetOwner(Property p);
        bool GetVal(Item key, out T val);
        bool SetVal(Item key, T value);
    }
}
