using ModelGraph.Core;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class ModelSymbolControl : Page, IPageControl, IModelPageControl
    {
        IPageModel Model;

        public (int Width, int Height) PreferredSize => throw new System.NotImplementedException();

        public IPageModel PageModel => throw new System.NotImplementedException();

        public ModelSymbolControl(IPageModel model)
        {
            Model = model;
            InitializeComponent();
            SymbolCanvas.Initialize(model as ICanvasModel);
            SymbolCanvas.SetFixedSiize(520, 520);
            SymbolCanvas.SetOverview(32, 32);
            SymbolCanvas.ShowPicker1(32, 32);
            SymbolCanvas.ShowPicker2(32);
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
            SymbolCanvas.Refresh();
        }

        public void SetSize(double width, double height)
        {
        }
        #endregion

    }
}
