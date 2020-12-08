using ModelGraph.Core;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class ModelGraphControl : Page, IPageControl, IModelPageControl
    {
        public IPageModel PageModel { get; }
        IDrawModel DrawModel => PageModel.LeadModel as IDrawModel;
        public (int Width, int Height) PreferredSize => throw new System.NotImplementedException();

        #region Constructor  ==================================================
        public ModelGraphControl(IPageModel model)
        {
            PageModel = model;
            this.InitializeComponent();
            GraphCanvas.Initialize(DrawModel);
            GraphCanvas.SetOverview(120, 120, true);
        }
        #endregion

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
        public void Release() => DrawModel.Release();
        public void Refresh()
        {
            var mode = DrawModel.DrawState & DrawState.ModeMask;
            switch (mode)
            {
                case DrawState.ViewMode:
                case DrawState.EditMode:
                    EditSelect.IsChecked = true;
                    break;
                case DrawState.MoveMode:
                    MoveSelect.IsChecked = true;
                    break;
                case DrawState.CopyMode:
                    CopySelect.IsChecked = true;
                    break;
                case DrawState.LinkMode:
                    LinkSelect.IsChecked = true;
                    break;
                case DrawState.CreateMode:
                    CreateSelect.IsChecked = true;
                    break;
                case DrawState.OperateMode:
                    OperateSelect.IsChecked = true;
                    break;
            }
            GraphCanvas.Refresh();
        }
        public void SetSize(double width, double height)
        {
        }
        #endregion

        #region RadioButton_Events  ===========================================
        private void EditSelect_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.SetEditMode);
        private void MoveSelect_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.SetMoveMode);
        private void LinkSelect_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.SetLinkMode);
        private void CopySelect_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.SetCopyMode);
        private void CreateSelect_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.SetCreateMode);
        private void OperateSelect_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.SetCreateMode);

        internal void PostEvent(DrawEvent evt)
        {
            if (DrawModel.TryGetDrawEventAction(evt, out Action action))
                _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { action(); });
        }
        #endregion

        private void OverviewOnOffTextBlock_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if ((DrawModel.VisibleDrawItems & DrawItem.Overview) == 0)
            {
                OverviewOnOffTextBlock.Text = "\uF0AD";
                DrawModel.VisibleDrawItems |= DrawItem.Overview;
                GraphCanvas.Refresh();
            }
            else
            {
                OverviewOnOffTextBlock.Text = "\uF0AE";
                DrawModel.VisibleDrawItems &= ~DrawItem.Overview;
                GraphCanvas.Refresh();
            }
        }
    }
}
