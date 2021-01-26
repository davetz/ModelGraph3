using System;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public class DrawData : IDrawData
    {
        internal Func<Extent> GetExtent;
        internal void Clear() { Text.Clear(); Parms.Clear(); DataDelta++; }
        internal void AddText(((Vector2, string), (byte, byte, byte, byte)) data) => Text.Add(data);
        internal void AddParms((Vector2[], (ShapeType, StrokeType, byte), (byte, byte, byte, byte)) data) => Parms.Add(data);
        internal void AddTargets(Vector2[] points, TargetType ttype) => Parms.Add((points, TargetParm[ttype], TargetColor));

        private static readonly (byte, byte, byte, byte) TargetColor = (255, 255, 255, 255);
        private static readonly Dictionary<TargetType, (ShapeType, StrokeType, byte)> TargetParm = new Dictionary<TargetType, (ShapeType, StrokeType, byte)>()
        {
            [TargetType.SolidPin2] = (ShapeType.Pin2, StrokeType.Filled, 1),
            [TargetType.SolidPin4] = (ShapeType.Pin4, StrokeType.Filled, 1),
            [TargetType.HollowPin2] = (ShapeType.Pin2, StrokeType.Simple, 1),
            [TargetType.HollowPin4] = (ShapeType.Pin4, StrokeType.Simple, 1),
            [TargetType.SolidGrip2] = (ShapeType.Grip2, StrokeType.Filled, 1),
            [TargetType.SolidGrip4] = (ShapeType.Grip4, StrokeType.Filled, 1),
            [TargetType.HollowGrip2] = (ShapeType.Grip2, StrokeType.Simple, 1),
            [TargetType.HollowGrip4] = (ShapeType.Grip4, StrokeType.Simple, 1),
        };


        public uint DataDelta { get; private set; }

        public virtual Extent Extent { get => (GetExtent is null) ? new Extent() : GetExtent(); }
        public Vector2 Point1 { get; set; }
        public Vector2 Point2 { get; set; }
        public Vector2 PointDelta(bool reset = false)
        {
            var delta = Point2 - Point1;
            if (reset) Point1 = Point2;
            return delta;
        }

        public List<((Vector2, string), (byte, byte, byte, byte))> Text { get; } = new List<((Vector2, string), (byte, byte, byte, byte))>();
        public List<(Vector2[], (ShapeType, StrokeType, byte), (byte, byte, byte, byte))> Parms { get; } = new List<(Vector2[], (ShapeType, StrokeType, byte), (byte, byte, byte, byte))>();
    }
}
