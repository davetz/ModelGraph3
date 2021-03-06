﻿
namespace ModelGraph.Core
{
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
        //=====================================================================
        Is_Covert = 0x8000, // Don't include this item in the model change log
        Is_External = 0x4000, // This item is serialized/deserialize to/from a repository
        Is_Reference = 0x2000, // This item can be referenced by an external item                            

        KeyMask = 0xFFF,
        FlagMask = 0xF000,
        EnumMask = 0x3F,
        IndexMask = 0xF,

        #endregion

        #region Command  =============================================(01-0DF)
        SaveCommand = 0x1,
        ApplyCommand = 0x2,
        CloseCommand = 0x3,
        RevertCommand = 0x4,
        SaveAsCommand = 0x5,
        ReloadCommand = 0x6,

        EditCommand = 0x7,
        ViewCommand = 0x8,
        UndoCommand = 0x9,
        RedoCommand = 0xA,
        MergeCommand = 0xB,
        InsertCommand = 0xC,
        RemoveCommand = 0xD,
        CreateCommand = 0xE,
        RefreshCommand = 0xF,
        ValidateCommand = 0x10,
        MakeGraphLinkCommand = 0x11,
        MakePathHeadCommand = 0x12,
        MakeGroupHeadCommand = 0x13,
        MakeEgressHeadCommand = 0x14,
        AlignTopCommand = 0x15,
        AlignLeftCommand = 0x16,
        AlignRightCommand = 0x17,
        AlignBottomCommand = 0x18,
        AlignCenterCommand = 0x19,
        GraphUndoCommand = 0x1A,
        GraphRedoCommand = 0x1B,
        FlipVertCommand = 0x1C,
        FlipHorzCommand = 0x1D,
        Rotate90LeftCommand = 0x1E,
        Rotate90RightCommand = 0x1F,
        Rotate45LeftCommand = 0x20,
        Rotate45RightCommand = 0x21,
        Rotate30LeftCommand = 0x22,
        Rotate30RightCommand = 0x23,
        Rotate22LeftCommand = 0x24,
        Rotate22RightCommand = 0x25,
        CutCommand = 0x26,
        CopyCommand = 0x27,

        FlipRotateMode = 0xD2, //configure auto flip rotate
        TerminalMode = 0xD3, //add terminals to symbol
        OperateMode = 0xD4,
        GravityMode = 0xD5,
        ReshapeMode = 0xD6, //adjust the shape of an object (add,move,remove) inflection points
        DeleteMode = 0xD7,
        UnlinkMode = 0xD8,
        PasteMode = 0xD9,
        LinkMode = 0xDA,
        CopyMode = 0xDB,
        MoveMode = 0xDC,
        EditMode = 0xDD,
        ViewMode = 0xDE,
        NewMode = 0xDF, //select from the palette and add a copy of selected the object to the drawing
        #endregion

        #region Store ================================================(0E0-0FF)
        // root level containers for the hierarchal item trees

        EnumXRoot = 0x0E1 | Is_Reference,
        ViewXRoot = 0x0E2 | Is_Reference,
        TableXRoot = 0x0E3 | Is_Reference,
        GraphXRoot = 0x0E4 | Is_Reference,
        QueryXRoot = 0x0E5 | Is_Reference,
        SymbolXRoot = 0x0E7 | Is_Reference,
        ColumnXRoot = 0x0E8 | Is_Reference,
        ComputeXRoot = 0x0E9 | Is_Reference,
        RelationXRoot = 0x0EA | Is_Reference,

        EnumRoot = 0x0F1,
        ErrorRoot = 0x0F2,
        GroupRoot = 0x0F3,
        PropertyRoot = 0x0F4 | Is_Reference,
        RelationRoot = 0x0F5 | Is_Reference,

        #endregion

        #region Item  ================================================(100-1FF)

        //=========================================
        DummyItem = 0x100 | Is_Reference,
        DummyQueryX = 0x101 | Is_Reference,

