
namespace ModelGraph.Core
{
    public interface IRepository
    {
        string Name { get; }
        string FullName { get; }
        bool HasNoStorage { get; }
    }
}
