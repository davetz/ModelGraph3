namespace ModelGraph.Core
{/*
 */
    public enum ChangeType
    {
        None,
        OneUp,
        TwoUp,
        PageUp,
        GoToTop,
        OneDown,
        TwoDown,
        PageDown,
        LastChild,
        FirstChild,
        GoToBottom,
        ToggleLeft,
        ToggleRight,
        ToggleFilter,
        ExpandAllLeft,
        ExpandAllRight,
        ViewListChanged,
        FilterSortChanged,
    }
}
