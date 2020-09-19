using System;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer.DragDrop.Core;

namespace ModelGraph.Core
{
    public abstract class ItemModel : ChildOf<ItemModel>
    {
        static readonly List<ItemModel> _noItems = new List<ItemModel>(0);
        virtual internal List<ItemModel> Items => _noItems;

        private ModelFlags _modelFlags;
        public byte Depth;      // depth of tree hierarchy

        public ItemModel ParentModel => Owner;
        internal override Item GetOwner() => Owner;

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
        internal Store ItemStore => GetItem() as Store;
        internal bool IsValidModel(ItemModel m) => !IsInvalidModel(m);
        internal bool IsInvalidModel(ItemModel m) => IsInvalid(m) || IsInvalid(m.GetItem());
        internal bool ToggleLeft(Root root) => IsExpandedLeft ? CollapseLeft() : ExpandLeft(root);
        internal bool ToggleRight(Root root) => IsExpandedRight ? CollapseRight() : ExpandRight(root);

        internal bool ExpandAllLeft(Root root)
        {
            var anyChange = false;
            if (CanExpandLeft && Depth < 10)
            {
                anyChange |= ExpandLeft(root);
                foreach (var m in Items)
                {
                    anyChange |= m.ExpandAllLeft(root);
                }
            }
            return anyChange;
        }
        internal bool ExpandAllRight(Root root)
        {
            var anyChange = false;
            if (Depth < 10)
            {
                if (CanExpandLeft && !CanExpandRight)
                {
                    anyChange |= ExpandLeft(root);
                    foreach (var m in Items)
                    {
                        anyChange |= m.ExpandAllRight(root);
                    }
                }
                else if (!CanExpandLeft && CanExpandRight)
                {
                    anyChange |= ExpandRight(root);
                }
                else if (CanExpandLeft && CanExpandRight)
                {
                    anyChange |= ExpandRight(root);
                    anyChange |= ExpandLeft(root);
                    foreach (var m in Items)
                    {
                        if (m.CanExpandLeft || m.CanExpandRight)
                            anyChange |= m.ExpandAllRight(root);
                    }
                }
            }
            return anyChange;
        }

        protected bool CollapseLeft()
        {
            IsExpandedLeft = false;
            IsExpandedRight = false;
            if (Count == 0) return false;
            DiscardChildren();
            Clear();
            return true;
        }
        protected bool CollapseRight()
        {
            var anyChange = false;
            if (IsExpandedRight)
            {
                int L = Count - 1;
                for (int i = L; i >= 0; i--)
                {
                    var item = Items[i];
                    if (item is PropertyModel)
                    {
                        anyChange = true;
                        RemoveAt(i);
                        item.Discard();
                    }
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
        /// <summary>Walk up item tree hierachy to find the parent TreeModel</summary>
        internal PageModel GetPageModel()
        {
            var item = this;
            while (item != null)
            {
                if (item is TreeModel treeModel) return treeModel.PageModel;
                item = item.Owner;
            }
            throw new Exception("GetRootModel: Corrupted item hierarchy"); // I seriously hope this never happens
        }

        public void DragStart(Root root) => Root.DragDropSource = this;
        public DropAction DragEnter(Root root)
        {
            var m = Root.DragDropSource;
            return (IsValidModel(m) && this != m)  ? (Owner == m.Owner) ? CanReorderItems ? DropAction.Move : DropAction.None : ModelDrop(root, m, false) : DropAction.None;
        }
        public void DragDrop(Root root)
        {
            var m = Root.DragDropSource;
            if (IsValidModel(m))
            {
                if (Owner == m.Owner)
                {
                    if (CanReorderItems && ReorderItems(root, m))
                    {
                        Owner.ChildDelta -= 3;
                        root.PostRefresh();
                    }
                }
                else
                {
                    ModelDrop(root, Root.DragDropSource, true);
                    root.PostRefresh();
                }
            }
        }
        protected bool ReorderStoreItems(Root root, Store sto, Item refItem, Item moveItem)
        {
            var index1 = sto.IndexOf(moveItem);
            var index2 = sto.IndexOf(refItem);
            if (!(index1 < 0 || index2 < 0 || index1 == index2))
            {
                ItemMoved.Record(root, moveItem, index1, index2);
                return true;
            }
            return false;
        }
        protected bool ReorderChildItems(Root root, Relation rel, Item key, Item refItem, Item moveItem)
        {
            if (rel.TryGetChildren(key, out List<Item> list))
            {
                var index1 = list.IndexOf(moveItem);
                var index2 = list.IndexOf(refItem);
                if (!(index1 < 0 || index2 < 0 || index1 == index2))
                {
                    ItemChildMoved.Record(root, rel, key, moveItem, index1, index2);
                    return true;
                }
            }
            return false;
        }
        protected bool ReorderParentItems(Root root, Relation rel, Item key, Item refItem, Item moveItem)
        {
            if (rel.TryGetParents(key, out List<Item> list))
            {
                var index1 = list.IndexOf(moveItem);
                var index2 = list.IndexOf(refItem);
                if (index1 < 0 || index2 < 0 || index1 == index2) return false;
                if (!(index1 < 0 || index2 < 0 || index1 == index2))
                {
                    ItemParentMoved.Record(root, rel, key, moveItem, index1, index2);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Virtual Functions  ============================================
        internal abstract Item GetItem();

        internal virtual bool UseItemIdentity => true;

        internal virtual bool ExpandLeft(Root root) => false;
        internal virtual bool ExpandRight(Root root) => false;
        internal virtual Property[] GetProperties() => null;

        internal virtual bool IsItemUsed => true;

        public virtual int TotalCount => 0;
        internal virtual string GetFilterSortId() => GetItem().GetKindId() + GetItem().GetNameId();

        public byte ItemDelta => (byte)(GetItem().ChildDelta + GetItem().ModelDelta);
        public virtual bool CanDrag => false;
        public virtual bool CanSort => false;
        public virtual bool CanFilter => false;
        public virtual bool CanReorderItems => false;
        public virtual bool CanExpandAll => false;
        public virtual bool CanExpandLeft => false;
        public virtual bool CanExpandRight => false;
        public virtual bool CanFilterUsage => false;

        public virtual string GetModelInfo(Root root) => default;

        public virtual void GetMenuCommands(Root root, List<ItemCommand> list) { list.Clear(); }
        public virtual void GetButtonCommands(Root root, List<ItemCommand> list) { list.Clear(); }

        internal  virtual DropAction ModelDrop(Root root, ItemModel dropModel, bool doDrop ) => DropAction.None;
        public virtual bool ReorderItems(Root root, ItemModel dropModel) => false;

        public virtual Error TryGetError(Root root) => default;

        public virtual string GetModelIdentity() =>  $"{IdKey}";

        /// <summary>Validate model against the model's item, return true if any child list changed</summary>
        internal virtual bool Validate(Root root, Dictionary<Item, ItemModel> prev)
        {
            var viewListChange = false;
            foreach (var child in Items)
            {
                viewListChange |= child.Validate(root, prev);
            }
            return viewListChange;
        }

        public virtual int Count => 0;
        internal virtual void Add(ItemModel child) { }
        internal virtual void Remove(ItemModel child) { }
        internal virtual void RemoveAt(int index) { }
        internal virtual void Clear() { }
        internal virtual void AddPropertyModel(PropertyModel child) { }

        #endregion
    }
}
