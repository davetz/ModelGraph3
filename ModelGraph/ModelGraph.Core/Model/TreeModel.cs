using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    /// <summary>Flat list of LineModel that emulates a UI tree view</summary>
    public abstract class TreeModel : LineModel, IRootModel
    {
        private ModelBuffer _buffer = new ModelBuffer(20);

        public Item RootItem => Item;
        public IPageControl PageControl { get; set; } // reference the UI PageControl       
        public ControlType ControlType { get; private set; }

        #region Constructor  ==================================================
        internal TreeModel(Root root) //========== invoked in the RootModel constructor
        {
            Owner = Item = root;
            Depth = 254;
            ControlType = ControlType.PrimaryTree;

            new RootModel_612(this, root);
            root.Add(this);
        }
        internal TreeModel(Root root, Action<Root,TreeModel> newLineModel)
        {
            Owner = Item = root;
            Depth = 254;
            ControlType = ControlType.PartialTree;

            newLineModel(root, this);
            root.Add(this);
        }
        #endregion

        #region IRootModel  ===================================================
        public string TitleName => DataRoot.TitleName;
        public string TitleSummary => DataRoot.TitleSummary;

        public void Release()
        {
            if (Owner is null) return;

            DataRoot.Remove(this);
            Discard(); //discard myself and recursivly discard all my children

            if (this is RootModel)
                DataRoot.Discard(); //kill off the dataChef

            Owner = null;
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
            if (_buffer.IsEmpty) _buffer.Refresh(Items[0], viewSize, leading);

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
                        _buffer.Refresh(Items[0], viewSize, -1);
                        break;
                    case ChangeType.TwoUp:
                        _buffer.Refresh(Items[0], viewSize, -2);
                        break;
                    case ChangeType.PageUp:
                        _buffer.Refresh(Items[0], viewSize, -viewSize);
                        break;
                    case ChangeType.OneDown:
                        _buffer.Refresh(Items[0], viewSize, 1);
                        break;
                    case ChangeType.TwoDown:
                        _buffer.Refresh(Items[0], viewSize, 2);
                        break;
                    case ChangeType.PageDown:
                        _buffer.Refresh(Items[0], viewSize, viewSize);
                        break;
                    case ChangeType.ToggleLeft:
                        selected.ToggleLeft(Owner as Root);
                        _buffer.Refresh(Items[0], viewSize, leading);
                        break;
                    case ChangeType.ToggleRight:
                        selected.ToggleRight(Owner as Root);
                        _buffer.Refresh(Items[0], viewSize, leading);
                        break;
                    case ChangeType.ToggleFilter:
                        selected.IsFilterVisible = !selected.IsFilterVisible;
                        break;
                    case ChangeType.ViewListChanged:
                    case ChangeType.FilterSortChanged:
                        _buffer.Refresh(Items[0], viewSize, leading);
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
            var viewListChanged = false;
            foreach (var model in Items)
            {
                viewListChanged |= model.Validate(Owner as Root, prev);
            }
            if (viewListChanged) 
                RefreshViewList(ChangeType.ViewListChanged);
            else
                PageControl?.Refresh();
        }
        #endregion

        #region OverrideMethods  ==============================================
        public override (string, string) GetKindNameId(Root root) => (null, BlankName);
        #endregion
    }
}