        DataRoot = 0x102, //112

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
        RowX = 0x141 | Is_External,
        PairX = 0x142 | Is_External,
        EnumX = 0x143 | Is_External,
        ViewX = 0x144 | Is_External,
        TableX = 0x145 | Is_External,
        GraphX = 0x146 | Is_External,
        QueryX = 0x147 | Is_External,
        SymbolX = 0x148 | Is_External,
        ColumnX = 0x149 | Is_External,
        ComputeX = 0x14A | Is_External,
        CommandX = 0x14B | Is_External,
        RelationX = 0x14C | Is_External,

        //=========================================
        // QueryX detail, used to lookup resource strings
        QueryIsCorrupt = 0x150 | Is_External,
        QueryGraphRoot = 0x151 | Is_External,
        QueryGraphLink = 0x152 | Is_External,
        QueryViewRoot = 0x153 | Is_External,
        QueryViewHead = 0x154 | Is_External,
        QueryViewLink = 0x155 | Is_External,
        QueryPathHead = 0x156 | Is_External,
        QueryPathLink = 0x157 | Is_External,
        QueryGroupHead = 0x158 | Is_External,
        QueryGroupLink = 0x159 | Is_External,
        QuerySegueHead = 0x15A | Is_External,
        QuerySegueLink = 0x15B | Is_External,
        QueryValueRoot = 0x15C | Is_External,
        QueryValueHead = 0x15D | Is_External,
        QueryValueLink = 0x15E | Is_External,
        QueryNodeSymbol = 0x15F | Is_External,

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

        Error_200_ExportFailed = 0x200,
        Error_201_ImportFailed = 0x201,

        Error_210_ComputeProblemRelatedWhereSelect = 0x210,
        Error_211_ComputeMissingRelatedSelect = 0x211,
        Error_212_ComputeUnresolvedSelect = 0x212,
        Error_213_ComputeInvalidSelect = 0x213,
        Error_214_ComputeCircularDependance = 0x214,
        Error_215_ComputeMissingSelect = 0x215,

        Error_216_ComputeMissingRootQuery = 0x216,
        Error_217_ComputeValueOverflow = 0x217,

        Error_220_QueryUnresolvedWhere = 0x220,
        Error_221_QueryInvalidWhere = 0x221,

        Error_222_QueryUnresolvedSelect = 0x222,
        Error_223_QueryInvalidSelect = 0x223,
        Error_224_QueryMissingSelect = 0x224,

        Error_225_QueryValueOverflowdWhere = 0x225,
        Error_226_QueryValueOverflowSelect = 0x226,

        #endregion

        #region Relation  ============================================(300-3FF)

        Relation = 0x300,
        //=========================================
        Relation_RowX_RowX = 0x301 | Is_External,

        //=========================================
        Relation_EnumX_ColumnX = 0x311 | Is_Reference,
        Relation_Store_ColumnX = 0x312 | Is_Reference,
        Relation_Store_NameProperty = 0x313 | Is_Reference,
        Relation_Store_SummaryProperty = 0x314 | Is_Reference,
        Relation_StoreX_ChildRelation = 0x315 | Is_Reference,
        Relation_StoreX_ParentRelation = 0x316 | Is_Reference,

        //=========================================
        Relation_ViewX_ViewX = 0x332 | Is_Reference,
        Relation_ViewX_QueryX = 0x333 | Is_Reference,
        Relation_QueryX_ViewX = 0x334 | Is_Reference,
        Relation_Property_ViewX = 0x335 | Is_Reference,
        Relation_Relation_ViewX = 0x336 | Is_Reference,
        Relation_ViewX_Property = 0x337 | Is_Reference,
        Relation_QueryX_Property = 0x338 | Is_Reference,

