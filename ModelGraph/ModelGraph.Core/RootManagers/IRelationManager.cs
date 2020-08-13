
namespace ModelGraph.Core
{
    internal interface IRelationManager
    {
        Relation[] GetRelationArray();
        (Store, Store) GetHeadTail(Relation rx);
    }
}
