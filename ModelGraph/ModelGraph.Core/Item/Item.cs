using System;

namespace ModelGraph.Core
{
    public abstract class Item
    {
        private byte _flags;        //IsNew, IsDeleted, IsDiscarded, AutoExpandLeft, AutoExpandRight,..
        internal byte ModelDelta;   //incremented when a property or relation is changed
        internal byte ChildDelta;   //incremented when list of child items is changed 
        internal byte ErrorDelta;   //incremented when item's error state has changed

        internal const string BlankName = "???";
        internal const string InvalidItem = "######";

        #region Identity  =====================================================
        internal virtual IdKey IdKey => IdKey.Empty;                            //all items should have an IdKey

        internal virtual string Name { get => "??"; set => _ = value; }         //most external items have a name string
        internal virtual string Summary { get => ""; set => _ = value; }        //most external items have a summary string
        internal virtual string Description { get => ""; set => _ = value; }    //most external items may have a discription

        public virtual string GetKindId() => GetRoot().GetKindId(IdKey);
        public virtual string GetNameId() => GetRoot().GetNameId(IdKey);
        public virtual Item GetParent() => GetOwner();
        public virtual string GetFullNameId() => $"{GetParent().GetNameId()} : {GetNameId()}";
        public virtual string GetChangeLogId() => GetFullNameId();
        public virtual string GetSummaryId() => GetRoot().GetSummaryId(IdKey);
        public virtual string GetDescriptionId() => GetRoot().GetDescriptionId(IdKey);
        public virtual string GetAcceleratorId() => GetRoot().GetAcceleratorId(IdKey);
        internal string GetIndexId()
        {
            var inx = Index;
            return (inx < 0) ? InvalidItem : $"#{inx}";
        }

        public virtual Root Root => GetRoot();
        /// <summary>Walk up item tree hierachy to find the parent DataRoot</summary>
        internal abstract Item GetOwner();
        internal Root GetRoot()
        {
            var itm = this;
            for (int i = 0; i < 20; i++)
            {
                if (itm is null) break;
                if (itm is Root root) return root;
                itm = itm.GetOwner();
            }
            throw new Exception("GetRoot: Corrupted item hierarchy");
        }
        #endregion

        #region IdKey  ========================================================
        internal bool IsCovert => (IdKey & IdKey.Is_Covert) != 0;
        internal bool IsExternal => (IdKey & IdKey.Is_External) != 0;
        internal bool IsReference => (IdKey & IdKey.Is_Reference) != 0;

        internal ushort ItemKey => (ushort)(IdKey & IdKey.KeyMask);
        #endregion

        #region State  ========================================================
        // not all items need state bits, but if they do, they share a common definition.
        // and since they only make sense to for the specific class the exact bit identities can overlap
        virtual internal State State { get => State.Empty; set => _ = value; }  //not all items need state bits      

        private bool GetFlag(State flag) => (State & flag) != 0;
        private void SetFlag(State flag, bool value = true) { if (value) State |= flag; else State &= ~flag; }

        internal bool HasState() => State != State.Empty;
        internal ushort GetState() => (ushort) State;
        internal void SetState(ushort value) => State = (State)value;

        internal QueryType QueryKind { get { return (QueryType)(State & State.Index); } set { State = ((State & ~State.Index) | (State)(value)); } }

        internal bool IsHead { get { return GetFlag(State.IsHead); } set { SetFlag(State.IsHead, value); } }
        internal bool IsTail { get { return GetFlag(State.IsTail); } set { SetFlag(State.IsTail, value); } }
        internal bool IsRoot { get { return GetFlag(State.IsRoot); } set { SetFlag(State.IsRoot, value); } }
        internal bool IsReversed { get { return GetFlag(State.IsReversed); } set { SetFlag(State.IsReversed, value); } }
        internal bool IsRadial { get { return GetFlag(State.IsRadial); } set { SetFlag(State.IsRadial, value); } }

        internal bool IsBreakPoint { get { return GetFlag(State.IsBreakPoint); } set { SetFlag(State.IsBreakPoint, value); } }


        internal bool IsUndone { get { return GetFlag(State.IsUndone); } set { SetFlag(State.IsUndone, value); } }
        internal bool IsVirgin { get { return GetFlag(State.IsVirgin); } set { SetFlag(State.IsVirgin, value); } }
        internal bool IsCongealed { get { return GetFlag(State.IsCongealed); } set { SetFlag(State.IsCongealed, value); } }

