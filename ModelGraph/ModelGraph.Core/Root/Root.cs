using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public partial class Root : ChildOfStoreOf<Item, Item> // where the items are UI root models (treeModel, graphModel, symbolModel,..)
    {
        private readonly Dictionary<Type, Item> Type_InstanceOf = new Dictionary<Type, Item>(200);  // used to get a specific type instance
        private readonly Dictionary<ushort, Item> IdKey_ReferenceItem = new Dictionary<ushort, Item>(200); // used to get specific type from its IdKey
        private readonly Dictionary<Type, Property[]> Type_StaticProperties = new Dictionary<Type, Property[]>(100); // used for property name lookup

        private readonly Relation_Store_ChildRelation Internal_Store_ChildRelation;     // used to enforce relational integrity
        private readonly Relation_Store_ParentRelation Internal_Store_ParentRelation;   // used to enforce relational integrity

        private readonly List<(Guid, ISerializer)> ItemSerializers = new List<(Guid, ISerializer)>(20); //serialized first
        private readonly List<(Guid, ISerializer)> LinkSerializers = new List<(Guid, ISerializer)>(10); //serialized last

        public IRepository Repository { get; set; }
        public static LineModel DragDropSource;
        internal override IdKey IdKey => IdKey.DataRoot;
        internal string TitleName => Repository.Name;
        internal string TitleSummary => Repository.FullName;

        #region Constructor  ==================================================
        internal Root(bool createTestModel = false)
        {
            ModelDelta = ChildDelta = 1;

            Internal_Store_ChildRelation = new Relation_Store_ChildRelation(this);
            Internal_Store_ParentRelation = new Relation_Store_ParentRelation(this);

            Initialize(); 

            if (createTestModel) CreateTestModel();
        }
        #endregion

        #region Identity  =====================================================
        private Func<string, string> _localize = (s) => s; //dummy default localizer
        public void SetLocalizer(Func<string, string> localizer) => _localize = localizer;

        internal string GetKindId(IdKey idKe) => _localize($"{(int)(idKe & IdKey.KeyMask):X3}K");
        internal string GetNameId(IdKey idKe) => _localize($"{(int)(idKe & IdKey.KeyMask):X3}N");
        internal string GetDoubleNameId(IdKey idKe1, IdKey idKe2) => $"{GetNameId(idKe1)} : {GetNameId(idKe2)}";
        internal string GetSummaryId(IdKey idKe) => _localize($"{(int)(idKe & IdKey.KeyMask):X3}S");
        internal string GetDescriptionId(IdKey idKe) => _localize($"{(int)(idKe & IdKey.KeyMask):X3}V");
        internal string GetAcceleratorId(IdKey idKe) => _localize($"{(int)(idKe & IdKey.KeyMask):X3}A".ToUpper());
        internal (string, string) GetKindNameId(IdKey idKe) => (GetKindId(idKe), GetNameId(idKe));
        #endregion

        #region Initialize  ===================================================
        private void Initialize()
        {
            RegisterReferenceItem(new DummyItem(this));
            RegisterReferenceItem(Internal_Store_ChildRelation);
            RegisterReferenceItem(Internal_Store_ParentRelation);

            RegisterReferenceItem(new RelationRoot(this));
            RegisterReferenceItem(new PropertyRoot(this));
            RegisterPrivateItem(new ChangeRoot(this));
            RegisterPrivateItem(new ErrorRoot(this));
            RegisterPrivateItem(new EnumRoot(this));

            RegisterReferenceItem(new EnumXRoot(this));
            RegisterReferenceItem(new ViewXRoot(this));
            RegisterReferenceItem(new TableXRoot(this));
            RegisterReferenceItem(new GraphXRoot(this));

            RegisterReferenceItem(new QueryXRoot(this));
            RegisterReferenceItem(new ColumnXRoot(this));
            RegisterReferenceItem(new SymbolXRoot(this));
            RegisterReferenceItem(new ComputeXRoot(this));
            RegisterReferenceItem(new RelationXRoot(this));

            var instanceList = Type_InstanceOf.Values.ToArray(); // instance of things that I just created and regestered
            foreach (var item in instanceList)
            {
                if (item is IPrimeRoot pr) pr.CreateSecondaryHierarchy(this);
            }

            foreach (var item in instanceList)
            {
                if (item is IPrimeRoot pr) pr.RegisterRelationalReferences(this);
            }
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

        internal void RegisterStaticProperties(Type type, Property[] props) => Type_StaticProperties.Add(type, props);

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
            if (Type_StaticProperties.TryGetValue(store.GetChildType(), out Property[] arr))
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
            Get<EnumXRoot>(),
            Get<ViewXRoot>(),
            Get<TableXRoot>(),
            Get<GraphXRoot>(),
            Get<QueryXRoot>(),
            Get<ColumnXRoot>(),
            Get<SymbolXRoot>(),
            Get<ComputeXRoot>(),
            Get<RelationXRoot>(),
            Get<RelationRoot>(),
            Get<PropertyRoot>(),
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
            Type_StaticProperties.Clear();
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
