namespace ModelGraph.Core
{
    internal class ErrorOneAux2 : ErrorOne
    {
        internal Item Aux1;
        internal Item Aux2;

        #region Constructor  ==================================================
        internal ErrorOneAux2(StoreOf<Error> owner, Item item, Item aux1, Item aux2, IdKey idKe, string text = null) : base(owner, item, idKe, text)
        {
            Aux1 = aux1;
            Aux2 = aux2;
        }
        #endregion
    }
}
