
namespace ModelGraph.Core
{
    public interface IPageControl
    {
        void Refresh();
        void Reload();
        void SaveAs();
        void Save();
        void Close();
        
        void NewView(IDataModel model);
    }
}
