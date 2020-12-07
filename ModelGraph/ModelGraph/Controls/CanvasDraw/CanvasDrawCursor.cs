﻿using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl
    {
        private void RestorePointerCursor() => TrySetNewCursor(CoreCursorType.Arrow);
        private void TrySetNewCursor(CoreCursorType cursorType, int id = 0)
        {
            if (_currentCusorType == cursorType) return;
            if (cursorType == CoreCursorType.Custom && _customCursor.TryGetValue(id, out CoreCursor customCursor))
            {
                _currentCusorType = CoreCursorType.Custom;
                Window.Current.CoreWindow.PointerCursor = customCursor;
            }
            else if (_standardCursors.TryGetValue(cursorType, out CoreCursor standardCursor))
            {
                _currentCusorType = cursorType;
                Window.Current.CoreWindow.PointerCursor = standardCursor;
            }
            else if (_standardCursors.TryGetValue(CoreCursorType.Arrow, out CoreCursor arrowCursor))
            {
                _currentCusorType = cursorType;
                Window.Current.CoreWindow.PointerCursor = arrowCursor;
            }
        }
        private CoreCursorType _currentCusorType;
        readonly Dictionary<CoreCursorType, CoreCursor> _standardCursors = new Dictionary<CoreCursorType, CoreCursor>()
        {
            [CoreCursorType.Pin] = new CoreCursor(CoreCursorType.Pin, 0),
            [CoreCursorType.Hand] = new CoreCursor(CoreCursorType.Hand, 0),
            [CoreCursorType.Wait] = new CoreCursor(CoreCursorType.Wait, 0),
            [CoreCursorType.Help] = new CoreCursor(CoreCursorType.Help, 0),
            [CoreCursorType.Arrow] = new CoreCursor(CoreCursorType.Arrow, 0),
            [CoreCursorType.IBeam] = new CoreCursor(CoreCursorType.IBeam, 0),
            [CoreCursorType.Cross] = new CoreCursor(CoreCursorType.Cross, 0),
            [CoreCursorType.Person] = new CoreCursor(CoreCursorType.Person, 0),
            [CoreCursorType.UpArrow] = new CoreCursor(CoreCursorType.UpArrow, 0),
            [CoreCursorType.SizeAll] = new CoreCursor(CoreCursorType.SizeAll, 0),
            [CoreCursorType.UniversalNo] = new CoreCursor(CoreCursorType.UniversalNo, 0),
            [CoreCursorType.SizeWestEast] = new CoreCursor(CoreCursorType.SizeWestEast, 0),
            [CoreCursorType.SizeNorthSouth] = new CoreCursor(CoreCursorType.SizeNorthSouth, 0),
            [CoreCursorType.SizeNortheastSouthwest] = new CoreCursor(CoreCursorType.SizeNortheastSouthwest, 0),
            [CoreCursorType.SizeNorthwestSoutheast] = new CoreCursor(CoreCursorType.SizeNorthwestSoutheast, 0),
        };
        readonly Dictionary<int, CoreCursor> _customCursor = new Dictionary<int, CoreCursor>()
        {
            [101] = new CoreCursor(CoreCursorType.Custom, 101),
            [102] = new CoreCursor(CoreCursorType.Custom, 102),
            [103] = new CoreCursor(CoreCursorType.Custom, 103),
            [104] = new CoreCursor(CoreCursorType.Custom, 104),
            [105] = new CoreCursor(CoreCursorType.Custom, 105),
            [106] = new CoreCursor(CoreCursorType.Custom, 106),
            [107] = new CoreCursor(CoreCursorType.Custom, 107),
            [108] = new CoreCursor(CoreCursorType.Custom, 108),
            [109] = new CoreCursor(CoreCursorType.Custom, 109),
        };
    }
}
