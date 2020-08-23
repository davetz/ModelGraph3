using ModelGraph.Core;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class ModelGraphControl : Page, IPageControl, IModelPageControl
    {
        IDataModel Model;

        public (int Width, int Height) PreferredSize => throw new System.NotImplementedException();

        public IDataModel DataModel => throw new System.NotImplementedException();

        public ModelGraphControl(IDataModel model)
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
        public void NewView(IDataModel model)
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
