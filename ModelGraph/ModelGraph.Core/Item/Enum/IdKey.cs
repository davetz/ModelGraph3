
namespace ModelGraph.Core
{
    /*
        Item		- X,R,Z
        Enum		- X,Z
        Event		- V
        Action		- A
        Store		- X,R,Z
        Model 		- X,R
        Group		- X,R
        Proptery	- X,R
        Relation	- X,R,Z

        instence    - Y
        modeling
        runtime
        external
        reference
        private

        X,Y,Z
        A,V,I,E,V,S,M,G,P,R
    */

    public enum IdKey : ushort
    {/*
        Provides identity for item, enum, pair, store, model, relation, property, and commands. 
        It also is used as a key to locate resource strings.
        
        Resource string keys are of the form:
        xxxK - the item's Kind 
        xxxN - the item's Name
        xxxS - the item's Summary (tooltip text)
        xxxV - the item's Description
        where xxx are the three hex digits enumerated in this file
     */
        #region Flags  ========================================================
        Empty = 0,
        Item = 1,
        //=====================================================================
        IsCovert = 0x8000, // Don't include this item in the model change log
        IsExternal = 0x4000, // This item is serialized/deserialize to/from a repository
        IsReference = 0x2000, // This item can be referenced by an external item                            

        KeyMask = 0xFFF,
        FlagMask = 0xF000,
        EnumMask = 0x3F,
        IndexMask = 0xF,

        #endregion

        #region Command  =============================================(020-07F)

        NewCommand = 0x21,
        SaveCommand = 0x23,
        ReloadCommand = 0x25,
        CloseCommand = 0x26,

        EditCommand = 0x30,
        ViewCommand = 0x31,
        UndoCommand = 0x32,
        RedoCommand = 0x33,
        MergeCommand = 0x34,
        InsertCommand = 0x35,
        RemoveCommand = 0x36,
        CreateCommand = 0x37,
        RefreshCommand = 0x38,
        ExpandAllCommand = 0x39,
        MakeRootLinkCommand = 0x3A,
        MakePathHeadCommand = 0x3B,
        MakeGroupHeadCommand = 0x3C,
        MakeEgressHeadCommand = 0x3D,
        #endregion

        #region Store ================================================(0E0-0FF)
        // root level containers for the hierarchal item trees

        EnumXRoot = 0x0E1 | IsReference,
        ViewXRoot = 0x0E2 | IsReference,
        TableXRoot = 0x0E3 | IsReference,
        GraphXRoot = 0x0E4 | IsReference,
        QueryXRoot = 0x0E5 | IsReference,
        ValueXRoot = 0x0E6 | IsReference,
        SymbolXRoot = 0x0E7 | IsReference,
        ColumnXRoot = 0x0E8 | IsReference,
        ComputeXRoot = 0x0E9 | IsReference,
        RelationXRoot = 0x0EA | IsReference,

        PrimeRoot = 0x0F0, // exposes internal tables (metadata / configuration)
        EnumZRoot = 0x0F1,
        ErrorRoot = 0x0F2,
        GroupRoot = 0x0F3,
        PropertyRoot = 0x0F4 | IsReference,
        RelationRoot = 0x0F5 | IsReference,
        GraphParams = 0xFF,

        #endregion

        #region Item  ================================================(100-1FF)

        //=========================================
        DummyItem = 0x100 | IsReference,
        DummyQueryX = 0x101 | IsReference,

        DataRoot = 0x112,

        //=========================================
        ChangeRoot = 0x131,
        ChangeSet = 0x132,
        ItemUpdated = 0x133,
        ItemCreated = 0x134,
        ItemRemoved = 0x135,
        ItemLinked = 0x136,
        ItemUnlinked = 0x137,
        ItemMoved = 0x138,
        ItemChildMoved = 0x139,
        ItemParentMoved = 0x13A,

        //=========================================
        // External (user-defined) item classes
        RowX = 0x141 | IsExternal,
        PairX = 0x142 | IsExternal,
        EnumX = 0x143 | IsExternal,
        ViewX = 0x144 | IsExternal,
        TableX = 0x145 | IsExternal,
        GraphX = 0x146 | IsExternal,
        QueryX = 0x147 | IsExternal,
        SymbolX = 0x148 | IsExternal,
        ColumnX = 0x149 | IsExternal,
        ComputeX = 0x14A | IsExternal,
        CommandX = 0x14B | IsExternal,
        RelationX = 0x14C | IsExternal,

