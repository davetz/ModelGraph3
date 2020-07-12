﻿namespace ModelGraph.Core
{
    internal class ErrorNoneAux2 : ErrorNone
    {
        internal Item Aux1;
        internal Item Aux2;

        #region Constructor  ==================================================
        internal ErrorNoneAux2(StoreOf<Error> owner, Item item, Item aux1, Item aux2, IdKey idKe) : base(owner, item, idKe)
        {
            Aux1 = aux1;
            Aux1 = aux2;
        }
        #endregion
    }
}
