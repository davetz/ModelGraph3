namespace ModelGraph.Core
{
    internal class ErrorNoneAux : ErrorNone
    {
        internal Item Aux;

        #region Constructor  ==================================================
        internal ErrorNoneAux(ErrorRoot owner, Item item, Item aux, IdKey idKe) : base(owner, item, idKe)
        {
            Aux = aux;
        }
        #endregion
    }
}
