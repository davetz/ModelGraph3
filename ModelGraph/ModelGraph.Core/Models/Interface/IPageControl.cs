
namespace ModelGraph.Core
{
    public interface IPageControl
    {
        void RefreshAsync();
        void Reload();
        void SaveAs();
        void Save();
        void Close();
        
        void NewView(IPageModel model);
    }
}
