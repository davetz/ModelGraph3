
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

        //Custom Cursors have id value {101, 102, 103, 104,.. 127}
        //It seems like you can't have an index > 127, causes a linker error
        CustomCursorsBegin = 100, //an id value > 100 triggers custom cursor loading
        Splat = 101, //is shown when pointer is OnVoid and a pointer tap will cause a paste operation
        Armed = 102, //is shown when pointer is OnVoid and one of the fallowing draw modes are true
        Edit = 103, //is shown when pointer is on an Editable item and a pointer tap will bring up a property/command dialog
        Move = 104, //is shown when pointer is on a Movable item and a pointer drag or arrow keys will move the item
        Copy = 105, //is shown when pointer is on as Copyable item and a pointer tao wil copy the item
        Delete = 106, //is shown when pointer is on a Deletable item and a pointer tao wil delete the item
        Gravity = 107, //is shown when pointer is on an item affected by gravity and a pointer tao wil activate it
        Operate = 108, //is shown when pointer is on an item with an operable action and a pointer tap will activate it
        C109 = 109, // start of the available spares 
        C110 = 110,
        C111 = 111,
        C112 = 112,
        C113 = 113,
        C114 = 114,
        C115 = 115,
        C116 = 116,
        C117 = 117,
        C118 = 118,
        C119 = 119,
        C120 = 120,
        C121 = 121,
        C122 = 122,
        C123 = 123,
        C124 = 124,
        C125 = 125,
        C126 = 126,
        C127 = 127,
    }
}