        internal bool HasEnumXRef { get { return GetFlag(State.HasEnumXRef); } set { SetFlag(State.HasEnumXRef, value); } }
        internal bool IsChoice { get { return GetFlag(State.IsChoice); } set { SetFlag(State.IsChoice, value); } }

        internal bool IsPersistent { get { return GetFlag(State.IsPersistent); } set { SetFlag(State.IsPersistent, value); } }
        internal bool IsRequired { get { return GetFlag(State.IsRequired); } set { SetFlag(State.IsRequired, value); } }

        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 
        // deleted items can be restored, but discarded items are gone forever
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 
        internal bool IsNew { get { return (_flags & B1) != 0; } set { _flags = value ? (byte)(_flags | B1) : (byte)(_flags & ~B1); } }
        internal bool IsDeleted { get { return (_flags & B2) != 0; } set { _flags = value ? (byte)(_flags | B2) : (byte)(_flags & ~B2); } }
        internal bool IsDiscarded { get { return (_flags & B3) != 0; } set { _flags = value ? (byte)(_flags | B3) : (byte)(_flags & ~B3); } }
        internal bool IsUnsable { get => IsDeleted || IsDiscarded; }
        internal bool AutoExpandLeft { get { return (_flags & B4) != 0; } set { _flags = value ? (byte)(_flags | B4) : (byte)(_flags & ~B4); } }
        internal bool AutoExpandRight { get { return (_flags & B5) != 0; } set { _flags = value ? (byte)(_flags | B5) : (byte)(_flags & ~B5); } }
        internal bool IsRefreshTriggerItem { get { return (_flags & B6) != 0; } set { _flags = value ? (byte)(_flags | B6) : (byte)(_flags & ~B6); } }
        #endregion

        #region StringKeys  ===================================================
        internal string KindKey => GetKindKey(IdKey);
        internal string NameKey => GetNameKey(IdKey);
        internal string SummaryKey => GetSummaryKey(IdKey);
        internal string DescriptionKey => GetDescriptionKey(IdKey);

        internal string GetKindKey(IdKey idKe) => $"{(int)(idKe & IdKey.KeyMask):X3}K";
        internal string GetNameKey(IdKey idKe) => $"{(int)(idKe & IdKey.KeyMask):X3}N";
        internal string GetSummaryKey(IdKey idKe) => $"{(int)(idKe & IdKey.KeyMask):X3}S";
        internal string GetDescriptionKey(IdKey idKe) => $"{(int)(idKe & IdKey.KeyMask):X3}V";
        internal string GetAcceleratorKey(IdKey idKe) => $"{(int)(idKe & IdKey.KeyMask):X3}A".ToUpper();
        #endregion

        #region Property/Methods ==============================================
        internal int Index => (GetOwner() is Store st) ? st.IndexOf(this) : -1;

        internal Store Store => GetOwner() as Store;

        internal bool IsValid(Item itm) => !IsInvalid(itm);
        internal bool IsInvalid(Item itm)
        {
            for (int i = 0; i < 20; i++)// avoid an infinite loop
            {
                if (itm is null) return true;
                if (itm.IsUnsable) return true;
                if (itm is Root) return false;
                itm = itm.GetOwner();
            }
            return true;
        }
        #endregion

        #region Discard  ======================================================
        // there is no coming back from Discard()
        /// <summary>Discard my self and recursivly discard all child Items</summary>
        internal virtual void Discard()
        {
            IsDiscarded = true;
            DiscardChildren();
        }
        internal virtual void DiscardChildren() { }
        #endregion

        #region BitFlags  =====================================================
        // used during serialization to indicate properties with non-default values
        public const byte BZ = 0;
        public const byte B1 = 0x1;
        public const byte B2 = 0x2;
        public const byte B3 = 0x4;
        public const byte B4 = 0x8;
        public const byte B5 = 0x10;
        public const byte B6 = 0x20;
        public const byte B7 = 0x40;
        public const byte B8 = 0x80;

        public const ushort SZ = 0;
        public const ushort S1 = 0x1;
        public const ushort S2 = 0x2;
        public const ushort S3 = 0x4;
        public const ushort S4 = 0x8;
        public const ushort S5 = 0x10;
        public const ushort S6 = 0x20;
        public const ushort S7 = 0x40;
        public const ushort S8 = 0x80;
        public const ushort S9 = 0x100;
        public const ushort S10 = 0x200;
        public const ushort S11 = 0x400;
        public const ushort S12 = 0x800;
        public const ushort S13 = 0x1000;
        public const ushort S14 = 0x2000;
        public const ushort S15 = 0x4000;
        public const ushort S16 = 0x8000;
        #endregion
    }
}
