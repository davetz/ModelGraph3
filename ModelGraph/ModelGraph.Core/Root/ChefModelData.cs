namespace ModelGraph.Core
{
    public partial class Root
    {

        //#region AddChildModel  ================================================
        //internal bool AddChildModel(List<IModel> prev, IModel m, IdKey idKe, Item item, Item aux1, Item aux2, ModelAction get)
        //{/*
        //    I am construction a new list of itemModels but if posible I want to reuse an existing model from the previous itemModel list.
        //    The existing models are compared with the parameters of the candidate model to see if it matches. A new model will be created if necessary.
        //    In lists of 20,000 itemModels it is important to be strategic. The new list will be very much, if not exactly, like the previous one.
        //    It is not posible to know what changed or why, however I have the previous list and am being feed parameters for candidates one at a time,
        // */
        //    var C = m.ChildModelCount;  // index of next model to be added
        //    var N = prev.Count;         // length of the previous model list
        //    var M = N - 1;   // last index of previous list

        //    if (C > M)
        //        C = (N / 2);  // keep within the constraints
        //    else if (TryCopyPrevious(C))
        //        return false; // lucky dog, got it on the first try.

        //    for (int i = 0, j = 0, k = C; i < N; i++)
        //    {/*
        //        First look at the index then on either side of the index,  
        //        alternating from left to right in increasing increments.
        //     */
        //        k = (i % 2 == 0) ? (k + i) : (k - i); // right (+0, +2, +4,..)  left (-1, -3, -5,..)
        //        j = (k < 0) ? (k + N) : (k > M) ? (k - N) : k; // wrap arround if necessary

        //        if (TryCopyPrevious(j)) return true; // I reused the existing model.
        //    }
        //    m.ChildModels.Add(IModel.Create(m, idKe, item, aux1, aux2, get));
        //    return true; // I had to create a new model

        //    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    bool TryCopyPrevious(int inx)
        //    {
        //        if (IsMatch(prev[inx]))
        //        {
        //            m.ChildModels.Add(prev[inx]);
        //            prev[inx] = null;
        //            return true;
        //        }
        //        return false;

        //        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 

        //        bool IsMatch(IModel cm)
        //        {
        //            if (cm == null) return false;
        //            if (cm.ParentModel != m) return false;
        //            if (cm.IdKey != idKe) return false;
        //            if (cm.Item != item) return false;
        //            if (cm.Aux1 != aux1) return false;
        //            if (cm.Aux2 != aux2) return false;
        //            if (cm.Get != get) return false;
        //            return true;
        //        }
        //    }
        //}
        //#endregion

        //#region AddProperyModel  ==============================================
        //private bool AddProperyModels(List<IModel> prev, IModel model, IEnumerable<ColumnX> cols)
        //{
        //    var anyChange = false;
        //    var item = model.Item;
        //    foreach (var col in cols)
        //    {
        //        anyChange |= NewPropertyModel(prev, model, item, col);
        //    }
        //    return anyChange;
        //}
        //private bool AddProperyModels(List<IModel> prev, IModel model, IEnumerable<Property> propList)
        //{
        //    var anyChange = false;
        //    foreach (var prop in propList)
        //    {
        //        anyChange |= AddProperyModel(prev, model, prop);
        //    }
        //    return anyChange;
        //}
        //private bool AddProperyModel(List<IModel> prev, IModel model, Property prop)
        //{
        //    var item = model.Item;
        //    if (prop.IsColumnX)
        //        return NewPropertyModel(prev, model, item, (prop as ColumnX));
        //    else if (prop.IsComputeX)
        //        return NewPropertyModel(prev, model, item, (prop as ComputeX));
        //    else
        //        return NewPropertyModel(prev, model, item, prop);
        //}
        //private bool NewPropertyModel(List<IModel> prev, IModel model, Item item, ColumnX col)
        //{
        //    if (EnumX_ColumnX.TryGetParent(col, out EnumX enu))
        //        return AddChildModel(prev, model, IdKey.ComboPropertyModel, item, col, enu, ComboColumn_X);
        //    else if (col.Value.ValType == ValType.Bool)
        //        return AddChildModel(prev, model, IdKey.CheckPropertyModel, item, col, null, CheckColumn_X);
        //    else
        //        return AddChildModel(prev, model, IdKey.TextPropertyModel, item, col, null, TextColumn_X);
        //}
        //private bool NewPropertyModel(List<IModel> prev, List<IModel> curr, IModel model, Item item, ComputeX cx)
        //{
        //    if (EnumX_ColumnX.TryGetParent(cx, out EnumX enu))
        //        return AddChildModel(prev, model, IdKey.ComboPropertyModel, item, cx, enu, ComboProperty_X);
        //    else if (cx.Value.ValType == ValType.Bool)
        //        return AddChildModel(prev, model, IdKey.CheckPropertyModel, item, cx, null, CheckProperty_X);
        //    else
        //        return AddChildModel(prev, model, IdKey.TextPropertyModel, item, cx, null, TextCompute_X);
        //}
        //private bool NewPropertyModel(List<IModel> prev, IModel model, Item item, Property prop)
        //{
        //    if (Property_Enum.TryGetValue(prop, out EnumZ enu))
        //        return AddChildModel(prev, model, IdKey.ComboPropertyModel, item, prop, enu, ComboProperty_X);
        //    else if (prop.Value.ValType == ValType.Bool)
        //        return AddChildModel(prev, model, IdKey.CheckPropertyModel, item, prop, null, CheckProperty_X);
        //    else
        //        return AddChildModel(prev, model, IdKey.TextPropertyModel, item, prop, null, TextProperty_X);
        //}
        //#endregion

        //#region RefreshViewFlatList  ==========================================
        //internal void RefreshViewFlatList(RootModelOld root, int scroll = 0, ChangeType change = ChangeType.NoChange)
        //{
        //    var select = root.SelectModel;
        //    var viewList = root.ViewFlatList;
        //    var capacity = root.ViewCapacity;
        //    var offset = viewList.IndexOf(select);

        //    if (capacity > 0)
        //    {
        //        var first = IModel.FirstValidModel(viewList);
        //        var start = (first == null);
        //        var previous = new List<IModel>();
        //        var modelStack = new TreeModelStack();

        //        UpdateSelectModel(select, change);

        //        if (root.IsForcedRefresh)
        //        {
        //            modelStack.PushRoot(root);
        //        }
        //        else
        //        {
        //            if (root.ChildModelCount == 0)
        //            {
        //                root.Validate(previous);
        //                root.ViewModels = root.ChildModels;
        //            }
        //            modelStack.PushChildren(root);
        //        }

        //        var S = (scroll < 0) ? -scroll : scroll;
        //        var N = capacity;
        //        var buffer = new CircularBuffer(N, S);

        //        #region GoTo<End,Home>  =======================================
        //        if ((change == ChangeType.GoToEnd || change == ChangeType.GoToHome) && offset >= 0 && first != null)
        //        {
        //            var pm = select.ParentModel;
        //            var ix = pm.GetChildlIndex(select);
        //            var last = pm.ChildModelCount - 1;

        //            if (change == ChangeType.GoToEnd)
        //            {
        //                if (ix < last)
        //                {
        //                    select = pm.ViewModels[last];
        //                    if (!viewList.Contains(select)) FindFirst();
        //                }
        //            }
        //            else
        //            {
        //                if (ix > 0)
        //                {
        //                    select = pm.ViewModels[0];
        //                    if (!viewList.Contains(select)) FindFirst();
        //                }
        //            }
        //            root.SelectModel = select;

        //            void FindFirst()
        //            {
        //                first = select;
        //                var absoluteFirst = root.ViewModels[0];

        //                for (; offset > 0; offset--)
        //                {
        //                    if (first == absoluteFirst) break;

        //                    var p = first.ParentModel;
        //                    var i = p.GetChildlIndex(first);

        //                    first = (i > 0) ? p.ViewModels[i - 1] : p;
        //                }
        //            }
        //        }
        //        #endregion

        //        #region TraverseModelTree   ===================================

        //        if (scroll < 0) S = 0;

        //        while (modelStack.IsNotEmpty && (N + S) > 0)
        //        {
        //            var m = modelStack.PopNext();
        //            buffer.Add(m);

        //            if (!start && m == first) start = buffer.SetFirst();
        //            if (start) { if (N > 0) N--; else S--; }

        //            ValidateModel(m, previous);
        //            IModel.Release(previous);
        //            modelStack.PushChildren(m);
        //        }
        //        #endregion

        //        #region ScrollViewList  =======================================
        //        viewList.Clear();

        //        if (scroll == -1)
        //        {
        //            if (buffer.GetHead(viewList))
        //                root.SelectModel = viewList[0];
        //        }
        //        else if (scroll == 1)
        //        {
        //            if (buffer.GetTail(viewList))
        //                root.SelectModel = viewList[viewList.Count - 1];
        //        }
        //        else if (scroll == 0)
        //        {
        //            if (buffer.GetHead(viewList))
        //                if (!viewList.Contains(select))
        //                    root.SelectModel = viewList[0];
        //        }
        //        else if (scroll < -1)
        //        {
        //            if (buffer.GetHead(viewList))
        //                if (!viewList.Contains(select))
        //                    root.SelectModel = viewList[viewList.Count - 1];
        //        }
        //        else if (scroll > 1)
        //        {
        //            if (buffer.GetTail(viewList))
        //                if (!viewList.Contains(select))
        //                    root.SelectModel = viewList[0];
        //        }
        //        #endregion
        //    }
        //    else
        //    {
        //        root.ViewFlatList.Clear();
        //        root.SelectModel = null;
        //    }

        //    root.UIRequestRefreshModel();
        //}

        //#region TreeModelStack  ===============================================
        //private class TreeModelStack
        //{/*
        //    Keep track of unvisited nodes in a depth first graph traversal
        // */
        //    private List<(List<IModel> Models, int Index)> _stack;
        //    internal TreeModelStack() { _stack = new List<(List<IModel> Models, int Index)>(); }
        //    internal bool IsNotEmpty => (_stack.Count > 0);
        //    internal void PushRoot(IModel m) { _stack.Add((new List<IModel>(1) { m }, 0)); }
        //    internal void PushChildren(IModel m) { if (m.ViewModelCount > 0) _stack.Add((m.ViewModels, 0)); }
        //    internal IModel PopNext()
        //    {
        //        var end = _stack.Count - 1;
        //        var (Models, Index) = _stack[end];
        //        var model = Models[Index++];

        //        if (Index < Models.Count)
        //            _stack[end] = (Models, Index);
        //        else
        //            _stack.RemoveAt(end);

        //        return model;
        //    }
        //}
        //#endregion

        //#region CircularBuffer  ===============================================
        //class CircularBuffer
        //{/*
        //    Trace the traversal of an itemModel tree.
        //    The end result is a flat list of itemModels (of a predefined length).
        // */
        //    IModel[] _buffer;
        //    int _first;
        //    int _count;
        //    readonly int _scroll;
        //    readonly int _length;

        //    internal CircularBuffer(int length, int scroll)
        //    {
        //        _scroll = scroll;
        //        _length = length;
        //        _buffer = new IModel[length + scroll];
        //    }

        //    internal void Add(IModel m) => _buffer[Index(_count++)] = m;

        //    internal bool SetFirst() { _first = (_count - 1); return true; }

        //    internal bool GetHead(List<IModel> list)
        //    {
        //        var first = (_first - _scroll);
        //        if (first < 0) first = 0;
        //        return CopyBuffer(first, list);
        //    }

        //    internal bool GetTail(List<IModel> list)
        //    {
        //        var first = (_count < _buffer.Length) ? 0 : ((_count - _first) < _length) ? (_count - _length + _scroll) : _first + _scroll;
        //        return CopyBuffer(first, list);
        //    }

        //    #region PrivateMethods  ===========================================
        //    int Index(int inx) => inx % _buffer.Length;
        //    bool CopyBuffer(int first, List<IModel> list)
        //    {
        //        for (int i = 0, j = first; (i < _length && j < _count); i++, j++)
        //        {
        //            list.Add(_buffer[Index(j)]);
        //        }
        //        return (_count > 0);
        //    }
        //    #endregion
        //}
        //#endregion

        //#region UpdateSelectModel  ============================================
        //void UpdateSelectModel(IModel m, ChangeType change)
        //{
        //    if (m != null)
        //    {
        //        m.ChildDelta -= 3;
        //        switch (change)
        //        {
        //            case ChangeType.ToggleLeft:
        //                m.IsExpandedLeft = !m.IsExpandedLeft;
        //                if (!m.IsExpandedLeft) CollapseLeft();
        //                break;

        //            case ChangeType.ExpandLeft:
        //                m.IsExpandedLeft = true;
        //                break;

        //            case ChangeType.CollapseLeft:
        //                CollapseLeft();
        //                break;

        //            case ChangeType.ToggleRight:
        //                m.IsExpandedRight = !m.IsExpandedRight;
        //                break;

        //            case ChangeType.ExpandRight:
        //                m.IsExpandedRight = true;
        //                break;

        //            case ChangeType.CollapseRight:
        //                m.IsExpandedRight = false;
        //                break;

        //            case ChangeType.ToggleFilter:
        //                m.IsFilterVisible = !m.IsFilterVisible;
        //                if (m.IsFilterVisible)
        //                    m.ViewFilter = string.Empty;
        //                else
        //                    m.ViewFilter = null;
        //                break;

        //            case ChangeType.ExpandFilter:
        //                m.IsFilterVisible = true;
        //                m.ViewFilter = string.Empty;
        //                break;

        //            case ChangeType.CollapseFilter:
        //                m.IsFilterVisible = false;
        //                m.ViewFilter = null;
        //                break;
        //        }

        //        void CollapseLeft()
        //        {
        //            m.IsExpandedLeft = false;
        //            m.IsExpandedRight = false;
        //            m.IsFilterVisible = false;
        //            m.ViewFilter = null;
        //        }
        //    }
        //}
        //#endregion

        //#region ValidateModel  ================================================
        //bool ValidateModel(IModel m, List<IModel> previous)
        //{
        //    if (m is null || m.Item is null) return false;
        //    if (m.Item.AutoExpandLeft)
        //    {
        //        m.IsExpandedLeft = true;
        //        m.Item.AutoExpandLeft = false;
        //    }
        //    if (m.Item.AutoExpandRight)
        //    {
        //        m.IsExpandedRight = true;
        //        m.Item.AutoExpandRight = false;
        //    }

        //    if (!m.IsExpanded && !(m.IsFilterVisible && m.HasFilterText)) return WithNoChildren();

        //    var (hasChildModels, hasChildListChanged) = m.Validate(previous);

        //    if (!hasChildModels) return WithNoChildren();

        //    if (!hasChildListChanged && !m.AnyFilterSortChanged) return WithNoChange();

        //    if (!m.IsSorted && !m.IsFiltered) return WithAllChildren();

        //    var filterList = new List<(string, IModel)>(m.ChildModelCount);

        //    if (m.IsFiltered)
        //    {
        //        if (m.ChangedFilter)
        //        {
        //            var filter = m.RegexViewFilter;

        //            foreach (var cm in m.ChildModels)
        //            {
        //                if (filter != null && !filter.IsMatch(cm.FilterSortName)) continue;
        //                if (m.IsUsedFilter && !m.ModelUsed(cm)) continue;
        //                if (m.IsNotUsedFilter && m.ModelUsed(cm)) continue;

        //                filterList.Add((cm.FilterSortName, cm));
        //            }
        //        }
        //    }

        //    if (m.IsSorted)
        //    {
        //        if (filterList.Count == 0 && m.ViewModelCount > 0)
        //        {
        //            foreach (var cm in m.ViewModels)
        //            {
        //                filterList.Add((cm.FilterSortName, cm));
        //            }
        //        }
        //        filterList.Sort(FilterListCompare);
        //        if (m.IsSortDescending) filterList.Reverse();
        //    }

        //    if (filterList.Count > 0)
        //    {
        //        m.ViewModels = new List<IModel>(filterList.Count);
        //        foreach (var e in filterList) { m.ViewModels.Add(e.Item2); }
        //    }
        //    else
        //    {
        //        m.ViewModels = null;
        //    }
        //    return true;

        //    bool WithNoChange()
        //    {
        //        return true;
        //    }
        //    bool WithNoChildren()
        //    {
        //        IModel.Release(m.ChildModels);

        //        m.ChildModels = null;
        //        m.ViewModels = null;
        //        m.ClearSortUsageMode();
        //        return false;
        //    }
        //    bool WithAllChildren()
        //    {
        //        m.ViewModels = new List<IModel>(m.ChildModels);
        //        return true;
        //    }
        //}

        //int FilterListCompare((string, IModel) x, (string, IModel) y) => x.Item1.CompareTo(y.Item1);
        //#endregion
        //#endregion

        //#region GetAppTabName  ================================================
        //internal string GetAppTabName(RootModelOld root)
        //{
        //    switch (root.ControlType)
        //    {
        //        case ControlType.PrimaryTree:
        //        case ControlType.PartialTree:
        //        case ControlType.GraphDisplay:
        //        case ControlType.SymbolEditor:
        //            return Repository.Name;
        //    }
        //    return BlankName;
        //}
        //#endregion

        //#region GetAppTabSummary  =============================================
        //internal string GetAppTabSummary(RootModelOld root)
        //{
        //    switch (root.ControlType)
        //    {

        //        case ControlType.PrimaryTree:
        //            if (Repository == null)
        //                return _localize(GetNameKey(IdKey.NewModel));

        //            var name = Repository.Name;
        //            var index = name.LastIndexOf(".");
        //            if (index < 0) return name;
        //            return name.Substring(0, index);

        //        case ControlType.GraphDisplay:
        //            return null;

        //        case ControlType.SymbolEditor:
        //            return null;

        //        case ControlType.PartialTree:
        //            return null;
        //    }
        //    return null;
        //}
        //#endregion

        //#region GetAppTitleName  ==============================================
        //internal string GetAppTitleName(RootModelOld root)
        //{
        //    switch (root.ControlType)
        //    {

        //        case ControlType.PrimaryTree:
        //            return RepoName();

        //        case ControlType.PartialTree:
        //            return $"{RepoName()} - {GetName(root.IdKey)}";

        //        case ControlType.GraphDisplay:
        //            var g = root.Graph;
        //            var gx = g.GraphX;
        //            if (g.SeedItem == null)
        //                return $"{gx.Name}";
        //            else
        //                return $"{gx.Name} - {GetIdentity(g.SeedItem, IdentityStyle.Double)}";

        //        case ControlType.SymbolEditor:

        //            return $"{GetName(IdKey.EditSymbol)} : {GetIdentity(root.Item, IdentityStyle.Single)}";
        //    }
        //    return BlankName;

        //    string RepoName() => (Repository == null) ? BlankName : Repository.Name;
        //}
        //#endregion

        //#region GetAppTitleSummary  ===========================================
        //internal string GetAppTitleSummary(RootModelOld root)
        //{
        //    switch (root.ControlType)
        //    {

        //        case ControlType.PrimaryTree:
        //            if (Repository == null)
        //                return _localize(GetNameKey(IdKey.NewModel));

        //            var name = Repository.Name;
        //            var index = name.LastIndexOf(".");
        //            if (index < 0) return name;
        //            return name.Substring(0, index);

        //        case ControlType.GraphDisplay:
        //            return null;

        //        case ControlType.SymbolEditor:
        //            return null;

        //        case ControlType.PartialTree:
        //            return null;
        //    }
        //    return null;
        //}
        //#endregion


        //#region 600 ParmDebugList_X  ==========================================
        //internal ModelAction ParmDebugList_X;
        //void Initer_ParmDebugList_X()
        //{
        //    ParmDebugList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            if (m.ChildModelCount == 1) return (true, false);

        //            m.InitChildModels(prev);

        //            AddProperyModel(prev, m, ShowItemIndexProperty);

        //            return (true, true);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 601 S_601_X  ==================================================
        //internal ModelAction S_601_X;
        //void Initer_S_601_X()
        //{
        //    S_601_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 602 S_602_X  ==================================================
        //internal ModelAction S_602_X;
        //void Initer_S_602_X()
        //{
        //    S_602_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 603 S_603_X  ==================================================
        //internal ModelAction S_603_X;
        //void Initer_S_603_X()
        //{
        //    S_603_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 604 S_604_X  ==================================================
        //internal ModelAction S_604_X;
        //void Initer_S_604_X()
        //{
        //    S_604_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 605 S_605_X  ==================================================
        //internal ModelAction S_605_X;
        //void Initer_S_605_X()
        //{
        //    S_605_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 606 S_606_X  ==================================================
        //internal ModelAction S_606_X;
        //void Initer_S_606_X()
        //{
        //    S_606_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 607 S_607_X  ==================================================
        //internal ModelAction S_607_X;
        //void Initer_S_607_X()
        //{
        //    S_607_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 608 S_608_X  ==================================================
        //internal ModelAction S_608_X;
        //void Initer_S_608_X()
        //{
        //    S_608_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 609 S_609_X  ==================================================
        //internal ModelAction S_609_X;
        //void Initer_S_609_X()
        //{
        //    S_609_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 60A S_60A_X  ==================================================
        //internal ModelAction S_60A_X;
        //void Initer_S_60A_X()
        //{
        //    S_60A_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 60B S_60B_X  ==================================================
        //internal ModelAction S_60B_X;
        //void Initer_S_60B_X()
        //{
        //    S_60B_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 60C S_60C_X  ==================================================
        //internal ModelAction S_60C_X;
        //void Initer_S_60C_X()
        //{
        //    S_60C_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 60D S_60D_X  ==================================================
        //internal ModelAction S_60D_X;
        //void Initer_S_60D_X()
        //{
        //    S_60D_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 60E S_60E_X  ==================================================
        //internal ModelAction S_60E_X;
        //void Initer_S_60E_X()
        //{
        //    S_60E_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 60F S_60F_X  ==================================================
        //internal ModelAction S_60F_X;
        //void Initer_S_60F_X()
        //{
        //    S_60F_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion



        //#region 610 S_610_X  ==================================================
        //internal ModelAction S_610_X;
        //void Initer_S_610_X()
        //{
        //    S_610_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion
       
        //#region 611 S_611_X  ==================================================
        //internal ModelAction S_611_X;
        //void Initer_S_611_X()
        //{
        //    S_611_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 613 DataChef_X  ===============================================
        //internal ModelAction DataChef_X;
        //void Initer_DataChef_X()
        //{
        //    DataChef_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            m.CanExpandLeft = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            var root = m.GetRootModel();
        //            switch (root.ControlType)
        //            {
        //                case ControlType.PrimaryTree:
        //                    if (root.Chef.Repository == null || root.Chef.Repository.HasNoStorage)
        //                    {
        //                        bc.Add(new ModelCommand(this, root, IdKey.SaveAsCommand, SaveAsModel));
        //                        bc.Add(new ModelCommand(this, root, IdKey.CloseCommand, CloseModel));
        //                    }
        //                    else
        //                    {
        //                        bc.Add(new ModelCommand(this, root, IdKey.SaveCommand, SaveModel));
        //                        bc.Add(new ModelCommand(this, root, IdKey.CloseCommand, CloseModel));
        //                        bc.Add(new ModelCommand(this, root, IdKey.ReloadCommand, ReloadModel));
        //                    }
        //                    break;

        //                case ControlType.PartialTree:
        //                    break;

        //                case ControlType.GraphDisplay:
        //                    break;

        //                case ControlType.SymbolEditor:
        //                    bc.Add(new ModelCommand(this, root, IdKey.SaveCommand, AppSaveSymbol));
        //                    bc.Add(new ModelCommand(this, root, IdKey.ReloadCommand, AppReloadSymbol));
        //                    break;
        //            }
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            if (m.ChildModelCount == 5) return (true, false);

        //            m.InitChildModels(prev);

        //            AddChildModel(prev, m, IdKey.ParmRootModel, ErrorStore, null, null, ParmRoot_X);
        //            AddChildModel(prev, m, IdKey.ErrorRootModel, ErrorStore, null, null, ErrorRoot_X);
        //            AddChildModel(prev, m, IdKey.ChangeRootModel, ChangeRoot, null, null, ChangeRoot_X);
        //            AddChildModel(prev, m, IdKey.MetadataRootModel, m.Item, null, null, MetadataRoot_X);
        //            AddChildModel(prev, m, IdKey.ModelingRootModel, m.Item, null, null, ModelingRoot_X);

        //            return (true, true);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    #region ButtonCommands  ===========================================
        //    void SaveAsModel(IModel model)
        //    {
        //        var root = model as RootModelOld;
        //        root.IsChanged = true;
        //        root.UIRequestSaveAsModel();
        //    }
        //    void SaveModel(IModel model)
        //    {
        //        var root = model as RootModelOld;
        //        var dataChef = root.Chef;
        //        root.UIRequestSaveModel();
        //    }

        //    void CloseModel(IModel m) => m.GetRootModel().UIRequestCloseModel();
        //    void AppSaveSymbol(IModel m) => m.GetRootModel().UIRequestSaveModel();
        //    void AppReloadSymbol(IModel m) => m.GetRootModel().UIRequestReloadModel();

        //    void ReloadModel(IModel m)
        //    {
        //        var repo = Repository;
        //        var root = m.GetRootModel();
        //        root.UIRequestReloadModel();
        //    }
        //    #endregion
        //}
        //#endregion

        //#region 614 TextColumn_X  =============================================
        //ModelAction TextColumn_X;
        //void Initer_TextColumn_X()
        //{
        //    TextColumn_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetColumnXKindName(m);

        //            return (kind, name, 0, ModelType.TextProperty);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetColumnXKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.ColumnX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDescription = (m) => m.ColumnX.Description,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        TextValue = (m) => m.ColumnX.Value.GetString(m.Item),
        //    };
        //}

        ////= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //(string, string) GetColumnXKindName(IModel m) => (null, m.ColumnX.Name);
        //#endregion

        //#region 615 CheckColumn_X  ============================================
        //ModelAction CheckColumn_X;
        //void Initer_CheckColumn_X()
        //{
        //    CheckColumn_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetColumnXKindName(m);

        //            return (kind, name, 0, ModelType.CheckProperty);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetColumnXKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.ColumnX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDescription = (m) => m.ColumnX.Description,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        BoolValue = (m) => m.ColumnX.Value.GetBool(m.Item),
        //    };
        //}
        //#endregion

        //#region 616 ComboColumn_X  ============================================
        //ModelAction ComboColumn_X;
        //void Initer_ComboColumn_X()
        //{
        //    ComboColumn_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetColumnXKindName(m);

        //            return (kind, name, 0, ModelType.ComboProperty);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetColumnXKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.ColumnX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDescription = (m) => m.ColumnX.Description,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ListValue = (m) => GetEnumDisplayValues(m.EnumX),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        IndexValue = (m) => GetComboSelectedIndex(m.Item, m.ColumnX, m.EnumX),
        //    };
        //}
        ////=====================================================================
        //string[] GetEnumDisplayValues(EnumX e)
        //{
        //    string[] values = null;
        //    if (e != null && e.IsValid)
        //    {
        //        var items = e.Items;
        //        var count = e.Count;
        //        values = new string[count];

        //        for (int i = 0; i < count; i++)
        //        {
        //            var p = items[i] as PairX;
        //            values[i] = p.DisplayValue;
        //        }
        //    }
        //    return values;
        //}
        ////=====================================================================
        //string[] GetEnumActualValues(EnumX e)
        //{
        //    string[] values = null;
        //    if (e != null && e.IsValid)
        //    {
        //        var items = e.Items;
        //        var count = e.Count;
        //        values = new string[count];

        //        for (int i = 0; i < count; i++)
        //        {
        //            var p = items[i] as PairX;
        //            values[i] = p.ActualValue;
        //        }
        //    }
        //    return values;
        //}
        ////=====================================================================
        //int GetComboSelectedIndex(Item itm, Property col, EnumX enu)
        //{
        //    var value = col.Value.GetString(itm);
        //    var values = GetEnumActualValues(enu);
        //    var len = (values == null) ? 0 : values.Length;
        //    for (int i = 0; i < len; i++) { if (value == values[i]) return i; }
        //    return -1;
        //}
        //#endregion

        //#region 617 TextProperty_X  ===========================================
        //ModelAction TextProperty_X;
        //void Initer_TextProperty_X()
        //{
        //    TextProperty_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetPropertyKindName(m);

        //            return (kind, name, 0, ModelType.TextProperty);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetPropertyKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.Property.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDescription = (m) => _localize(m.Property.DescriptionKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        TextValue = (m) => m.Property.Value.GetString(m.Item),
        //    };

        //}

        ////= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //(string, string) GetPropertyKindName(IModel m)
        //{
        //    var name = _localize(m.Property.NameKey);
        //    return (null, m.Property.HasItemName ? $"{m.Property.GetItemName(m.Item)} {name}" : name);
        //}
        //#endregion

        //#region 618 CheckProperty_X  ==========================================
        //ModelAction CheckProperty_X;
        //void Initer_CheckProperty_X()
        //{
        //    CheckProperty_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetPropertyKindName(m);

        //            return (kind, name, 0, ModelType.CheckProperty);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetPropertyKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.Property.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDescription = (m) => _localize(m.Property.DescriptionKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        BoolValue = (m) => m.Property.Value.GetBool(m.Item),
        //    };
        //}
        //#endregion

        //#region 619 ComboProperty_X  ==========================================
        //ModelAction ComboProperty_X;
        //void Initer_ComboProperty_X()
        //{
        //    ComboProperty_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetPropertyKindName(m);

        //            return (kind, name, 0, ModelType.ComboProperty);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetPropertyKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.Property.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDescription = (m) => _localize(m.Property.DescriptionKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ListValue = (m) => GetEnumZNames(m.EnumZ),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        IndexValue = (m) => GetEnumZIndex(m.EnumZ, m.Property.Value.GetString(m.Item)),
        //    };
        //}
        //#endregion

        //#region 61A TextCompute_X  ============================================
        //ModelAction TextCompute_X;
        //void Initer_TextCompute_X()
        //{
        //    TextCompute_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.TextProperty);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.ComputeX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDescription = (m) => m.ComputeX.Description,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        TextValue = (m) => m.ComputeX.Value.GetString(m.Item),
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, m.ComputeX.Name);
        //}
        //#endregion

        //#region 61B S_61B_X  ==================================================
        //internal ModelAction S_61B_X;
        //void Initer_S_61B_X()
        //{
        //    S_61B_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 61C S_61C_X  ==================================================
        //internal ModelAction S_61C_X;
        //void Initer_S_61C_X()
        //{
        //    S_61C_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 61D S_61D_X  ==================================================
        //internal ModelAction S_61D_X;
        //void Initer_S_61D_X()
        //{
        //    S_61D_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 61E S_61E_X  ==================================================
        //internal ModelAction S_61E_X;
        //void Initer_S_61E_X()
        //{
        //    S_61E_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 61F S_61F_X  ==================================================
        //internal ModelAction S_61F_X;
        //void Initer_S_61F_X()
        //{
        //    S_61F_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion


        //#region RemoveItemMenuCommand  ========================================
        //void RemoveItemMenuCommand(IModel m, List<ModelCommand> mc)
        //{
        //    mc.Add(new ModelCommand(this, m, IdKey.RemoveCommand, RemoveItem));
        //}
        //void RemoveItem(IModel m) => RemoveItem(m.Item);
        //#endregion


        //#region 620 ParmRoot  =================================================
        //ModelAction ParmRoot_X;
        //void Initer_ParmRoot_X()
        //{
        //    ParmRoot_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            if (m.ChildModelCount == 1) return (true, false);

        //            m.InitChildModels(prev);

        //            AddChildModel(prev, m, IdKey.ParmDebugListModel, this, null, null, ParmDebugList_X);

        //            return (true, true);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 621 ErrorRoot  ================================================
        //ModelAction ErrorRoot_X;
        //void Initer_ErrorRoot_X()
        //{
        //    ErrorRoot_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var count = ErrorStore.Count;
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.ViewCommand, CreateSecondaryMetadataTree));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (ErrorStore.Count == 0) return (false, false);
        //            if (ErrorStore.ChildDelta == m.ChildDelta) return (true, false);

        //            m.InitChildModels(prev, ErrorStore.Count);

        //            var list = ErrorStore.Items;
        //            var anyChange = prev.Count != list.Count;
        //            foreach (var itm in list)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.ErrorTypeModel, itm, null, null, ErrorType_X);
        //            }

        //            return (true, anyChange);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void CreateSecondaryMetadataTree(IModel m) => m.GetRootModel().UIRequestCreatePage(ControlType.PartialTree, IdKey.ErrorRootModel, m.Item, ErrorRoot_X);
        //}
        //#endregion

        //#region 622 ChangeRoot  ===============================================
        //ModelAction ChangeRoot_X;
        //void Initer_ChangeRoot_X()
        //{
        //    ChangeRoot_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var count = ChangeRoot.Count;
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelInfo = (m) => m.IsExpandedLeft ? null : _changeRootInfoText,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, list) =>
        //        {
        //            if (ChangeRoot.Count > 0 && m.IsExpandedLeft == false)
        //                list.Add(new ModelCommand(this, m, IdKey.ExpandAllCommand, ExpandAllChangeSets));
        //            list.Add(new ModelCommand(this, m, IdKey.ViewCommand, CreateSecondaryMetadataTree));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (ChangeRoot.Count == 0) return (false, false);
        //            if (ChangeRoot.ChildDelta == m.ChildDelta) return (true, false);

        //            var anyChange = false;
        //            var items = ChangeRoot.Items;
        //            m.InitChildModels(prev, items.Count);
        //            for (int i = (items.Count - 1); i >= 0; i--)
        //            {
        //                var itm = items[i];
        //                anyChange |= AddChildModel(prev, m, IdKey.ChangeSetModel, itm, null, null, ChangeSet_X);
        //            }

        //            return (true, anyChange);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void CreateSecondaryMetadataTree(IModel m) => m.GetRootModel().UIRequestCreatePage(ControlType.PartialTree, IdKey.ChangeRootModel, m.Item, ChangeRoot_X);
        //}
        //#endregion

        //#region 623 MetadataRoot  =============================================
        //ModelAction MetadataRoot_X;
        //void Initer_MetadataRoot_X()
        //{
        //    MetadataRoot_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.ViewCommand, CreateSecondaryMetadataTree));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (m.ChildModelCount == 5) return (true, false);

        //            m.InitChildModels(prev);

        //            AddChildModel(prev, m, IdKey.MetaViewViewListModel, ViewXDomain, null, null, MetaViewXViewList_X);
        //            AddChildModel(prev, m, IdKey.MetaEnumListModel, EnumZStore, null, null, MetaEnumList_X);
        //            AddChildModel(prev, m, IdKey.MetaTableListModel, TableXDomain, null, null, MetaTableList_X);
        //            AddChildModel(prev, m, IdKey.MetaGraphListModel, GraphXDomain, null, null, MetaGraphList_X);
        //            AddChildModel(prev, m, IdKey.InternalStoreListModel, this, null, null, InternalStoreList_X);

        //            return (true, true);
        //        },                
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void CreateSecondaryMetadataTree(IModel m) => m.GetRootModel().UIRequestCreatePage(ControlType.PartialTree, IdKey.MetadataSubRootModel, m.Item, MetadataSubRoot_X);
        //}
        //#endregion

        //#region 624 ModelingRoot  =============================================
        //ModelAction ModelingRoot_X;
        //void Initer_ModelingRoot_X()
        //{
        //    ModelingRoot_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.ViewCommand, CreateSecondaryModelingTree));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (m.ChildModelCount == 4) return (true, false);

        //            m.InitChildModels(prev);

        //            AddChildModel(prev, m, IdKey.ViewViewListModel, m.Item, null, null, MetaViewXViewList_X);
        //            AddChildModel(prev, m, IdKey.TableListModel, m.Item, null, null, TableList_X);
        //            AddChildModel(prev, m, IdKey.GraphListModel, m.Item, null, null, GraphList_X);
        //            AddChildModel(prev, m, IdKey.PrimeComputeModel, m.Item, null, null, PrimeCompute_X);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void CreateSecondaryModelingTree(IModel m) => m.GetRootModel().UIRequestCreatePage(ControlType.PartialTree, IdKey.ModelingSubRootModel, m.Item, ModelingSubRoot_X);
        //}
        //#endregion

        //#region 625 MetaRelationList_X  =======================================
        //ModelAction MetaRelationList_X;
        //void Initer_MetaRelationList_X()
        //{
        //    MetaRelationList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (m.ChildModelCount == 2) return (true, false);

        //            m.InitChildModels(prev);

        //            AddChildModel(prev, m, IdKey.MetaNameColumnRelationModel, m.Item, Store_NameProperty, null, MetaNameColumnRelation_X);
        //            AddChildModel(prev, m, IdKey.MetaSummaryColumnRelationModel, m.Item, Store_SummaryProperty, null, MetaSummaryColumnRelation_X);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 626 ErrorType  ================================================
        //ModelAction ErrorType_X;
        //void Initer_ErrorType_X()
        //{
        //    ErrorType_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var count = m.Error.Count;
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.Item.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var er = m.Error;
        //            if (er.Count == 0) return (false, false);
        //            if (er.Count == m.ChildModelCount) return (true, false);

        //            m.InitChildModels(prev, er.Count);
        //            for (int i = 0; i < er.Count; i++)
        //            {
        //                AddChildModel(prev, m, IdKey.ErrorTextModel, er, null, null, ErrorText_X);
        //            }

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.Item.NameKey));
        //}
        //#endregion

        //#region 627 ErrorText  ================================================
        //ModelAction ErrorText_X;
        //void Initer_ErrorText_X()
        //{
        //    ErrorText_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, GetName(m));

        //    string GetName(IModel m)
        //    {
        //        var e = m.Error;
        //        var i = m.ParentModel.GetChildlIndex(m);
        //        return (i < 0 || e.Count <= i) ? InvalidItem : e.GetError(i);
        //    }
        //}
        //#endregion

        //#region 628 ChangeSet  ================================================
        //ModelAction ChangeSet_X;
        //void Initer_ChangeSet_X()
        //{
        //    ChangeSet_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var count = m.ChangeSet.Count;
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            var cs = m.ChangeSet;
        //            if (cs.CanMerge)
        //                bc.Add(new ModelCommand(this, m, IdKey.MergeCommand, ModelMerge));
        //            if (cs.CanUndo)
        //                bc.Add(new ModelCommand(this, m, IdKey.UndoCommand, ModelUndo));
        //            if (cs.CanRedo)
        //                bc.Add(new ModelCommand(this, m, IdKey.RedoCommand, ModelRedo));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var cs = m.ChangeSet;
        //            if (cs.Count == 0) return (false, false);
        //            if (cs.ChildDelta == m.ChildDelta) return (true, false);

        //            var anyChange = false;
        //            var items = cs.Items;
        //            m.InitChildModels(prev, cs.Count);
        //            for (int i = (cs.Count - 1); i >= 0; i--) // show in most recent change order
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.ItemChangeModel, items[i], null, null, ItemChanged_X);
        //            }

        //            return (true, anyChange);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, GetName(m));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    string GetName(IModel m)
        //    {
        //        var cs = m.ChangeSet;
        //        return cs.IsCongealed ? _localize(cs.NameKey) : cs.Name;
        //    }

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void ModelMerge(IModel model)
        //    {
        //        var chg = model.Item as ChangeSet;
        //        chg.Merge();
        //    }

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void ModelUndo(IModel model)
        //    {
        //        var chg = model.Item as ChangeSet;
        //        Undo(chg);
        //    }

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void ModelRedo(IModel model)
        //    {
        //        var chg = model.Item as ChangeSet;
        //        Redo(chg);
        //    }
        //}
        //#endregion

        //#region 629 ItemChanged  ==============================================
        //ModelAction ItemChanged_X;
        //void Initer_ItemChange_X()
        //{
        //    ItemChanged_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.Item.KindKey), m.ItemChange.Name);
        //}
        //#endregion

        //#region 62A S_62A_X  ==================================================
        //internal ModelAction S_62A_X;
        //void Initer_S_62A_X()
        //{
        //    S_62A_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 62B S_62B_X  ==================================================
        //internal ModelAction S_62B_X;
        //void Initer_S_62B_X()
        //{
        //    S_62B_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 62C S_62C_X  ==================================================
        //internal ModelAction S_62C_X;
        //void Initer_S_62C_X()
        //{
        //    S_62C_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 62D S_62D_X  ==================================================
        //internal ModelAction S_62D_X;
        //void Initer_S_62D_X()
        //{
        //    S_62D_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            return (false, false);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 62E MetadataSubRoot_X  ========================================
        //internal ModelAction MetadataSubRoot_X;
        //void Initer_MetadataSubRoot_X()
        //{
        //    MetadataSubRoot_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = true;
        //            m.IsExpandedLeft = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            if (m.ChildModelCount == 5) return (true, false);

        //            m.InitChildModels(prev);

        //            AddChildModel(prev, m, IdKey.MetaViewViewListModel, ViewXDomain, null, null, MetaViewXViewList_X);
        //            AddChildModel(prev, m, IdKey.MetaEnumListModel, EnumZStore, null, null, MetaEnumList_X);
        //            AddChildModel(prev, m, IdKey.MetaTableListModel, TableXDomain, null, null, MetaTableList_X);
        //            AddChildModel(prev, m, IdKey.MetaGraphListModel, GraphXDomain, null, null, MetaGraphList_X);
        //            AddChildModel(prev, m, IdKey.InternalStoreListModel, this, null, null, InternalStoreList_X);

        //            return (true, true);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 62F ModelingSubRoot_X  ========================================
        //internal ModelAction ModelingSubRoot_X;
        //void Initer_ModelingSubRoot_X()
        //{
        //    ModelingSubRoot_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = true;
        //            m.IsExpandedLeft = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            if (m.ChildModelCount == 4) return (true, false);

        //            m.InitChildModels(prev);

        //            AddChildModel(prev, m, IdKey.ViewViewListModel, m.Item, null, null, MetaViewXViewList_X);
        //            AddChildModel(prev, m, IdKey.TableListModel, m.Item, null, null, TableList_X);
        //            AddChildModel(prev, m, IdKey.GraphListModel, m.Item, null, null, GraphList_X);
        //            AddChildModel(prev, m, IdKey.PrimeComputeModel, m.Item, null, null, PrimeCompute_X);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion



        //#region 631 MetaViewXViewList_X  ======================================
        //ModelAction MetaViewXViewList_X;
        //void Initer_MetaViewXViewList_X()
        //{
        //    MetaViewXViewList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var views = ViewXDomain.Items;
        //            var (kind, name) = GetKindName(m);
        //            var count = 0;
        //            foreach (var vx in views) { if (ViewX_ViewX.HasNoParent(vx)) count++; }

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.InsertCommand, Insert));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            var vxDrop = d.ViewX;
        //            if (vxDrop != null && vxDrop.Owner == ViewXDomain)
        //            {
        //                if (doDrop)
        //                {
        //                    if (ViewX_ViewX.TryGetParent(vxDrop, out ViewX vxDropParent))
        //                        RemoveLink(ViewX_ViewX, vxDropParent, vxDrop);

        //                    var prevIndex = ViewXDomain.IndexOf(vxDrop);
        //                    ItemMoved(vxDrop, prevIndex, 0);
        //                }
        //                return DropAction.Move;
        //            }
        //            return DropAction.None;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (ViewXDomain.Count == 0) return (false, false);

        //            if (ViewXDomain.ChildDelta == m.ChildDelta) return (true, false);
        //            m.ChildDelta = ViewXDomain.ChildDelta;

        //            var items = ViewXDomain.Items;
        //            m.InitChildModels(prev, items.Count);

        //            var anyChange = prev.Count != items.Count;
        //            foreach (var itm in items)
        //            {
        //                if ((ViewX_ViewX.HasNoParent(itm)))
        //                {
        //                    anyChange |= AddChildModel(prev, m, IdKey.MetaViewViewModel, itm, null, null, MetaViewView_X);
        //                }
        //            }

        //            return (true, anyChange);
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void Insert(IModel model)
        //    {
        //        ItemCreated(new ViewX(ViewXDomain, true));
        //    }
        //}
        //#endregion

        //#region 632 MetaViewView_X  ===========================================
        //ModelAction MetaViewView_X;
        //void Initer_MetaViewView_X()
        //{
        //    MetaViewView_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var vx = m.ViewX;
        //            var (kind, name) = GetKindName(m);
        //            var count = (ViewX_ViewX.ChildCount(vx) + ViewX_QueryX.ChildCount(vx) + ViewX_Property.ChildCount(vx));

        //            m.CanDrag = true;
        //            m.CanSort = count > 1;
        //            m.CanFilter = count > 2;
        //            m.CanExpandLeft = count > 0;
        //            m.CanExpandRight = true;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.ViewX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.InsertCommand, Insert));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderRelatedChild,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            var view = m.ViewX;
        //            if (view != null)
        //            {
        //                if (d.Item is ViewX vx)
        //                {
        //                    if (vx.Owner == ViewXDomain)
        //                    {
        //                        if (ViewX_QueryX.HasNoChildren(view) && ViewX_Property.HasNoChildren(view))
        //                        {
        //                            if (doDrop)
        //                            {
        //                                if (ViewX_ViewX.TryGetParent(vx, out ViewX oldParent))
        //                                    RemoveLink(ViewX_ViewX, oldParent, vx);
        //                                AppendLink(ViewX_ViewX, view, vx);
        //                            }
        //                            return DropAction.Move;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (d.Item is Store st)
        //                    {
        //                        if (ViewX_ViewX.HasNoChildren(view) && ViewX_QueryX.HasNoChildren(view) && ViewX_Property.HasNoChildren(view))
        //                        {
        //                            if (doDrop)
        //                            {
        //                                CreateQueryX(view, st);
        //                            }
        //                            return DropAction.Link;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (d.Item is Relation re)
        //                        {
        //                            if (ViewX_ViewX.HasNoChildren(view) && ViewX_Property.HasNoChildren(view))
        //                            {
        //                                if (doDrop)
        //                                {
        //                                    CreateQueryX(view, re);
        //                                }
        //                                return DropAction.Link;
        //                            }
        //                        }
        //                        else if (d.Item is Property pr)
        //                        {
        //                            if (ViewX_ViewX.HasNoChildren(view) && ViewX_QueryX.HasNoChildren(view))
        //                            {
        //                                if (doDrop)
        //                                {
        //                                    AppendLink(ViewX_Property, view, pr);
        //                                }
        //                                return DropAction.Link;
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //            return DropAction.None;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var vx = m.ViewX;
        //            var anyChange = false;
        //            m.InitChildModels(prev);

        //            if (m.IsExpandedRight)
        //            {
        //                anyChange |= AddProperyModel(prev, m, ViewXDomain.NameProperty);
        //                anyChange |= AddProperyModel(prev, m, ViewXDomain.SummaryProperty);
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                if (ViewX_Property.TryGetChildren(vx, out IList<Property> pls))
        //                {
        //                    foreach (var pc in pls)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.MetaViewPropertyModel, pc, ViewX_Property, vx, MetaViewProperty_X);
        //                    }
        //                }

        //                if (ViewX_QueryX.TryGetChildren(vx, out IList<QueryX> qls))
        //                {
        //                    foreach (var qc in qls)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.MetaViewQueryModel, qc, ViewX_QueryX, vx, MetaViewQuery_X);
        //                    }
        //                }

        //                if (ViewX_ViewX.TryGetChildren(vx, out IList<ViewX> vls))
        //                {
        //                    foreach (var vc in vls)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.MetaViewViewModel, vc, ViewX_ViewX, vx, MetaViewView_X);
        //                    }
        //                }
        //            }

        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, m.ViewX.Name);

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void Insert(IModel model)
        //    {
        //        var vx = new ViewX(ViewXDomain, true);
        //        ItemCreated(vx);
        //        AppendLink(ViewX_ViewX, model.Item, vx);
        //    }
        //}
        //internal void ViewXView_M(IModel m, RootModelOld root)
        //{
        //    var view = m.Item as ViewX;

        //}
        //#endregion

        //#region 633 MetaViewQuery_X  ==========================================
        //ModelAction MetaViewQuery_X;
        //void Initer_MetaViewQuery_X()
        //{
        //    MetaViewQuery_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var qx = m.Item as QueryX;
        //            var (kind, name) = GetKindName(m);
        //            var count = (QueryX_ViewX.ChildCount(qx) + QueryX_QueryX.ChildCount(qx) + QueryX_Property.ChildCount(qx));

        //            m.CanSort = (m.IsExpandedLeft && count > 1);
        //            m.CanFilter = count > 2;
        //            m.CanExpandLeft = count > 0;

        //            if (Relation_QueryX.TryGetParent(qx, out Relation _))
        //            {
        //                return (kind, name, count, ModelType.Default);
        //            }
        //            else
        //            {
        //                m.CanDrag = true;
        //                m.CanExpandRight = true;

        //                return (kind, name, count, ModelType.Default);
        //            }
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDescription = (m) => _localize(m.DescriptionKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.InsertCommand, Insert));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            var qx = m.Item as QueryX;
        //            if (d.Item is Relation rel)
        //            {
        //                if (doDrop)
        //                {
        //                    CreateQueryX(qx, rel, QueryType.View).AutoExpandRight = false;
        //                }
        //                return DropAction.Link;
        //            }
        //            else if (d.Item is Property pro)
        //            {
        //                if (doDrop)
        //                {
        //                    AppendLink(QueryX_Property, qx, pro);
        //                }
        //                return DropAction.Link;
        //            }
        //            return DropAction.None;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var qx = m.QueryX;
        //            var anyChange = false;
        //            m.InitChildModels(prev);

        //            if (m.IsExpandedRight)
        //            {
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RelationProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.IsReversedProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RootWhereProperty);
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                if (QueryX_Property.TryGetChildren(qx, out IList<Property> pls))
        //                {
        //                    foreach (var pc in pls)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.MetaViewPropertyModel, pc, QueryX_Property, qx, MetaViewProperty_X);
        //                    }
        //                }

        //                if (QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> qls))
        //                {
        //                    foreach (var qc in qls)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.MetaViewQueryModel, qc, QueryX_QueryX, qx, MetaViewQuery_X);
        //                    }
        //                }

        //                if (QueryX_ViewX.TryGetChildren(qx, out IList<ViewX> vls))
        //                {
        //                    foreach (var vc in vls)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.MetaViewViewModel, vc, QueryX_ViewX, qx, MetaViewView_X);
        //                    }
        //                }
        //            }

        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m)
        //    {
        //        var qx = m.Item as QueryX;
        //        if (Relation_QueryX.TryGetParent(qx, out Relation re))
        //        {
        //            return (_localize(m.KindKey), GetIdentity(qx, IdentityStyle.Single));
        //        }
        //        else if (Store_QueryX.TryGetParent(qx, out Store sto))
        //        {
        //            return (GetIdentity(sto, IdentityStyle.Kind), GetIdentity(sto, IdentityStyle.Double));
        //        }
        //        return (Chef.BlankName, Chef.BlankName);
        //    }

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void Insert(IModel model)
        //    {
        //        var vx = new ViewX(ViewXDomain, true);
        //        ItemCreated(vx);
        //        AppendLink(QueryX_ViewX, model.Item, vx);
        //    }
        //}
        //#endregion

        //#region 634 MetaViewCommand_X  ========================================
        //ModelAction MetaViewCommand_X;
        //void Initer_MetaViewCommand_X()
        //{
        //    MetaViewCommand_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            return (false, false);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 635 MetaViewProperty_X  =======================================
        //ModelAction MetaViewProperty_X;
        //void Initer_MetaViewProperty_X()
        //{
        //    MetaViewProperty_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (GetIdentity(m.Item, IdentityStyle.Kind), GetIdentity(m.Item, IdentityStyle.Double));
        //}
        //#endregion

        //#region 63A ViewView_ZM  ==============================================
        //ModelAction ViewView_X;
        //void Initer_ViewView_X()
        //{
        //    ViewView_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var views = ViewXDomain.Items;
        //            var (kind, name) = GetKindName(m);
        //            var count = 0;
        //            foreach (var vx in views) { if (ViewX_ViewX.HasNoParent(vx)) count++; }

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (ViewXDomain.Count == 0) return (false, false);

        //            if (ViewXDomain.ChildDelta == m.ChildDelta) return (true, false);
        //            m.ChildDelta = ViewXDomain.ChildDelta;

        //            m.InitChildModels(prev);
        //            var items = ViewXDomain.Items;

        //            var anyChange = prev.Count != items.Count;
        //            foreach (var itm in items)
        //            {
        //                if (ViewX_ViewX.HasNoParent(itm)) anyChange |= AddChildModel(prev, m, IdKey.ViewViewModel, itm, null, null, ViewView_X);
        //            }

        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 63B ViewView_M  ===============================================
        //ModelAction ViewViewM_X;
        //void Initer_ViewViewM_X()
        //{
        //    ViewViewM_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var vx = m.ViewX;
        //            var key = m.Aux1; // may be null
        //            var (kind, name) = GetKindName(m);
        //            var count = 0;
        //            if (ViewX_QueryX.TryGetChildren(vx, out IList<QueryX> querys))
        //            {
        //                if (querys.Count == 1 && Store_QueryX.HasParentLink(querys[0]))
        //                {
        //                    if (TryGetQueryItems(querys[0], out List<Item> keys)) count = keys.Count;
        //                }
        //                else if (key != null)
        //                    count = querys.Count;
        //            }
        //            else
        //            {
        //                count = ViewX_ViewX.ChildCount(vx);
        //            }

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.ViewX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            m.InitChildModels(prev);

        //            var vx = m.ViewX;
        //            var key = m.Aux1; // may be null
        //            var anyChange = false;

        //            var L2 = (ViewX_QueryX.TryGetChildren(vx, out IList<QueryX> queryList)) ? queryList.Count : 0;
        //            var L3 = (ViewX_ViewX.TryGetChildren(vx, out IList<ViewX> viewList)) ? viewList.Count : 0;
        //            if ((L2 + L3) == 0) return (false, false);


        //            if (L2 == 1 && Store_QueryX.HasParentLink(queryList[0]) && TryGetQueryItems(queryList[0], out List<Item> items))
        //            {
        //                foreach (var itm in items)
        //                {
        //                    anyChange |= AddChildModel(prev, m, IdKey.ViewItemModel, itm, queryList[0], null, ViewItem_X);
        //                }
        //            }
        //            else if (key != null && L2 > 0)
        //            {
        //                foreach (var qx in queryList)
        //                {
        //                    anyChange |= AddChildModel(prev, m, IdKey.ViewQueryModel, qx, key, null, ViewQuery_X);
        //                }
        //            }
        //            else if (L3 > 0)
        //            {
        //                foreach (var v in viewList)
        //                {
        //                    anyChange |= AddChildModel(prev, m, IdKey.ViewViewModel, v, null, null, ViewView_X);
        //                }
        //            }

        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, m.ViewX.Name);
        //}
        //#endregion

        //#region 63C ViewItem_M  ===============================================
        //ModelAction ViewItem_X;
        //void Initer_ViewItem_X()
        //{
        //    ViewItem_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var item = m.Item;
        //            var qx = m.Aux1 as QueryX;

        //            var (L1, PropertyList, L2, QueryList, L3, ViewList) = GetQueryXChildren(qx);
        //            var (kind, name) = GetKindName(m);
        //            var count = (L2 + L3);

        //            m.CanExpandLeft = (count > 0);
        //            m.CanExpandRight = L1 > 0;
        //            m.CanFilterUsage = (m.IsExpandedLeft && count > 1);

        //            return (GetIdentity(item.Owner, IdentityStyle.Single), GetIdentity(item, IdentityStyle.Single), count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var item = m.Item;
        //            var qx = m.Aux1 as QueryX;
        //            var (L1, PropertyList, L2, QueryList, L3, ViewList) = GetQueryXChildren(qx);

        //            int R = (m.IsExpandedRight) ? L1 : 0;
        //            int L = (m.IsExpandedLeft) ? (L2 + L3) : 0;

        //            var N = R + L;
        //            if (N == 0) return (false, false);

        //            var anyChange = false;
        //            m.InitChildModels(prev);

        //            if (R > 0)
        //            {
        //                anyChange |= AddProperyModels(prev, m, PropertyList);
        //            }

        //            if (R > 0)
        //            {
        //                anyChange |= AddProperyModels(prev, m, PropertyList);
        //            }

        //            if (L > 0)
        //            {
        //                if (L2 > 0)
        //                {
        //                    foreach (var q in QueryList)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.ViewQueryModel, item, q, null, ViewQuery_X);
        //                    }
        //                }
        //                if (L3 > 0)
        //                {
        //                }
        //            }

        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (GetIdentity(m.Item.Owner, IdentityStyle.Single), GetIdentity(m.Item, IdentityStyle.Single));
        //}
        //#endregion

        //#region 63D ViewQuery_M  ==============================================
        //ModelAction ViewQuery_X;
        //void Initer_ViewQuery_X()
        //{
        //    ViewQuery_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var key = m.Item;
        //            var qx = m.QueryX;
        //            var (kind, name) = GetKindName(m);
        //            var count = TryGetQueryItems(qx, out List<Item> items, key) ? items.Count : 0;

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var key = m.Item;
        //            var qx = m.QueryX;

        //            if (!TryGetQueryItems(qx, out List<Item> items, key))  return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != items.Count;
        //            foreach (var itm in items)
        //            {
        //                anyChange = AddChildModel(prev, m, IdKey.ViewItemModel, itm, qx, null, ViewItem_X);
        //            }

        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), GetIdentity(m.QueryX, IdentityStyle.Single));
        //}
        //#endregion




        //#region 642 MetaEnumList_X  ===========================================
        //ModelAction MetaEnumList_X;
        //void Initer_MetaEnumList_X()
        //{
        //    MetaEnumList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = EnumXDomain.Count;

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.InsertCommand, Insert));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var store = EnumXDomain;
        //            if (store.Count == 0) return (false, false);
        //            if (store.ChildDelta == m.ChildDelta) return (true, false);

        //            m.ChildDelta = store.ChildDelta;
        //            m.InitChildModels(prev, store.Count);

        //            var items = store.Items;

        //            var anyChange = prev.Count != items.Count;
        //            foreach (var itm in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.MetaEnumModel, itm, null, null, MetaEnum_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void Insert(IModel model)
        //    {
        //        ItemCreated(new EnumX(EnumXDomain, true));
        //    }
        //}
        //#endregion

        //#region 643 MetaTableList_X  ==========================================
        //ModelAction MetaTableList_X;
        //void Initer_MetaTableList_X()
        //{
        //    MetaTableList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var count = TableXDomain.Count;
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.InsertCommand, Insert));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var store = TableXDomain;
        //            if (store.Count == 0) return (false, false);
        //            if (store.ChildDelta == m.ChildDelta) return (true, false);

        //            m.ChildDelta = store.ChildDelta;
        //            m.InitChildModels(prev, store.Count);

        //            var items = store.Items;

        //            var anyChange = prev.Count != items.Count;
        //            foreach (var itm in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.MetaTableModel, itm, null, null, MetaTable_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void Insert(IModel model)
        //    {
        //        ItemCreated(new TableX(TableXDomain, true));
        //    }
        //}
        //#endregion

        //#region 644 MetaGraphList_X  ==========================================
        //ModelAction MetaGraphList_X;
        //void Initer_MetaGraphList_X()
        //{
        //    MetaGraphList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = GraphXDomain.Count;

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.InsertCommand, Insert));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var store = GraphXDomain;
        //            if (store.Count == 0) return (false, false);
        //            if (store.ChildDelta == m.ChildDelta) return (true, false);

        //            m.ChildDelta = store.ChildDelta;
        //            m.InitChildModels(prev, store.Count);

        //            var items = store.Items;

        //            var anyChange = prev.Count != items.Count;
        //            foreach (var itm in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphModel, itm, null, null, MetaGraph_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void Insert(IModel model)
        //    {
        //        ItemCreated(new GraphX(GraphXDomain, true));
        //        model.IsExpandedLeft = true;
        //    }
        //}
        //#endregion

        //#region 645 MetaSymbolList_X  =========================================
        //ModelAction MetaSymbolList_X;
        //void Initer_MetaSymbolList_X()
        //{
        //    MetaSymbolList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var gx = m.Item as GraphX;
        //            var (kind, name) = GetKindName(m);
        //            var count = GraphX_SymbolX.ChildCount(gx);

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.InsertCommand, Insert));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!d.Item.IsSymbolX) return DropAction.None;
        //            var sx = d.Item as SymbolX;
        //            if (doDrop)
        //            {
        //                var gd = m.Item as GraphX;
        //                var sym = new SymbolX(SymbolXDomain, true);
        //                ItemCreated(sym);
        //                AppendLink(GraphX_SymbolX, gd, sym);
        //                m.IsExpandedLeft = true;
        //                sym.Data = sx.Data;
        //                sym.Name = sx.Name;
        //                sym.Summary = sx.Summary;
        //            }
        //            return DropAction.Copy;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var gx = m.GraphX;
        //            if (!GraphX_SymbolX.TryGetChildren(gx, out IList<SymbolX> items)) return (false, false);

        //            m.InitChildModels(prev, items.Count);

        //            var anyChange = prev.Count != items.Count;
        //            foreach (var itm in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.MetaSymbolModel, itm, GraphX_SymbolX, gx, MetaSymbol_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void Insert(IModel model)
        //    {
        //        var gd = model.Item as GraphX;
        //        var sym = new SymbolX(SymbolXDomain, true);
        //        ItemCreated(sym);
        //        AppendLink(GraphX_SymbolX, gd, sym);
        //        model.IsExpandedLeft = true;
        //    }
        //}
        //#endregion

        //#region 646 MetaGraphParmList_X  ======================================
        //ModelAction MetaGraphParmList_X;
        //void Initer_MetaGraphParmList_X()
        //{
        //    MetaGraphParmList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var gx = m.Item as GraphX;
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            var gx = m.GraphX;
        //            if (m.ChildModelCount == 3) return (true, false);

        //            m.InitChildModels(prev);

        //            AddProperyModel(prev, m, GraphXDomain.TerminalLengthProperty);
        //            AddProperyModel(prev, m, GraphXDomain.TerminalSpacingProperty);
        //            AddProperyModel(prev, m, GraphXDomain.TerminalStretchProperty);
        //            AddProperyModel(prev, m, GraphXDomain.SymbolSizeProperty);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 647 TableList  ================================================
        //ModelAction TableList_X;
        //void Initer_TableList_X()
        //{
        //    TableList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = TableXDomain.Count;

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var store = TableXDomain;
        //            if (store.Count == 0) return (false, false);
        //            if (store.ChildDelta == m.ChildDelta) return (true, false);

        //            m.ChildDelta = store.ChildDelta;
        //            m.InitChildModels(prev, store.Count);

        //            var items = store.Items;

        //            var anyChange = prev.Count != items.Count;
        //            foreach (var itm in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.TableModel, itm, null, null, Table_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 648 GraphList  ================================================
        //ModelAction GraphList_X;
        //void Initer_GraphList_X()
        //{
        //    GraphList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = GraphXDomain.Count;

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var store = GraphXDomain;
        //            if (store.Count == 0) return (false, false);
        //            if (store.ChildDelta == m.ChildDelta) return (true, false);

        //            m.ChildDelta = store.ChildDelta;
        //            m.InitChildModels(prev, store.Count);

        //            var items = store.Items;

        //            var anyChange = prev.Count != items.Count;
        //            foreach (var itm in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.GraphXRefModel, itm, null, null, GraphXRef_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion



        //#region 652 MetaPair_X  ===============================================
        //ModelAction MetaPair_X;
        //void Initer_MetaPair_X()
        //{
        //    MetaPair_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanDrag = true;
        //            m.CanExpandRight = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.PairX.ActualValue,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderStoreItem,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (!m.IsExpandedRight) return (false, false);
        //            if (m.ChildModelCount == 2) return (true, false);

        //            m.InitChildModels(prev);

        //            AddProperyModel(prev, m, EnumXDomain.TextProperty);
        //            AddProperyModel(prev, m, EnumXDomain.ValueProperty);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, m.PairX.DisplayValue);
        //}
        //#endregion

        //#region 653 MetaEnum_X  ===============================================
        //ModelAction MetaEnum_X;
        //void Initer_MetaEnum_X()
        //{
        //    MetaEnum_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var count = m.EnumX.Count;
        //            var (kind, name) = GetKindName(m);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = count > 0;
        //            m.CanExpandRight = true;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => (m.Item as EnumX).Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderStoreItem,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.InsertCommand, Insert));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var ex = m.EnumX;
        //            m.InitChildModels(prev);

        //            var anyChange = false;
        //            if (m.IsExpandedRight)
        //            {
        //                anyChange |= AddProperyModel(prev, m, EnumXDomain.NameProperty);
        //                anyChange |= AddProperyModel(prev, m, EnumXDomain.SummaryProperty);
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.MetaEnumPairListModel, ex, null, null, MetaEnumPairList_X);
        //                anyChange |= AddChildModel(prev, m, IdKey.MetaEnumColumnListModel, ex, null, null, MetaEnumColumnList_X);
        //            }

        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, m.EnumX.Name);

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void Insert(IModel m)
        //    {
        //        ItemCreated(new PairX(m.EnumX, true));
        //    }
        //}
        //#endregion

        //#region 654 MetaTable_X  ==============================================
        //ModelAction MetaTable_X;
        //void Initer_MetaTable_X()
        //{
        //    MetaTable_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = true;
        //            m.CanExpandRight = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.TableX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderStoreItem,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var R = m.IsExpandedRight ? 2 : 0;
        //            var L = m.IsExpandedLeft ? 5 : 0;
        //            if (R + L == 0) return (false, false);
        //            if (R + L == m.ChildModelCount) return (true, false);

        //            m.InitChildModels(prev);

        //            if (m.IsExpandedRight)
        //            {
        //                AddProperyModel(prev, m, TableXDomain.NameProperty);
        //                AddProperyModel(prev, m, TableXDomain.SummaryProperty);
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                var tx = m.TableX;
        //                AddChildModel(prev, m, IdKey.MetaColumnListModel, tx, null, null, MetaColumnList_X);
        //                AddChildModel(prev, m, IdKey.MetaComputeListModel, tx, null, null, MetaComputeList_X);
        //                AddChildModel(prev, m, IdKey.MetaChildRelationListModel, tx, null, null, MetaChildRelationList_X);
        //                AddChildModel(prev, m, IdKey.MetaParentRelatationListModel, tx, null, null, MetaParentRelatationList_X);
        //                AddChildModel(prev, m, IdKey.MetaRelationListModel, tx, null, null, MetaRelationList_X);
        //            }
        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, m.TableX.Name);
        //}
        //#endregion

        //#region 655 MetaGraph_X  ==============================================
        //ModelAction MetaGraph_X;
        //void Initer_MetaGraph_X()
        //{
        //    MetaGraph_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = true;
        //            m.CanExpandRight = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.GraphX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderStoreItem,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var R = m.IsExpandedRight ? 2 : 0;
        //            var L = m.IsExpandedLeft ? 5 : 0;
        //            if (R + L == 0) return (false, false);
        //            if (R + L == m.ChildModelCount) return (true, false);

        //            m.InitChildModels(prev);

        //            if (m.IsExpandedRight)
        //            {
        //                AddProperyModel(prev, m, GraphXDomain.NameProperty);
        //                AddProperyModel(prev, m, GraphXDomain.SummaryProperty);
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                var gx = m.GraphX;
        //                RefreshGraphX(gx);
        //                AddChildModel(prev, m, IdKey.MetaGraphColoringModel, gx, null, null, MetaGraphColoring_X);
        //                AddChildModel(prev, m, IdKey.MetaGraphRootListModel, gx, null, null, MetaGraphRootList_X);
        //                AddChildModel(prev, m, IdKey.MetaGraphNodeListModel, gx, null, null, MetaGraphNodeList_X);
        //                AddChildModel(prev, m, IdKey.MetaSymbolListModel, gx, null, null, MetaSymbolList_X);
        //                AddChildModel(prev, m, IdKey.MetaGraphParmListModel, gx, null, null, MetaGraphParmList_X);
        //            }
        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, m.GraphX.Name);
        //}
        //#endregion

        //#region 656 MetaSymbol_X  =============================================
        //ModelAction MetaSymbol_X;
        //void Initer_MetaSymbol_X()
        //{
        //    MetaSymbol_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanDrag = true;
        //            m.CanExpandRight = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.SymbolX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderRelatedChild,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.EditCommand, CreateSecondarySymbolEdit));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (!m.IsExpandedRight) return (false, false);
        //            if (m.ChildModelCount == 6) return (true, false);

        //            m.InitChildModels(prev);

        //            AddProperyModel(prev, m, SymbolXDomain.NameProperty);
        //            AddProperyModel(prev, m, SymbolXDomain.AttachProperty);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, m.SymbolX.Name);

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void CreateSecondarySymbolEdit(IModel m) => m.GetRootModel().UIRequestCreatePage(ControlType.SymbolEditor, IdKey.SymbolEditorModel, m.Item, SymbolEditor_X);
        //}
        //#endregion

        //#region 657 MetaColumn_X  =============================================
        //ModelAction MetaColumn_X;
        //void Initer_MetaColumn_X()
        //{
        //    MetaColumn_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanDrag = true;
        //            m.CanExpandRight = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.ColumnX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderRelatedChild,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (!m.IsExpandedRight) return (false, false);
        //            if (m.ChildModelCount == 4) return (true, false);

        //            m.InitChildModels(prev);

        //            AddProperyModel(prev, m, ColumnXDomain.NameProperty);
        //            AddProperyModel(prev, m, ColumnXDomain.SummaryProperty);
        //            AddProperyModel(prev, m, ColumnXDomain.TypeOfProperty);
        //            AddProperyModel(prev, m, ColumnXDomain.IsChoiceProperty);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, m.ColumnX.Name);
        //}
        //#endregion

        //#region 658 MetaCompute_X  ============================================
        //ModelAction MetaCompute_X;
        //void Initer_MetaCompute_X()
        //{
        //    MetaCompute_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var cx = m.ComputeX;
        //            var count = ComputeX_QueryX.TryGetChild(cx, out QueryX qx) ? QueryX_QueryX.ChildCount(qx) : 0;
        //            var (kind, name) = GetKindName(m);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = (count > 0);
        //            m.CanExpandRight = true;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.ComputeX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderRelatedChild,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!(d.Item is Relation rel)) return DropAction.None;

        //            var cx = m.ComputeX;
        //            if (!ComputeX_QueryX.TryGetChild(cx, out QueryX root)) return DropAction.None;
        //            if (!Store_ComputeX.TryGetParent(cx, out Store sto)) return DropAction.None;

        //            GetHeadTail(rel, out Store sto1, out Store sto2);
        //            if (sto != sto1 && sto != sto2) return DropAction.None;

        //            if (doDrop)
        //            {
        //                CreateQueryX(root, rel, QueryType.Value).IsReversed = (sto == sto2);
        //                m.IsExpandedLeft = true;
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var cx = m.Item as ComputeX;
        //            if (!ComputeX_QueryX.TryGetChild(cx, out QueryX qx)) return (false, false);

        //            m.InitChildModels(prev);

        //            if (m.IsExpandedRight)
        //            {
        //                switch (cx.CompuType)
        //                {
        //                    case CompuType.RowValue:
        //                        if (qx.HasSelect)
        //                        {
        //                            AddProperyModel(prev, m, ComputeXDomain.NameProperty);
        //                            AddProperyModel(prev, m, ComputeXDomain.SummaryProperty);
        //                            AddProperyModel(prev, m, ComputeXDomain.CompuTypeProperty);
        //                            AddProperyModel(prev, m, ComputeXDomain.SelectProperty);
        //                            AddProperyModel(prev, m, ComputeXDomain.ValueTypeProperty);
        //                        }
        //                        else
        //                        {
        //                            AddProperyModel(prev, m, ComputeXDomain.NameProperty);
        //                            AddProperyModel(prev, m, ComputeXDomain.SummaryProperty);
        //                            AddProperyModel(prev, m, ComputeXDomain.CompuTypeProperty);
        //                            AddProperyModel(prev, m, ComputeXDomain.SelectProperty);
        //                        }
        //                        break;

        //                    case CompuType.RelatedValue:
        //                        AddProperyModel(prev, m, ComputeXDomain.NameProperty);
        //                        AddProperyModel(prev, m, ComputeXDomain.SummaryProperty);
        //                        AddProperyModel(prev, m, ComputeXDomain.CompuTypeProperty);
        //                        AddProperyModel(prev, m, ComputeXDomain.ValueTypeProperty);
        //                        break;

        //                    case CompuType.CompositeString:
        //                    case CompuType.CompositeReversed:
        //                        AddProperyModel(prev, m, ComputeXDomain.NameProperty);
        //                        AddProperyModel(prev, m, ComputeXDomain.SummaryProperty);
        //                        AddProperyModel(prev, m, ComputeXDomain.CompuTypeProperty);
        //                        AddProperyModel(prev, m, ComputeXDomain.SeparatorProperty);
        //                        AddProperyModel(prev, m, ComputeXDomain.SelectProperty);
        //                        AddProperyModel(prev, m, ComputeXDomain.ValueTypeProperty);
        //                        break;
        //                }
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                if (QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list))
        //                {
        //                    foreach (var itm in list)
        //                    {
        //                        AddChildModel(prev, m, IdKey.MetaValueHeadModel, itm, null, null, ValueHead_X);
        //                    }
        //                }
        //            }
        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, m.ComputeX.Name);
        //}
        //#endregion

        //#region 659 SymbolEditor_X  ===========================================
        //ModelAction SymbolEditor_X;
        //void Initer_SymbolEditor_X()
        //{
        //    SymbolEditor_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.SymbolX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderRelatedChild,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.SaveCommand, Save));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m, prev) =>
        //        {
        //            if (!m.IsExpandedRight) return (false, false);
        //            if (m.ChildModelCount == 1) return (true, false);

        //            m.InitChildModels(prev);

        //            AddProperyModel(prev, m, SymbolXDomain.NameProperty);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, m.SymbolX.Name);

        //    #region ButtonCommands  ===========================================
        //    void Save(IModel m)
        //    {
        //        var root = m as RootModelOld;
        //        root.UIRequestSaveSymbol();
        //    }
        //    #endregion
        //}
        //#endregion



        //#region 661 MetaColumnList_X  =========================================
        //ModelAction MetaColumnList_X;
        //void Initer_MetaColumnList_X()
        //{
        //    MetaColumnList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = Store_ColumnX.ChildCount(m.Item);

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderStoreItem,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.InsertCommand, Insert));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var tx = m.TableX;
        //            if (!Store_ColumnX.TryGetChildren(tx, out IList<ColumnX> list)) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != list.Count;
        //            foreach (var itm in list)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.MetaColumnModel, itm, Store_ColumnX, tx, MetaColumn_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void Insert(IModel model)
        //    {
        //        var col = new ColumnX(ColumnXDomain, true);
        //        ItemCreated(col); AppendLink(Store_ColumnX, model.Item, col);
        //    }
        //}
        //#endregion

        //#region 662 MetaChildRelationList_X  ==================================
        //ModelAction MetaChildRelationList_X;
        //void Initer_MetaChildRelationList_X()
        //{
        //    MetaChildRelationList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = Store_ChildRelation.ChildCount(m.Item);

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderStoreItem,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.InsertCommand, Insert));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var tx = m.TableX;
        //            if (!Store_ChildRelation.TryGetChildren(tx, out IList<Relation> list)) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != list.Count;
        //            foreach (var rel in list)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.MetaChildRelationModel, rel, Store_ChildRelation, tx, MetaChildRelation_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void Insert(IModel model)
        //    {
        //        var rel = new RelationXO(RelationXDomain);
        //        ItemCreated(rel); AppendLink(Store_ChildRelation, model.Item, rel);
        //    }
        //}
        //#endregion

        //#region 663 MetaParentRelatationList_X  ===============================
        //ModelAction MetaParentRelatationList_X;
        //void Initer_MetaParentRelatationList_X()
        //{
        //    MetaParentRelatationList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = Store_ParentRelation.ChildCount(m.Item);

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderStoreItem,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.InsertCommand, Insert));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var tx = m.TableX;
        //            if (!Store_ParentRelation.TryGetChildren(tx, out IList<Relation> list)) return (false, false);

        //            m.InitChildModels(prev);
        //            var anyChange = prev.Count != list.Count;
        //            foreach (var rel in list)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.MetaParentRelationModel, rel, Store_ParentRelation, tx, MetaParentRelation_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void Insert(IModel model)
        //    {
        //        var rel = new RelationXO(RelationXDomain, true); ItemCreated(rel);
        //        AppendLink(Store_ParentRelation, model.Item, rel);
        //    }
        //}
        //#endregion

        //#region 664 MetaEnumPairList_X  =======================================
        //ModelAction MetaEnumPairList_X;
        //void Initer_MetaEnumPairList_X()
        //{
        //    MetaEnumPairList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var count = m.EnumX.Count;
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderStoreItem,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.InsertCommand, Insert));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var ex = m.EnumX;
        //            if (ex.Count == 0) return (false, false);
        //            if (ex.ChildDelta == m.ChildDelta) return (true, false);

        //            m.ChildDelta = ex.ChildDelta;
        //            m.InitChildModels(prev);

        //            var list = ex.Items;
        //            var anyChange = prev.Count != list.Count;
        //            foreach (var px in list)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.MetaPairModel, px, ex, null, MetaPair_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void Insert(IModel model)
        //    {
        //        ItemCreated(new PairX(model.Item as EnumX, true));
        //    }
        //}
        //#endregion

        //#region 665 MetaEnumColumnList_X  =====================================
        //ModelAction MetaEnumColumnList_X;
        //void Initer_MetaEnumColumnList_X()
        //{
        //    MetaEnumColumnList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = EnumX_ColumnX.ChildCount(m.Item);

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderStoreItem,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!d.Item.IsColumnX) return DropAction.None;

        //            if (doDrop)
        //            {
        //                AppendLink(EnumX_ColumnX, m.Item, d.Item);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var ex = m.EnumX;
        //            if (!EnumX_ColumnX.TryGetChildren(ex, out IList<ColumnX> list)) return (false, false);

        //            m.InitChildModels(prev);
        //            var anyChange = prev.Count != list.Count;
        //            foreach (var cx in list)
        //            {
        //                if (Store_ColumnX.TryGetParent(cx, out Store tx))
        //                {
        //                    anyChange |= AddChildModel(prev, m, IdKey.MetaEnumRelatedColumnModel, cx, tx, ex, EnumRelatedColumn_X);
        //                }
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 666 MetaComputeList_X  ========================================
        //ModelAction MetaComputeList_X;
        //void Initer_MetaComputeList_X()
        //{
        //    MetaComputeList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = Store_ComputeX.ChildCount(m.Item);

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderStoreItem,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.InsertCommand, Insert));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var st = m.Store;
        //            if (!Store_ComputeX.TryGetChildren(st, out IList<ComputeX> list)) return (false, false);

        //            m.InitChildModels(prev);
        //            var anyChange = prev.Count != list.Count;
        //            foreach (var itm in list)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.MetaComputeModel, itm, Store_ComputeX, st, MetaCompute_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void Insert(IModel model)
        //    {
        //        var st = model.Item as Store;
        //        var cx = new ComputeX(ComputeXDomain);
        //        ItemCreated(cx);
        //        AppendLink(Store_ComputeX, st, cx);

        //        CreateQueryX(cx, st);
        //    }
        //}
        //#endregion



        //#region 671 MetaChildRelation_X  ======================================
        //ModelAction MetaChildRelation_X;
        //void Initer_MetaChildRelation_X()
        //{
        //    MetaChildRelation_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanDrag = true;
        //            m.CanExpandRight = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.RelationX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderRelatedChild,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!d.Item.IsTableX) return DropAction.None;

        //            if (doDrop)
        //            {
        //                AppendLink(Store_ParentRelation, d.Item, m.Item);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (!m.IsExpandedRight) return (false, false);
        //            if (m.ChildModelCount == 4) return (true, false);

        //            m.InitChildModels(prev);

        //            AddProperyModel(prev, m, RelationXDomain.NameProperty);
        //            AddProperyModel(prev, m, RelationXDomain.SummaryProperty);
        //            AddProperyModel(prev, m, RelationXDomain.PairingProperty);
        //            AddProperyModel(prev, m, RelationXDomain.IsRequiredProperty);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, GetIdentity(m.RelationX, IdentityStyle.Single));
        //}
        //#endregion

        //#region 672 MetaParentRelation_X  =====================================
        //ModelAction MetaParentRelation_X;
        //void Initer_MetaParentRelation_X()
        //{
        //    MetaParentRelation_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanDrag = true;
        //            m.CanExpandRight = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.RelationX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderRelatedChild,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!d.Item.IsTableX) return DropAction.None;

        //            if (doDrop)
        //            {
        //                AppendLink(Store_ChildRelation, d.Item, m.Item);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (!m.IsExpandedRight) return (false, false);
        //            if (m.ChildModelCount == 5) return (true, false);

        //            m.InitChildModels(prev);

        //            AddProperyModel(prev, m, RelationXDomain.NameProperty);
        //            AddProperyModel(prev, m, RelationXDomain.SummaryProperty);
        //            AddProperyModel(prev, m, RelationXDomain.PairingProperty);
        //            AddProperyModel(prev, m, RelationXDomain.IsRequiredProperty);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, GetIdentity(m.RelationX, IdentityStyle.Single));
        //}
        //#endregion

        //#region 673 MetaNameColumnRelation_X  =================================
        //ModelAction MetaNameColumnRelation_X;
        //void Initer_MetaNameColumnRelation_X()
        //{
        //    MetaNameColumnRelation_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = Store_NameProperty.ChildCount(m.Item);

        //            m.CanExpandLeft = (count > 0);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderStoreItem,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!(d.Item is Property)) return DropAction.None;

        //            if (doDrop)
        //            {
        //                if (m.IsChildModel(d))
        //                    RemoveLink(Store_NameProperty, m.Item, d.Item);
        //                else
        //                {
        //                    AppendLink(Store_NameProperty, m.Item, d.Item);
        //                    m.IsExpandedLeft = true;
        //                }
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var tx = m.TableX;
        //            if (!Store_NameProperty.TryGetChild(tx, out Property pr)) return (false, false);
        //            if (m.ChildModelCount == 1) return (true, false);

        //            m.InitChildModels(prev);

        //            AddChildModel(prev, m, IdKey.MetaNameColumnModel, pr, Store_NameProperty, tx, MetaNameColumn_X);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 674 MetaSummaryColumnRelation_X  ==============================
        //ModelAction MetaSummaryColumnRelation_X;
        //void Initer_MetaSummaryColumnRelation_X()
        //{
        //    MetaSummaryColumnRelation_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = Store_SummaryProperty.ChildCount(m.Item);

        //            m.CanExpandLeft = (count > 0);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderStoreItem,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!(d.Item is Property)) return DropAction.None;

        //            if (doDrop)
        //            {
        //                if (m.IsChildModel(d))
        //                    RemoveLink(Store_SummaryProperty, m.Item, d.Item);
        //                else
        //                {
        //                    AppendLink(Store_SummaryProperty, m.Item, d.Item);
        //                    m.IsExpandedLeft = true;
        //                }
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var tx = m.TableX;
        //            if (!Store_SummaryProperty.TryGetChild(tx, out Property pr)) return (false, false);
        //            if (m.ChildModelCount == 1) return (true, false);

        //            m.InitChildModels(prev);

        //            AddChildModel(prev, m, IdKey.MetaSummaryColumnModel, pr, Store_SummaryProperty, tx, MetaSummaryColumn_X);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 675 MetaNameColumn_X  =========================================
        //ModelAction MetaNameColumn_X;
        //void Initer_MetaNameColumn_X()
        //{
        //    MetaNameColumn_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) =>
        //        {
        //            if (m.Item.IsColumnX) return m.ColumnX.Summary;
        //            if (m.Item.IsComputeX) return m.ComputeX.Summary;
        //            throw new Exception("Corrupt ItemModelTree");
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = (m, mc) =>
        //        {
        //            mc.Add(new ModelCommand(this, m, IdKey.RemoveCommand, UnlinkRelatedChild));
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m)
        //    {
        //        if (m.Item.IsColumnX) return (null, m.ColumnX.Name);
        //        if (m.Item.IsComputeX) return (null, m.ComputeX.Name);
        //        throw new Exception("Corrupt ItemModelTree");
        //    }
        //}
        //#endregion

        //#region 676 MetaSummaryColumn_X  ======================================
        //ModelAction MetaSummaryColumn_X;
        //void Initer_MetaSummaryColumn_X()
        //{
        //    MetaSummaryColumn_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) =>
        //        {
        //            if (m.Item.IsColumnX) return m.ColumnX.Summary;
        //            if (m.Item.IsComputeX) return m.ComputeX.Summary;
        //            throw new Exception("Corrupt ItemModelTree");
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = (m, mc) =>
        //        {
        //            mc.Add(new ModelCommand(this, m, IdKey.RemoveCommand, UnlinkRelatedChild));
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m)
        //    {
        //        if (m.Item.IsColumnX) return (null, m.ColumnX.Name);
        //        if (m.Item.IsComputeX) return (null, m.ComputeX.Name);
        //        throw new Exception("Corrupt ItemModelTree");
        //    }
        //}
        //#endregion



        //#region 681 MetaGraphColoring_X  ======================================
        //ModelAction MetaGraphColoring_X;
        //void Initer_MetaGraphColoring_X()
        //{
        //    MetaGraphColoring_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = GraphX_ColorColumnX.ChildCount(m.GraphX);

        //            m.CanExpandLeft = (count > 0);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDescription = (m) => _localize(m.DescriptionKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            var gx = m.GraphX;
        //            var col = d.ColumnX;
        //            if (col == null) return DropAction.None;
        //            if (!gx.IsGraphX) return DropAction.None;

        //            if (doDrop)
        //            {
        //                AppendLink(GraphX_ColorColumnX, gx, col);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (!(GraphX_ColorColumnX.TryGetChild(m.Item, out ColumnX cx) && Store_ColumnX.TryGetParent(cx, out Store tx))) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = AddChildModel(prev, m, IdKey.MetaGraphColorColumnModel, cx, tx, null, MetaGraphColorColumn_X);

        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 682 MetaGraphRootList_X  ======================================
        //ModelAction MetaGraphRootList_X;
        //void Initer_MetaGraphRootList_X()
        //{
        //    MetaGraphRootList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = GraphX_QueryX.ChildCount(m.Item);

        //            m.CanExpandLeft = (count > 0);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDescription = (m) => _localize(m.DescriptionKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!(m.Item is GraphX gx)) return DropAction.None;
        //            if (!(d.Item is Store st)) return DropAction.None;
        //            if (GraphXAlreadyHasThisRoot(gx, st)) return DropAction.None;

        //            if (doDrop)
        //            {
        //                CreateQueryX(gx, st);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var gx = m.GraphX;
        //            if (!GraphX_QueryX.TryGetChildren(gx, out IList<QueryX> list)) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != list.Count;
        //            foreach (var itm in list)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphRootModel, itm, gx, null, MetaGraphRoot_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    bool GraphXAlreadyHasThisRoot(Item gd, Item table)
        //    {
        //        if (GraphX_QueryX.TryGetChildren(gd, out IList<QueryX> list))
        //        {
        //            foreach (var qx in list)
        //            {
        //                if (Store_QueryX.ContainsLink(table, qx)) return true;
        //            }
        //        }
        //        return false;
        //    }
        //}
        //#endregion

        //#region 683 MetaGraphNodeList_X  ======================================
        //ModelAction MetaGraphNodeList_X;
        //void Initer_MetaGraphNodeList_X()
        //{
        //    MetaGraphNodeList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.GraphX.NodeOwners.Count;

        //            m.CanExpandLeft = (count > 0);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var gx = m.GraphX;
        //            var owners = gx.NodeOwners;
        //            if (owners.Count == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != owners.Count;
        //            foreach (var sto in owners)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphNodeModel, sto, gx, null, MetaGraphNode_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion
        
        //#region 684 MetaGraphNode_X  ==========================================
        //ModelAction MetaGraphNode_X;
        //void Initer_MetaGraphNode_X()
        //{
        //    MetaGraphNode_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var st = m.Item as Store;
        //            var (kind, name) = GetKindName(m);
        //            var count = GetSymbolQueryXCount(m.GraphX, st);

        //            m.CanExpandLeft = (count > 0);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!(m.Item is Store st)) return DropAction.None;
        //            if (!(m.Aux1 is GraphX gx)) return DropAction.None;
        //            if (!(d.Item is SymbolX sx)) return DropAction.None;

        //            if (doDrop)
        //            {
        //                CreateQueryX(gx, sx, st);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var gx = m.GraphX;
        //            var st = m.Item as Store;

        //            (var symbols, var querys) = GetSymbolXQueryX(gx, st);
        //            if (querys == null) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != querys.Count;
        //            foreach (var qx in querys)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphNodeSymbolModel, qx, GraphX_SymbolQueryX, gx, MetaGraphNodeSymbol_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), GetIdentity(m.Item, IdentityStyle.Single));
        //}
        //#endregion

        //#region 685 MetaGraphColorColumn_X  ===================================
        //ModelAction MetaGraphColorColumn_X;
        //void Initer_MetaGraphColorColumn_X()
        //{
        //    MetaGraphColorColumn_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), $"{m.TableX.Name} : {m.ColumnX.Name}");
        //}
        //#endregion


        //#region MetaGraphMenuCommands  ========================================
        //void MetaGraphMenuCommands(IModel m, List<ModelCommand> mc)
        //{
        //    mc.Add(new ModelCommand(this, m, IdKey.RemoveCommand, RemoveItem));
        //    mc.Add(new ModelCommand(this, m, IdKey.MakePathHeadCommand, MakePathtHead));
        //    mc.Add(new ModelCommand(this, m, IdKey.MakeGroupHeadCommand, MakeGroupHead));
        //    mc.Add(new ModelCommand(this, m, IdKey.MakeEgressHeadCommand, MakeEgressHead));

        //    void MakePathtHead(IModel mt) => TryConvert(mt, QueryType.Path);
        //    void MakeGroupHead(IModel mt) => TryConvert(mt, QueryType.Group);
        //    void MakeEgressHead(IModel mt) => TryConvert(mt, QueryType.Egress);

        //    void TryConvert(IModel mt, QueryType kind)
        //    {
        //        var qx = mt.QueryX;
        //        var N = QueryX_QueryX.ChildCount(qx);
        //        var list = new List<QueryX>() { qx };
        //        while (N == 1)
        //        {
        //            if (!QueryX_QueryX.TryGetChild(qx, out qx)) return; // can not convert 
        //            list.Add(qx);
        //            N = QueryX_QueryX.ChildCount(qx);
        //        }
        //        if (N > 1) return; // can not convert 

        //        N = list.Count;
        //        var M = N - 1;

        //        list[0].PathParm = (kind == QueryType.Path) ? new PathParm() : null;

        //        for (int i = 0; i < N; i++)
        //        {
        //            list[i].QueryKind = kind;
        //            list[i].IsHead = (i == 0);
        //            list[i].IsTail = (i == M);
        //        }
        //    }
        //}
        //#endregion


        //#region 691 MetaGraphRoot_X  ==========================================
        //ModelAction MetaGraphRoot_X;
        //void Initer_MetaGraphRoot_X()
        //{
        //    MetaGraphRoot_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = QueryX_QueryX.ChildCount(m.Item);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = (count > 0);
        //            m.CanExpandRight = true;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDescription = (m) => _localize(m.DescriptionKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!(d.Item is Relation re)) return DropAction.None;
        //            if (!(m.Item is QueryX qx)) return DropAction.None;

        //            if (doDrop)
        //            {
        //                CreateQueryX(qx, re, QueryType.Graph);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var qx = m.QueryX;
        //            if (!m.IsExpandedLeft && m.IsExpandedRight && m.ChildModelCount == 1) return (true, false);

        //            //if (!QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list)) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = false;
        //            if (m.IsExpandedRight)
        //            {
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RootWhereProperty);
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                if (QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list))
        //                {
        //                    foreach (var qc in list)
        //                    {
        //                        if (qc.IsPath)
        //                        {
        //                            if (qc.IsHead)
        //                                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphPathHeadModel, qc, null, null, MetaGraphPathHead_X);
        //                            else
        //                                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphPathLinkModel, qc, null, null, MetaGraphPathLink_X);
        //                        }
        //                        else if (qc.IsGroup)
        //                        {
        //                            if (qc.IsHead)
        //                                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphGroupHeadModel, qc, null, null, MetaGraphGroupHead_X);
        //                            else
        //                                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphGroupLinkModel, qc, null, null, MetaGraphGroupLink_X);
        //                        }
        //                        else if (qc.IsSegue)
        //                        {
        //                            if (qc.IsHead)
        //                                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphEgressHeadModel, qc, null, null, MetaGraphEgressHead_X);
        //                            else
        //                                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphEgressLinkModel, qc, null, null, MetaGraphEgressLink_X);
        //                        }
        //                        else
        //                        {
        //                            anyChange |= AddChildModel(prev, m, IdKey.MetaGraphLinkModel, qc, null, null, MetaGraphLink_X);
        //                        }
        //                    }
        //                }
        //            }
        //            return(true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m)
        //    {
        //        var name = (Store_QueryX.TryGetParent(m.Item, out Store sto)) ? GetIdentity(sto, IdentityStyle.Single) : Chef.BlankName;
        //        return (_localize(m.NameKey), name);
        //    }
        //}
        //#endregion

        //#region 692 MetaGraphLink_X  ==========================================
        //ModelAction MetaGraphLink_X;
        //void Initer_MetaGraphLink_X()
        //{
        //    MetaGraphLink_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = QueryX_QueryX.ChildCount(m.Item);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = (count > 0);
        //            m.CanExpandRight = true;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetQueryXRelationName(m),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = MetaGraphMenuCommands,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (d.Item.IsRelationX)
        //            {
        //                if (doDrop)
        //                {
        //                    var qx = m.Item as QueryX;
        //                    var re = d.Item as Relation;
        //                    CreateQueryX(qx, re, QueryType.Graph);
        //                }
        //                return DropAction.Link;
        //            }
        //            else if (m.Item.IsQueryGraphLink)
        //            {
        //                if (doDrop)
        //                {
        //                    RemoveItem(d);
        //                }
        //                return DropAction.Link;
        //            }
        //            return DropAction.None;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var qx = m.Item as QueryX;
        //            if (!m.IsExpandedLeft && m.IsExpandedRight && m.ChildModelCount == 3) return (true, false);

        //            m.InitChildModels(prev);

        //            var anyChange = false;
        //            if (m.IsExpandedRight)
        //            {
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RelationProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.IsReversedProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RootWhereProperty);
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                if (QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list))
        //                {
        //                    foreach (var qc in list)
        //                    {
        //                        if (qc.IsPath)
        //                        {
        //                            if (qc.IsHead)
        //                                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphPathHeadModel, qc, null, null, MetaGraphPathHead_X);
        //                            else
        //                                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphPathLinkModel, qc, null, null, MetaGraphPathLink_X);
        //                        }
        //                        else if (qc.IsGroup)
        //                        {
        //                            if (qc.IsHead)
        //                                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphGroupHeadModel, qc, null, null, MetaGraphGroupHead_X);
        //                            else
        //                                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphGroupLinkModel, qc, null, null, MetaGraphGroupLink_X);
        //                        }
        //                        else if (qc.IsSegue)
        //                        {
        //                            if (qc.IsHead)
        //                                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphEgressHeadModel, qc, null, null, MetaGraphEgressHead_X);
        //                            else
        //                                anyChange |= AddChildModel(prev, m, IdKey.MetaGraphEgressLinkModel, qc, null, null, MetaGraphEgressLink_X);
        //                        }
        //                        else
        //                        {
        //                            anyChange |= AddChildModel(prev, m, IdKey.MetaGraphLinkModel, qc, null, null, MetaGraphLink_X);
        //                        }
        //                    }
        //                }
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXLinkName(m));
        //}
        //#endregion

        //#region 693 MetaGraphPathHead_X  ======================================
        //ModelAction MetaGraphPathHead_X;
        //void Initer_MetaGraphPathHead_X()
        //{
        //    MetaGraphPathHead_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = QueryX_QueryX.ChildCount(m.Item);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = (count > 0);
        //            m.CanExpandRight = true;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetQueryXRelationName(m),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderRelatedChild,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = MetaGraphMenuCommands,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!(m.Item is QueryX qx)) return DropAction.None;
        //            if (!(d.Item is Relation re)) return DropAction.None;
        //            if (!CanDropQueryXRelation(qx, d.Item as RelationXO)) return DropAction.None;

        //            if (doDrop)
        //            {
        //                CreateQueryX(qx, re, QueryType.Path);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var qx = m.Item as QueryX;
        //            if (!m.IsExpandedRight && m.IsExpandedLeft && QueryX_QueryX.ChildCount(qx) == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = false;
        //            if (m.IsExpandedRight)
        //            {
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RelationProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.IsReversedProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.IsBreakPointProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.ExclusiveKeyProperty);
        //                                        anyChange |= AddProperyModel(prev, m, QueryXDomain.RootWhereProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.LineColorProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.LineStyleProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.DashStyleProperty);

        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.Facet1Property);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.Connect1Property);

        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.Facet2Property);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.Connect2Property);
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                if (QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list))
        //                {
        //                    foreach (var qc in list)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.MetaGraphPathLinkModel, qc, null, null, MetaGraphPathLink_X);
        //                    }
        //                }
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXHeadName(m));
        //}
        //#endregion

        //#region 694 MetaGraphPathLink_X  ======================================
        //ModelAction MetaGraphPathLink_X;
        //void Initer_MetaGraphPathLink_X()
        //{
        //    MetaGraphPathLink_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = QueryX_QueryX.ChildCount(m.Item);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = (count > 0);
        //            m.CanExpandRight = true;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetQueryXRelationName(m),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!(m.Item is QueryX qx)) return DropAction.None;
        //            if (!(d.Item is Relation re)) return DropAction.None;
        //            if (!CanDropQueryXRelation(qx, d.Item as RelationXO)) return DropAction.None;

        //            if (doDrop)
        //            {
        //                CreateQueryX(qx, re, QueryType.Path);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var qx = m.Item as QueryX;
        //            if (!m.IsExpandedRight && m.IsExpandedLeft && QueryX_QueryX.ChildCount(qx) == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = false;
        //            if (m.IsExpandedRight)
        //            {
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RelationProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.IsReversedProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.IsBreakPointProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RootWhereProperty);
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                if (QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list))
        //                {
        //                    foreach (var qc in list)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.MetaGraphPathLinkModel, qc, null, null, MetaGraphPathLink_X);
        //                    }
        //                }
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXLinkName(m));
        //}
        //#endregion

        //#region 695 MetaGraphGroupHead_X  =====================================
        //ModelAction MetaGraphGroupHead_X;
        //void Initer_MetaGraphGroupHead_X()
        //{
        //    MetaGraphGroupHead_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = QueryX_QueryX.ChildCount(m.Item);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = (count > 0);
        //            m.CanExpandRight = true;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = MetaGraphMenuCommands,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!(m.Item is QueryX qx)) return DropAction.None;
        //            if (!(d.Item is Relation re)) return DropAction.None;
        //            if (!CanDropQueryXRelation(qx, d.Item as RelationXO)) return DropAction.None;

        //            if (doDrop)
        //            {
        //                CreateQueryX(qx, re, QueryType.Group);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var qx = m.Item as QueryX;
        //            if (!m.IsExpandedRight && m.IsExpandedLeft && QueryX_QueryX.ChildCount(qx) == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = false;
        //            if (m.IsExpandedRight)
        //            {
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RelationProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.IsReversedProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RootWhereProperty);
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                if (QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list))
        //                {
        //                    foreach (var qc in list)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.MetaGraphGroupLinkModel, qc, null, null, MetaGraphGroupLink_X);
        //                    }
        //                }
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXHeadName(m));
        //}
        //#endregion

        //#region 696 MetaGraphGroupLink_X  =====================================
        //ModelAction MetaGraphGroupLink_X;
        //void Initer_MetaGraphGroupLink_X()
        //{
        //    MetaGraphGroupLink_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = QueryX_QueryX.ChildCount(m.Item);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = (count > 0);
        //            m.CanExpandRight = true;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetQueryXRelationName(m),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!(m.Item is QueryX qx)) return DropAction.None;
        //            if (!(d.Item is Relation re)) return DropAction.None;
        //            if (!CanDropQueryXRelation(qx, d.Item as RelationXO)) return DropAction.None;

        //            if (doDrop)
        //            {
        //                CreateQueryX(qx, re, QueryType.Group);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var qx = m.Item as QueryX;
        //            if (!m.IsExpandedRight && m.IsExpandedLeft && QueryX_QueryX.ChildCount(qx) == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = false;
        //            if (m.IsExpandedRight)
        //            {
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RelationProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.IsReversedProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RootWhereProperty);
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                if (QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list))
        //                {
        //                    foreach (var qc in list)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.MetaGraphGroupLinkModel, qc, null, null, MetaGraphGroupLink_X);
        //                    }
        //                }
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXLinkName(m));
        //}
        //#endregion

        //#region 697 MetaGraphEgressHead_X  ====================================
        //ModelAction MetaGraphEgressHead_X;
        //void Initer_MetaGraphEgressHead_X()
        //{
        //    MetaGraphEgressHead_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = QueryX_QueryX.ChildCount(m.Item);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = (count > 0);
        //            m.CanExpandRight = true;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetQueryXRelationName(m),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = MetaGraphMenuCommands,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!(m.Item is QueryX qx)) return DropAction.None;
        //            if (!(d.Item is Relation re)) return DropAction.None;
        //            if (!CanDropQueryXRelation(qx, d.Item as RelationXO)) return DropAction.None;

        //            if (doDrop)
        //            {
        //                CreateQueryX(qx, re, QueryType.Egress);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var qx = m.Item as QueryX;
        //            if (!m.IsExpandedRight && m.IsExpandedLeft && QueryX_QueryX.ChildCount(qx) == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = false;
        //            if (m.IsExpandedRight)
        //            {
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RelationProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.IsReversedProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RootWhereProperty);
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                if (QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list))
        //                {
        //                    foreach (var qc in list)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.MetaGraphEgressLinkModel, qc, null, null, MetaGraphEgressLink_X);
        //                    }
        //                }
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXHeadName(m));
        //}
        //#endregion

        //#region 698 MetaGraphEgressLink_X  ====================================
        //ModelAction MetaGraphEgressLink_X;
        //void Initer_MetaGraphEgressLink_X()
        //{
        //    MetaGraphEgressLink_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = QueryX_QueryX.ChildCount(m.Item);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = (count > 0);
        //            m.CanExpandRight = true;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetQueryXRelationName(m),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!(m.Item is QueryX qx)) return DropAction.None;
        //            if (!(d.Item is Relation re)) return DropAction.None;
        //            if (!CanDropQueryXRelation(qx, d.Item as RelationXO)) return DropAction.None;

        //            if (doDrop)
        //            {
        //                CreateQueryX(qx, re, QueryType.Egress);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var qx = m.Item as QueryX;
        //            if (!m.IsExpandedRight && m.IsExpandedLeft && QueryX_QueryX.ChildCount(qx) == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = false;
        //            if (m.IsExpandedRight)
        //            {
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RelationProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.IsReversedProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RootWhereProperty);
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                if (QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list))
        //                {
        //                    foreach (var qc in list)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.MetaGraphEgressLinkModel, qc, null, null, MetaGraphEgressLink_X);
        //                    }
        //                }
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXLinkName(m));
        //}
        //#endregion

        //#region 699 MetaGraphNodeSymbol_X  ====================================
        //ModelAction MetaGraphNodeSymbol_X;
        //void Initer_MetaGraphNodeSymbol_X()
        //{
        //    MetaGraphNodeSymbol_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanDrag = true;
        //            m.CanExpandRight = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetQueryXRelationName(m),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderRelatedChild,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (m.ChildModelCount == 1) return (true, false);

        //            m.InitChildModels(prev);

        //            AddProperyModel(prev, m, QueryXDomain.WhereProperty);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), (SymbolX_QueryX.TryGetParent(m.Item, out SymbolX sym)) ? sym.Name : null);
        //}
        //#endregion



        //#region 69E ValueXHead  ===============================================
        //ModelAction ValueHead_X;
        //void Initer_ValueHead_X()
        //{
        //    ValueHead_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = QueryX_QueryX.ChildCount(m.Item);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = (count > 0);
        //            m.CanExpandRight = true;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => QueryXComputeName(m),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!(d.Item is Relation re)) return DropAction.None;
        //            if (!(m.Item is QueryX qx)) return DropAction.None;
        //            GetHeadTail(qx, out Store tb1Head, out Store tb1Tail);
        //            GetHeadTail(re, out Store tb2Head, out Store tb2Tail);
        //            if ((tb1Head != tb2Head && tb1Head != tb2Tail) && (tb1Tail != tb2Head && tb1Tail != tb2Tail)) return DropAction.None;

        //            if (doDrop)
        //            {
        //                CreateQueryX(qx, re, QueryType.Value).IsReversed = (tb1Tail == tb2Tail);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var qx = m.Item as QueryX;
        //            if (!m.IsExpandedRight && m.IsExpandedLeft && QueryX_QueryX.ChildCount(qx) == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = false;
        //            if (m.IsExpandedRight)
        //            {
        //                if (qx.HasSelect)
        //                {
        //                    anyChange |= AddProperyModel(prev, m, QueryXDomain.RelationProperty);
        //                    anyChange |= AddProperyModel(prev, m, QueryXDomain.IsReversedProperty);
        //                    anyChange |= AddProperyModel(prev, m, QueryXDomain.RootWhereProperty);
        //                    anyChange |= AddProperyModel(prev, m, QueryXDomain.SelectProperty);
        //                    anyChange |= AddProperyModel(prev, m, QueryXDomain.ValueTypeProperty);

        //                }
        //                else
        //                {
        //                    anyChange |= AddProperyModel(prev, m, QueryXDomain.RelationProperty);
        //                    anyChange |= AddProperyModel(prev, m, QueryXDomain.IsReversedProperty);
        //                    anyChange |= AddProperyModel(prev, m, QueryXDomain.RootWhereProperty);
        //                    anyChange |= AddProperyModel(prev, m, QueryXDomain.SelectProperty);
        //                }
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                if (QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list))
        //                {
        //                    foreach (var qc in list)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.MetaValueLinkModel, qc, null, null, ValueLink_X);
        //                    }
        //                }
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXLinkName(m));
        //}
        //#endregion

        //#region 69F ValueXLink  ===============================================
        //ModelAction ValueLink_X;
        //void Initer_ValueLink_X()
        //{
        //    ValueLink_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = QueryX_QueryX.ChildCount(m.Item);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = (count > 0);
        //            m.CanExpandRight = true;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => QueryXComputeName(m),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!(d.Item is Relation re)) return DropAction.None;
        //            if (!(m.Item is QueryX qx)) return DropAction.None;
        //            GetHeadTail(qx, out Store tb1Head, out Store tb1Tail);
        //            GetHeadTail(re, out Store tb2Head, out Store tb2Tail);
        //            if ((tb1Head != tb2Head && tb1Head != tb2Tail) && (tb1Tail != tb2Head && tb1Tail != tb2Tail)) return DropAction.None;

        //            if (doDrop)
        //            {
        //                CreateQueryX(qx, re, QueryType.Value).IsReversed = (tb1Tail == tb2Tail);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var qx = m.Item as QueryX;
        //            if (!m.IsExpandedRight && m.IsExpandedLeft && QueryX_QueryX.ChildCount(qx) == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = false;
        //            if (m.IsExpandedRight)
        //            {
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RelationProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.IsReversedProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.RootWhereProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.SelectProperty);
        //                anyChange |= AddProperyModel(prev, m, QueryXDomain.ValueTypeProperty);
        //            }

        //            if (m.IsExpandedLeft)
        //            {
        //                if (QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list))
        //                {
        //                    foreach (var qc in list)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.MetaValueLinkModel, qc, null, null, ValueLink_X);
        //                    }
        //                }
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXLinkName(m));
        //}
        //string QueryXComputeName(IModel m)
        //{
        //    var sd = m.Item as QueryX;

        //    GetHeadTail(sd, out Store head1, out Store tail1);
        //    var sd2 = GetValueeDefTail(sd);
        //    GetHeadTail(sd2, out Store head2, out Store tail2);

        //    StringBuilder sb = new StringBuilder(132);
        //    sb.Append(GetIdentity(head1, IdentityStyle.Single));
        //    sb.Append(parentNameSuffix);
        //    sb.Append(GetIdentity(tail2, IdentityStyle.Single));
        //    return sb.ToString();

        //    QueryX GetValueeDefTail(QueryX q)
        //    {
        //        var q2 = q;
        //        var q3 = q2;
        //        while (q3 != null)
        //        {
        //            q2 = q3;
        //            QueryX_QueryX.TryGetChild(q3, out q3);
        //        }
        //        return q2;
        //    }
        //}
        //#endregion



        //#region 6A1 RowX  =====================================================
        //ModelAction RowX_X;
        //void Initer_Row_X()
        //{
        //    RowX_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);


        //            m.CanDrag = true;
        //            m.CanExpandLeft = true;
        //            m.CanExpandRight = HasChoiceColumns(m.RowX.TableX);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetIdentity(m.Item, IdentityStyle.Summary),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderStoreItem,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = RemoveItemMenuCommand,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = RowX_VX,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, GetIdentity(m.Item, IdentityStyle.Single));
        //}

        ////= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //private (bool, bool) RowX_VX(IModel m, List<IModel> prev)
        //{
        //    var rx = m.RowX;

        //    m.InitChildModels(prev);

            
        //    var anyChange = false;
        //    if (m.IsExpandedRight && TryGetChoiceColumns(rx.TableX, out IList<ColumnX> columns))
        //    {
        //        anyChange |= AddProperyModels(prev, m, columns);
        //    }

        //    if (m.IsExpandedLeft)
        //    {
        //        anyChange |= AddChildModel(prev, m, IdKey.RowPropertyListModel, rx, Store_ColumnX, null, RowPropertyList_X);
        //        anyChange |= AddChildModel(prev, m, IdKey.RowComputeListModel, rx, Store_ComputeX, null, RowComputeList_X);
        //        anyChange |= AddChildModel(prev, m, IdKey.RowChildRelationListModel, rx, Store_ChildRelation, null, RowChildRelationList_X);
        //        anyChange |= AddChildModel(prev, m, IdKey.RowParentRelationListModel, rx, Store_ParentRelation, null, RowParentRelationList_X);
        //    }
        //    if (prev.Count != m.ChildModelCount) anyChange = true;
        //    return (true, anyChange);
        //}

        //private DropAction ReorderStoreItem(IModel m, IModel d, bool doDrop)
        //{
        //    if (!(m.Item.Owner is Store sto)) return DropAction.None;
        //    if (!m.IsSiblingModel(d)) return DropAction.None;
            
        //    var item1 = d.Item;
        //    var item2 = m.Item;
        //    var index1 = sto.IndexOf(item1);
        //    var index2 = sto.IndexOf(item2);
        //    if (index1 < 0 || index2 < 0 || index1 == index2) return DropAction.None;

        //    if (doDrop)
        //    {
        //        ItemMoved(d.Item, index1, index2);
        //    }
        //    return DropAction.Move;
        //}
        //#endregion

        //#region 6A3 View  =====================================================
        //ModelAction View_X;
        //void Initer_View_X()
        //{
        //    View_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.ViewX.Summary,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, m.ViewX.Name);
        //}
        //#endregion

        //#region 6A4 TableX  ===================================================
        //ModelAction Table_X;
        //void Initer_Table_X()
        //{
        //    Table_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.TableX.Count;

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.TableX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderStoreItem,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.InsertCommand, Insert));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var tx = m.TableX;
        //            if (tx.Count == 0) return (false, false);
        //            if (tx.ChildDelta == m.ChildDelta) return (true, false);

        //            m.ChildDelta = tx.ChildDelta;
        //            m.InitChildModels(prev, tx.Count);

        //            var items = tx.Items;
        //            var anyChange = (items.Count != prev.Count);
        //            if (Store_NameProperty.TryGetChild(tx, out Property cx))
        //            {
        //                foreach (var rx in items)
        //                {
        //                    anyChange |= AddChildModel(prev, m, IdKey.RowModel, rx, tx, cx, RowX_X);
        //                }
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, m.TableX.Name);

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void Insert(IModel m) => ItemCreated(new RowX(m.TableX, true));
        //}
        //#endregion

        //#region 6A5 Graph  ====================================================
        //ModelAction Graph_X;
        //void Initer_Graph_X()
        //{
        //    Graph_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.Graph.GraphX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.ViewCommand, CreateSecondaryModelGraph));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (m.ChildModelCount == 5) return (true, false);

        //            var g = m.Graph;
        //            m.InitChildModels(prev);

        //            AddChildModel(prev, m, IdKey.GraphNodeListModel, g, null, null, GraphNodeList_X);
        //            AddChildModel(prev, m, IdKey.GraphEdgeListModel, g, null, null, GraphEdgeList_X);
        //            AddChildModel(prev, m, IdKey.GraphOpenListModel, g, null, null, GraphOpenList_X);
        //            AddChildModel(prev, m, IdKey.GraphRootListModel, g, null, null, GraphRootList_X);
        //            AddChildModel(prev, m, IdKey.GraphLevelListModel, g, null, null, GraphLevelList_X);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), m.Graph.GraphX.Name);

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void CreateSecondaryModelGraph(IModel m)
        //    {
        //        var g = m.Graph;
        //        RegisterGraphInstance(g);

        //        m.GetRootModel().UIRequestCreatePage(ControlType.GraphDisplay, IdKey.GraphRefModel, g, GraphRef_X);
        //    }
        //}
        //#endregion

        //#region 6A6 GraphRef  =================================================
        //ModelAction GraphRef_X;
        //void Initer_GraphRef_X()
        //{
        //    GraphRef_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.Graph.GraphX.Summary,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), m.Graph.GraphX.Name);
        //}
        //#endregion

        //#region 6A7 RowChildRelation  =========================================
        //ModelAction RowChildRelation_X;
        //void Initer_RowChildRelation_X()
        //{
        //    RowChildRelation_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Relation.ChildCount(m.Item);

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.RelationX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!d.Item.IsRowX) return DropAction.None;
        //            if (!m.Item.IsRowX) return DropAction.None;
        //            if (!m.Aux1.IsRelationX) return DropAction.None;
        //            if (!Store_ParentRelation.TryGetParent(m.Aux1, out Store expectedOwner)) return DropAction.None;
        //            if (d.Item.Owner != expectedOwner) return DropAction.None;

        //            if (doDrop)
        //            {
        //                var rel = m.Aux1 as RelationXO;
        //                if (m.IsChildModel(d))
        //                    RemoveLink(rel, m.Item, d.Item);
        //                else
        //                    AppendLink(rel, m.Item, d.Item);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var rx = m.RowX;
        //            var re = m.RelationX;
        //            if (!re.TryGetChildren(rx, out IList<RowX> list)) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != list.Count;
        //            foreach (var rr in list)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.RowRelatedChildModel, rr, re, rx, RowRelatedChild_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), GetRelationName(m.RelationX));
        //}
        //#endregion

        //#region 6A8 RowParentRelation  ========================================
        //ModelAction RowParentRelation_X;
        //void Initer_RowParentRelation_X()
        //{
        //    RowParentRelation_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Relation.ParentCount(m.Item);

        //            m.CanExpandLeft = (count > 0);
        //            m.CanFilter = (count > 2);
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            if (!d.Item.IsRowX) return DropAction.None;
        //            if (!m.Item.IsRowX) return DropAction.None;
        //            if (!m.Aux1.IsRelationX) return DropAction.None;
        //            if (!Store_ChildRelation.TryGetParent(m.Aux1, out Store expectedOwner)) return DropAction.None;
        //            if (d.Item.Owner != expectedOwner) return DropAction.None;

        //            if (doDrop)
        //            {
        //                var rel = m.Aux1 as RelationXO;
        //                if (m.IsChildModel(d))
        //                    RemoveLink(rel, d.Item, m.Item);
        //                else
        //                    AppendLink(rel, d.Item, m.Item);
        //            }
        //            return DropAction.Link;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var rx = m.RowX;
        //            var re = m.RelationX;
        //            if (!re.TryGetParents(rx, out IList<RowX> list)) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != list.Count;
        //            foreach (var rr in list)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.RowRelatedParentModel, rr, re, rx, RowRelatedParent_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), GetRelationName(m.RelationX));
        //}
        //#endregion

        //#region 6A9 RowRelatedChild  ==========================================
        //ModelAction RowRelatedChild_X;
        //void Initer_RowRelatedChild_X()
        //{
        //    RowRelatedChild_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = true;

        //            return (kind, name, m.RelationX.ChildCount(m.RowX), ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => RowSummary(m.RowX),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = ReorderRelatedChild,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = (m, mc) =>
        //        {
        //            mc.Add(new ModelCommand(this, m, IdKey.RemoveCommand, UnlinkRelatedChild));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = RowX_VX,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (m.RowX.TableX.Name, GetRowName(m.RowX));
        //}

        ////= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //void UnlinkRelatedChild(IModel m)
        //{
        //    var key = m.Aux2;
        //    var rel = m.Aux1 as Relation;
        //    var item = m.Item;
        //    RemoveLink(rel, key, item);
        //}

        ////= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //DropAction ReorderRelatedChild (IModel model, IModel drop, bool doDrop)
        //{
        //    if (model.Aux2 == null) return DropAction.None;
        //    if (model.Aux1 == null || !(model.Aux1 is Relation rel)) return DropAction.None;

        //    var key = model.Aux2;
        //    var item1 = drop.Item;
        //    var item2 = model.Item;
        //    (int index1, int index2) = rel.GetChildrenIndex(key, item1, item2);

        //    if (index1 < 0 || index2 < 0 || index1 == index2) return DropAction.None;

        //    if (doDrop) ItemChildMoved(rel, key, item1, index1, index2);

        //    return DropAction.Move;
        //}
        //#endregion

        //#region 6AA RowRelatedParent  =========================================
        //ModelAction RowRelatedParent_X;
        //void Initer_RowRelatedParent_X()
        //{
        //    RowRelatedParent_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Relation.ParentCount(m.RowX);

        //            m.CanDrag = true;
        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => RowSummary(m.RowX),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ReorderItems = (m, d, doDrop) =>
        //        {
        //            if (m.Aux2 == null) return DropAction.None;
        //            if (m.Aux1 == null || !(m.Aux1 is Relation rel)) return DropAction.None;

        //            var key = m.Aux2;
        //            var item1 = d.Item;
        //            var item2 = m.Item;
        //            (int index1, int index2) = rel.GetParentsIndex(key, item1, item2);

        //            if (index1 < 0 || index2 < 0 || index1 == index2) return DropAction.None;

        //            if (doDrop) ItemParentMoved(rel, key, item1, index1, index2);

        //            return DropAction.Move;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = (m, mc) =>
        //        {
        //            mc.Add(new ModelCommand(this, m, IdKey.RemoveCommand, UnlinkRelatedParent));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = RowX_VX,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (m.RowX.TableX.Name, GetRowName(m.RowX));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void UnlinkRelatedParent(IModel m)
        //    {
        //        var key = m.Item;
        //        var rel = m.Aux1 as Relation;
        //        var item = m.Aux2;
        //        RemoveLink(rel, key, item);
        //    }
        //}
        //#endregion

        //#region 6AB EnumRelatedColumn  ========================================
        //ModelAction EnumRelatedColumn_X;
        //void Initer_EnumRelatedColumn_X()
        //{
        //    EnumRelatedColumn_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = (m, mc) =>
        //        {
        //            mc.Add(new ModelCommand(this, m, IdKey.RemoveCommand, UnlinkRelatedColumn));
        //        },
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), $"{m.TableX.Name}: {m.ColumnX.Name}");

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    void UnlinkRelatedColumn(IModel m)
        //    {
        //        var col = m.Item;
        //        var tbl = m.Aux1;
        //        var enu = m.Aux2;
        //        RemoveLink(EnumX_ColumnX, enu, col);
        //    }
        //}
        //#endregion



        //#region 6B1 RowPropertyList  ==========================================
        //ModelAction RowPropertyList_X;
        //void Initer_RowPropertyList_X()
        //{
        //    RowPropertyList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = Store_ColumnX.ChildCount(m.RowX.TableX);

        //            m.CanFilterUsage = count > 0;
        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelUsed = (m,cm) => cm.ColumnX.Value.IsSpecific(cm.RowX),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var rx = m.RowX;
        //            if (!Store_ColumnX.TryGetChildren(rx.TableX, out IList<ColumnX> list)) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = AddProperyModels(prev, m, list);

        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 6B2 RowChildRelationList  =====================================
        //ModelAction RowChildRelationList_X;
        //void Initer_RowChildRelationList_X()
        //{
        //    RowChildRelationList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = Store_ChildRelation.ChildCount(m.RowX.TableX);

        //            m.CanFilterUsage = count > 0;
        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =
                
        //        ModelUsed = (m,cm) => cm.RelationX.ChildCount(cm.RowX) > 0,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var rx = m.RowX;
        //            if (!Store_ChildRelation.TryGetChildren(rx.TableX, out IList<Relation> list)) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != list.Count;
        //            foreach (var rel in list)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.RowChildRelationModel, rx, rel, null, RowChildRelation_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 6B3 RowParentRelationList  ====================================
        //ModelAction RowParentRelationList_X;
        //void Initer_RowParentRelationList_X()
        //{
        //    RowParentRelationList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = Store_ParentRelation.ChildCount(m.RowX.TableX);

        //            m.CanFilterUsage = count > 0;
        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelUsed = (m, cm) => cm.RelationX.ParentCount(cm.RowX) > 0,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var rx = m.RowX;
        //            if (!Store_ParentRelation.TryGetChildren(rx.TableX, out IList<Relation> list)) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != list.Count;
        //            foreach (var re in list)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.RowParentRelationModel, rx, re, null, RowParentRelation_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 6B7 RowComputeList  ===========================================
        //ModelAction RowComputeList_X;
        //void Initer_RowComputeList_X()
        //{
        //    RowComputeList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = Store_ComputeX.ChildCount(m.Item.Owner);

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var itm = m.Item;
        //            if (!Store_ComputeX.TryGetChildren(itm.Owner, out IList<ComputeX> list)) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != list.Count;
        //            foreach (var cx in list)
        //            {
        //                anyChange = AddChildModel(prev, m, IdKey.TextPropertyModel, itm, cx, null, TextCompute_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion



        //#region 6C1 QueryRootLink  ============================================
        //ModelAction QueryRootLink_X;
        //void Initer_QueryRootLink_X()
        //{
        //    QueryRootLink_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.ItemCount;

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var q = m.Query;
        //            var list = q.Items;
        //            if (list == null) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != list.Length;
        //            foreach (var itm in list)
        //            {
        //                anyChange = AddChildModel(prev, m, IdKey.QueryRootItemModel, itm, q, null, QueryRootItem_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXFilterName(m.Query.QueryX));
        //}
        //private string QueryLinkName(IModel modle)
        //{
        //    var s = modle.Query;
        //    return QueryXFilterName(s.QueryX);
        //}
        //#endregion

        //#region 6C2 QueryPathHead  ============================================
        //ModelAction QueryPathHead_X;
        //void Initer_QueryPathHead_X()
        //{
        //    QueryPathHead_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.ItemCount;

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = QueryPathLink_VX,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXFilterName(m.Query.QueryX));
        //}
        //#endregion

        //#region 6C3 QueryPathLink  ============================================
        //ModelAction QueryPathLink_X;
        //void Initer_QueryPathLink_X()
        //{
        //    QueryPathLink_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.ItemCount;

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = QueryPathLink_VX,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXFilterName(m.Query.QueryX));
        //}
        //private (bool, bool) QueryPathLink_VX(IModel m, List<IModel> prev)
        //{
        //    var q = m.Query;
        //    var items = q.Items;
        //    if (items == null) return (false, false);

        //    m.InitChildModels(prev);

        //    var anyChange = false;
        //    if (q.IsTail)
        //        foreach (var itm in items)
        //        {
        //            anyChange |= AddChildModel(prev, m, IdKey.QueryPathTailModel, itm, q, null, QueryPathTail_X);
        //        }
        //    else
        //        foreach (var itm in items)
        //        {
        //            anyChange |= AddChildModel(prev, m, IdKey.QueryPathStepModel, itm, q, null, QueryPathStep_X);
        //        }
        //    return (true, anyChange);
        //}
        //#endregion

        //#region 6C4 QueryGroupHead  ===========================================
        //ModelAction QueryGroupHead_X;
        //void Initer_QueryGroupHead_X()
        //{
        //    QueryGroupHead_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.ItemCount;

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = QueryGroupLink_VX,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXFilterName(m.Query.QueryX));
        //}
        //#endregion

        //#region 6C5 QueryGroupLink  ===========================================
        //ModelAction QueryGroupLink_X;
        //void Initer_QueryGroupLink_X()
        //{
        //    QueryGroupLink_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.ItemCount;

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = QueryGroupLink_VX,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXFilterName(m.Query.QueryX));
        //}
        //private (bool, bool) QueryGroupLink_VX(IModel m, List<IModel> prev)
        //{
        //    var q = m.Query;
        //    var items = q.Items;
        //    if (items == null) return (false, false);

        //    m.InitChildModels(prev);

        //    var anyChange = prev.Count != items.Length;
        //    if (q.IsTail)
        //        foreach (var itm in items)
        //        {
        //            anyChange |= AddChildModel(prev, m, IdKey.QueryGroupTailModel, itm, q, null, QueryGroupTail_X);
        //        }
        //    else
        //        foreach (var itm in items)
        //        {
        //            anyChange |= AddChildModel(prev, m, IdKey.QueryGroupStepModel, itm, q, null, QueryGroupStep_X);
        //        }
        //    return (true, anyChange);
        //}
        //#endregion

        //#region 6C6 QueryEgressHead  ==========================================
        //ModelAction QueryEgressHead_X;
        //void Initer_QueryEgressHead_X()
        //{
        //    QueryEgressHead_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.ItemCount;

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = QueryEgressLink_VX,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXFilterName(m.Query.QueryX));
        //}
        //#endregion

        //#region 6C7 QueryEgressLink  ==========================================
        //ModelAction QueryEgressLink_X;
        //void Initer_QueryEgressLink_X()
        //{
        //    QueryEgressLink_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.ItemCount;

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = QueryEgressLink_VX,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), QueryXFilterName(m.Query.QueryX));
        //}
        //private (bool, bool) QueryEgressLink_VX(IModel m, List<IModel> prev)
        //{
        //    var q = m.Query;
        //    var items = q.Items;
        //    if (items == null) return (false, false);

        //    m.InitChildModels(prev);

        //    var anyChange = prev.Count != items.Length;
        //    if (q.IsTail)
        //        foreach (var itm in items)
        //        {
        //            anyChange |= AddChildModel(prev, m, IdKey.QueryEgressTailModel, itm, q, null, QueryEgressTail_X);
        //        }
        //    else
        //        foreach (var itm in items)
        //        {
        //            anyChange |= AddChildModel(prev, m, IdKey.QueryEgressStepModel, itm, q, null, QueryEgressStep_X);
        //        }
        //    return (true, anyChange);
        //}
        //#endregion


        //#region 6D1 QueryRootItem  ============================================
        //ModelAction QueryRootItem_X;
        //void Initer_QueryRootItem_X()
        //{
        //    QueryRootItem_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.QueryCount(m.Item);

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (!m.Query.TryGetQuerys(m.Item, out Query[] items)) return (false, false);

        //            m.InitChildModels(prev);

        //            var itm = m.Item;
        //            var anyChange = prev.Count != items.Length;
        //            foreach (var q in items)
        //            {
        //                if (q.IsGraphLink)
        //                    anyChange |= AddChildModel(prev, m, IdKey.QueryRootLinkModel, itm, q, null, QueryRootLink_X);
        //                else if (q.IsPathHead)
        //                    anyChange |= AddChildModel(prev, m, IdKey.QueryPathHeadModel, itm, q, null, QueryPathHead_X);
        //                else if (q.IsGroupHead)
        //                    anyChange |= AddChildModel(prev, m, IdKey.QueryGroupHeadModel, itm, q, null, QueryGroupHead_X);
        //                else if (q.IsSegueHead)
        //                    anyChange |= AddChildModel(prev, m, IdKey.QueryEgressHeadModel, itm, q, null, QueryEgressHead_X);
        //                else
        //                    throw new Exception("Invalid Query");
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (m.RowX.TableX.Name, GetRowName(m.RowX));
        //}
        //#endregion

        //#region 6D2 QueryPathStep  ============================================
        //ModelAction QueryPathStep_X;
        //void Initer_QueryPathStep_X()
        //{
        //    QueryPathStep_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.QueryCount(m.Item);

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (!m.Query.TryGetQuerys(m.Item, out Query[] items)) return (false, false);

        //            m.InitChildModels(prev);

        //            var itm = m.Item;
        //            var anyChange = prev.Count != items.Length;
        //            foreach (var q in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.QueryPathLinkModel, itm, q, null, QueryPathLink_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => ($"{_localize(m.KindKey)} {m.RowX.TableX.Name}", GetRowName(m.RowX));
        //}
        //#endregion

        //#region 6D3 QueryPathTail  ============================================
        //ModelAction QueryPathTail_X;
        //void Initer_QueryPathTail_X()
        //{
        //    QueryPathTail_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.QueryCount(m.Item);

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => ($"{_localize(m.KindKey)} {m.RowX.TableX.Name}", GetRowName(m.RowX));
        //}
        //#endregion

        //#region 6D4 QueryGroupStep  ===========================================
        //ModelAction QueryGroupStep_X;
        //void Initer_QueryGroupStep_X()
        //{
        //    QueryGroupStep_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.QueryCount(m.Item);

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (!m.Query.TryGetQuerys(m.Item, out Query[] items)) return (false, false);

        //            m.InitChildModels(prev);

        //            var itm = m.Item;
        //            var anyChange = prev.Count != items.Length;
        //            foreach (var q in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.QueryGroupLinkModel, itm, q, null, QueryGroupLink_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => ($"{_localize(m.KindKey)} {m.RowX.TableX.Name}", GetRowName(m.RowX));
        //}
        //#endregion

        //#region 6D5 QueryGroupTail  ===========================================
        //ModelAction QueryGroupTail_X;
        //void Initer_QueryGroupTail_X()
        //{
        //    QueryGroupTail_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.QueryCount(m.Item);

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => ($"{_localize(m.KindKey)} {m.RowX.TableX.Name}", GetRowName(m.RowX));
        //}
        //#endregion

        //#region 6D6 QueryEgressStep  ==========================================
        //ModelAction QueryEgressStep_X;
        //void Initer_QueryEgressStep_X()
        //{
        //    QueryEgressStep_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.QueryCount(m.Item);

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (!m.Query.TryGetQuerys(m.Item, out Query[] items)) return (false, false);

        //            m.InitChildModels(prev);

        //            var itm = m.Item;
        //            var anyChange = prev.Count != items.Length;
        //            foreach (var q in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.QueryEgressLinkModel, itm, q, null, QueryEgressLink_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => ($"{_localize(m.KindKey)} {m.RowX.TableX.Name}", GetRowName(m.RowX));
        //}
        //#endregion

        //#region 6D7 QueryEgressTail  ==========================================
        //ModelAction QueryEgressTail_X;
        //void Initer_QueryEgressTail_X()
        //{
        //    QueryEgressTail_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.QueryCount(m.Item);

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => ($"{_localize(m.KindKey)} {m.RowX.TableX.Name}", GetRowName(m.RowX));
        //}
        //#endregion



        //#region 6E1 GraphXRef  ================================================
        //ModelAction GraphXRef_X;
        //void Initer_GraphXRef_X()
        //{
        //    GraphXRef_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.GraphX.Count;

        //            m.CanExpandLeft = count > 0;

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => m.GraphX.Summary,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ButtonCommands = (m, bc) =>
        //        {
        //            bc.Add(new ModelCommand(this, m, IdKey.CreateCommand, CreateGraph));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDrop = (m, d, doDrop) =>
        //        {
        //            var gx = m.GraphX;
        //            Store st = null;

        //            if (GraphX_QueryX.ChildCount(gx) == 0) return DropAction.None;

        //            if (GraphX_QueryX.TryGetChildren(gx, out IList<QueryX> list))
        //            {
        //                foreach (var item in list)
        //                {
        //                    if (item.IsQueryGraphRoot && Store_QueryX.TryGetParent(item, out st) && d.Item.Owner == st) break;
        //                }
        //                if (st == null) return DropAction.None;
        //            }

        //            foreach (var tg in gx.Items)
        //            {
        //                if (tg.SeedItem == d.Item) return DropAction.None;
        //            }

        //            if (doDrop)
        //            {
        //                CreateGraph(gx, out Graph g, d.Item);
        //                RegisterGraphInstance(g);

        //                m.IsExpandedLeft = true;

        //                var root = m.GetRootModel();
        //                root.UIRequestCreatePage(ControlType.GraphDisplay, IdKey.GraphRefModel, g, GraphRef_X);
        //            }
        //            return DropAction.Copy;
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var gx = m.GraphX;
        //            if (gx.Count == 0) return (false, false);
        //            if (gx.ChildDelta == m.ChildDelta) return (true, false);

        //            m.ChildDelta = gx.ChildDelta;
        //            m.InitChildModels(prev);

        //            var list = gx.Items;
        //            var anyChange = prev.Count != list.Count;
        //            foreach (var g in list)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.GraphModel, g, null, null, Graph_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (m.GraphX.ErrorId.ToString(), m.GraphX.Name);
        //}

        //void CreateGraph(IModel m)
        //{
        //    CreateGraph(m.GraphX, out Graph g);
        //    RegisterGraphInstance(g);

        //    m.IsExpandedLeft = true;
        //    var root = m.GetRootModel();
        //    root.UIRequestCreatePage(ControlType.GraphDisplay, IdKey.GraphRefModel, g, GraphRef_X);
        //}
        //#endregion

        //#region 6E2 GraphNodeList  ============================================
        //ModelAction GraphNodeList_X;
        //void Initer_GraphNodeList_X()
        //{
        //    GraphNodeList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Graph.NodeCount;

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var g = m.Graph;
        //            var items = g.Nodes;
        //            if (items.Count == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != items.Count;
        //            foreach (var itm in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.GraphNodeModel, itm, null, null, GraphNode_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 6E3 GraphEdgeList  ============================================
        //ModelAction GraphEdgeList_X;
        //void Initer_GraphEdgeList_X()
        //{
        //    GraphEdgeList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Graph.EdgeCount;

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var g = m.Graph;
        //            var items = g.Edges;
        //            if (items.Count == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != items.Count;
        //            foreach (var itm in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.GraphEdgeModel, itm, null, null, GraphEdge_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 6E4 GraphRootList  ============================================
        //ModelAction GraphRootList_X;
        //void Initer_GraphRootList_X()
        //{
        //    GraphRootList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Graph.QueryCount;

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var g = m.Item as Graph;
        //            var items = g.Forest;
        //            if (items == null) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != items.Length;
        //            foreach (var q in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.GraphRootModel, q.Item, q, null, GraphRoot_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 6E5 GraphLevelList  ===========================================
        //ModelAction GraphLevelList_X;
        //void Initer_GraphLevelList_X()
        //{
        //    GraphLevelList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Graph.Levels.Count;

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var items = m.Graph.Levels;
        //            if (items.Count == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != items.Count;
        //            foreach (var lv in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.GraphLevelModel, lv, null, null, GraphLevel_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 6E6 GraphLevel  ===============================================
        //ModelAction GraphLevel_X;
        //void Initer_GraphLevel_X()
        //{
        //    GraphLevel_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Level.Count;

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var paths = m.Level.Paths;
        //            if (paths.Count == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != paths.Count;
        //            foreach (var p in paths)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.GraphPathModel, p, null, null, GraphPath_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), m.Level.Name);
        //}
        //#endregion

        //#region 6E7 GraphPath  ================================================
        //ModelAction GraphPath_X;
        //void Initer_GraphPath_X()
        //{
        //    GraphPath_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Path.Count;

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (m.Path.Count == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var items = m.Path.Paths;
        //            var anyChange = prev.Count != items.Length;
        //            foreach (var itm in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.GraphPathModel, itm, null, null, GraphPath_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (GetPathKind(m.Path), GetPathName(m.Path));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    string GetPathName(Path path)
        //    {
        //        return GetHeadTailName(path.Head, path.Tail);
        //    }

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    string GetPathKind(Path path)
        //    {
        //        var name = _localize(path.NameKey);
        //        var kind = path.IsRadial ? _localize(GetKindKey(IdKey.RadialPath)) : _localize(GetKindKey(IdKey.LinkPath));
        //        return $"{name}{kind}";
        //    }
        //}

        ////= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //string GetHeadTailName(Item head, Item tail)
        //{
        //    var headName = GetIdentity(head, IdentityStyle.Double);
        //    var tailName = GetIdentity(tail, IdentityStyle.Double);
        //    return $"{headName} --> {tailName}";
        //}
        //#endregion

        //#region 6E8 GraphRoot  ================================================
        //ModelAction GraphRoot_X;
        //void Initer_GraphRoot_X()
        //{
        //    GraphRoot_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Query.ItemCount;

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var q = m.Query;
        //            if (!q.TryGetItems(out Item[] items)) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != items.Length;
        //            foreach (var itm in items)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.QueryRootItemModel, itm, q, null, QueryRootItem_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, m.TableX.Name);
        //}
        //#endregion

        //#region 6E9 GraphNode  ================================================
        //ModelAction GraphNode_X;
        //void Initer_GraphNode_X()
        //{
        //    GraphNode_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Node.Graph.Node_Edges.TryGetValue(m.Node, out List<Edge> edges) ? edges.Count : 0;

        //            m.CanExpandRight = true;
        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            m.InitChildModels(prev);

        //            var anyChange = false;
        //            if (m.IsExpandedRight)
        //            {
        //                anyChange |= AddProperyModel(prev, m, GraphParams.CenterXYProperty);
        //                anyChange |= AddProperyModel(prev, m, GraphParams.SizeWHProperty);
        //                anyChange |= AddProperyModel(prev, m, GraphParams.OrientationProperty);
        //                anyChange |= AddProperyModel(prev, m, GraphParams.LabelingProperty);
        //                anyChange |= AddProperyModel(prev, m, GraphParams.ResizingProperty);
        //                anyChange |= AddProperyModel(prev, m, GraphParams.BarWidthProperty);
        //            }
                    
        //            if (m.IsExpandedLeft)
        //            {
        //                if (m.Node.Graph.Node_Edges.TryGetValue(m.Node, out List<Edge> edges))
        //                {
        //                    foreach (var e in edges)
        //                    {
        //                        anyChange |= AddChildModel(prev, m, IdKey.GraphEdgeModel, e, null, null, GraphEdge_X);
        //                    }
        //                }
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), GetIdentity(m.Node.Item, IdentityStyle.Double));
        //}
        //#endregion

        //#region 6EA GraphEdge  ================================================
        //ModelAction GraphEdge_X;
        //void Initer_GraphEdge_X()
        //{
        //    GraphEdge_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandRight = true;
                    
        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (m.ChildModelCount == 6) return (true, false);

        //            m.InitChildModels(prev);

        //            AddProperyModel(prev, m, GraphParams.Facet1Property);
        //            AddProperyModel(prev, m, GraphParams.Facet2Property);

        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), GetHeadTailName(m.Edge.Node1.Item, m.Edge.Node2.Item));
        //}
        //#endregion

        //#region 6EB GraphOpenList  ============================================
        //ModelAction GraphOpenList_X;
        //void Initer_GraphOpenList_X()
        //{
        //    GraphOpenList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = m.Graph.OpenQuerys.Count;

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var g = m.Graph;
        //            var list = g.OpenQuerys;
        //            if (list.Count == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != list.Count;
        //            foreach (var (q1, q2) in list)
        //            {
        //                var h = q1;
        //                var t = q2;
        //                anyChange |= AddChildModel(prev, m, IdKey.GraphOpenModel, g, h, t, GraphOpen_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 6EC GraphOpen  ================================================
        //ModelAction GraphOpen_X;
        //void Initer_GraphOpen_X()
        //{
        //    GraphOpen_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,
        //    };
        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string Kind, string Name) GetKindName(IModel m)
        //    {
        //        GetHeadTail(m.Query.QueryX, out Store head, out Store tail);
        //        var name = $"{GetIdentity(m.Item, IdentityStyle.Double)}  -->  {GetIdentity(tail, IdentityStyle.Single)}: <?>";

        //        return (GetIdentity(m.Item, IdentityStyle.Double), name);
        //    }
        //}
        //#endregion



        //#region 7D0 PrimeCompute  =============================================
        //ModelAction PrimeCompute_X;
        //void Initer_PrimeCompute_X()
        //{
        //    PrimeCompute_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var count = 0;
        //            foreach (var sto in PrimeStores) { if (Store_ComputeX.HasChildLink(sto)) count += 1; }

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(GetSummaryKey(IdKey.PrimeComputeModel)),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDescription = (m) => _localize(GetDescriptionKey(IdKey.PrimeComputeModel)),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            m.InitChildModels(prev);

        //            var anyChange = false;
        //            foreach (var st in PrimeStores)
        //            {
        //                if (Store_ComputeX.HasChildLink(st))
        //                {
        //                    anyChange |= AddChildModel(prev, m, IdKey.ComputeStoreModel, st, null, null, ComputeStore_X);
        //                }
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(GetNameKey(IdKey.PrimeComputeModel)));
        //}
        //#endregion

        //#region 7D1 ComputeStore  =============================================
        //ModelAction ComputeStore_X;
        //void Initer_ComputeStore_X()
        //{
        //    ComputeStore_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var st = m.Store;
        //            var (kind, name) = GetKindName(m);
        //            var count = Store_ComputeX.ChildCount(st);

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetIdentity(m.Store, IdentityStyle.Summary),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var st = m.Store;
        //            if (!Store_ComputeX.TryGetChildren(st, out IList<ComputeX> list)) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != list.Count;
        //            foreach (var cx in list)
        //            {
        //                anyChange |= AddChildModel(prev, m,  IdKey.TextPropertyModel, st, cx, null, TextCompute_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, GetIdentity(m.Store, IdentityStyle.Single));
        //}
        //#endregion



        //#region 7F0 InternalStoreList  ========================================
        //ModelAction InternalStoreList_X;
        //void Initer_InternalStoreList_X()
        //{
        //    InternalStoreList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = true;

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(GetSummaryKey(IdKey.InternalStoreListModel)),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =


        //        ModelDescription = (m) => _localize(GetDescriptionKey(IdKey.InternalStoreListModel)),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (m.ChildModelCount == 11) return (true, false);

        //            m.InitChildModels(prev);

        //            AddChildModel(prev, m, IdKey.InternalStoreModel, ViewXDomain, null, null, InternalStore_X);
        //            AddChildModel(prev, m, IdKey.InternalStoreModel, EnumXDomain, null, null, InternalStore_X);
        //            AddChildModel(prev, m,  IdKey.InternalStoreModel, TableXDomain, null, null, InternalStore_X);
        //            AddChildModel(prev, m, IdKey.InternalStoreModel, GraphXDomain, null, null, InternalStore_X);
        //            AddChildModel(prev, m, IdKey.InternalStoreModel, QueryXDomain, null, null, InternalStore_X);
        //            AddChildModel(prev, m, IdKey.InternalStoreModel, SymbolXDomain, null, null, InternalStore_X);
        //            AddChildModel(prev, m,  IdKey.InternalStoreModel, ColumnXDomain, null, null, InternalStore_X);
        //            AddChildModel(prev, m, IdKey.InternalStoreModel, RelationXDomain, null, null, InternalStore_X);
        //            AddChildModel(prev, m, IdKey.InternalStoreModel, ComputeXDomain, null, null, InternalStore_X);
        //            AddChildModel(prev, m, IdKey.InternalStoreModel, RelationDomain, null, null, InternalStore_X);
        //            AddChildModel(prev, m, IdKey.InternalStoreModel, PropertyDomain, null, null, InternalStore_X);
                    
        //            return (true, true);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(GetNameKey(IdKey.InternalStoreListModel)));
        //}
        //#endregion

        //#region 7F1 InternalStore  ============================================
        //ModelAction InternalStore_X;
        //void Initer_InternalStore_X()
        //{
        //    InternalStore_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var st = m.Store;
        //            var (kind, name) = GetKindName(m);
        //            var count = st.Count;

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanDrag = true;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var st = m.Store;
        //            if (st.Count == 0) return (false, false);
        //            if (st.ChildDelta == m.ChildDelta) return (true, false);

        //            m.ChildDelta = st.ChildDelta;
        //            m.InitChildModels(prev);

        //            var list = st.GetItems();
        //            var anyChange = prev.Count != list.Count;
        //            foreach (var item in list)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.StoreItemModel, item, null, null, StoreItem_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.Store.NameKey));
        //}
        //#endregion

        //#region 7F2 StoreItem  ================================================
        //ModelAction StoreItem_X;
        //void Initer_StoreItem_X()
        //{
        //    StoreItem_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var item = m.Item;
        //            var (hasItems, hasLinks, hasChildRels, hasParentRels, count) = GetItemParms(item);
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanDrag = true;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetIdentity(m.Item, IdentityStyle.Summary),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDescription = (m) => GetIdentity(m.Item, IdentityStyle.Description),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = (m, mc) =>
        //        {
        //            if (m.Item.IsExternal) mc.Add(new ModelCommand(this, m, IdKey.RemoveCommand, RemoveItem));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var item = m.Item;
        //            var (hasItems, hasLinks, hasChildRels, hasParentRels, count) = GetItemParms(item);
        //            if (count == 0) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = false;
        //            if (hasItems)
        //                anyChange |= AddChildModel(prev, m, IdKey.StoreItemItemListModel, item, null, null, StoreItemItemList_X);
        //            if (hasLinks)
        //                anyChange |= AddChildModel(prev, m, IdKey.StoreRelationLinkListModel, item, null, null, StoreRelationLinkList_X);
        //            if (hasChildRels)
        //                anyChange |= AddChildModel(prev, m, IdKey.StoreChildRelationListModel, item, null, null, StoreChildRelationList_X);
        //            if (hasParentRels)
        //                anyChange |= AddChildModel(prev, m, IdKey.StoreParentRelationListModel, item, null, null, StoreParentRelationList_X);

        //            return (true, anyChange);
        //        }
        //    };

        //    (string, string) GetKindName(IModel m) => (GetIdentity(m.Item, IdentityStyle.Kind), GetIdentity(m.Item, IdentityStyle.StoreItem));
        //}

        ////= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //(bool, bool, bool, bool, int) GetItemParms(Item item)
        //{
        //    var hasItems = (item is Store sto && sto.Count > 0);
        //    var hasLinks = (item is Relation rel && rel.GetLinksCount() > 0);
        //    var hasChildRels = false;
        //    var hasParentRels = false;

        //    var count = 0;
        //    if (hasItems) count++;
        //    if (hasLinks) count++;
        //    if (hasChildRels) count++;
        //    if (hasParentRels) count++;

        //    return (hasItems, hasLinks, hasChildRels, hasParentRels, count);
        //}
        //#endregion

        //#region 7F4 StoreItemItemList  ========================================
        //ModelAction StoreItemItemList_X;
        //void Initer_StoreItemItemList_X()
        //{
        //    StoreItemItemList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var count = m.Store.Count;
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var st = m.Store;
        //            if (st.Count == 0) return (false, false);

        //            m.InitChildModels(prev, st.Count);

        //            var list = st.GetItems();

        //            var anyChange = prev.Count != list.Count;
        //            foreach (var itm in list)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.StoreItemItemModel, itm, null, null, StoreItemItem_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) =>  (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 7F5 StoreRelationLinkList  ====================================
        //ModelAction StoreRelationLinkList_X;
        //void Initer_StoreRelationLinkList_X()
        //{
        //    StoreRelationLinkList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var count = m.Relation.GetLinksCount();
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var re = m.Relation;
        //            var N = re.GetLinks(out List<Item> parents, out List<Item> children);
        //            if (N == 0) return (false, false);

        //            m.InitChildModels(prev, N);

        //            var anyChange = prev.Count != N;
        //            for (int i = 0; i < N; i++)
        //            {
        //                var parent = parents[i];
        //                var child = children[i];
        //                anyChange = AddChildModel(prev, m, IdKey.StoreRelationLinkModel, re, parent, child, StoreRelationLink_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 7F6 StoreChildRelationList  ===================================
        //ModelAction StoreChildRelationList_X;
        //void Initer_StoreChildRelationList_X()
        //{
        //    StoreChildRelationList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var count = 0;
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            return (false, false);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 7F7 StoreParentRelationList  ==================================
        //ModelAction StoreParentRelationList_X;
        //void Initer_StoreParentRelationList_X()
        //{
        //    StoreParentRelationList_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var count = 0;
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => _localize(m.SummaryKey),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            return (false, false);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (null, _localize(m.NameKey));
        //}
        //#endregion

        //#region 7F8 StoreItemItem  ============================================
        //ModelAction StoreItemItem_X;
        //void Initer_StoreItemItem_X()
        //{
        //    StoreItemItem_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetIdentity(m.Item, IdentityStyle.Summary),
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.Item.KindKey), GetIdentity(m.Item, IdentityStyle.Double));
        //}
        //#endregion

        //#region 7F9 StoreRelationLink  ========================================
        //ModelAction StoreRelationLink_X;
        //void Initer_StoreRelationLink_X()
        //{
        //    StoreRelationLink_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);

        //            return (kind, name, 0, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetIdentity(m.Relation, IdentityStyle.Summary),
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (_localize(m.KindKey), $"({GetIdentity(m.Aux1, IdentityStyle.Double)}) --> ({GetIdentity(m.Aux2, IdentityStyle.Double)})");
        //}
        //#endregion

        //#region 7FA StoreChildRelation  =======================================
        //ModelAction StoreChildRelation_X;
        //void Initer_StoreChildRelation_X()
        //{
        //    StoreChildRelation_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var count = m.Relation.ChildCount(m.Aux1);
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetIdentity(m.Relation, IdentityStyle.Summary),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (!m.Relation.TryGetChildren(m.Aux1, out List<Item> list)) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != list.Count;
        //            foreach (var itm in list)
        //            {
        //                anyChange = AddChildModel(prev, m, IdKey.StoreRelatedItemModel, itm, null, null, StoreRelatedItem_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) =>  (GetKind(m.Relation.ErrorId), GetIdentity(m.Relation, IdentityStyle.Single));

        //}
        //#endregion

        //#region 7FA StoreParentRelation  ======================================
        //ModelAction StoreParentRelation_X;
        //void Initer_StoreParentRelation_X()
        //{
        //    StoreParentRelation_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var count = m.Relation.ParentCount(m.Aux1);
        //            var (kind, name) = GetKindName(m);

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetIdentity(m.Relation, IdentityStyle.Summary),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            if (!m.Relation.TryGetParents(m.Aux1, out List<Item> list)) return (false, false);

        //            m.InitChildModels(prev);

        //            var anyChange = prev.Count != list.Count;
        //            foreach (var itm in list)
        //            {
        //                anyChange = AddChildModel(prev, m, IdKey.StoreRelatedItemModel, itm, null, null, StoreRelatedItem_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (GetKind(m.Relation.ErrorId), GetIdentity(m.Relation, IdentityStyle.Single));
        //}
        //#endregion

        //#region 7FC StoreRelatedItem  =========================================
        //ModelAction StoreRelatedItem_X;
        //void Initer_StoreRelatedItem_X()
        //{
        //    StoreRelatedItem_X = new ModelAction
        //    {
        //        ModelParms = (m) =>
        //        {
        //            var (kind, name) = GetKindName(m);
        //            var (hasChildRels, hasParentRels, count) = GetItemParms(m.Item);

        //            m.CanExpandLeft = count > 0;
        //            m.CanFilter = count > 2;
        //            m.CanSort = (m.IsExpandedLeft && count > 1);

        //            return (kind, name, count, ModelType.Default);
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelKindName = GetKindName,

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelSummary = (m) => GetIdentity(m.Item, IdentityStyle.Summary),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        ModelDescription = (m) => GetIdentity(m.Item, IdentityStyle.Description),

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        MenuCommands = (m, mc) =>
        //        {
        //            if (m.Item.IsExternal) mc.Add(new ModelCommand(this, m, IdKey.RemoveCommand, RemoveItem));
        //        },

        //        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //        Validate = (m,prev) =>
        //        {
        //            var itm = m.Item;
        //            var (hasChildRels, hasParentRels, N) = GetItemParms(itm);
        //            if (N == 0) return (false, false);

        //            m.InitChildModels(prev);


        //            var anyChange = false;
        //            if (hasChildRels)
        //            {
        //                anyChange |= AddChildModel(prev, m,  IdKey.StoreChildRelationListModel, itm, null, null, StoreChildRelationList_X);
        //            }
        //            if (hasParentRels)
        //            {
        //                anyChange |= AddChildModel(prev, m, IdKey.StoreParentRelationListModel, itm, null, null, StoreParentRelationList_X);
        //            }
        //            return (true, anyChange);
        //        }
        //    };

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (string, string) GetKindName(IModel m) => (GetIdentity(m.Item, IdentityStyle.Kind), GetIdentity(m.Item, IdentityStyle.StoreItem));

        //    //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        //    (bool, bool, int) GetItemParms(Item item)
        //    {
        //        var hasChildRels = false;
        //        var hasParentRels = false;
        //        var count = 0;
        //        if (hasChildRels) count++;
        //        if (hasParentRels) count++;

        //        return (hasChildRels, hasParentRels, count);
        //    }
        //}
        //#endregion

    }
}
