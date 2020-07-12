namespace ModelGraph.Core
{
    internal class ErrorManyAux : ErrorMany
    {
        internal Item Aux;

        #region Constructor  ==================================================
        internal ErrorManyAux(StoreOf<Error> owner, Item item, Item aux1, IdKey idKe, string text = null) : base(owner, item, idKe, text)
        {
            Aux = aux1;
        }
        #endregion
    }
}
