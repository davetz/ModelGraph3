
namespace ModelGraph.Core
{
    public interface IPageControl
    {
        void Refresh();
        void CreateNewPage(IRootModel model, ControlType ctlType);
    }
}
