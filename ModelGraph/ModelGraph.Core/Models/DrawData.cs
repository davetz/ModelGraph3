using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public class DrawData : IDrawData
    {
        internal void Clear()
        {
            Text.Clear();
            Lines.Clear();
            Shapes.Clear();
        }
        internal void AddText(((Vector2, string), (byte, byte, byte, byte)) data) => Text.Add(data);
        internal void AddLine((Vector2[], (ShapeType, StrokeType, byte), (byte, byte, byte, byte)) data) => Lines.Add(data);
        internal void AddShape(((Vector2, Vector2), (ShapeType, StrokeType, byte), (byte, byte, byte, byte)) data) => Shapes.Add(data);

        public List<((Vector2, string), (byte, byte, byte, byte))> Text { get; } = new List<((Vector2, string), (byte, byte, byte, byte))>();
        public List<(Vector2[], (ShapeType, StrokeType, byte), (byte, byte, byte, byte))> Lines { get; } = new List<(Vector2[], (ShapeType, StrokeType, byte), (byte, byte, byte, byte))>();
        public List<((Vector2, Vector2), (ShapeType, StrokeType, byte), (byte, byte, byte, byte))> Shapes { get; } = new List<((Vector2, Vector2), (ShapeType, StrokeType, byte), (byte, byte, byte, byte))>();
    }
}
