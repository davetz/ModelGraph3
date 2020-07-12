
namespace ModelGraph.Core
{
    public class RelationX<T1,T2> : RelationOf<T1, T2> where T1 : Item where T2 : Item
    {
        internal override IdKey IdKey => IdKey.RelationX;

        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        #region Constructors  =================================================
        internal RelationX() { }
        internal RelationX(RelationXRoot owner, IdKey idKe, bool autoExpandRight = false)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
        #endregion
    }
}
