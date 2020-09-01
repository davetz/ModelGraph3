using ModelGraph.Core;
using ModelGraph.Helpers;
using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;

namespace ModelGraph.Controls
{
    public sealed partial class TreeCanvasControl : UserControl
    {
        #region Constructor  ==================================================
        public TreeCanvasControl()
        {
            InitializeComponent();
        }
        #endregion

        #region SetSize/TreeCanvas_Loaded  ====================================
        internal (double, double) GetSize()
        {
            var hieght = 32 + Count * ElementHieght;
            var width = 340;
            return (width, hieght);
        }
        
        internal void SetSize(double width, double height)
        {
            if (_treeCanvasLoaded && height > 0)
            {
                TreeCanvas.Width = Width = width;
                TreeCanvas.Height = Height = height;

                _ = RefreshViewListAsync();
            }
        }
        private int ViewSize => (int)(TreeCanvas.ActualHeight / ElementHieght);
        private void TreeCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            TreeCanvas.Loaded -= TreeCanvas_Loaded;
            InitializeCache();
            _treeCanvasLoaded = true;
        }
        private bool _treeCanvasLoaded;
        #endregion


        #region ModelUICache  =================================================
        readonly Stack<ItemModelUI> FreeCacheStack = new Stack<ItemModelUI>(37);
        readonly Dictionary<ItemModel, ItemModelUI> Model_Cache = new Dictionary<ItemModel, ItemModelUI>(31);
        readonly HashSet<ItemModel> DefunctModels = new HashSet<ItemModel>();

        private void InitializeCache() =>  ItemModelUI.Allocate(this, 31, FreeCacheStack);

        private void DiscardCache()
        {
            while (FreeCacheStack.Count > 0)
            {
                var fc = FreeCacheStack.Pop();
                fc.Discard();
            }
            foreach (var lc in Model_Cache.Values)
            {
                lc.Discard();
            }
            Model_Cache.Clear();
        }
        private void RefreshCache(List<ItemModel> newModels)
        {
            ModelList = newModels; // save this for future reference

            DefunctModels.Clear();
            foreach (var m in Model_Cache.Keys) { DefunctModels.Add(m); }

            for (int i = 0; i < newModels.Count; i++)
            {
                var m = newModels[i];
                if (Model_Cache.TryGetValue(m, out ItemModelUI mc))
                {
                    mc.Validate(i);
                    DefunctModels.Remove(m);
                }
                else
                {
                    if (FreeCacheStack.Count == 0) ItemModelUI.Allocate(this, 31, FreeCacheStack);
                    var c = FreeCacheStack.Pop();
                    c.Initialize(m, i);
                    Model_Cache.Add(m, c);
                }
            }
            foreach (var m in DefunctModels)  // reclaim and save the uiCach from the defunct models
            {
                var oc = GetModelUI(m);
                oc.Clear();
                FreeCacheStack.Push(oc);
                Model_Cache.Remove(m);
            }
        }
        private ItemModelUI GetModelUI(ItemModel m)
        {
            if (!Model_Cache.TryGetValue(m, out ItemModelUI mc))
                throw new Exception("ExceptionCorruptLineModelCache".GetLocalized());
            return mc;
        }
        #endregion

