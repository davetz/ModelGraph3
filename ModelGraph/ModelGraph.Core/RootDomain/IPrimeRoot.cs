
namespace ModelGraph.Core
{
    internal interface IPrimeRoot
    {
        void CreateSecondaryHierarchy(Root root);
        void RegisterRelationalReferences(Root root);
    }
}