        //=========================================
        // QueryX detail, used to lookup resource strings
        QueryIsCorrupt = 0x150,
        QueryGraphRoot = 0x151,
        QueryGraphLink = 0x152,
        QueryViewRoot = 0x153,
        QueryViewHead = 0x154,
        QueryViewLink = 0x155,
        QueryPathHead = 0x156,
        QueryPathLink = 0x157,
        QueryGroupHead = 0x158,
        QueryGroupLink = 0x159,
        QuerySegueHead = 0x15A,
        QuerySegueLink = 0x15B,
        QueryValueRoot = 0x15C,
        QueryValueHead = 0x15D,
        QueryValueLink = 0x15E,
        QueryNodeSymbol = 0x15F,

        //=========================================
        QueryWhere = 0x161, // used to lookup kind resource string "Where"
        QuerySelect = 0x162, // used to lookup kind resource string "Select"

        //=========================================

        //=========================================

        //=========================================

        //=========================================
        Graph = 0x1C1,
        Query = 0x1B0,
        Level = 0x1C2,
        Node = 0x1C3,
        Edge = 0x1C4,
        Open = 0x1C5,

        //=========================================
        QueryPath = 0x1E3,
        FlaredPath = 0x1E4,
        ForkedPath = 0x1E5,
        SeriesPath = 0x1E6,
        ParallelPath = 0x1E7,

        LinkPath = 0x1EE, // used to lookup kind resource string "_Link"
        RadialPath = 0x1EF, // used to lookup kind resource string "_Radial"

        #endregion

        #region Error  ===============================================(200-2FF)

        ExportError = 0x200,
        ImportError = 0x201,

        ComputeProblemRelatedWhereSelectError = 0x210,
        ComputeMissingRelatedSelectError = 0x211,
        ComputeUnresolvedSelectError = 0x212,
        ComputeInvalidSelectError = 0x213,
        ComputeCircularDependanceError = 0x214,
        ComputeMissingSelectError = 0x215,

        ComputeMissingRootQueryError = 0x216,
        ComputeValueOverflowError = 0x217,

        QueryUnresolvedWhereError = 0x220,
        QueryInvalidWhereError = 0x221,

        QueryUnresolvedSelectError = 0x222,
        QueryInvalidSelectError = 0x223,
        QueryMissingSelectError = 0x224,

        QueryValueOverflowdWhereError = 0x225,
        QueryValueOverflowSelectError = 0x226,

        #endregion

        #region Relation  ============================================(300-3FF)

        Relation = 0x300,
        //=========================================
        RowX_RowX = 0x301 | IsExternal,

        //=========================================
        EnumX_ColumnX = 0x311 | IsReference,
        Store_ColumnX = 0x312 | IsReference,
        Store_NameProperty = 0x313 | IsReference,
        Store_SummaryProperty = 0x314 | IsReference,
        StoreX_ChildRelation = 0x315 | IsReference,
        StoreX_ParentRelation = 0x316 | IsReference,

        //=========================================
        Item_Error = 0x331,
        ViewX_ViewX = 0x332 | IsReference,
        ViewX_QueryX = 0x333 | IsReference,
        QueryX_ViewX = 0x334 | IsReference,
        Property_ViewX = 0x335 | IsReference,
        Relation_ViewX = 0x336 | IsReference,
        ViewX_Property = 0x337 | IsReference,
        QueryX_Property = 0x338 | IsReference,

        //=========================================
        GraphX_SymbolX = 0x341 | IsReference,
        SymbolX_QueryX = 0x342 | IsReference,
        GraphX_QueryX = 0x343 | IsReference,
        QueryX_QueryX = 0x344 | IsReference,
        GraphX_ColorColumnX = 0x345 | IsReference,
        GraphX_SymbolQueryX = 0x346 | IsReference,

        //=========================================
        Store_QueryX = 0x351 | IsReference,
        Relation_QueryX = 0x352 | IsReference,