        #region RefreshViewListAsync  =========================================
        private async System.Threading.Tasks.Task RefreshViewListAsync(ChangeType change = ChangeType.None)
        {
            var (leading, selected) = GetLeadingSelected();
            //ResetCacheDelta(_selected);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { TCM.RefreshViewList(ViewSize, leading, selected, change); });
        }
        private async System.Threading.Tasks.Task SetUsageAsync(ItemModel model, Usage usage)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { TCM.SetUsage(model, usage); });
            _ = RefreshViewListAsync(ChangeType.FilterSortChanged);
        }
        private async System.Threading.Tasks.Task SetSortingAsync(ItemModel model, Sorting sorting)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { TCM.SetSorting(model, sorting); });
            _ = RefreshViewListAsync(ChangeType.FilterSortChanged);
        }
        private async System.Threading.Tasks.Task SetFilterAsync(ItemModel model, string text)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { TCM.SetFilter(model, text); });
            _ = RefreshViewListAsync(ChangeType.FilterSortChanged);
        }
        private (ItemModel, ItemModel) GetLeadingSelected()
        {
            var leading = (ModelList is null || ModelList.Count == 0) ? null : ModelList[0];
            var selected = (Selected is null) ? leading : Selected;
            return (leading, selected);
        }
        #endregion



        #region ItemModelUI  =================================================
        internal (int, Sorting, Usage, string) GetFilterParms(ItemModel m) => TCM.GetFilterParms(m);
        internal Canvas Canvas => TreeCanvas;

        #endregion



        #region Properties  ===================================================
        private ITreeCanvasModel TCM;

        ItemModel Selected;

        int Count => (ModelList is null) ? 0 : ModelList.Count;
        private List<ItemModel> ModelList = new List<ItemModel>(0);

        readonly List<ItemCommand> MenuCommands = new List<ItemCommand>();
        readonly List<ItemCommand> ButtonCommands = new List<ItemCommand>();

        ToolTip[] MenuItemTips;
        ToolTip[] ItemButtonTips;

        Button[] ItemButtons;
        MenuFlyoutItem[] MenuItems;
        int MenuItemsCount;

        internal ToolTip ItemIdentityTip { get; private set; }
        internal ToolTip ModelIdentityTip { get; private set; }

        internal int LevelIndent { get; private set; }
        internal int ElementHieght { get; private set; }

        internal Style ExpanderStyle { get; private set; }
        internal Style ItemKindStyle { get; private set; }
        internal Style ItemNameStyle { get; private set; }
        internal Style ItemInfoStyle { get; private set; }
        internal Style SortModeStyle { get; private set; }
        internal Style UsageModeStyle { get; private set; }
        internal Style TotalCountStyle { get; private set; }
        internal Style IndentTreeStyle { get; private set; }
        internal Style FilterModeStyle { get; private set; }
        internal Style FilterTextStyle { get; private set; }
        internal Style FilterCountStyle { get; private set; }
        internal Style ItemHasErrorStyle { get; private set; }
        internal Style PropertyNameStyle { get; private set; }
        internal Style TextPropertyStyle { get; private set; }
        internal Style CheckPropertyStyle { get; private set; }
        internal Style ComboPropertyStyle { get; private set; }
        internal Style ModelIdentityStyle { get; private set; }
        internal Style PropertyBorderStyle { get; private set; }

        // segoe ui symbol font glyphs  =====================
        internal string LeftCanExtend => "\u25b7";
        internal string LeftIsExtended => "\u25e2";

        internal string RightCanExtend => "\u25c1";
        internal string RightIsExtended => "\u25e3";

        internal string SortNone => "\u2012";
        internal string SortAscending => "\u2228";
        internal string SortDescending => "\u2227";

        internal string UsageAll => "A";
        internal string UsageIsUsed => "U";
        internal string UsageIsNotUsed => "N";

        internal string FilterCanShow => "\uE71C";
        internal string FilterIsShowing => "\uE71C\uEBE7";

        internal string ItemHasErrorText => "\uE783";

        internal string SortModeTip { get; private set; }
        internal string UsageModeTip { get; private set; }
        internal string LeftExpandTip { get; private set; }
        internal string TotalCountTip { get; private set; }
        internal string FilterTextTip { get; private set; }
        internal string FilterCountTip { get; private set; }
        internal string RightExpandTip { get; private set; }
        internal string FilterExpandTip { get; private set; }
        internal string ItemHasErrorTip { get; private set; }

        private bool ScrollingUp;
        private bool ScrollingDown;
        private bool AtEnd;
        private bool AtStart;
        #endregion

        #region Initialize  ===================================================
        internal void Initialize(ITreeCanvasModel tcm)
        {
            TCM = tcm;

            LevelIndent = (int)(Resources["LevelIndent"] as double?).Value;
            ElementHieght = (int)(Resources["ElementHieght"] as double?).Value;

            ExpanderStyle = Resources["ExpanderStyle"] as Style;
            ItemKindStyle = Resources["ItemKindStyle"] as Style;
            ItemNameStyle = Resources["ItemNameStyle"] as Style;
            ItemInfoStyle = Resources["ItemInfoStyle"] as Style;
            SortModeStyle = Resources["SortModeStyle"] as Style;
            UsageModeStyle = Resources["UsageModeStyle"] as Style;
            TotalCountStyle = Resources["TotalCountStyle"] as Style;
            IndentTreeStyle = Resources["IndentTreeStyle"] as Style;
            FilterModeStyle = Resources["FilterModeStyle"] as Style;
            FilterTextStyle = Resources["FilterTextStyle"] as Style;
            FilterCountStyle = Resources["FilterCountStyle"] as Style;
            ItemHasErrorStyle = Resources["ItemHasErrorStyle"] as Style;
            PropertyNameStyle = Resources["PropertyNameStyle"] as Style;
            TextPropertyStyle = Resources["TextPropertyStyle"] as Style;
            CheckPropertyStyle = Resources["CheckPropertyStyle"] as Style;
            ComboPropertyStyle = Resources["ComboPropertyStyle"] as Style;
            ModelIdentityStyle = Resources["ModelIdentityStyle"] as Style;
            PropertyBorderStyle = Resources["PropertyBorderStyle"] as Style;

            ItemButtons = new Button[]
{
                ItemButton1,
                ItemButton2,
                ItemButton3
};
            MenuItems = new MenuFlyoutItem[]
            {
                MenuItem1,
                MenuItem2,
                MenuItem3,
                MenuItem4,
                MenuItem5,
                MenuItem6,
            };

            ItemIdentityTip = new ToolTip();
            ItemIdentityTip.Opened += ItemIdentityTip_Opened;

            ModelIdentityTip = new ToolTip();
            ModelIdentityTip.Opened += ModelIdentityTip_Opened;

            ItemButtonTips = new ToolTip[ItemButtons.Length];
            for (int i = 0; i < ItemButtons.Length; i++)
            {
                var tip = new ToolTip();
                ItemButtonTips[i] = tip;
                ToolTipService.SetToolTip(ItemButtons[i], tip);
            }

            MenuItemTips = new ToolTip[MenuItems.Length];
            for (int i = 0; i < MenuItems.Length; i++)
            {
                var tip = new ToolTip();
                MenuItemTips[i] = tip;
                ToolTipService.SetToolTip(MenuItems[i], tip);
            }

        }
        #endregion


        #region KeyboardAccelerators  =========================================
        private void KeyPageUp_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            _ = RefreshViewListAsync(ChangeType.PageUp);
            args.Handled = true;
        }
        private void KeyPageDown_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            _ = RefreshViewListAsync(ChangeType.PageDown);
            args.Handled = true;
        }

        private void KeyUp_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            TryGetPrevModel();
            args.Handled = true;
        }
        private void KeyDown_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            TryGetNextModel();
            args.Handled = true;
        }

        private void KeyEnd_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            _ = RefreshViewListAsync(ChangeType.GoToBottom);
            args.Handled = true;
        }
        private void KeyHome_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            _ = RefreshViewListAsync(ChangeType.GoToTop);
            args.Handled = true;
        }

        private void KeyLeft_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (Selected.CanExpandLeft)
            {
                _ = RefreshViewListAsync(ChangeType.ToggleLeft);
            }
            args.Handled = true;
        }
        private void KeyCtrlLeft_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (Selected.CanExpandAll)
            {
                _ = RefreshViewListAsync(ChangeType.ExpandAllLeft);
            }
            args.Handled = true;
        }
        private void KeyRight_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (Selected.CanExpandRight)
            {
                _ = RefreshViewListAsync(ChangeType.ToggleRight);
            }
            args.Handled = true;
        }
        private void KeyCtrlRight_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (Selected.CanExpandAll)
            {
                _ = RefreshViewListAsync(ChangeType.ExpandAllRight);
            }
            args.Handled = true;
        }
        private void KeyEnter_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            TrySetControlFocus();
            args.Handled = true;
        }

        private void KeyEscape_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            SetDefaultFocus();
            if (Selected != null)
            {
                Selected.IsFilterVisible = false;
                Selected.IsExpandedLeft = false;
                _ = RefreshViewListAsync(ChangeType.FilterSortChanged);
            }
            args.Handled = true;
        }
        private void FocusButtonKeyEscape_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (Selected != null && Selected.Depth > 0 && Selected.Depth < 100)
            {
                var parent = Selected.ParentModel;
                if (!ModelList.Contains(parent)) ModelList[0] = parent;
                Selected = parent;
                _ = RefreshViewListAsync(ChangeType.ToggleLeft);
            }
            args.Handled = true;
        }
        private void KeyMenu_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;
        }
        void TryGetPrevModel()
        {
            SetDefaultFocus();
            if (Selected is null)
            {
                if (ModelList is null || ModelList.Count == 0) return;
                Selected = ModelList[0];
                RefreshSelector();
            }
            else
            {
                var i = ModelList.IndexOf(Selected) - 1;
                if (i >= 0 && i < ModelList.Count)
                {
                    Selected = ModelList[i];
                    RefreshSelector();
                }
                else if (!AtStart)
                {
                    ScrollingUp = true;
                    _ = RefreshViewListAsync(ChangeType.OneUp);
                }
            }
        }
        void TryGetNextModel()
        {
            SetDefaultFocus();
            if (Selected is null)
            {
                if (ModelList is null || ModelList.Count == 0) return;
                Selected = ModelList[0];
                RefreshSelector();
            }
            else
            {
                var i = ModelList.IndexOf(Selected) + 1;
                if (i > 0 && i < ModelList.Count)
                {
                    Selected = ModelList[i];
                    RefreshSelector();
                }
                else if (!AtEnd)
                {
                    ScrollingDown = true;
                    _ = RefreshViewListAsync(ChangeType.OneDown);
                }
            }
        }
        #endregion

        #region AppButton_Click  ==============================================
        private void AppButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var cmd = btn.Tag as ItemCommand;
            cmd.Execute();
        }
        #endregion

        #region Button_Click  =================================================
        void ItemButton_Click(object sender, RoutedEventArgs e)
        {
            var obj = sender as Button;
            var cmd = obj.DataContext as ItemCommand;
            cmd.Execute();
        }
        void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var obj = sender as MenuFlyoutItem;
            var cmd = obj.DataContext as ItemCommand;
            cmd.Execute();
        }
        #endregion

        #region PointerWheelChanged  ==========================================
        bool _pointWheelEnabled;
        void TreeCanvas_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            e.Handled = true;
            if (_pointWheelEnabled)
            {
                var cp = e.GetCurrentPoint(TreeCanvas);
                if (cp.Properties.MouseWheelDelta < 0)
                    _ = RefreshViewListAsync(ChangeType.TwoDown);
                else
                    _ = RefreshViewListAsync(ChangeType.TwoUp);
            }
        }
        #endregion

        #region ToolTip_Opened  ===============================================
        void ItemIdentityTip_Opened(object sender, RoutedEventArgs e)
        {
            var tip = sender as ToolTip;
            if (tip.DataContext is ItemModelUI mc)
            {
                var mdl = mc.Model;
                var content = mdl?.GetSummaryId();
                tip.Content = string.IsNullOrWhiteSpace(content) ? null : content;
            }
            else
                tip.Content = null;
        }
        void ModelIdentityTip_Opened(object sender, RoutedEventArgs e)
        {
            var tip = sender as ToolTip;
            if (tip.DataContext is ItemModelUI mc)
            {
                var mdl = mc.Model;
                var content = mdl.GetModelIdentity();
                tip.Content = string.IsNullOrWhiteSpace(content) ? null : content;
            }
            else
                tip.Content = null;
        }
        #endregion

        #region MenuFlyout_Opening  ===========================================
        void MenuFlyout_Opening(object sender, object e)
        {
            var fly = sender as MenuFlyout;
            fly.Items.Clear();
            for (int i = 0; i < MenuItemsCount; i++)
            {
                fly.Items.Add(MenuItems[i]);
            }
        }

        #endregion

        #region PointerPressed  ===============================================
        void TreeGrid_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Selected = PointerModel(e);
            SetDefaultFocus();
            RefreshSelector();
            e.Handled = true;
        }
        ItemModel PointerModel(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(TreeCanvas);
            var i = (int)(p.Position.Y / ElementHieght);
            return (Count == 0) ? null : (i < 0) ? ModelList[0] : (i < Count) ? ModelList[i] : ModelList[Count - 1];
        }
        #endregion


        #region Refresh  ======================================================
        internal void Refresh()
        {
            if (TCM is null) return;

            _pointWheelEnabled = false;

            var (oldLeading, oldSelected) = GetLeadingSelected();
            var (newModels, newSelected, atStart, atEnd) = TCM.GetCurrentView(ViewSize, oldLeading, oldSelected);

            AtEnd = atEnd;
            AtStart = atStart;
            Selected = newSelected;

            var n = newModels.Count - 1;
            if (n < 0)
                Selected = null;
            else if (ScrollingUp)
                Selected = newModels[0];
            else if (ScrollingDown)
                Selected = newModels[n];

            ScrollingUp = false;
            ScrollingDown = false;

            RefreshCache(newModels);
            RefreshRoot();
            RefreshSelector();

            _pointWheelEnabled = true;
        }
        #endregion

        #region RefreshRoot  ==================================================
        private void RefreshRoot()
        {
            var buttonCommands = new List<ItemCommand>();
            TCM.GetButtonCommands(buttonCommands);

            var N = buttonCommands.Count;
            var M = ControlPanel.Children.Count;
            for (int i = 0; i < M; i++)
            {
                if (ControlPanel.Children[i] is Button btn)
                {
                    if (i < N)
                    {
                        var cmd = buttonCommands[i];
                        btn.Tag = cmd;
                        btn.Content = cmd.GetNameId();
                        btn.Visibility = Visibility.Visible;
                        ToolTipService.SetToolTip(btn, cmd.GetSummaryId());
                    }
                    else
                    {
                        btn.Visibility = Visibility.Collapsed;
                    }
                }
            }
            HeaderTitle.Text = TCM.HeaderTitle;
            //_root.IsChanged = false;
        }

        #endregion

        #region RefreshSelector  ==============================================
        void RefreshSelector(bool restoreFocus = true)
        {
            TreeCanvas.KeyboardAccelerators.Clear();
            _acceleratorKeyCommands.Clear();

            _sortControl = _filterControl = null;

            if (Count == 0 || Selected == null)
            {
                foreach (var btn in ItemButtons) { btn.Visibility = Visibility.Collapsed; }

                return;
            }

            SetSelectorGridPlacement();


            var mc = GetModelUI(Selected);

            if (mc.SortMode != null && !string.IsNullOrEmpty(mc.SortMode.Text))
            {
                _sortControl = mc.SortMode;
                var acc = new KeyboardAccelerator { Key = VirtualKey.S, Modifiers = VirtualKeyModifiers.Control };
                acc.Invoked += Accelerator_SortMode_Invoked;
                TreeCanvas.KeyboardAccelerators.Add(acc);
            }

            if (mc.UsageMode != null && !string.IsNullOrEmpty(mc.UsageMode.Text))
            {
                _usageControl = mc.UsageMode;
                var acc = new KeyboardAccelerator { Key = VirtualKey.U, Modifiers = VirtualKeyModifiers.Control };
                acc.Invoked += Accelerator_UsageMode_Invoked;
                TreeCanvas.KeyboardAccelerators.Add(acc);
            }

            if (Selected.CanDrag)
            {
                var acc = new KeyboardAccelerator { Key = VirtualKey.C, Modifiers = VirtualKeyModifiers.Control };
                acc.Invoked += Accelerator_ModelCopy_Invoked;
                TreeCanvas.KeyboardAccelerators.Add(acc);
            }

            if (TCM.DragEnter(Selected) != DropAction.None)
            {
                var acc = new KeyboardAccelerator { Key = VirtualKey.V, Modifiers = VirtualKeyModifiers.Control };
                acc.Invoked += Accelerator_ModelPaste_Invoked;
                TreeCanvas.KeyboardAccelerators.Add(acc);
            }

            if (mc.FilterMode != null && string.IsNullOrEmpty(mc.FilterMode.Text))
            {
                _filterControl = mc.FilterMode;
            }


            if (Selected.GetDescriptionId() != null)
            {
                HelpButton.Visibility = Visibility.Visible;
                PopulateItemHelp(Selected.GetDescriptionId());
            }
            else
            {
                HelpButton.Visibility = Visibility.Collapsed;
            }

            TCM.GetMenuCommands(Selected, MenuCommands);
            TCM.GetButtonCommands(Selected, ButtonCommands);

            var cmds = ButtonCommands;
            var len1 = cmds.Count;
            var len2 = ItemButtons.Length;

            for (int i = 0; i < len2; i++)
            {
                if (i < len1)
                {
                    var cmd = ButtonCommands[i];
                    ItemButtons[i].DataContext = cmd;
                    ItemButtons[i].Content = cmd.GetNameId();
                    ItemButtonTips[i].Content = cmd.GetSummaryId();
                    ItemButtons[i].Visibility = Visibility.Visible;
                    var key = cmd.GetAcceleratorId();
                    if (cmd.IsInsertCommand)
                    {
                        var acc = new KeyboardAccelerator { Key = VirtualKey.Insert };
                        acc.Invoked += Accelerator_Invoked;
                        _acceleratorKeyCommands.Add(acc, cmd);
                        TreeCanvas.KeyboardAccelerators.Add(acc);
                    }
                    else if (cmd.IsRemoveCommand)
                    {
                        var acc = new KeyboardAccelerator { Key = VirtualKey.Delete };
                        acc.Invoked += Accelerator_Invoked;
                        _acceleratorKeyCommands.Add(acc, cmd);
                        TreeCanvas.KeyboardAccelerators.Add(acc);
                    }
                    else if (_virtualKeys.TryGetValue(key, out VirtualKey vkey))
                    {
                        var acc = new KeyboardAccelerator { Key = vkey, Modifiers = VirtualKeyModifiers.Control };
                        acc.Invoked += Accelerator_Invoked;
                        _acceleratorKeyCommands.Add(acc, cmd);
                        TreeCanvas.KeyboardAccelerators.Add(acc);
                    }
                }
                else
                {
                    ItemButtons[i].Visibility = Visibility.Collapsed;
                }
            }

            cmds = MenuCommands;
            if (cmds.Count == 0)
            {
                MenuButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MenuButton.Visibility = Visibility.Visible;

                MenuItemsCount = len1 = cmds.Count;
                len2 = MenuItems.Length;

                for (int i = 0; i < len2; i++)
                {
                    if (i < len1)
                    {
                        var cmd = cmds[i];
                        MenuItems[i].DataContext = cmd;
                        MenuItems[i].Text = cmd.GetNameId();
                        MenuItemTips[i].Content = cmd.GetSummaryId();
                    }
                }
            }

            if (restoreFocus) TryRestoreFocus();
        }
        #endregion

        #region SetSelectorGridPlacement  =====================================
        void SetSelectorGridPlacement()
        {
            var i = ModelList.IndexOf(Selected);
            if (i < 0) i = 0;
            SelectorGrid.Width = ActualWidth;
            Canvas.SetTop(SelectorGrid, (i * ElementHieght));

        }
        #endregion

        #region TrySetControlFocus  ===========================================
        // given the focusModel try to determine what is the most
        // logical text box to enter, then set the keyboard focus to it,
        // otherwise set focus to our reliable dummy FocusButton
        private void TrySetControlFocus(ItemModel focusModel = null)
        {
            if (focusModel != null) Selected = focusModel;

            var lc = GetModelUI(Selected);

            if (Selected.CanFilter)
            {
                if (Selected.IsFilterVisible)
                {
                    Selected.IsFilterFocus = false;
                    SetFocus(lc.FilterText);
                }
                else
                {
                    _tryAfterRefresh = true;
                    _ = RefreshViewListAsync(ChangeType.ToggleFilter);
                }
            }
            else if (Selected is PropertyModel pm)
            {
                if (pm.IsTextModel) SetFocus(lc.TextProperty);
                else if (pm.IsCheckModel) SetFocus(lc.CheckProperty);
                else if (pm.IsComboModel) SetFocus(lc.ComboProperty);
            }
            else
                SetFocus(FocusButton);


            void SetFocus(Control ctrl)
            {
                _focusControl = ctrl;
                _tryAfterRefresh = false;
                ctrl.Focus(FocusState.Keyboard);
            }
        }
        private void TryRestoreFocus()
        {
            if (_tryAfterRefresh || _focusControl != FocusButton)
                TrySetControlFocus();
            else
                SetDefaultFocus();
        }
        private void SetDefaultFocus()
        {
            _focusControl = FocusButton;
            _tryAfterRefresh = false;
            FocusButton.Focus(FocusState.Keyboard);
        }
        bool _tryAfterRefresh;
        Control _focusControl;
        #endregion

        #region AcceleratorKeyCommands  =======================================
        private void Accelerator_ModelCopy_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (Selected.CanDrag)
                TCM.DragStart(Selected);
            args.Handled = true;
        }
        private void Accelerator_ModelPaste_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (TCM.DragEnter(Selected) != DropAction.None)
                TCM.DragDrop(Selected);
            args.Handled = true;
        }
        private void Accelerator_SortMode_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_sortControl != null) ExecuteSort(_sortControl);
            args.Handled = true;
        }
        private void Accelerator_UsageMode_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_usageControl != null) ExecuteUsage(_usageControl);
            args.Handled = true;
        }
        private void Accelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_acceleratorKeyCommands.TryGetValue(sender, out ItemCommand cmd)) cmd.Execute();
            args.Handled = true;
        }
        private readonly Dictionary<KeyboardAccelerator, ItemCommand> _acceleratorKeyCommands = new Dictionary<KeyboardAccelerator, ItemCommand>();


        static readonly Dictionary<string, Windows.System.VirtualKey> _virtualKeys = new Dictionary<string, VirtualKey>
        {
            ["A"] = VirtualKey.A,
            ["B"] = VirtualKey.B,
            ["C"] = VirtualKey.C,
            ["D"] = VirtualKey.D,
            ["E"] = VirtualKey.E,
            ["F"] = VirtualKey.F,
            ["G"] = VirtualKey.G,
            ["H"] = VirtualKey.H,
            ["I"] = VirtualKey.I,
            ["J"] = VirtualKey.J,
            ["K"] = VirtualKey.K,
            ["L"] = VirtualKey.L,
            ["M"] = VirtualKey.M,
            ["N"] = VirtualKey.N,
            ["O"] = VirtualKey.O,
            ["P"] = VirtualKey.P,
            ["Q"] = VirtualKey.Q,
            ["R"] = VirtualKey.R,
            ["S"] = VirtualKey.S,
            ["T"] = VirtualKey.T,
            ["U"] = VirtualKey.U,
            ["V"] = VirtualKey.V,
            ["W"] = VirtualKey.W,
            ["X"] = VirtualKey.X,
            ["Y"] = VirtualKey.Y,
            ["Z"] = VirtualKey.Z,
        };
        #endregion

        #region PopulateItemHelp  =============================================
        void PopulateItemHelp(string input)
        {
            ItemHelp.Blocks.Clear();

            var strings = SplitOnNewLines(input);
            if (strings.Length == 0) return;

            foreach (var str in strings)
            {
                var run = new Run { Text = str };
                var para = new Paragraph();

                para.Inlines.Add(run);
                para.Margin = _spacing;

                ItemHelp.Blocks.Add(para);
            }
        }
        static readonly Thickness _spacing = new Thickness(0, 0, 0, 6);

        string[] SplitOnNewLines(string input)
        {
            var chars = input.ToCharArray();
            var output = new List<string>();
            var len = chars.Length;
            int j, i = 0;
            while (i < len)
            {
                if (chars[i] < ' ') { i++; continue; }

                for (j = i; j < len; j++)
                {
                    if (chars[j] >= ' ') continue;

                    output.Add(input.Substring(i, (j - i)));
                    i = j;
                    break;
                }
                if (i != j)
                {
                    output.Add(input.Substring(i, (len - i)));
                    break;
                }
            }
            return output.ToArray();
        }
        #endregion


        #region ItemName  =====================================================
        internal void ItemName_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as TextBlock;
            ItemIdentityTip.DataContext = obj.DataContext;
            ToolTipService.SetToolTip(obj, ItemIdentityTip);
        }
        internal void ItemName_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            //args.DragUI.SetContentFromDataPackage();
            var obj = sender as TextBlock;
            if (obj.DataContext is ItemModelUI mc && mc.Model is ItemModel mdl)
            {
                if (mdl.CanDrag)
                {
                    TCM.DragStart(mdl);
                }
                else
                {
                    args.Cancel = true;
                }
            }
        }
        internal void ItemName_DragOver(object sender, DragEventArgs e)
        {
            e.DragUIOverride.IsContentVisible = false;
            var obj = sender as TextBlock;
            if (obj.DataContext is ItemModelUI mc && mc.Model is ItemModel mdl)
            {
                var type = TCM.DragEnter(mdl);
                switch (type)
                {
                    case DropAction.None:
                        e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.None;
                        break;
                    case DropAction.Move:
                        e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
                        break;
                    case DropAction.Link:
                        e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Link;
                        break;
                    case DropAction.Copy:
                        e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
                        break;
                    default:
                        e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.None;
                        break;
                }
            }
        }
        internal void ItemName_Drop(object sender, DragEventArgs e)
        {
            var obj = sender as TextBlock;
            if (obj.DataContext is ItemModelUI mc && mc.Model is ItemModel mdl)
            {
                TCM.DragDrop(mdl);
            }
        }
        #endregion

        #region ExpandLeft  ===================================================
        internal void TextBlockHightlight_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as TextBlock;
            obj.Opacity = 1.0;
        }
        internal void TextBlockHighlight_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as TextBlock;
            obj.Opacity = 0.5;
        }

        internal void RefreshExpandTree(ItemModel model, TextBlock obj)
        {
            if (model.CanExpandLeft)
            {
                obj.Text = model.IsExpandedLeft ? LeftIsExtended : LeftCanExtend;
            }
            else
            {
                obj.Text = string.Empty;
            }
        }
        internal void ExpandTree_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Selected == PointerModel(e))
            {
                var obj = sender as TextBlock;
                if (obj.DataContext is ItemModelUI mc && mc.Model is ItemModel mdl)
                {
                    Selected = mdl;
                    _ = RefreshViewListAsync(ChangeType.ToggleLeft);
                }
            }
        }
        #endregion

        #region ExpandRight  ==================================================
        internal void ExpandChoice_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Selected == PointerModel(e))
            {
                var obj = sender as TextBlock;
                if (obj.DataContext is ItemModelUI mc && mc.Model is ItemModel mdl)
                {
                    Selected = mdl;
                    _ = RefreshViewListAsync(ChangeType.ToggleRight);
                }
            }
        }
        #endregion

        #region ModelIdentity  ================================================
        internal void ModelIdentity_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as TextBlock;
            ModelIdentityTip.DataContext = obj.DataContext;
            ToolTipService.SetToolTip(obj, ModelIdentityTip);
        }
        #endregion

        #region SortMode  =====================================================
        TextBlock _sortControl;
        internal void SortMode_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Selected == PointerModel(e))
            {
                var obj = sender as TextBlock;
                ExecuteSort(obj);
            }
        }
        void ExecuteSort(TextBlock obj)
        {
            if (obj != null && obj.DataContext is ItemModelUI mc && mc.Model is ItemModel mdl)
            {
                if (mdl.IsSortAscending)
                {
                    mdl.IsSortAscending = false;
                    mdl.IsSortDescending = true;
                    obj.Text = SortDescending;
                    _ = SetSortingAsync(mdl, Sorting.Descending);
                }
                else if (mdl.IsSortDescending)
                {
                    mdl.IsSortAscending = false;
                    mdl.IsSortDescending = false;
                    obj.Text = SortNone;
                    _ = SetSortingAsync(mdl, Sorting.Unsorted);
                }
                else
                {
                    mdl.IsSortAscending = true;
                    obj.Text = SortAscending;
                    _ = SetSortingAsync(mdl, Sorting.Ascending);
                }
            }
        }
        #endregion

        #region UsageMode  ====================================================
        TextBlock _usageControl;
        internal void UsageMode_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Selected == PointerModel(e))
            {
                var obj = sender as TextBlock;
                ExecuteUsage(obj);
            }
        }
        void ExecuteUsage(TextBlock obj)
        {
            if (obj != null && obj.DataContext is ItemModelUI mc && mc.Model is ItemModel mdl)
            {
                Usage usage;
                if (obj.Text == UsageIsUsed)
                {
                    obj.Text = UsageIsNotUsed;
                    usage = Usage.IsNotUsed;
                }
                else if (obj.Text == UsageAll)
                {
                    obj.Text = UsageIsUsed;
                    usage = Usage.IsUsed;
                }
                else
                {
                    obj.Text = UsageAll;
                    usage = Usage.None;
                }
                _ = SetUsageAsync(mdl, usage);
            }
        }
        #endregion

        #region FilterMode  ===================================================
        TextBlock _filterControl;
        internal void FilterMode_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Selected == PointerModel(e))
            {
                var obj = sender as TextBlock;
                ExecuteFilterMode(obj);
            }
        }
        void ExecuteFilterMode(TextBlock obj)
        {
            if (obj == null) return;

            _ = RefreshViewListAsync(ChangeType.ToggleFilter);
        }
        #endregion

        #region FilterText  ===================================================
        internal void FilterText_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var obj = sender as TextBox;
            if (obj != null && obj.DataContext is ItemModelUI mc && mc.Model is ItemModel mdl)
            {
                if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Tab)
                {
                    e.Handled = true;

                    var txt = string.IsNullOrWhiteSpace(obj.Text) ? string.Empty : obj.Text;
                    if (string.Compare(txt, (string)obj.Tag, true) == 0)
                    {
                        SetDefaultFocus();
                        return;
                    }

                    obj.Tag = txt;
                    _ = SetFilterAsync(mdl, txt);
                    mdl.IsExpandedLeft = true;

                    _tryAfterRefresh = true;
                    _ = RefreshViewListAsync(ChangeType.FilterSortChanged);
                }
                if (e.Key == Windows.System.VirtualKey.Escape)
                {
                    e.Handled = true;

                    mdl.IsFilterVisible = false;
                    mdl.IsExpandedLeft = false;
                    SetDefaultFocus();
                    _ = RefreshViewListAsync(ChangeType.FilterSortChanged);
                }
            }
        }
        #endregion

        #region TextProperty  =================================================
        internal void TextProperty_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var obj = sender as TextBox;
            _focusControl = obj;
            if (obj.DataContext is ItemModelUI mc && mc.Model is ItemModel mdl)
            {
                Selected = mdl;
                RefreshSelector(false);
            }
        }
        internal void TextProperty_LostFocus(object sender, RoutedEventArgs e)
        {
            var obj = sender as TextBox;
            if (obj.DataContext is ItemModelUI mc && mc.PropModel is PropertyModel mdl)
            {
                if ((string)obj.Tag != obj.Text)
                {
                    TCM.PostSetTextValue(mdl, obj.Text);
                }
            }
        }
        internal void TextProperty_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Tab)
            {
                e.Handled = true;
                var obj = sender as TextBox;
                if (obj.DataContext is ItemModelUI mc && mc.PropModel is PropertyModel mdl)
                {
                    if ((string)obj.Tag != obj.Text)
                    {
                        TCM.PostSetTextValue(mdl, obj.Text);
                    }
                    if (e.Key == Windows.System.VirtualKey.Enter)
                        FocusButton.Focus(FocusState.Keyboard);
                    else
                        FindNextItemModel(mdl);
                }
            }
            else if (e.Key == Windows.System.VirtualKey.Escape)
            {
                e.Handled = true;
                var obj = sender as TextBox;
                if (obj.DataContext is ItemModelUI mc && mc.PropModel is PropertyModel mdl)
                {
                    if ((string)obj.Tag != obj.Text)
                    {
                        obj.Text = TCM.GetTextValue(mdl) ?? string.Empty;
                    }
                    ToggleParentExpandRight(mdl);
                }
            }
        }
        #endregion

        #region CheckProperty  ================================================
        internal void CheckProperty_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var obj = sender as CheckBox;
            _focusControl = obj;
            if (obj.DataContext is ItemModelUI mc && mc.Model is ItemModel mdl)
            {
                Selected = mdl;
                RefreshSelector(false);
            }
        }
        internal void Check_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var obj = sender as CheckBox;
            if (obj.DataContext is ItemModelUI mc && mc.PropModel is PropertyModel mdl)
            {
                var val = obj.IsChecked ?? false;

                if (e.Key == VirtualKey.Escape)
                {
                    e.Handled = true;
                    ToggleParentExpandRight(mdl);
                }
                else if (e.Key == Windows.System.VirtualKey.Enter)
                {
                    e.Handled = true;
                    _ignoreNextCheckBoxEvent = true;
                    TCM.PostSetBoolValue(mdl, !val);
                }
                else if (e.Key == Windows.System.VirtualKey.Tab)
                {
                    e.Handled = true;
                    FindNextItemModel(mdl);
                }
            }
        }
        bool _ignoreNextCheckBoxEvent;
        internal void CheckProperty_Checked(object sender, RoutedEventArgs e)
        {
            if (_ignoreNextCheckBoxEvent)
            {
                _ignoreNextCheckBoxEvent = false;
            }
            else
            {
                var obj = sender as CheckBox;
                if (obj.DataContext is ItemModelUI mc && mc.PropModel is PropertyModel mdl)
                {
                    var val = obj.IsChecked ?? false;
                    TCM.PostSetBoolValue(mdl, val);
                }
            }
        }
        #endregion

        #region ComboProperty  ================================================
        internal void ComboProperty_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var obj = sender as ComboBox;
            _focusControl = obj;
            if (obj.DataContext is ItemModelUI mc && mc.Model is ItemModel mdl)
            {
                Selected = mdl;
                RefreshSelector(false);
            }
        }
        internal void ComboProperty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as ComboBox;
            if (obj.DataContext is ItemModelUI mc && mc.PropModel is PropertyModel mdl && obj.SelectedIndex >= 0)
            {
                TCM.PostSetIndexValue(mdl, obj.SelectedIndex);
            }
        }
        internal void ComboProperty_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var obj = sender as ComboBox;
            if (obj.DataContext is ItemModelUI mc && mc.Model is ItemModel mdl)
            {
                if (e.Key == VirtualKey.Escape)
                {
                    e.Handled = true;
                    ToggleParentExpandRight(mdl);
                }
                else if (e.Key == VirtualKey.Tab)
                {
                    e.Handled = true;
                    FindNextItemModel(mdl);
                }
            }
        }
        #endregion

        #region ToggleParentExpandRight  ======================================
        private void ToggleParentExpandRight(ItemModel child)
        {
            var parent = child.ParentModel;
            if (ModelList.Contains(parent))
            {
                Selected = parent;
                _ = RefreshViewListAsync(ChangeType.ToggleRight);
            }
        }
        #endregion

        #region PropertyBorder  ===============================================
        internal void PropertyBorder_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as Border;
            ItemIdentityTip.DataContext = obj.DataContext;
        }
        #endregion

        #region FindNextItemModel  ============================================
        void FindNextItemModel(ItemModel m)
        {
            var k = ModelList.IndexOf(m) + 1;
            for (int i = k; i < ModelList.Count; i++)
            {
                var mdl = ModelList[i];
                if (!(mdl is PropertyModel)) continue;
                TrySetControlFocus(mdl);
                return;
            }
            SetDefaultFocus();
        }
        #endregion
    }
}
