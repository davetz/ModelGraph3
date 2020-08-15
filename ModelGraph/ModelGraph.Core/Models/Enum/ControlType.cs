
namespace ModelGraph.Core
{
    /// <summary>Specify the type of control to create</summary>
    public enum ControlType
    {
        PrimaryTree,    // full model hierarchy tree control
        PartialTree,    // partial model hierarchy tree control
        SymbolEditor,   // graphical editor for graph node symbols
        GraphDisplay,   // graphical interface for relational graphs
    }
}
