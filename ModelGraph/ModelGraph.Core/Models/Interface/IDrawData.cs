﻿using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public interface IDrawData
    {
        List<((Vector2, string), (byte, byte, byte, byte))> Text { get; }
        List<(Vector2[], (ShapeType, StrokeType, byte), (byte, byte, byte, byte))> Lines { get; }
        List<((Vector2, Vector2), (ShapeType, StrokeType, byte), (byte, byte, byte, byte))> Shapes { get; }
    }
}