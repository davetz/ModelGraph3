
namespace ModelGraph.Core
{
    /// <summary>Compatable with Window.UI.Core.CoreCursorType</summary>
    public enum DrawCursor
    {
        Arrow = 0,
        Cross = 1,
        Custom = 2,
        Hand = 3,
        Help = 4,
        IBeam = 5,
        SizeAll = 6,
        SizeNortheastSouthwest = 7,
        SizeNorthSouth = 8,
        SizeNorthwestSoutheast = 9,
        SizeWestEast = 10,
        UniversalNo = 11,
        UpArrow = 12,
        Wait = 13,
        Pin = 14,
        Person = 15,

        //Custom Cursors have id value {101, 102, 103, 104, ...}
        CustomCursorsBegin = 100, //any id value > 100 triggers custom cursor loading
        New = 101,
        View = 102,
        Move = 103,
        Link = 104,
        Copy = 105,
        Delete = 106,
        UnLink = 107,
        Operate = 108,
        Gravity = 109,
    }
}
