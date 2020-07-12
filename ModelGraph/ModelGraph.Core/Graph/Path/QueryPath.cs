namespace ModelGraph.Core
{/*
 */
    public class QueryPath : Path
    {
        internal Query HeadQuery;
        internal Query TailQuery;
        internal int TailIndex;
        internal override Path[] Paths => null;
        internal override IdKey IdKey => IdKey.QueryPath;

        #region Constructor  ==================================================
        internal QueryPath(Graph owner, Query head, Query tail, int taiIndex = 0, bool isRadial = false, bool isReversed = false)
        {
            Owner = owner;
            HeadQuery = head;
            TailQuery = tail;
            TailIndex = taiIndex;

            IsRadial = isRadial;
            IsReversed = isReversed;

            owner.Add(this);
        }
        #endregion

        #region Properties/Methods  ===========================================
        internal override Query Query => HeadQuery;

        internal Item EdgeHead => HeadQuery.Item;
        internal Item EdgeTail => TailQuery.GetItem(TailIndex);

        internal override Item Head => IsReversed ? TailQuery.GetItem(TailIndex) : HeadQuery.Item;
        internal override Item Tail => IsReversed ? HeadQuery.Item : TailQuery.GetItem(TailIndex);

        internal override double Width => 40;
        internal override double Height => 10;
        #endregion
    }
}
