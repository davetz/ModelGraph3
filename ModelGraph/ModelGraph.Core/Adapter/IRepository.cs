
using System.Threading.Tasks;

namespace ModelGraph.Core
{
    public interface IRepository
    {
        string Name { get; }
        string FullName { get; }
        bool HasNoStorage { get; }

        void New(Root root);
        Task<bool> OpenAsync(Root root);
        Task<bool> SaveAsync(Root root);
        Task<bool> ReloadAsync(Root root);
        void SaveAS(Root root);
    }
}
