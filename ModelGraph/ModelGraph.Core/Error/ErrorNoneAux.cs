namespace ModelGraph.Core
{
    internal class ErrorNoneAux : ErrorNone
    {
        internal Item Aux;
        internal override bool IsErrorAux1 => true;

        #region Constructor  ==================================================
        internal ErrorNoneAux(ErrorRoot owner, Item item, Item aux, IdKey idKe) : base(owner, item, idKe)
        {
            Aux = aux;
        }
        #endregion
    }
}
