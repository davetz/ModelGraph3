
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
        New = 101,        //pointer1
        Edit = 102,       //pointer2
        Move = 103,       //pointer3
        Link = 104,       //pointer4
        Copy = 105,       //pointer5
        Delete = 106,     //pointer6
        UnLink = 107,     //pointer7
        Operate = 108,    //pointer8
        Gravity = 109,    //pointer9
        EditHit = 110,    //pointerA 
        MoveHit = 111,    //pointerB
        LinkHit = 112,    //pointerC
        CopyHit = 113,    //pointerD
        DeleteHit = 114,  //pointerE
        UnLinkHit = 115,  //pointerF
        OperateHit = 116, //pointerG
        GravityHit = 117, //pointerH
    }
}
