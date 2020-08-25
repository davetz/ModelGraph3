
namespace ModelGraph.Core
{
    public interface IPageModel
    {
        Root GetRoot();          // Root
        string TitleName { get; }       // title that appears in the windows title bar
        string TitleSummary { get; }       // title that appears in the windows title bar
        ControlType ControlType { get; } // tells the UI what kind control to create for this model
        IPageControl PageControl { get; set; } //set by the UI
        void Release();                 // release all references and remove this model from memory
        ICanvasModel GetDrawCanvas(CanvasId id); // gets the draw canvas interface (for the given canvasId)
        void TriggerUIRefresh();
    }
}