        //=========================================
        Store_ComputeX = 0x361 | IsReference,
        ComputeX_QueryX = 0x362 | IsReference,

        //=========================================
        Store_Property = 0x3FD | IsReference,
        Store_ChildRelation = 0x3FE | IsReference,
        Store_ParentRelation = 0x3FF | IsReference,

        #endregion

        #region Property  ============================================(400-5FF)

        Property = 0x400,

        //=========================================
        ItemNameProperty = 0x401 | IsReference, // works for all items
        ItemSummaryProperty = 0x402 | IsReference, // works for all items
        ItemDescriptionProperty = 0x403 | IsReference, // works for all items

        //=========================================
        IncludeItemIdentityIndexProperty = 0x404 | IsCovert,

        //=========================================
        EnumNameProperty = 0x411 | IsReference,
        EnumSummaryProperty = 0x412 | IsReference,
        EnumTextProperty = 0x413 | IsReference,
        EnumValueProperty = 0x414 | IsReference,

        //=========================================
        TableNameProperty = 0x421 | IsReference,
        TableSummaryProperty = 0x422 | IsReference,

        //=========================================
        ColumnNameProperty = 0x431 | IsReference,
        ColumnSummaryProperty = 0x432 | IsReference,
        ColumnValueTypeProperty = 0x433 | IsReference,
        ColumnIsChoiceProperty = 0x436 | IsReference,

        //=========================================
        RelationNameProperty = 0x441 | IsReference,
        RelationSummaryProperty = 0x442 | IsReference,
        RelationPairingProperty = 0x443 | IsReference,
        RelationIsRequiredProperty = 0x444 | IsReference,
        RelationIsReferenceProperty = 0x445 | IsReference,

        //=========================================
        GraphNameProperty = 0x451 | IsReference,
        GraphSummaryProperty = 0x452 | IsReference,
        GraphTerminalLengthProperty = 0x453 | IsReference,
        GraphTerminalSpacingProperty = 0x454 | IsReference,
        GraphTerminalStretchProperty = 0x455 | IsReference,
        GraphSymbolSizeProperty = 0x456 | IsReference,

        //=========================================
        QueryXSelectProperty = 0x460 | IsReference,
        QueryXWhereProperty = 0x461 | IsReference,

        QueryXConnect1Property = 0x462 | IsReference,
        QueryXConnect2Property = 0x463 | IsReference,

        QueryXRelationProperty = 0x466 | IsReference,
        QueryXIsReversedProperty = 0x467 | IsReference,
        QueryXIsImmediateProperty = 0x468 | IsReference,
        QueryXIsPersistentProperty = 0x469 | IsReference,
        QueryXIsBreakPointProperty = 0x46A | IsReference,
        QueryXExclusiveKeyProperty = 0x46B | IsReference,
        QueryXAllowSelfLoopProperty = 0x46C | IsReference,
        QueryXIsPathReversedProperty = 0x46D | IsReference,
        QueryXIsFullTableReadProperty = 0x46E | IsReference,
        QueryXFacet1Property = 0x46F | IsReference,
        QueryXFacet2Property = 0x470 | IsReference,
        ValueXWhereProperty = 0x471 | IsReference,
        ValueXSelectProperty = 0x472| IsReference,
        ValueXIsReversedProperty = 0x473 | IsReference,
        ValueXValueTypeProperty = 0x474 | IsReference,
        QueryXLineStyleProperty = 0x475 | IsReference,
        QueryXDashStyleProperty = 0x476 | IsReference,
        QueryXLineColorProperty = 0x477 | IsReference,

        //=========================================
        SymbolXNameProperty = 0x481 | IsReference,
        SymbolXAttatchProperty = 0x486 | IsReference,

        //=========================================
        NodeCenterXYProperty = 0x491 | IsCovert,
        NodeSizeWHProperty = 0x492 | IsCovert,
        NodeLabelingProperty = 0x493 | IsCovert,
        NodeResizingProperty = 0x494 | IsCovert,
        NodeBarWidthProperty = 0x495 | IsCovert,
        NodeOrientationProperty = 0x496 | IsCovert,
        NodeFlipRotateProperty = 0x497 | IsCovert,

