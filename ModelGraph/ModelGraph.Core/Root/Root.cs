using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public partial class Root : ChildOfStoreOf<Item, Item> // where the items are UI root models (treeModel, graphModel, symbolModel,..)
    {
        private readonly Dictionary<Type, Item> Type_InstanceOf = new Dictionary<Type, Item>(200);  // used to get a specific type instance
        private readonly Dictionary<ushort, Item> IdKey_ReferenceItem = new Dictionary<ushort, Item>(200); // used to get specific type from its IdKey
        private readonly Dictionary<Type, Property[]> Type_InternalProperties = new Dictionary<Type, Property[]>(100); // used for property name lookup

        private readonly Relation_Store_ChildRelation Internal_Store_ChildRelation;     // used to enforce relational integrity
        private readonly Relation_Store_ParentRelation Internal_Store_ParentRelation;   // used to enforce relational integrity

        private readonly List<(Guid, ISerializer)> ItemSerializers = new List<(Guid, ISerializer)>(20); //serialized first
        private readonly List<(Guid, ISerializer)> LinkSerializers = new List<(Guid, ISerializer)>(10); //serialized last
        private readonly List<IManager> PrimeRootManagers = new List<IManager>();

        public IRepository Repository { get; set; }
        public static LineModel DragDropSource; // source model at time of DragStart
        internal override IdKey IdKey => IdKey.DataRoot;
        internal string TitleName => Repository.Name;
        internal string TitleSummary => Repository.FullName;

        #region Constructor  ==================================================
        internal Root(bool createTestModel = false)
        {
            ModelDelta = ChildDelta = 1;

            Internal_Store_ChildRelation = new Relation_Store_ChildRelation(this);
            Internal_Store_ParentRelation = new Relation_Store_ParentRelation(this);

            RegisterPrivateItem(Internal_Store_ChildRelation);
            RegisterPrivateItem(Internal_Store_ParentRelation);

            Initialize(); 

            if (createTestModel) CreateTestModel();
        }
        #endregion

        #region Identity  =====================================================
        private static Func<string, string> _localize = (s) => s; //dummy default localizer
        public static void SetLocalizer(Func<string, string> localizer) => _localize = localizer;

        public override string GetNameId() => TitleName;
        public override string GetSummaryId() => TitleSummary;

        internal static string GetKindId(IdKey idKe) => _localize($"{(int)(idKe & IdKey.KeyMask):X3}K");
        internal static string GetNameId(IdKey idKe) => _localize($"{(int)(idKe & IdKey.KeyMask):X3}N");
        internal static string GetDoubleNameId(IdKey idKe1, IdKey idKe2) => $"{GetNameId(idKe1)} : {GetNameId(idKe2)}";
        internal static string GetSummaryId(IdKey idKe) => _localize($"{(int)(idKe & IdKey.KeyMask):X3}S");
        internal static string GetDescriptionId(IdKey idKe) => _localize($"{(int)(idKe & IdKey.KeyMask):X3}V");
        internal static string GetAcceleratorId(IdKey idKe) => _localize($"{(int)(idKe & IdKey.KeyMask):X3}A".ToUpper());
        #endregion

        #region Initialize  ===================================================
        private void Initialize()
        {
            RegisterReferenceItem(new DummyItem(this));

            RegisterReferenceItem(new RelationManager(this));
            RegisterReferenceItem(new PropertyManager(this));
            RegisterPrivateItem(new ChangeManager(this));
            RegisterPrivateItem(new ErrorManager(this));
            RegisterPrivateItem(new EnumManager(this));

            RegisterReferenceItem(new EnumXManager(this));
            RegisterReferenceItem(new ViewXManager(this));
            RegisterReferenceItem(new TableXManager(this));
            RegisterReferenceItem(new GraphXManager(this));

            RegisterReferenceItem(new QueryXManager(this));
            RegisterReferenceItem(new ColumnXManager(this));
            RegisterReferenceItem(new SymbolXManager(this));
            RegisterReferenceItem(new ComputeXManager(this));
            RegisterReferenceItem(new RelationXManager(this));

            foreach (var item in Type_InstanceOf.Values) { if (item is IManager r) PrimeRootManagers.Add(r); }

            foreach (var pr in PrimeRootManagers) { pr.CreateSecondaryHierarchy(this); }

            foreach (var pr in PrimeRootManagers) { pr.RegisterRelationalReferences(this); }
        }
        #endregion

        #region RegisteredItems  ==============================================
        internal T Get<T>() where T : Item
        {
            if (Type_InstanceOf.TryGetValue(typeof(T), out Item itm) && itm is T val)
                return val;
            throw new InvalidOperationException($"Chef GetInstanceOf<T>() : could not find type {typeof(T)}");
        }

        public void RegisterItemSerializer((Guid, ISerializer) serializer)
        {
            if (ItemSerializers.Count == 0)
                ItemSerializers.Add((_serilizerGuid, this)); //the internal reference serializer should be first

            ItemSerializers.Add(serializer); //item serializers added according to registration order
        }

        public void RegisterLinkSerializer((Guid, ISerializer) serializer) => LinkSerializers.Add(serializer); //link serializers will be called last

        internal void RegisterInternalProperties(Type type, Property[] props) => Type_InternalProperties.Add(type, props);

        internal void RegisterPrivateItem(Item item) => Type_InstanceOf[item.GetType()] = item;

        internal void RegisterReferenceItem(Item item) { IdKey_ReferenceItem[item.ItemKey] = item; Type_InstanceOf[item.GetType()] = item; }

        internal void RegisterChildRelation(Store sto, Relation rel)
        {
            Internal_Store_ChildRelation.SetLink(sto, rel);
        }
        internal void RegisterParentRelation(Store sto, Relation rel)
        {
            Internal_Store_ParentRelation.SetLink(sto, rel);
        }
        #endregion

        #region LookUpProperty  ===============================================
        internal bool TryLookUpProperty(Store store, string name, out Property prop)
        {
            prop = null;

            if (string.IsNullOrWhiteSpace(name)) return false;

            if (Get<Relation_Store_ColumnX>().TryGetChildren(store, out IList<ColumnX> ls1))
            {
                foreach (var col in ls1)
                {
                    if (string.IsNullOrWhiteSpace(col.Name)) continue;
                    if (string.Compare(col.Name, name, true) == 0) { prop = col; return true; }
                }
            }
            if (Get<Relation_Store_ComputeX>().TryGetChildren(store, out IList<ComputeX> ls2))
            {
                foreach (var cd in ls2)
                {
                    var n = cd.Name;
                    if (string.IsNullOrWhiteSpace(n)) continue;
                    if (string.Compare(n, name, true) == 0) { prop = cd; return true; }
                }
            }
            if (Type_InternalProperties.TryGetValue(store.GetChildType(), out Property[] arr))
            {
                foreach (var pr in arr)
                {
                    if (string.Compare(name, _localize(pr.NameKey), true) == 0) { prop = pr; return true; }
                }
            }
            return false;
        }
        #endregion

        #region PrimeStores  ==================================================
        internal Store[] PrimeStores => new Store[]
        {
            Get<EnumXManager>(),
            Get<ViewXManager>(),
            Get<TableXManager>(),
            Get<GraphXManager>(),
            Get<QueryXManager>(),
            Get<ColumnXManager>(),
            Get<SymbolXManager>(),
            Get<ComputeXManager>(),
            Get<RelationXManager>(),
            Get<RelationManager>(),
            Get<PropertyManager>(),
        };
        #endregion

        #region Override Discard()  ===========================================
        /// <summary>Remove references that were created by this dataChef</summary>
        internal override void Discard()
        {
            IsDiscarded = true;
            foreach (var item in Type_InstanceOf.Values)
            {
                item.Discard();
            }
            Type_InternalProperties.Clear();
            Type_InstanceOf.Clear();
            IdKey_ReferenceItem.Clear();
            ItemSerializers.Clear();
            LinkSerializers.Clear();
            Internal_Store_ChildRelation.Discard();
            Internal_Store_ParentRelation.Discard();
    }
    #endregion
}
}