        //=========================================
        Relation_GraphX_SymbolX = 0x341 | Is_Reference,
        Relation_SymbolX_QueryX = 0x342 | Is_Reference,
        Relation_GraphX_QueryX = 0x343 | Is_Reference,
        Relation_QueryX_QueryX = 0x344 | Is_Reference,
        Relation_GraphX_SymbolQueryX = 0x345 | Is_Reference,
        Relation_GraphX_ColorProperty = 0x346 | Is_Reference,
        Relation_GraphX_ToolTipProperty = 0x347 | Is_Reference,

        //=========================================
        Relation_Store_QueryX = 0x351 | Is_Reference,
        Relation_Relation_QueryX = 0x352 | Is_Reference,

        //=========================================
        Relation_Store_ComputeX = 0x361 | Is_Reference,
        Relation_ComputeX_QueryX = 0x362 | Is_Reference,

        //=========================================
        Relation_Store_Property = 0x3FD | Is_Reference,
        Relation_Store_ChildRelation = 0x3FE,
        Relation_Store_ParentRelation = 0x3FF,

        #endregion

        #region Property  ============================================(400-5FF)

        Property = 0x400,

        //=========================================
        ItemNameProperty = 0x401 | Is_Reference, // works for all items
        ItemSummaryProperty = 0x402 | Is_Reference, // works for all items
        ItemDescriptionProperty = 0x403 | Is_Reference, // works for all items

        //=========================================
        IncludeItemIdentityIndexProperty = 0x404 | Is_Covert,

        //=========================================
        EnumNameProperty = 0x411 | Is_Reference,
        EnumSummaryProperty = 0x412 | Is_Reference,
        EnumTextProperty = 0x413 | Is_Reference,
        EnumValueProperty = 0x414 | Is_Reference,

        //=========================================
        TableNameProperty = 0x421 | Is_Reference,
        TableSummaryProperty = 0x422 | Is_Reference,

        //=========================================
        ColumnNameProperty = 0x431 | Is_Reference,
        ColumnSummaryProperty = 0x432 | Is_Reference,
        ColumnValueTypeProperty = 0x433 | Is_Reference,
        ColumnIsChoiceProperty = 0x436 | Is_Reference,

        //=========================================
        RelationNameProperty = 0x441 | Is_Reference,
        RelationSummaryProperty = 0x442 | Is_Reference,
        RelationPairingProperty = 0x443 | Is_Reference,
        RelationIsRequiredProperty = 0x444 | Is_Reference,
        RelationIsReferenceProperty = 0x445 | Is_Reference,

        //=========================================
        GraphNameProperty = 0x451 | Is_Reference,
        GraphSummaryProperty = 0x452 | Is_Reference,
        GraphTerminalLengthProperty = 0x453 | Is_Reference,
        GraphTerminalSpacingProperty = 0x454 | Is_Reference,
        GraphTerminalStretchProperty = 0x455 | Is_Reference,
        GraphSymbolSizeProperty = 0x456 | Is_Reference,

        //=========================================
        QueryXSelectProperty = 0x460 | Is_Reference,
        QueryXWhereProperty = 0x461 | Is_Reference,

        QueryXConnect1Property = 0x462 | Is_Reference,
        QueryXConnect2Property = 0x463 | Is_Reference,

        QueryXRelationProperty = 0x466 | Is_Reference,
        QueryXIsReversedProperty = 0x467 | Is_Reference,
        QueryXIsImmediateProperty = 0x468 | Is_Reference,
        QueryXIsPersistentProperty = 0x469 | Is_Reference,
        QueryXIsBreakPointProperty = 0x46A | Is_Reference,
        QueryXExclusiveKeyProperty = 0x46B | Is_Reference,
        QueryXAllowSelfLoopProperty = 0x46C | Is_Reference,
        QueryXIsPathReversedProperty = 0x46D | Is_Reference,
        QueryXIsFullTableReadProperty = 0x46E | Is_Reference,
        QueryXFacet1Property = 0x46F | Is_Reference,
        QueryXFacet2Property = 0x470 | Is_Reference,
        ValueXWhereProperty = 0x471 | Is_Reference,
        ValueXSelectProperty = 0x472| Is_Reference,
        ValueXIsReversedProperty = 0x473 | Is_Reference,
        ValueXValueTypeProperty = 0x474 | Is_Reference,
        QueryXLineStyleProperty = 0x475 | Is_Reference,
        QueryXDashStyleProperty = 0x476 | Is_Reference,
        QueryXLineColorProperty = 0x477 | Is_Reference,

