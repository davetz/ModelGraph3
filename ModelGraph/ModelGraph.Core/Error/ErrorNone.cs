namespace ModelGraph.Core
{
    internal class ErrorNone : Error
    {
        #region Constructor  ==================================================
        internal ErrorNone(ErrorRoot owner, Item item, IdKey idKe)
        {
            Owner = owner;
            Item = item;
            ErrorId = idKe;
            owner.Add(this);
        }
        internal ErrorRoot Owner;
        internal override Item GetOwner() => Owner;
        #endregion

        #region Overrides  ====================================================
        internal override void Add(string text) { }
        internal override void Clear() { }

        internal override int Count => 0;
        internal override string GetError(int index = 0) => string.Empty;
        #endregion
    }
}
