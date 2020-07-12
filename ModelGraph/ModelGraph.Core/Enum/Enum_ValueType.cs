
namespace ModelGraph.Core
{
    public class Enum_ValueType : EnumZ
    {
        internal override IdKey IdKey => IdKey.ValueTypeEnum;

        #region Constructor  ==================================================
        internal Enum_ValueType(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.ValueType_Bool);
            new PairZ(this, IdKey.ValueType_Char);
            new PairZ(this, IdKey.ValueType_SByte);
            new PairZ(this, IdKey.ValueType_Int16);
            new PairZ(this, IdKey.ValueType_Int32);
            new PairZ(this, IdKey.ValueType_Int64);
            new PairZ(this, IdKey.ValueType_Single);
            new PairZ(this, IdKey.ValueType_Double);
            new PairZ(this, IdKey.ValueType_Decimal);
            new PairZ(this, IdKey.ValueType_DateTime);
            new PairZ(this, IdKey.ValueType_String);
            new PairZ(this, IdKey.ValueType_Byte);
            new PairZ(this, IdKey.ValueType_UInt16);
            new PairZ(this, IdKey.ValueType_UInt32);
            new PairZ(this, IdKey.ValueType_UInt64);
            new PairZ(this, IdKey.ValueType_BoolArray);
            new PairZ(this, IdKey.ValueType_CharArray);
            new PairZ(this, IdKey.ValueType_SByteArray);
            new PairZ(this, IdKey.ValueType_Int16Array);
            new PairZ(this, IdKey.ValueType_Int32Array);
            new PairZ(this, IdKey.ValueType_Int64Array);
            new PairZ(this, IdKey.ValueType_SingleArray);
            new PairZ(this, IdKey.ValueType_DoubleArray);
            new PairZ(this, IdKey.ValueType_DecimalArray);
            new PairZ(this, IdKey.ValueType_StringArray);
            new PairZ(this, IdKey.ValueType_ByteArray);
            new PairZ(this, IdKey.ValueType_UInt16Array);
            new PairZ(this, IdKey.ValueType_UInt32Array);
            new PairZ(this, IdKey.ValueType_UInt64Array);
        }
        #endregion
    }
}