        //=========================================
        EdgeFace1Property = 0x4A1 | IsCovert,
        EdgeFace2Property = 0x4A2 | IsCovert,
        EdgeFacet1Property = 0x4A3 | IsCovert,
        EdgeFacet2Property = 0x4A4 | IsCovert,
        EdgeConnect1Property = 0x4A5 | IsCovert,
        EdgeConnect2Property = 0x4A6 | IsCovert,

        //=========================================
        ComputeXNameProperty = 0x4B1 | IsReference,
        ComputeXSummaryProperty = 0x4B2 | IsReference,
        ComputeXCompuTypeProperty = 0x4B3 | IsReference,
        ComputeXWhereProperty = 0x4B4 | IsReference,
        ComputeXSelectProperty = 0x4B5 | IsReference,
        ComputeXSeparatorProperty = 0x4B6 | IsReference,
        ComputeXValueTypeProperty = 0x4B7 | IsReference,
        ComputeXNumericSetProperty = 0x4B8 | IsReference,
        ComputeXResultsProperty = 0x4B9 | IsReference,
        ComputeXSortingProperty = 0x4BA | IsReference,
        ComputeXTakeSetProperty = 0x4BB | IsReference,
        ComputeXTakeLimitProperty = 0x4BC | IsReference,
        #endregion

        #region Model ================================================(600-7FF)

        //=====================================================================
        ParmDebugListModel = 0x600,
        //=====================================================================
        RootModel_612 = 0x612,
        PropertyTextModel_617 = 0x617,
        PropertyCheckModel_618 = 0x618,
        PropertyComboModel_619 = 0x619,
        //=====================================================================
        RootParmModel_620 = 0x620,
        ErrorRootModel_621 = 0x621,
        ChangeRootModel_622 = 0x622,
        MetadataRootModel_623 = 0x623,
        ModelingRootModel_624 = 0x624,
        MetaRelationListModel_625 = 0x625, // defunct - remove this
        ErrorTypeModel_626 = 0x626,
        ErrorTextModel_627 = 0x627,
        ChangeSetModel_628 = 0x628,
        ChangeItemModel_629 = 0x629,
        //=====================================================================
        ViewListModel_631 = 0x631,
        MetaViewView_Model = 0x632,
        MetaViewQuery_Model = 0x633,
        MetaViewCommand_Model = 0x634,
        MetaViewProperty_Model = 0x635,
        ViewListModel_63A = 0x63A,
        ViewView_Model = 0x63B,
        ViewItem_Model = 0x63C,
        ViewQuery_Model = 0x63D,
        //=====================================================================
        EnumListModel_624 = 0x642,
        TableListModel_643 = 0x643,
        GraphListModel_644 = 0x644,
        SymbolListModel_645 = 0x645,
        GraphParmList_MetaModel = 0x646,

        TableListModel_647 = 0x647,
        GraphListModel_648 = 0x648,
        //=====================================================================
        PairX_MetaModel = 0x652,
        EnumX_MetaModel = 0x653,
        TableModel_654 = 0x654,
        GraphModel_655 = 0x655,
        SymbolX_MetaModel = 0x656,
        ColumnModel_657 = 0x657,
        ComputeModel_658 = 0x658,
        SymbolEditor_Model = 0x659,
        //=====================================================================
        ColumnListModel_661 = 0x661,
        ChildRelationListModel_662 = 0x662,
        ParentRelatationListModel_663 = 0x663,
        EnumXPairList_MetaModel = 0x664,
        EnumXColumnList_MetaModel = 0x665,
        ComputeListModel_666 = 0x666,
        EnumXRelatedColumn_MetaModel = 0x667,
        //=====================================================================
        ChildRelationModel_671 = 0x671,
        ParentRelationModel_672 = 0x672,
        NamePropertyRelationModel_673 = 0x673,
        SummaryPropertyRelationModel_674 = 0x674,
        NamePropertyModel_675 = 0x675,
        SummaryPropertyModel_676 = 0x676,
        //=====================================================================
        MetaGraphColoring_Model = 0x681,
        MetaGraphRootList_Model = 0x682,
        MetaGraphNodeList_Model = 0x683,
        MetaGraphNode_Model = 0x684,
        MetaGraphColorColumn_Model = 0x685,
        //=====================================================================
        GraphRoot_MetaModel = 0x691,
        GraphLink_MetaModel = 0x692,
        GraphPathHead_MetaModel = 0x693,
        GraphPathLink_MetaModel = 0x694,
        GraphGroupHead_MetaModel = 0x695,
        GraphGroupLink_MetaModel = 0x696,
        GraphEgressHead_MetaModel = 0x697,
        GraphEgressLink_MetaModel = 0x698,
        GraphNodeSymbol_MetaModel = 0x699,