        //=========================================
        SymbolXNameProperty = 0x481 | Is_Reference,
        SymbolXAttatchProperty = 0x486 | Is_Reference,

        //=========================================
        NodeCenterXYProperty = 0x491 | Is_Covert,
        NodeSizeWHProperty = 0x492 | Is_Covert,
        NodeLabelingProperty = 0x493 | Is_Covert,
        NodeResizingProperty = 0x494 | Is_Covert,
        NodeBarWidthProperty = 0x495 | Is_Covert,
        NodeOrientationProperty = 0x496 | Is_Covert,
        NodeFlipRotateProperty = 0x497 | Is_Covert,

        //=========================================
        EdgeFace1Property = 0x4A1 | Is_Covert,
        EdgeFace2Property = 0x4A2 | Is_Covert,
        EdgeFacet1Property = 0x4A3 | Is_Covert,
        EdgeFacet2Property = 0x4A4 | Is_Covert,
        EdgeConnect1Property = 0x4A5 | Is_Covert,
        EdgeConnect2Property = 0x4A6 | Is_Covert,

        //=========================================
        ComputeXNameProperty = 0x4B1 | Is_Reference,
        ComputeXSummaryProperty = 0x4B2 | Is_Reference,
        ComputeXCompuTypeProperty = 0x4B3 | Is_Reference,
        ComputeXWhereProperty = 0x4B4 | Is_Reference,
        ComputeXSelectProperty = 0x4B5 | Is_Reference,
        ComputeXSeparatorProperty = 0x4B6 | Is_Reference,
        ComputeXValueTypeProperty = 0x4B7 | Is_Reference,
        ComputeXNumericSetProperty = 0x4B8 | Is_Reference,
        ComputeXResultsProperty = 0x4B9 | Is_Reference,
        ComputeXSortingProperty = 0x4BA | Is_Reference,
        ComputeXTakeSetProperty = 0x4BB | Is_Reference,
        ComputeXTakeLimitProperty = 0x4BC | Is_Reference,

        //=========================================
        ShapeStrokeStyleProperty = 0x4C0 | Is_Covert,
        ShapeStrokeWidthProperty = 0x4C1 | Is_Covert,
        ShapeStartCapProperty = 0x4C2 | Is_Covert,
        ShapeDashCapProperty = 0x4C3 | Is_Covert,
        ShapeEndCapProperty = 0x4C4 | Is_Covert,
        ShapeFactor1Property = 0x4C5 | Is_Covert,
        ShapeRadius1Property = 0x4C6 | Is_Covert,
        ShapeRadius2Property = 0x4C7 | Is_Covert,
        ShapeCenterXProperty = 0x4C8 | Is_Covert,
        ShapeCenterYProperty = 0x4C9 | Is_Covert,
        ShapeRotationProperty = 0x4CA | Is_Covert,
        ShapeDimensionProperty = 0x4CB | Is_Covert,
        ShapeIsImpairedProperty = 0x4CC | Is_Covert,
        ShapeExtentEastProperty = 0x4CD | Is_Covert,
        ShapeExtentWestProperty = 0x4CE | Is_Covert,
        ShapeExtentNorthProperty = 0x4CF | Is_Covert,
        ShapeExtentSouthProperty = 0x4D0 | Is_Covert,
        ShapeSizeXProperty = 0x4D1 | Is_Covert,
        ShapeSizeYProperty = 0x4D2 | Is_Covert,
        ShapeRotation1Property = 0x4D3 | Is_Covert,
        ShapeRotation2Property = 0x4D3 | Is_Covert,

