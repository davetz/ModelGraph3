namespace ModelGraph.Core
{/*
 */
    public class SeriesPath : Path
    {
        readonly Path[] _paths;
        internal override Path[] Paths => _paths;
        internal override IdKey IdKey => IdKey.SeriesPath;

        #region Constructor  ==================================================
        internal SeriesPath(Graph owner, Path[] paths, bool isRadial = false)
        {
            Owner = owner;
            IsRadial = isRadial;
            _paths = paths;

            owner.Add(this);
        }
        internal Graph Owner;
        internal override Item GetOwner() => Owner;
        #endregion

        #region Properties/Methods  ===========================================
        internal override Item Head => (IsReversed ? Paths[Last].Tail : Paths[0].Head);
        internal override Item Tail => (IsReversed ? Paths[0].Head : Paths[Last].Tail);
        internal override Query Query => Paths[0].Query;
        internal override double Width => 4; 
        internal override double Height => 4;
        #endregion
    }
}
