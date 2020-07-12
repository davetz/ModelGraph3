namespace ModelGraph.Core
{
    public class ErrorRoot : StoreOf<Error>
    {
        #region Constructors  =================================================
        internal ErrorRoot(Root root)
        {
            Owner = root;
            SetCapacity(20);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.ErrorRoot;
        #endregion
    }
}
