
namespace ModelGraph.Core
{
    internal interface IRootDomain
    {
        /// <summary>Create secondary items in the domain</summary>
        void CreateSecondaryHierarchy(Root root);

        /// <summary>Register relational references in the domain</summary>
        void RegisterRelationalReferences(Root root);

        /// <summary>Validate all items in the domain</summary>
        void ValidateDomain(Root root);
    }
}
