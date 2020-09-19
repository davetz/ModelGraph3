using ModelGraph.Core;
using ModelGraph.Services;
using System;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class ModelTreeControl : Page, IPageControl, IModelPageControl
    {
        public (int Width, int Height) PreferredSize => throw new System.NotImplementedException();

        public ModelTreeControl(IPageModel model)
        {
            PageModel = model;
            this.InitializeComponent();
            TreeCanvas.Initialize(model.LeadModel as ITreeModel);
        }


        #region IPageControl  =================================================
        public async void Close()
        {
            var pageService = ModelPageService.Current;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { pageService.CloseModel(this); });
        }
        public async void Save()
        {
            var pageService = ModelPageService.Current;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { _ = pageService.SaveModel(this); });
        }
        public async void SaveAs()
        {
            var pageService = ModelPageService.Current;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { _ = pageService.SaveModelAs(this); });
        }
        public async void Reload()
        {
            var pageService = ModelPageService.Current;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { _ = pageService.ReloadModel(this); });
        }
        public void NewView(IPageModel model)
        {
            if (model is null) return;
            _ = ModelPageService.Current.CreateNewPageAsync(model);
        }
        public void Refresh()
        {
            if (PageModel is null) return;
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { TreeCanvas.Refresh(); });
        }
        public IPageModel PageModel { get; }
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
            PageModel.Release();
        }

        public void SetSize(double width, double height)
        {
            TreeCanvas.SetSize(width, height);
        }
        #endregion

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
                Loaded -= Page_Loaded;
                Refresh();
        }
    }
}
