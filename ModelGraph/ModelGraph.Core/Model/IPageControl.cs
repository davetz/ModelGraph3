
namespace ModelGraph.Core
{
    public interface IPageControl
    {
        void Refresh();

        void Reload();
        void SaveAs();
        void Save();
        
        void NewView(IDataModel model);
    }
}