        ValueHead_MetaModel = 0x69E,
        ValueLink_MetaModel = 0x69F,
        //=====================================================================
        RowX_Model = 0x6A1,
        TableModel_6A4 = 0x6A4,
        GraphX_Model = 0x6A5,
        GraphXRef_Model = 0x6A6,
        RowXChildRelation_Model = 0x6A7,
        RowXParentRelation_Model = 0x6A8,
        RowXRelatedChild_Model = 0x6A9,
        RowXRelatedParent_Model = 0x6AA,
        //=====================================================================
        RowXPropertyList_Model = 0x6B1,
        RowXChildRelationList_Model = 0x6B2,
        RowXParentRelationList_Model = 0x6B3,
        RowXDefaultPropertyList_Model = 0x6B4,
        RowXUnusedChildRelationList_Model = 0x6B5,
        RowXUnusedParentRelationList_Model = 0x6B6,
        RowXComputeList_Model = 0x6B7,
        //=====================================================================
        QueryXRootLink_Model = 0x6C1,
        QueryXPathHead_Model = 0x6C2,
        QueryXPathLink_Model = 0x6C3,
        QueryXGroupHead_Model = 0x6C4,
        QueryXGroupLink_Model = 0x6C5,
        QueryXEgressHead_Model = 0x6C6,
        QueryXEgressLink_Model = 0x6C7,
        //=====================================================================
        QueryXRootItem_Model = 0x6D1,
        QueryXPathStep_Model = 0x6D2,
        QueryXPathTail_Model = 0x6D3,
        QueryXGroupStep_Model = 0x6D4,
        QueryXGroupTail_Model = 0x6D5,
        QueryXEgressStep_Model = 0x6D6,
        QueryEgressTail_Model = 0x6D7,
        //=====================================================================
        GraphRef_Model = 0x6E1,
        GraphNodeList_Model = 0x6E2,
        GraphEdgeList_Model = 0x6E3,
        GraphRootList_Model = 0x6E4,
        GraphLevelList_Model = 0x6E5,

        GraphLevel_Model = 0x6E6,
        GraphPath_Model = 0x6E7,
        GraphRoot_Model = 0x6E8,
        GraphNode_Model = 0x6E9,
        GraphEdge_Model = 0x6EA,

        GraphOpenList_Model = 0x6EB,
        GraphOpen_Model = 0x6EC,
        //=====================================================================
        PrimeCompute_Model = 0x7D0,
        ComputeStore_Model = 0x7D1,
        //=====================================================================

        DiagRootModel_7F0 = 0x7F0,
        DiagPrimeStoreModel_7F1 = 0x7F1,

        DiagItemModel_7F2 = 0x7F2,
        DiagStoreModel_7F3 = 0x7F3,
        DiagRelationModel_7F4 = 0x7F4,

        DiagChildListModel_7F5 = 0x7F5,
        DiagParentListModel_7F6 = 0x7F6,
        DiagParentChildModel_7F7 = 0x7F7,
        DiagChildParentModel_7F8 = 0x7F8,

        #endregion

        #region Enum  ================================================(800-FFF)
        // facilitates text localization for static enums/pairs

        ValueType_Bool = 0x800,
        ValueType_BoolArray = 0x801,

        ValueType_Char = 0x802,
        ValueType_CharArray = 0x803,

        ValueType_Byte = 0x804,
        ValueType_ByteArray = 0x805,

        ValueType_SByte = 0x806,
        ValueType_SByteArray = 0x807,

        ValueType_Int16 = 0x808,
        ValueType_Int16Array = 0x809,

        ValueType_UInt16 = 0x80A,
        ValueType_UInt16Array = 0x80B,

