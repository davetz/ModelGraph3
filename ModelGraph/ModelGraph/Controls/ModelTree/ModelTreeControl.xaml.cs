using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using ModelGraph.Core;
using ModelGraph.Helpers;
using ModelGraph.Services;
using Windows.System;

namespace ModelGraph.Controls
{
    public sealed partial class ModelTreeControl : Page, IPageControl, IModelPageControl
    {
        #region Properties  ===================================================
        Root DataRoot;
        TreeModel TreeRoot;
        LineModel Selected;
        private List<LineModel> ViewList = new List<LineModel>(0);
        readonly List<LineCommand> MenuCommands = new List<LineCommand>();
        readonly List<LineCommand> ButtonCommands = new List<LineCommand>();

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

        ToolTip[] MenuItemTips;
        ToolTip[] ItemButtonTips;

        Button[] ItemButtons;
        MenuFlyoutItem[] MenuItems;
        int MenuItemsCount;

        int Count => (ViewList == null) ? 0 : ViewList.Count;

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

        #region Constructor  ==================================================
        public ModelTreeControl(TreeModel root)
        {
            if (root is null) throw new NullReferenceException();
            TreeRoot = root;
            DataRoot = root.DataRoot;

            InitializeComponent();
            Initialize();
            Loaded += ModelTreeControl_Loaded;
        }
        private void ModelTreeControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= ModelTreeControl_Loaded;
            InitializeCache();
            Refresh();
        }
        #endregion

        #region ModelUICache  =================================================
        readonly Stack<ModelUICache> FreeCacheStack = new Stack<ModelUICache>(37);
        readonly Dictionary<LineModel, ModelUICache> Model_Cache = new Dictionary<LineModel, ModelUICache>(31);
        readonly HashSet<LineModel> DefunctModels = new HashSet<LineModel>();

        private void InitializeCache()
        {
            ModelUICache.Allocate(this, TreeCanvas, TreeRoot, DataRoot, 31, FreeCacheStack);
        }
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
        private void RefreshCache(List<LineModel> newModels)
        {
            ViewList = newModels; // save this for future reference

            DefunctModels.Clear();
            foreach (var m in Model_Cache.Keys) { DefunctModels.Add(m); }

            for (int i = 0; i < newModels.Count; i++)
            {
                var m = newModels[i];
                if (Model_Cache.TryGetValue(m, out ModelUICache mc))
                {
                    mc.Validate(i);
                    DefunctModels.Remove(m);
                }
                else
                {
                    if (FreeCacheStack.Count == 0) ModelUICache.Allocate(this, TreeCanvas, TreeRoot, DataRoot, 31, FreeCacheStack);
                    var c = FreeCacheStack.Pop();
                    c.Initialize(m, i);
                    Model_Cache.Add(m, c);
                }
            }
            foreach (var m in DefunctModels)  // reclaim and save the uiCach from the defunct models
            {
                var oc = GetModelUICache(m);
                oc.Clear();
                FreeCacheStack.Push(oc);
                Model_Cache.Remove(m);
            }
        }
        private ModelUICache GetModelUICache(LineModel m)
        {
            if (!Model_Cache.TryGetValue(m, out ModelUICache mc))
                throw new Exception("ExceptionCorruptLineModelCache".GetLocalized());
            return mc;
        }
        #endregion

