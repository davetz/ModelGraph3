namespace ModelGraph.Core
{
    class DummyQueryX : QueryX
    {
        internal override IdKey IdKey => IdKey.DummyQueryX;
        internal DummyQueryX(QueryXManager owner) //referenced in GraphParms
        {
            Owner = owner;
        }
    }
}
