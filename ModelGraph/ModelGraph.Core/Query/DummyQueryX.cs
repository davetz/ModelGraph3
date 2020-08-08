namespace ModelGraph.Core
{
    class DummyQueryX : QueryX
    {
        internal override IdKey IdKey => IdKey.DummyQueryX;
        internal DummyQueryX(QueryXRoot owner) //referenced in GraphParms
        {
            Owner = owner;
        }
    }
}
