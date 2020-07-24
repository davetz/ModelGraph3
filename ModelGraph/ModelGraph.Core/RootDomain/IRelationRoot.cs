
namespace ModelGraph.Core
{
    internal interface IRelationRoot
    {
        Relation[] GetRelationArray();
        (Store, Store) GetHeadTail(Relation rx);
    }
}