        ValueType_Int32 = 0x80C,
        ValueType_Int32Array = 0x80D,

        ValueType_UInt32 = 0x80E,
        ValueType_UInt32Array = 0x80F,

        ValueType_Int64 = 0x810,
        ValueType_Int64Array = 0x811,

        ValueType_UInt64 = 0x812,
        ValueType_UInt64Array = 0x813,

        ValueType_Single = 0x814,
        ValueType_SingleArray = 0x815,

        ValueType_Double = 0x816,
        ValueType_DoubleArray = 0x817,

        ValueType_Decimal = 0x818,
        ValueType_DecimalArray = 0x819,

        ValueType_DateTime = 0x81A,
        ValueType_DateTimeArray = 0x81B,

        ValueType_String = 0x81C,
        ValueType_StringArray = 0x81D,
        ValueTypeEnum = 0x83F,

        xxValueType_None = 0x840,
        xxValueTypeEnum = 0x87F,

        Pairing_OneToOne = 0x880,
        Pairing_OneToMany = 0x881,
        Pairing_ManyToMany = 0x882,
        PairingEnum = 0x8BF,

        Aspect_Point = 0x8C0,
        Aspect_Square = 0x8C1,
        Aspect_Vertical = 0x8C2,
        Aspect_Horizontal = 0x8C3,
        AspectEnum = 0x8FF,

        Labeling_None = 0x900,
        Labeling_Top = 0x901,
        Labeling_Left = 0x902,
        Labeling_Right = 0x903,
        Labeling_Bottom = 0x904,
        Labeling_Center = 0x905,
        Labeling_TopLeft = 0x906,
        Labeling_TopRight = 0x907,
        Labeling_BottomLeft = 0x908,
        Labeling_BottomRight = 0x909,
        Labeling_TopLeftSide = 0x90A,
        Labeling_TopRightSide = 0x90B,
        Labeling_TopLeftCorner = 0x90C,
        Labeling_TopRightCorner = 0x90D,
        Labeling_BottomLeftSide = 0x90E,
        Labeling_BottomRightSide = 0x90F,
        Labeling_BottomLeftCorner = 0x910,
        Labeling_BottomRightCorner = 0x911,
        LabelingEnum = 0x93F,

        FlipRotate_None = 0x940,
        FlipRotate_FlipVertical = 0x941,
        FlipRotate_FlipHorizontal = 0x942,
        FlipRotate_FlipBothWays = 0x943,
        FlipRotate_RotateClockwise = 0x944,
        FlipRotate_RotateFlipVertical = 0x945,
        FlipRotate_RotateFlipHorizontal = 0x946,
        FlipRotate_RotateFlipBothWays = 0x947,
        FlipRotateEnum = 0x97F,

        Resizing_Auto = 0x980,
        Resizing_Fixed = 0x981,
        Resizing_Manual = 0x982,
        ResizingEnum = 0x9BF,

        Naming_None = 0x9C0,
        Naming_Default = 0x9C1,
        Naming_UniqueNumber = 0x9C2,
        Naming_Alphabetic = 0x9C3,
        Naming_SubstituteParent = 0x9C4,
        NamingEnum = 0x9FF,

        BarWidth_Thin = 0xA00,
        BarWidth_Wide = 0xA01,
        BarWidth_ExtraWide = 0xA02,
        BarWidthEnum = 0xA3F,

        Contact_Any = 0xA40,
        Contact_One = 0xA41,
        Contact_None = 0xA42,
        ContactEnum = 0xA7F,

        Side_Any = 0xA80,
        Side_East = 0xA81,
        Side_West = 0xA82,
        Side_North = 0xA84,
        Side_South = 0xA88,
        SideEnum = 0xABF,

        Connect_Any = 0xAC0, //0
        Connect_East = 0xAC1, //1
        Connect_West = 0xAC2, //2
        Connect_East_West = 0xAC3, //1+2
        Connect_North = 0xAC4, //4
        Connect_North_East = 0xAC5, //4+1
        Connect_North_West = 0xAC6, //4+2
        Connect_North_East_West = 0xAC7, //4+1+2
        Connect_South = 0xAC8, //8
        Connect_South_East = 0xAC9, //8+1
        Connect_South_West = 0xACA, //8+2
        Connect_South_East_West = 0xACB, //8+1+2
        Connect_North_South = 0xACC, //4+8
        Connect_North_South_East = 0xACD, //4+8+1
        Connect_North_South_West = 0xACE, //4+8+2
        ConnectEnum = 0xAFF,

