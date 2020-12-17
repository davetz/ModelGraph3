using ModelGraph.Core;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
        public async void RefreshAsync()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, SetSelectedMode);

            GraphCanvas.Refresh();
        }
        private object _forcedSelectedMode;
        private object _previousSelectedMode;
        public void SetSize(double width, double height)
        {
        }
        #endregion

        #region ModeComboBox_SelectionChanged  ================================
        private void ModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itm = ModeComboBox.SelectedItem;
            if (itm != _forcedSelectedMode)
            {
                if (itm == AddMode) PostEvent(DrawEvent.SetAddMode);
                else if (itm == ViewMode) PostEvent(DrawEvent.SetViewMode);
                else if (itm == EditMode) PostEvent(DrawEvent.SetEditMode);
                else if (itm == MoveMode) PostEvent(DrawEvent.SetMoveMode);
                else if (itm == CopyMode) PostEvent(DrawEvent.SetCopyMode);
                else if (itm == LinkMode) PostEvent(DrawEvent.SetLinkMode);
                else if (itm == UnlinkMode) PostEvent(DrawEvent.SetUnlinkMode);
                else if (itm == CreateMode) PostEvent(DrawEvent.SetCreateMode);
                else if (itm == DeleteMode) PostEvent(DrawEvent.SetDeleteMode);
                else if (itm == OperateMode) PostEvent(DrawEvent.SetOperateMode);
                else if (itm == GravityMode) PostEvent(DrawEvent.SetGravityMode);
            }
        }
        internal void PostEvent(DrawEvent evt)
        {
            if (DrawModel.TryGetDrawEventAction(evt, out Action action))
                _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { action(); });
        }
        void SetSelectedMode()
        {
            _forcedSelectedMode = _previousSelectedMode = ModeComboBox.SelectedItem;
            var mode = DrawModel.DrawState & DrawState.ModeMask;
            switch (mode)
            {
                case DrawState.AddMode:
                    _forcedSelectedMode = AddMode;
                    break;
                case DrawState.ViewMode:
                    _forcedSelectedMode = ViewMode;
                    break;
                case DrawState.EditMode:
                    _forcedSelectedMode = EditMode;
                    break;
                case DrawState.MoveMode:
                    _forcedSelectedMode = MoveMode;
                    break;
                case DrawState.CopyMode:
                    _forcedSelectedMode = CopyMode;
                    break;
                case DrawState.LinkMode:
                    _forcedSelectedMode = LinkMode;
                    break;
                case DrawState.UnlinkMode:
                    _forcedSelectedMode = UnlinkMode;
                    break;
                case DrawState.CreateMode:
                    _forcedSelectedMode = CreateMode;
                    break;
                case DrawState.DeleteMode:
                    _forcedSelectedMode = DeleteMode;
                    break;
                case DrawState.OperateMode:
                    _forcedSelectedMode = OperateMode;
                    break;
                case DrawState.GravityMode:
                    _forcedSelectedMode = GravityMode;
                    break;
            }
            if (_forcedSelectedMode != _previousSelectedMode)
                ModeComboBox.SelectedItem = _forcedSelectedMode;

            GraphCanvas.Focus(Windows.UI.Xaml.FocusState.Programmatic);
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

        private void PalletOnOffTextBlock_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if ((DrawModel.VisibleDrawItems & DrawItem.Picker2) == 0)
            {
                PalletOnOffTextBlock.Text = "\uF0AD";
                DrawModel.VisibleDrawItems |= DrawItem.Picker2;
                GraphCanvas.Refresh();
            }
            else
            {
                PalletOnOffTextBlock.Text = "\uF0AE";
                DrawModel.VisibleDrawItems &= ~DrawItem.Picker2;
                GraphCanvas.Refresh();
            }
        }
    }
}
