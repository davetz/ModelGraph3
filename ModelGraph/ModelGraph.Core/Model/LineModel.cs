using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class LineModel : StoreOf<LineModel>
    {
        public Item Item { get; protected set; }
        private ModelFlags _modelFlags;
        public byte Depth;      // depth of tree hierarchy

        public LineModel ParentModel => Owner as LineModel;

        #region Constructor  ==================================================
        internal LineModel() { }
        internal LineModel(LineModel owner, Item item)
        {
            Item = item;
            Owner = owner;
            Depth = (byte)(owner.Depth + 1);

            if( item.AutoExpandRight)
            {
                item.AutoExpandRight = false;
                ExpandRight(DataRoot);
            }

            owner.CovertAdd(this);
        }
        #endregion

        #region ModelState  ===================================================
        [Flags]
        private enum ModelFlags : ushort
        {
            None = 0,

            IsChanged = S1,
            HasNoError = S2,
            ChangedSort = S3,
            ChangedFilter = S4,

            IsExpandLeft = S5,
            IsExpandRight = S6,
            IsFilterFocus = S7,
            IsFilterVisible = S8,

            IsUsedFilter = S9,
            IsNotUsedFilter = S10,
            IsAscendingSort = S11,
            IsDescendingSort = S12,

            HasFilterSortAllocation = S13,

            IsExpanded = IsExpandLeft | IsExpandRight,
            IsSorted = IsAscendingSort | IsDescendingSort,
            IsUsageFiltered = IsUsedFilter | IsNotUsedFilter,

            AnyFilterSortChanged = ChangedSort | ChangedFilter,
            SortUsageMode = IsUsageFiltered | IsSorted | ChangedSort | ChangedFilter,
        }
        private bool GetFlag(ModelFlags flag) => (_modelFlags & flag) != 0;
        private void SetFlag(ModelFlags flag, bool value) { if (value) _modelFlags |= flag; else _modelFlags &= ~flag; }
        private void SetFlag(ModelFlags flag, ModelFlags changedState, bool value) { var prev = GetFlag(flag); if (value) _modelFlags |= flag; else _modelFlags &= ~flag; if (prev != value) _modelFlags |= changedState; }

        public bool IsChanged { get { return GetFlag(ModelFlags.IsChanged); } set { SetFlag(ModelFlags.IsChanged, value); } }
        public bool IsFilterFocus { get { return GetFlag(ModelFlags.IsFilterFocus); } set { SetFlag(ModelFlags.IsFilterFocus, value); } }
        internal bool HasNoError { get { return GetFlag(ModelFlags.HasNoError); } set { SetFlag(ModelFlags.HasNoError, value); } }
        internal bool HasFilterSortAllocation { get { return GetFlag(ModelFlags.HasFilterSortAllocation); } set { SetFlag(ModelFlags.HasFilterSortAllocation, value); } }


        public bool IsUsedFilter { get { return GetFlag(ModelFlags.IsUsedFilter); } set { SetFlag(ModelFlags.IsUsedFilter, ModelFlags.ChangedFilter, value); } }
        public bool IsNotUsedFilter { get { return GetFlag(ModelFlags.IsNotUsedFilter); } set { SetFlag(ModelFlags.IsNotUsedFilter, ModelFlags.ChangedFilter, value); } }


        public bool IsSortAscending { get { return GetFlag(ModelFlags.IsAscendingSort); } set { SetFlag(ModelFlags.IsAscendingSort, ModelFlags.ChangedSort, value); } }
        public bool IsSortDescending { get { return GetFlag(ModelFlags.IsDescendingSort); } set { SetFlag(ModelFlags.IsDescendingSort, ModelFlags.ChangedSort, value); } }

        public bool IsExpandedLeft { get { return GetFlag(ModelFlags.IsExpandLeft); } set { SetFlag(ModelFlags.IsExpandLeft, value); if (!value) SetFlag(ModelFlags.IsExpandRight, false); } }
        public bool IsExpandedRight { get { return GetFlag(ModelFlags.IsExpandRight); } set { SetFlag(ModelFlags.IsExpandRight, value); } }

        public bool IsFilterVisible { get { return GetFlag(ModelFlags.IsFilterVisible); } set { SetFlag(ModelFlags.IsFilterVisible, value); if (value) IsFilterFocus = true; else _modelFlags |= ModelFlags.ChangedFilter; } }

        internal bool IsExpanded => GetFlag(ModelFlags.IsExpanded);
        internal bool IsUsageFiltered => GetFlag(ModelFlags.IsUsageFiltered);
        internal bool ChangedSort => GetFlag(ModelFlags.ChangedSort);
        internal bool ChangedFilter => GetFlag(ModelFlags.ChangedFilter);
        internal bool AnyFilterSortChanged => GetFlag(ModelFlags.AnyFilterSortChanged);
        internal void ClearChangedFlags() => _modelFlags &= ~ModelFlags.AnyFilterSortChanged;
        internal void ClearSortUsageMode() => _modelFlags &= ~ModelFlags.SortUsageMode;
        #endregion

        #region BufferTraverse  ===============================================
        /// <summary>Fill the circular buffer with the flattened lineModel hierarchy, return true if buffer.AddItem(model) hits the target model</summary>
        internal bool BufferTraverse(ModelBuffer buffer)
        {
            if (HasFilterSortAllocation && FilterSort.TryGetSelector(this, out List<(int I, bool IN, string TX)> selector))
            {
                foreach (var (I, IN, _) in selector)
                {
                    if (IN && I < Items.Count)
                    {
                        var child = Items[I];
                        if (buffer.AddItem(child)) return true; // abort, we are done
                        if (child.BufferTraverse(buffer)) return true; // abort, we are done;
                    }
                }
            }
            else
            {
                foreach (var child in Items)
                {
                    if (buffer.AddItem(child)) return true; // abort, we are done
                    if (child.BufferTraverse(buffer)) return true; // abort, we are done;
                }
            }
            return false; //finished all items with no aborts
        }
        #endregion

        #region CommonMethods   ===============================================
        internal Store ItemStore => Item as Store;
        internal bool IsValidModel(LineModel m) => !IsInvalidModel(m);
        internal bool IsInvalidModel(LineModel m) => IsInvalid(m) || IsInvalid(m.Item);
        internal bool ToggleLeft(Root root) => IsExpandedLeft ? CollapseLeft() : ExpandLeft(root);
        internal bool ToggleRight(Root root) => IsExpandedRight ? CollapseRight() : ExpandRight(root);

        protected bool CollapseLeft()
        {
            IsExpandedLeft = false;
            IsExpandedRight = false;
            if (Count == 0) return false;
            DiscardChildren();
            CovertClear();
            return true;
        }
        protected bool CollapseRight()
        {
            var anyChange = false;
            if (IsExpandedRight)
            {
                int N = Count;
                for (int i = 0; i < N; i++)
                {
                    var item = Items[0];
                    if (item is PropertyModel)
                    {
                        anyChange = true;
                        CovertRemove(item);
                        item.Discard();
                    }
                    else
                        break;
                }
                IsExpandedRight = false;
            }
            return anyChange;
        }
        internal override void Discard()
        {
            FilterSort.ReleaseFilter(this);
            IsDiscarded = true;
            DiscardChildren();
        }

        internal override void DiscardChildren()
        {
            foreach (var child in Items) 
            {
                FilterSort.ReleaseFilter(child);
                child.IsDiscarded = true;
                child.DiscardChildren();
            }
        }
        /// <summary>Walk up item tree hierachy to find the parent RootTreeModel</summary>
        public RootModel RootModel => GetRootModel();
        private RootModel GetRootModel()
        {
            var item = this;
            while (item != null)
            {
                if (item is RootModel root) return root;
                item = item.Owner as LineModel;
            }
            throw new Exception("GetRootModel: Corrupted item hierarchy"); // I seriously hope this never happens
        }

        public void DragStart(Root root) => Root.DragDropSource = this;
        public DropAction DragEnter(Root root) => IsValidModel(Root.DragDropSource) ? ModelDrop(root, Root.DragDropSource, false) : DropAction.None;
        public void DragDrop(Root root)
        {
            var dropModel = Root.DragDropSource;

            if (IsValidModel(dropModel))
            {
                ModelDrop(root, Root.DragDropSource, true);
                root.PostRefresh();
            }
        }

        #endregion

        #region Virtual Functions  ============================================
        internal virtual bool ExpandLeft(Root root) => false;
        internal virtual bool ExpandRight(Root root) => false;

        internal virtual bool IsItemUsed => true;

        public virtual int TotalCount => 0;
        internal virtual string GetFilterSortId(Root root) => $"{Item.GetKindId(root)}{Item.GetNameId(root)}";

        public byte ItemDelta => (byte)(Item.ChildDelta + Item.ModelDelta);
        public virtual bool CanDrag => false;
        public virtual bool CanSort => false;
        public virtual bool CanFilter => false;
        public virtual bool CanExpandLeft => false;
        public virtual bool CanExpandRight => false;
        public virtual bool CanFilterUsage => false;

        public virtual string GetModelInfo(Root root) => default;

        public virtual void GetMenuCommands(Root root, List<LineCommand> list) { list.Clear(); }
        public virtual void GetButtonCommands(Root root, List<LineCommand> list) { list.Clear(); }

        internal  virtual DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop ) => DropAction.None;
        public virtual DropAction ReorderItems(Root root, LineModel target, bool doDrop) => DropAction.None;

        public virtual Error TryGetError(Root root) => default;

        public virtual string GetModelIdentity() =>  $"{IdKey}";

        /// <summary>Validate model against the model's item, return true if any child list changed</summary>
        internal virtual bool Validate(Root root, Dictionary<Item, LineModel> prev)
        {
            var viewListChange = false;
            foreach (var child in Items)
            {
                viewListChange |= child.Validate(root, prev);
            }
            return viewListChange;
        }
        #endregion
    }
}
