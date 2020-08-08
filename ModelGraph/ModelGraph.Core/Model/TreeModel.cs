using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    /// <summary>Flat list of LineModel that emulates a UI tree view</summary>
    public class TreeModel : ItemModelOf<Root>, IDataModel
    {
        private ModelBuffer _buffer = new ModelBuffer(20);
        private LineModel _childModel; // there will only be one child model
        internal override void Add(LineModel child) => _childModel = child;

        public Item RootItem => Item;

        public IPageControl PageControl { get; set; } // reference the UI PageControl       
        public ControlType ControlType { get; private set; }

        #region Constructor  ==================================================
        internal override Item GetOwner() => Item;
        internal TreeModel() //========== invoked in the RootModel constructor
        {
            Item = new Root();
            Depth = 254;
            ControlType = ControlType.PrimaryTree;
            Item.Add(this);

            new Model_612_Root(this, Item);
        }
        internal TreeModel(Root root, Action<TreeModel> createChildModel)
        {
            Item = root;
            Depth = 254;
            ControlType = ControlType.PartialTree;
            Item.Add(this);

            createChildModel(this);
            _childModel.ExpandLeft(Item);
            if (_childModel.Count > 0) RefreshViewList(20, _childModel.Items[0], _childModel.Items[0], ChangeType.None);
        }
        #endregion

        #region IDataModel  ===================================================
        public string TitleName => Item.TitleName;
        public string TitleSummary => Item.TitleSummary;

        public void Release()
        {
            if (Item is null) return;

            Item.Remove(this);
            Discard(); //discard myself and recursivly discard all my children

            if (this is RootModel)
                DataRoot.Discard(); //kill off the dataChef

            Item = null;
        }
        #endregion

        #region FilterParms  ==================================================
        public void SetUsage(LineModel model, Usage usage) => FilterSort.SetUsage(model, usage);
        public void SetSorting(LineModel model, Sorting sorting) => FilterSort.SetSorting(model, sorting);
        public void SetFilter(LineModel model, string text) => FilterSort.SetText(model, text);
        public (int, Sorting, Usage, string) GetFilterParms(LineModel model) => FilterSort.GetParms(model);
        #endregion

        #region GetCurrentView  ===============================================
        /// <summary>We are scrolling back and forth in the flattened model hierarchy</summary>
        public (List<LineModel>, LineModel, bool, bool) GetCurrentView(int viewSize, LineModel leading, LineModel selected)
        {
            if (_buffer.IsEmpty) _buffer.Refresh(_childModel, viewSize, leading);

            var (list, eov, sov) = _buffer.GetList();

            if (IsInvalidModel(selected) || !list.Contains(selected)) selected = null;


            return (list, selected, sov, eov);
        }
        #endregion

        #region RefreshViewList  ==============================================
        // Runs on a background thread invoked by the ModelTreeControl 
        public void RefreshViewList(int viewSize, LineModel leading, LineModel selected, ChangeType change = ChangeType.None)
        {
            _viewSize = viewSize;
            _leading = leading;
            _selected = selected;
            RefreshViewList(change);
        }
        public void RefreshViewList(ChangeType change = ChangeType.None)
        {
            var viewSize = _viewSize;
            var leading = _leading;
            var selected = _selected;

            bool isValidSelect = IsValidModel(selected);

            if (isValidSelect)
            {
                switch (change)
                {
                    case ChangeType.OneUp:
                        _buffer.Refresh(_childModel, viewSize, -1);
                        break;
                    case ChangeType.TwoUp:
                        _buffer.Refresh(_childModel, viewSize, -2);
                        break;
                    case ChangeType.PageUp:
                        _buffer.Refresh(_childModel, viewSize, -viewSize);
                        break;
                    case ChangeType.OneDown:
                        _buffer.Refresh(_childModel, viewSize, 1);
                        break;
                    case ChangeType.TwoDown:
                        _buffer.Refresh(_childModel, viewSize, 2);
                        break;
                    case ChangeType.PageDown:
                        _buffer.Refresh(_childModel, viewSize, viewSize);
                        break;
                    case ChangeType.ToggleLeft:
                        selected.ToggleLeft(Item);
                        _buffer.Refresh(_childModel, viewSize, leading);
                        break;
                    case ChangeType.ToggleRight:
                        selected.ToggleRight(Item);
                        _buffer.Refresh(_childModel, viewSize, leading);
                        break;
                    case ChangeType.ToggleFilter:
                        selected.IsFilterVisible = !selected.IsFilterVisible;
                        break;
                    case ChangeType.ViewListChanged:
                    case ChangeType.FilterSortChanged:
                        _buffer.Refresh(_childModel, viewSize, leading);
                        break;
                }
            }
            PageControl?.Refresh();
        }
        int _viewSize;
        LineModel _leading;
        LineModel _selected;
        #endregion

        #region Validate  =====================================================
        /// <summary>Validate model against the model's item, return true if any child list changed</summary>
        internal void Validate()
        {
            var prev = new Dictionary<Item, LineModel>();
            if (_childModel.Validate(Item, prev)) // will return true if the lineModel hierarchy has changed
                RefreshViewList(ChangeType.ViewListChanged);
            else
                PageControl?.Refresh();
        }
        #endregion

        #region OverrideMethods  ==============================================
        public override string GetNameId() => BlankName;
        public override void GetButtonCommands(Root root, List<LineCommand> list) => _childModel.GetButtonCommands(root, list);
        #endregion

        #region Save/SaveAs/Reload  ===========================================
        public bool IsClosed { get; private set; }
        internal void Close()
        {
            IsClosed = true;
            PageControl?.Close();
        }

        internal void Save()
        {
            PageControl?.Save();
        }
        internal void SaveAs()
        {
            PageControl?.SaveAs();
        }
        internal void Reload()
        {
            PageControl?.Reload();
        }
        internal void NewView(Action<TreeModel> createChildModel = null)
        {
            var model = new TreeModel(Item, createChildModel);
            PageControl?.NewView(model);
        }
        internal void NewView(ControlType type)
        {
            switch (type)
            {
                case ControlType.SymbolEditor:
                    break;
                case ControlType.GraphDisplay:
                    break;
            }
        }
        #endregion
    }
}