        #region RefreshViewListAsync  =========================================
        private async System.Threading.Tasks.Task RefreshViewListAsync(ChangeType change = ChangeType.None)
        {
            var (leading, selected) = GetLeadingSelected();
            //ResetCacheDelta(_selected);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { TreeRoot.RefreshViewList(ViewSize, leading, selected, change); });
        }
        private async System.Threading.Tasks.Task SetUsageAsync(LineModel model, Usage usage)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { TreeRoot.SetUsage(model, usage); });
            _ = RefreshViewListAsync(ChangeType.FilterSortChanged);
        }
        private async System.Threading.Tasks.Task SetSortingAsync(LineModel model, Sorting sorting)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { TreeRoot.SetSorting(model, sorting); });
            _ = RefreshViewListAsync(ChangeType.FilterSortChanged);
        }
        private async System.Threading.Tasks.Task SetFilterAsync(LineModel model, string text)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { TreeRoot.SetFilter(model, text); });
            _ = RefreshViewListAsync(ChangeType.FilterSortChanged);
        }
        private (LineModel, LineModel) GetLeadingSelected()
        {
            var leading = (ViewList is null || ViewList.Count == 0) ? null : ViewList[0];
            var selected = (Selected is null) ? leading : Selected;
            return (leading, selected);
        }
        #endregion

        #region SetSize  ======================================================
        public void SetSize(double width, double height)
        {
            if (TreeRoot is null || TreeCanvas is null) return;
            if (height > 0)
            {
                TreeCanvas.Width = Width = width;
                TreeCanvas.Height = Height = height;

                _ = RefreshViewListAsync();
            }
        }
        int ViewSize => (int)(Height / ElementHieght);
        #endregion

        #region IPageControl  =================================================
        public void CreateNewPage(IRootModel model, ControlType ctlType)
        {
            if (model is null) return;
            _ = ModelPageService.Current.CreateNewPageAsync(model, ctlType);
        }
        public IRootModel IModel => TreeRoot;
        #endregion

        #region IModelControl  ================================================
        public void Apply()
        {
        }
        public void Revert()
        {
        }
        public (int Width, int Height) PreferredSize => (400, 320);
        #endregion

        #region Initialize  ===================================================
        void Initialize()
        {
            ItemIdentityTip = new ToolTip();
            ItemIdentityTip.Opened += ItemIdentityTip_Opened;

            ModelIdentityTip = new ToolTip();
            ModelIdentityTip.Opened += ModelIdentityTip_Opened;

            LevelIndent = (int)(Resources["LevelIndent"] as Double?).Value;
            ElementHieght = (int)(Resources["ElementHieght"] as Double?).Value;

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

            SortModeTip = "ModelTree_SortModeTip".GetLocalized();
            UsageModeTip = "ModelTree_UsageModeTip".GetLocalized();
            LeftExpandTip = "ModelTree_LeftExpandTip".GetLocalized();
            TotalCountTip = "ModelTree_TotalCountTip".GetLocalized();
            FilterTextTip = "ModelTree_FilterTextTip".GetLocalized();
            FilterCountTip = "ModelTree_FilterCountTip".GetLocalized();
            RightExpandTip = "ModelTree_RightExpandTip".GetLocalized();
            FilterExpandTip = "ModelTree_FilterExpandTip".GetLocalized();
            ItemHasErrorTip = "ModelTree_ItemHasErrorTip".GetLocalized();

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

        #region Release  ======================================================
        public void Release()
        {
            DiscardCache();

            TreeCanvas.Children.Clear();
            TreeCanvas = null;

            ItemIdentityTip.Opened -= ItemIdentityTip_Opened;
            ModelIdentityTip.Opened -= ModelIdentityTip_Opened;

            TreeRoot = null;
            Selected = null;
            ViewList.Clear();
            MenuCommands.Clear();
            ButtonCommands.Clear();
            ItemIdentityTip = null;
            ModelIdentityTip = null;
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
        private void KeyRight_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (Selected.CanExpandRight)
            {
                _ = RefreshViewListAsync(ChangeType.ToggleRight);
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
                if (!ViewList.Contains(parent)) ViewList[0] = parent;
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
                if (ViewList is null || ViewList.Count == 0) return;
                Selected = ViewList[0];
                RefreshSelector();
            }
            else
            {
                var i = ViewList.IndexOf(Selected) - 1;
                if (i >= 0 && i < ViewList.Count)
                {
                    Selected = ViewList[i];
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
                if (ViewList is null || ViewList.Count == 0) return;
                Selected = ViewList[0];
                RefreshSelector();
            }
            else
            {
                var i = ViewList.IndexOf(Selected) + 1;
                if (i > 0 && i < ViewList.Count)
                {
                    Selected = ViewList[i];
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
            var cmd = btn.Tag as LineCommand;
            cmd.Execute();
        }
        #endregion

        #region Button_Click  =================================================
        void ItemButton_Click(object sender, RoutedEventArgs e)
        {
            var obj = sender as Button;
            var cmd = obj.DataContext as LineCommand;
            cmd.Execute();
        }
        void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var obj = sender as MenuFlyoutItem;
            var cmd = obj.DataContext as LineCommand;
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
            if (tip.DataContext is ModelUICache mc)
            {
                var mdl = mc.Model;
                var content = mdl?.GetSummaryId(DataRoot);
                tip.Content = string.IsNullOrWhiteSpace(content) ? null : content;
            }
            else
                tip.Content = null;
        }
        void ModelIdentityTip_Opened(object sender, RoutedEventArgs e)
        {
            var tip = sender as ToolTip;
            if (tip.DataContext is ModelUICache mc)
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
        LineModel PointerModel(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(TreeCanvas);
            var i = (int)(p.Position.Y / ElementHieght);
            return (Count == 0) ? null : (i < 0) ? ViewList[0] : (i < Count) ? ViewList[i] : ViewList[Count - 1];
        }
        #endregion


        #region Refresh  ======================================================
        public void Refresh()
        {
            _pointWheelEnabled = false;

            var (oldLeading, oldSelected) = GetLeadingSelected();
            var (newModels, newSelected, atStart, atEnd) = TreeRoot.GetCurrentView(ViewSize, oldLeading, oldSelected);

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
            var buttonCommands = new List<LineCommand>();
           //_treeRoot.ButtonComands(buttonCommands);

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
                        btn.Content = cmd.GetNameId(DataRoot);
                        btn.Visibility = Visibility.Visible;
                        ToolTipService.SetToolTip(btn, cmd.GetSummaryId(DataRoot));
                    }
                    else
                    {
                        btn.Visibility = Visibility.Collapsed;
                    }
                }
            }
            ModelTitle.Text = TreeRoot.TitleName;
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
            
            
            var mc = GetModelUICache(Selected);

            if (mc.SortMode != null && !string.IsNullOrEmpty(mc.SortMode.Text))
            {
                _sortControl = mc.SortMode;
                var acc = new KeyboardAccelerator { Key = VirtualKey.S, Modifiers = VirtualKeyModifiers.Control};
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

            if (Selected.DragEnter(DataRoot) != DropAction.None)
            {
                var acc = new KeyboardAccelerator { Key = VirtualKey.V, Modifiers = VirtualKeyModifiers.Control };
                acc.Invoked += Accelerator_ModelPaste_Invoked;
                TreeCanvas.KeyboardAccelerators.Add(acc);
            }

            if (mc.FilterMode != null && string.IsNullOrEmpty(mc.FilterMode.Text))
            {
                _filterControl = mc.FilterMode;
            }


            if (Selected.GetDescriptionId(DataRoot) != null)
            {
                HelpButton.Visibility = Visibility.Visible;
                PopulateItemHelp(Selected.GetDescriptionId(DataRoot));
            }
            else
            {
                HelpButton.Visibility = Visibility.Collapsed;
            }

            Selected.GetMenuCommands(DataRoot, MenuCommands);
            Selected.GetButtonCommands(DataRoot, ButtonCommands);

            var cmds = ButtonCommands;
            var len1 = cmds.Count;
            var len2 = ItemButtons.Length;

            for (int i = 0; i < len2; i++)
            {
                if (i < len1)
                {
                    var cmd = ButtonCommands[i];
                    ItemButtons[i].DataContext = cmd;
                    ItemButtons[i].Content = cmd.GetNameId(DataRoot);
                    ItemButtonTips[i].Content = cmd.GetSummaryId(DataRoot);
                    ItemButtons[i].Visibility = Visibility.Visible;
                    var key = cmd.GetAcceleratorId(DataRoot);
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
                        MenuItems[i].Text = cmd.GetNameId(DataRoot);
                        MenuItemTips[i].Content = cmd.GetSummaryId(DataRoot);
                    }
                }
            }

            if (restoreFocus) TryRestoreFocus();
        }
        #endregion

        #region SetSelectorGridPlacement  =====================================
        void SetSelectorGridPlacement()
        {
            var i = ViewList.IndexOf(Selected);
            if (i < 0) i = 0;
            SelectorGrid.Width = ActualWidth;
            Canvas.SetTop(SelectorGrid, (i * ElementHieght));

        }
        #endregion

        #region TrySetControlFocus  ===========================================
        // given the focusModel try to determine what is the most
        // logical text box to enter, then set the keyboard focus to it,
        // otherwise set focus to our reliable dummy FocusButton
        private void TrySetControlFocus(LineModel focusModel = null)
        {
            if (focusModel != null) Selected = focusModel;

            var lc = GetModelUICache(Selected);

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
                Selected.DragStart(DataRoot);
            args.Handled = true;
        }
        private void Accelerator_ModelPaste_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (Selected.DragEnter(DataRoot) != DropAction.None)
            Selected.DragDrop(DataRoot);
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
            if (_acceleratorKeyCommands.TryGetValue(sender, out LineCommand cmd)) cmd.Execute();
            args.Handled = true;
        }
        private readonly Dictionary<KeyboardAccelerator, LineCommand> _acceleratorKeyCommands = new Dictionary<KeyboardAccelerator, LineCommand>();


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
            if (obj.DataContext is ModelUICache mc && mc.Model is LineModel mdl)
            {
                if (mdl.CanDrag)
                {
                    mdl.DragStart(DataRoot);
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
            if (obj.DataContext is ModelUICache mc && mc.Model is LineModel mdl)
            {
                var type = mdl.DragEnter(DataRoot);
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
            if (obj.DataContext is ModelUICache mc && mc.Model is LineModel mdl)
            {
                mdl.DragDrop(DataRoot);
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

        internal void RefreshExpandTree(LineModel model, TextBlock obj)
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
                if (obj.DataContext is ModelUICache mc && mc.Model is LineModel mdl)
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
                if (obj.DataContext is ModelUICache mc && mc.Model is LineModel mdl)
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
            if (obj != null && obj.DataContext is ModelUICache mc && mc.Model is LineModel mdl)
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
            if (obj != null && obj.DataContext is ModelUICache mc && mc.Model is LineModel mdl)
            {
                if (mdl.IsUsedFilter)
                {
                    mdl.IsUsedFilter = false;
                    mdl.IsNotUsedFilter = true;
                    obj.Text = UsageIsNotUsed;
                }
                else if (mdl.IsNotUsedFilter)
                {
                    mdl.IsUsedFilter = false;
                    mdl.IsNotUsedFilter = false;
                    obj.Text = UsageAll;
                }
                else
                {
                    mdl.IsUsedFilter = true;
                    obj.Text = UsageIsUsed;
                }
                _ = RefreshViewListAsync(ChangeType.FilterSortChanged);
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
            if (obj != null && obj.DataContext is ModelUICache mc && mc.Model is LineModel mdl)
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
            if (obj.DataContext is ModelUICache mc && mc.Model is LineModel mdl)
            {
                Selected = mdl;
                RefreshSelector(false);
            }
        }
        internal void TextProperty_LostFocus(object sender, RoutedEventArgs e)
        {
            var obj = sender as TextBox;
            if (obj.DataContext is ModelUICache mc && mc.PropModel is PropertyModel mdl)
            {
                if ((string)obj.Tag != obj.Text)
                {
                    mdl.PostSetTextValue(DataRoot, obj.Text);
                }
            }
        }
        internal void TextProperty_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Tab)
            {
                e.Handled = true;
                var obj = sender as TextBox;
                if (obj.DataContext is ModelUICache mc && mc.PropModel is PropertyModel mdl)
                {
                    if ((string)obj.Tag != obj.Text)
                    {
                        mdl.PostSetTextValue(DataRoot, obj.Text);
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
                if (obj.DataContext is ModelUICache mc && mc.PropModel is PropertyModel mdl)
                {
                    if ((string)obj.Tag != obj.Text)
                    {
                        obj.Text = mdl.GetTextValue(DataRoot) ?? string.Empty;
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
            if (obj.DataContext is ModelUICache mc && mc.Model is LineModel mdl)
            {
                Selected = mdl;
                RefreshSelector(false);
            }
        }
        internal void Check_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var obj = sender as CheckBox;
            if (obj.DataContext is ModelUICache mc && mc.PropModel is PropertyModel mdl)
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
                    mdl.PostSetBoolValue(DataRoot, !val);
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
                if (obj.DataContext is ModelUICache mc && mc.PropModel is PropertyModel mdl)
                {
                    var val = obj.IsChecked ?? false;
                    mdl.PostSetBoolValue(DataRoot, val);
                }
            }
        }
        #endregion

        #region ComboProperty  ================================================
        internal void ComboProperty_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var obj = sender as ComboBox;
            _focusControl = obj;
            if (obj.DataContext is ModelUICache mc && mc.Model is LineModel mdl)
            {
                Selected = mdl;
                RefreshSelector(false);
            }
        }
        internal void ComboProperty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as ComboBox;
            if (obj.DataContext is ModelUICache mc && mc.PropModel is PropertyModel mdl && obj.SelectedIndex >= 0)
            {
                mdl.PostSetIndexValue(DataRoot, obj.SelectedIndex);
            }
        }
        internal void ComboProperty_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var obj = sender as ComboBox;
            if (obj.DataContext is ModelUICache mc && mc.Model is LineModel mdl)
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
        private void ToggleParentExpandRight(LineModel child)
        {
            var parent = child.ParentModel;
            if (ViewList.Contains(parent))
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
        void FindNextItemModel(LineModel m)
        {
            var k = ViewList.IndexOf(m) + 1;
            for (int i = k; i < ViewList.Count; i++)
            {
                var mdl = ViewList[i];
                if (!(mdl is PropertyModel)) continue;
                TrySetControlFocus(mdl);
                return;
            }
            SetDefaultFocus();
        }
        #endregion
    }
}
