using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public class DrawData : IDrawData
    {
        private List<((Vector2, string), (byte, byte, byte, byte))> _text = new List<((Vector2, string), (byte, byte, byte, byte))>();
        private List<(Vector2[], (Stroke, byte), (byte, byte, byte, byte))> _lines = new List<(Vector2[], (Stroke, byte), (byte, byte, byte, byte))>();
        private List<(Vector2[], (Stroke, byte), (byte, byte, byte, byte))> _splines = new List<(Vector2[], (Stroke, byte), (byte, byte, byte, byte))>();
        private List<((Vector2, Vector2), (Stroke, byte), (byte, byte, byte, byte))> _rects = new List<((Vector2, Vector2), (Stroke, byte), (byte, byte, byte, byte))>();
        private List<((Vector2, float), (Stroke, byte), (byte, byte, byte, byte))> _circles = new List<((Vector2, float), (Stroke, byte), (byte, byte, byte, byte))>();

        internal void Clear()
        {
            _text.Clear();
            _lines.Clear();
            _rects.Clear();
            _circles.Clear();
            _splines.Clear();
        }
        internal void AddText(((Vector2, string), (byte, byte, byte, byte)) data) => _text.Add(data);
        internal void AddLine((Vector2[] Points, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB) data) => _lines.Add(data);
        internal void AddSpline((Vector2[] Points, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB) data) => _splines.Add(data);
        internal void AddRect(((Vector2, Vector2) centerRadius, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB) data) => _rects.Add(data);
        internal void AddCircle(((Vector2, float) centerRadius, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB) data) => _circles.Add(data);

        public List<((Vector2, string) topLeftText, (byte, byte, byte, byte) ARGB)> Text => _text;
        public List<(Vector2[] Points, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> Lines => _lines;
        public List<(Vector2[] Points, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> Splines => _splines;
        public List<((Vector2, Vector2) centerRadius, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> Rects => _rects;
        public List<((Vector2, float) centerRadius, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> Circles => _circles;
    }
}
