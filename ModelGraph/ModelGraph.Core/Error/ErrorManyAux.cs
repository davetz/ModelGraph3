namespace ModelGraph.Core
{
    internal class ErrorManyAux : ErrorMany
    {
        internal Item Aux;
        internal override bool IsErrorAux1 => true;

        #region Constructor  ==================================================
        internal ErrorManyAux(ErrorRoot owner, Item item, Item aux1, IdKey idKe, string text = null) : base(owner, item, idKe, text)
        {
            Aux = aux1;
        }
        #endregion
    }
}
