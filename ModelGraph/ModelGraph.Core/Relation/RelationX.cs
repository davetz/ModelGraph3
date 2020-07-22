
namespace ModelGraph.Core
{
    public abstract class RelationX<T0, T1, T2> : RelationOf<T0, T1, T2> where T0 : Item where T1 : Item where T2 : Item
    {
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;
    }
}
