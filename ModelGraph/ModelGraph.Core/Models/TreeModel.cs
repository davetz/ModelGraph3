using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    /// <summary>Flat list of LineModel that emulates a UI tree view</summary>
    public class TreeModel : ItemModelOf<PageModel>, ITreeModel, ILeadModel
    {
        private ModelBuffer _buffer = new ModelBuffer(20);
        internal override void Add(ItemModel headerModel) => HeaderModel = headerModel;
        public ItemModel HeaderModel { get; private set; }

        public PageModel PageModel => Item;
        internal override Item GetOwner() => Item; //normally an itemModel is owned by another parent ItemModel

        #region Constructor  ==================================================
        internal TreeModel(PageModel owner) //========== invoked in the RootModel constructor
        {
            Item = owner;
            Depth = 254;
            owner.Add(this);

            new Model_612_Root(this, Item.Owner);
        }
        internal TreeModel(PageModel owner, Action<TreeModel> createHeaderModel)
        {
            Item = owner;
            Depth = 254;
            owner.Add(this);
            SetHeaderModel(createHeaderModel);
        }
        internal void SetHeaderModel(Action<TreeModel> createHeaderModel)
        {
            HeaderModel?.Discard();
            if (createHeaderModel is null)
                new Model_600_Dummy(this);
            else
            {
                createHeaderModel(this);
                if (HeaderModel.CanExpandLeft)
                    HeaderModel.ExpandLeft(Item.Owner);
                else if (HeaderModel.CanExpandRight)
                    HeaderModel.ExpandRight(Item.Owner);
                if (HeaderModel.Count > 0) RefreshViewList(20, HeaderModel.Items[0], HeaderModel.Items[0], ChangeType.ViewListChanged);
            }
        }

        #endregion

        #region FilterParms  ==================================================
        public void SetUsage(ItemModel model, Usage usage) => FilterSort.SetUsage(model, usage);
        public void SetSorting(ItemModel model, Sorting sorting) => FilterSort.SetSorting(model, sorting);
        public void SetFilter(ItemModel model, string text) => FilterSort.SetText(model, text);
        public (int, Sorting, Usage, string) GetFilterParms(ItemModel model) => FilterSort.GetParms(model);
        #endregion

        #region GetCurrentView  ===============================================
        /// <summary>We are scrolling back and forth in the flattened model hierarchy</summary>
        public (List<ItemModel>, ItemModel, bool, bool) GetCurrentView(int viewSize, ItemModel leading, ItemModel selected)
        {
            if (_buffer.IsEmpty) _buffer.Refresh(HeaderModel, viewSize, leading);

            var (list, eov, sov) = _buffer.GetList();

            if (IsInvalidModel(selected) || !list.Contains(selected)) selected = null;


            return (list, selected, sov, eov);
        }
        #endregion

        #region RefreshViewList  ==============================================
        // Runs on a background thread invoked by the ModelTreeControl 
        public void RefreshViewList(int viewSize, ItemModel leading, ItemModel selected, ChangeType change = ChangeType.None)
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

            switch (change)
            {
                case ChangeType.OneUp:
                    _buffer.Refresh(HeaderModel, viewSize, -1);
                    break;
                case ChangeType.TwoUp:
                    _buffer.Refresh(HeaderModel, viewSize, -2);
                    break;
                case ChangeType.PageUp:
                    _buffer.Refresh(HeaderModel, viewSize, -viewSize);
                    break;
                case ChangeType.OneDown:
                    _buffer.Refresh(HeaderModel, viewSize, 1);
                    break;
                case ChangeType.TwoDown:
                    _buffer.Refresh(HeaderModel, viewSize, 2);
                    break;
                case ChangeType.PageDown:
                    _buffer.Refresh(HeaderModel, viewSize, viewSize);
                    break;
                case ChangeType.ToggleLeft:
                    selected.ToggleLeft(Item.Owner);
                    _buffer.Refresh(HeaderModel, viewSize, leading);
                    break;
                case ChangeType.ToggleRight when isValidSelect:
                    selected.ToggleRight(Item.Owner);
                    _buffer.Refresh(HeaderModel, viewSize, leading);
                    break;
                case ChangeType.ExpandAllLeft when isValidSelect:
                    selected.ExpandAllLeft(Item.Owner);
                    _buffer.Refresh(HeaderModel, viewSize, leading);
                    break;
                case ChangeType.ExpandAllRight when isValidSelect:
                    selected.ExpandAllRight(Item.Owner);
                    _buffer.Refresh(HeaderModel, viewSize, leading);
                    break;
                case ChangeType.ToggleFilter when isValidSelect:
                    selected.IsFilterVisible = !selected.IsFilterVisible;
                    break;
                case ChangeType.ViewListChanged:
                case ChangeType.FilterSortChanged:
                    _buffer.Refresh(HeaderModel, viewSize, leading);
                    break;
            }
            PageModel.TriggerUIRefresh();
        }
        int _viewSize;
        ItemModel _leading;
        ItemModel _selected;
        #endregion

        #region Validate  =====================================================
        /// <summary>Validate model against the model's item, return true if any child list changed</summary>
        internal void Validate()
        {
            var prev = new Dictionary<Item, ItemModel>();
            if (HeaderModel.Validate(Item.Owner, prev)) // will return true if the lineModel hierarchy has changed
                RefreshViewList(ChangeType.ViewListChanged);
        }
        #endregion
    }
}
