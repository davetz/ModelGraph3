
namespace ModelGraph.Core
{
    public interface IPageControl
    {
        void Refresh();
        void Reload();
        void CreateNewPage(IRootModel model, ControlType ctlType);
    }
}
