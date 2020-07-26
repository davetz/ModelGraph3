
namespace ModelGraph.Core
{
    public interface IPageControl
    {
        void Refresh();

        void Reload();
        void SaveAs();
        void Save();
        
        void CreateNewPage(IRootModel model, ControlType ctlType);
    }
}