        #endregion

        #region Model ================================================(600-7FF)

        //=====================================================================
        Model_600_Dummy = 0x600,
        Model_601_Shape = 0x601,

        //=====================================================================
        Model_612_Root = 0x612,
        Model_616_DeltaProperty = 0x616, //numeric with increment/decrement buttons
        Model_617_TextProperty = 0x617,
        Model_618_CheckProperty = 0x618,
        Model_619_ComboProperty = 0x619,
        //=====================================================================
        Model_620_RootParm = 0x620,
        Model_621_ErrorRoot = 0x621,
        Model_622_ChangeRoot = 0x622,
        Model_623_MetadataRoot = 0x623,
        Model_624_ModelingRoot = 0x624,
        //=====================================================================
        Model_626_ErrorType = 0x626,
        Model_627_ErrorText = 0x627,
        //=====================================================================
        Model_628_ChangeSet = 0x628,
        Model_629_ChangeItem = 0x629,
        //=====================================================================
        Model_631_ViewList = 0x631,
        MetaViewView_Model = 0x632,
        MetaViewQuery_Model = 0x633,
        MetaViewCommand_Model = 0x634,
        MetaViewProperty_Model = 0x635,
        Model_63A_ViewList = 0x63A,
        ViewView_Model = 0x63B,
        ViewItem_Model = 0x63C,
        ViewQuery_Model = 0x63D,
        //=====================================================================
        Model_642_EnumList = 0x642,
        Model_643_TableList = 0x643,
        Model_644_GraphList = 0x644,
        Model_645_GraphSymbolList = 0x645,
        Model_646_GraphParmList = 0x646,
        Model_647_TableList = 0x647,
        Model_648_GraphList = 0x648,

        //=====================================================================
        Model_652_Pair = 0x652,
        Model_653_Enum = 0x653,
        Model_654_Table = 0x654,
        Model_655_Graph = 0x655,
        Model_656_Symbol = 0x656,
        Model_657_Column = 0x657,
        Model_658_Compute = 0x658,
        Model_659_SymbolEdit = 0x659,
        //=====================================================================
        Model_661_ColumnList = 0x661,
        Model_662_ChildRelationList = 0x662,
        Model_663_ParentRelatationList = 0x663,
        Model_664_PairList = 0x664,
        Model_665_ColumnList = 0x665,
        Model_666_ComputeList = 0x666,
        Model_667_Column = 0x667,
        //=====================================================================
        Model_671_ChildRelation = 0x671,
        Model_672_ParentRelation = 0x672,
        Model_673_NamePropertyRelation = 0x673,
        Model_674_SummaryPropertyRelation = 0x674,
        Model_675_NameProperty = 0x675,
        Model_676_SummaryProperty = 0x676,
        //=====================================================================
        Model_681_GraphColoring = 0x681,
        Model_682_GraphRootList = 0x682,
        Model_683_GraphNodeList = 0x683,

        Model_684_GraphNode = 0x684,
        Model_685_GraphColorColumn = 0x685,  //defunct

        Model_686_NodeColorList = 0x686,
        Model_687_NodeColorProperty = 0x687,
        Model_688_NodeSymbolList = 0x688,
        Model_689_NodeSymbol = 0x689,
        Model_68A_NodeToolTipList = 0x68A,
        Model_68B_NodeToolTip = 0x68B,
        //=====================================================================
        Model_691_GraphRoot = 0x691,
        Model_692_GraphLink = 0x692,
        Model_693_GraphPathHead = 0x693,
        Model_694_GraphPathLink = 0x694,
        Model_695_GraphGroupHead = 0x695,
        Model_696_GraphGroupLink = 0x696,
        Model_697_GraphEgressHead = 0x697,
        Model_698_GraphEgressLink = 0x698,
        Model_699_GraphNodeSymbol = 0x699,

