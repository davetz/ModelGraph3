
namespace ModelGraph.Core
{
    public class PairZ : Item
    {
        internal override IdKey IdKey => _idKey;
        private IdKey _idKey;

        internal PairZ(EnumZ owner, IdKey idKe)
        {
            Owner = owner;
            _idKey = idKe;

            owner.Add(this);
        }
        internal EnumZ Owner;
        internal override Item GetOwner() => Owner;

        internal int EnumKey => (int)(_idKey & IdKey.EnumMask);
    }
}
