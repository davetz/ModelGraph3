using ModelGraph.Core;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class ModelGraphControl : Page, IPageControl, IModelPageControl
    {
        IPageModel Model;

        public (int Width, int Height) PreferredSize => throw new System.NotImplementedException();

        public IPageModel PageModel => throw new System.NotImplementedException();

        public ModelGraphControl(IPageModel model)
        {
            Model = model;
            this.InitializeComponent();
            GraphCanvas.Initialize(model.GetDrawCanvas(CanvasId.Graph));
        }

        #region IPageControl  =================================================
        public void Reload()
        {
        }
        public void SaveAs()
        {
        }
        public void Save()
        {
        }
        public void Close()
        {
        }
        public void NewView(IPageModel model)
        {
        }
        #endregion

        #region IModelPageControl  ============================================
        public void Apply()
        {
        }

        public void Revert()
        {
        }

        public void Release()
        {
        }

        public void Refresh()
        {
            GraphCanvas.Refresh();
        }

        public void SetSize(double width, double height)
        {
        }
        #endregion

    }
}
