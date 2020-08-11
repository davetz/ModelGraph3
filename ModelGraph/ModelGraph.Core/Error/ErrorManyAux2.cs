namespace ModelGraph.Core
{
    internal class ErrorManyAux2 : ErrorMany
    {
        internal Item Aux1;
        internal Item Aux2;
        internal override bool IsErrorAux2 => true;

        #region Constructor  ==================================================
        internal ErrorManyAux2(ErrorRoot owner, Item item, Item aux1, Item aux2, IdKey idKe, string text = null) : base(owner, item, idKe, text)
        {
            Aux1 = aux1;
            Aux2 = aux2;
        }
        #endregion
    }
}