        Model_69E_ValueHead = 0x69E,
        Model_69F_ValueLink = 0x69F,
        //=====================================================================
        Model_6A1_Row = 0x6A1,
        Model_6A2_ChildRow = 0x6A2,
        Model_6A3_ParentRow = 0x6A3,
        Model_6A4_Table = 0x6A4,

        Model_6A5_Graph = 0x6A5,
        Model_6A6_GraphList = 0x6A6,

        Model_6A7_ChildRelation = 0x6A7,
        Model_6A8_ParentRelation = 0x6A8,
        //=====================================================================
        Model_6B1_ColumnList = 0x6B1,
        Model_6B2_ComputeList = 0x6B2,
        Model_6B3_ChildRelationList = 0x6B3,
        Model_6B4_ParentRelationList = 0x6B4,
        //=====================================================================
        Model_6C1_QueryLink = 0x6C1,
        Model_6C2_PathHead = 0x6C2,
        Model_6C3_PathLink = 0x6C3,
        Model_6C4_GroupHead = 0x6C4,
        Model_6C5_GroupLink = 0x6C5,
        Model_6C6_EgressHead = 0x6C6,
        Model_6C7_EgressLink = 0x6C7,
        //=====================================================================
        Model_6D1_QueryItem = 0x6D1,
        Model_6D2_PathStep = 0x6D2,
        Model_6D3_PathTail = 0x6D3,
        Model_6D4_GroupStep = 0x6D4,
        Model_6D5_GroupTail = 0x6D5,
        Model_6D6_EgressStep = 0x6D6,
        Model_6D7_EgressTail = 0x6D7,
        //=====================================================================

        Model_6DA_HitNode = 0x6DA,
        Model_6DB_MoveNodeMenu = 0x6DB,

        //=====================================================================
        Model_6E1_Graph = 0x6E1,
        Model_6E2_NodeList = 0x6E2,
        Model_6E3_EdgeList = 0x6E3,
        Model_6E4_RootList = 0x6E4,
        Model_6E5_LevelList = 0x6E5,

        Model_6E6_Level = 0x6E6,
        Model_6E7_Path = 0x6E7,
        Model_6E8_Root = 0x6E8,
        Model_6E9_Node = 0x6E9,
        Model_6EA_Edge = 0x6EA,

        Model_6EB_OpenList = 0x6EB,
        Model_6EC_Open = 0x6EC,

        //=====================================================================
        Model_6F0_UndoRedo = 0x6F0,
        Model_6F1_VertAlign = 0x6F1,
        Model_6F2_HorzAlign = 0x6F2,
        Model_6F3_Flip = 0x6F3,
        Model_6F4_Rotate = 0x6F4,
        Model_6F5_CutCopy = 0x6F5,

        //=====================================================================
        PrimeCompute_Model = 0x7D0,
        ComputeStore_Model = 0x7D1,
        //=====================================================================

        Model_7F0_Root = 0x7F0,
        Model_7F1_PrimeStore = 0x7F1,

        Model_7F2_Item = 0x7F2,
        Model_7F3_Store = 0x7F3,
        Model_7F4_Relation = 0x7F4,

        Model_7F5_ChildList = 0x7F5,
        Model_7F6_ParentList = 0x7F6,
        Model_7F7_ParentChild = 0x7F7,
        Model_7F8_ChildParent = 0x7F8,

        Model_7FF_RelatedItems = 0x7FF,

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

        CapStyle_Flat = 0xC00,
        CapStyle_Square = 0xC01,
        CapStyle_Round = 0xC02,
        CapStyle_Triangle = 0xC03,
        CapStyleEnum = 0xC3F,

        StrokeStyle_Solid = 0xC40,
        StrokeStyle_Dashed = 0xC41,
        StrokeStyle_Dotted = 0xC42,
        StrokeStyle_Filled = 0xC43,
        StrokeStyleEnum = 0xC7F,

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

        StaticPairDC0 = 0xDC0,
        StaticEnumDFF = 0xDFF,

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