        Facet_None = 0xB00,
        Facet_Nubby = 0xB01,
        Facet_Diamond = 0xB02,
        Facet_InArrow = 0xB03,
        Facet_Force_None = 0xB20,
        Facet_Force_Nubby = 0xB21,
        Facet_Force_Diamond = 0xB22,
        Facet_Force_InArrow = 0xB23,
        FacetEnum = 0xB3F,

        CompuType_RowValue = 0xB40,
        CompuType_RelatedValue = 0xB41,
        CompuType_NumericValueSet = 0xB42,
        CompuType_CompositeString = 0xB43,
        CompuType_CompositeReversed = 0xB44,
        CompuTypeEnum = 0xB7F,

        NumericSet_Count = 0xB80,
        NumericSet_Count_Min_Max = 0xB81,
        NumericSet_Count_Min_Max_Sum = 0xB82,
        NumericSet_Count_Min_Max_Sum_Ave = 0xB83,
        NumericSet_Count_Min_Max_Sum_Ave_Std = 0xB84,
        NumericSetEnum = 0xBBF,

        NumericTerm_Count = 0xBC0,
        NumericTerm_Min = 0xBC1,
        NumericTerm_Max = 0xBC2,
        NumericTerm_Sum = 0xBC3,
        NumericTerm_Ave = 0xBC4,
        NumericTerm_Std = 0xBC5,
        NumericTermEnum = 0xBFF,

        StartLine_Flat = 0xC00,
        StartLine_Square = 0xC01,
        StartLine_Round = 0xC02,
        StartLine_Triangle = 0xC03,
        StartLineEnum = 0xC3F,

        EndLine_Flat = 0xC40,
        EndLine_Square = 0xC41,
        EndLine_Round = 0xC42,
        EndLine_Triangle = 0xC43,
        EndLineEnum = 0xC7F,

        Results_OneValue = 0xC80,
        Results_AllValues = 0xC81,
        Results_LimitedSet = 0xC82,
        ResultsEnum = 0xCBF,

        Sorting_Unsorted = 0xCC0,
        Sorting_Ascending = 0xCC1,
        Sorting_Descending = 0xCC2,
        SortingEnum = 0xCFF,

        TakeSet_First = 0xD00,
        TakeSet_Last = 0xD01,
        TakeSet_Both = 0xD02,
        TakeSetEnum = 0xD3F,

        Attatch_Normal = 0xD40,
        Attatch_Radial = 0xD41,
        Attatch_RightAngle = 0xD42,
        Attatch_SkewedAngle = 0xD43,
        AttatchEnum = 0xD7F,

        LineStyle_PointToPoint = 0xD80,
        LineStyle_SimpleSpline = 0xD81,
        LineStyle_DoubleSpline = 0xD82,
        LineStyleEnum = 0xDBF,

        DashStyle_Solid = 0xDC0,
        DashStyle_Dashed = 0xDC1,
        DashStyle_Dotted = 0xDC2,
        DashStyle_DashDot = 0xDC3,
        DashStyle_DashDotDot = 0xDC4,
        DashStyleEnum = 0xDFF,

        StaticPairE00 = 0xE00,
        StaticEnumE3F = 0xE3F,

        StaticPairE40 = 0xE40,
        StaticEnumE7F = 0xE7F,

        StaticPairE80 = 0xE80,
        StaticEnumEBF = 0xEBF,

        StaticPairEC0 = 0xEC0,
        StaticEnumEFF = 0xEFF,

        StaticPairF00 = 0xF00,
        StaticEnumF3F = 0xF3F,

        StaticPairF40 = 0xF40,
        StaticEnumF7F = 0xF7F,

        StaticPairF80 = 0xF80,
        StaticEnumFBF = 0xFBF,

        StaticPairFC0 = 0xFC0,
        StaticEnumFFF = 0xFFF,

        #endregion
    }
}
