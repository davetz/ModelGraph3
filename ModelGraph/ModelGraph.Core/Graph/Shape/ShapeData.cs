using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI.ApplicationSettings;

namespace ModelGraph.Core
{
    internal abstract partial class Shape
    {
        private const int PointsOffset = 14;
        private byte A = 0xFF; // of color(A, R, G, B)
        private byte R = 0xFF; // of color(A, R, G, B)
        private byte G = 0xFF; // of color(A, R, G, B)
        private byte B = 0xFF; // of color(A, R, G, B)
        protected byte SW = 2;  // stroke width
        private byte ST = 4;  // stroke type
        private byte R1 = 1;  // minor axis (inner, horzontal) (1 to 128)
        private byte R2 = 1;  // major axis (outer, vertical) (1 to 128)
        private byte F1;      // auxiliary factor (for PolyGear and PolyPulse) (0 to 100 %)
        private byte PD = 1;  // polyline dimension specific to the shape type
        private byte PL;      // the polyline control parameters are locked
        private byte A0;      // rotation index for 22.5 degree delta
        private byte A1;      // rotation index for 30.0 degree delta
        protected List<Vector2> DXY;  // one or more defined points

        #region Properties  ===================================================
        protected ShapeProperty LinePropertyFlags()
        {
            var ss = (StrokeType)ST & StrokeType.Filled;
            if (ss == StrokeType.Filled) return ShapeProperty.LineStyle;
            if (ss == StrokeType.Dashed || ss == StrokeType.Dotted) return (ShapeProperty.LineStyle | ShapeProperty.StartCap | ShapeProperty.DashCap | ShapeProperty.EndCap | ShapeProperty.LineWidth) & ValidLineProperty;
            return (ShapeProperty.LineStyle | ShapeProperty.StartCap | ShapeProperty.EndCap | ShapeProperty.LineWidth) & ValidLineProperty;
        }
        protected virtual ShapeProperty ValidLineProperty => ShapeProperty.LineStyle | ShapeProperty.StartCap | ShapeProperty.DashCap | ShapeProperty.EndCap | ShapeProperty.LineWidth;
        internal void SetStrokeWidth(byte sw) => SW = sw;
        internal void SetEndCap(CapStyle v)
        {
            var ss = (StrokeType)ST & ~StrokeType.EC_Triangle;
            if (v == CapStyle.Round) ss |= StrokeType.EC_Round;
            else if (v == CapStyle.Square) ss |= StrokeType.EC_Square;
            else if (v == CapStyle.Triangle) ss |= StrokeType.EC_Triangle;

            ST = (byte)ss;
        }
        internal void SetDashCap(CapStyle v)
        {
            var ss = (StrokeType)ST & ~StrokeType.DC_Triangle;
            var st = (StrokeType)ST & StrokeType.Filled;
            if (st == StrokeType.Dotted && v == CapStyle.Flat) ss |= StrokeType.DC_Square;
            else if (v == CapStyle.Round) ss |= StrokeType.DC_Round;
            else if (v == CapStyle.Square) ss |= StrokeType.DC_Square;
            else if (v == CapStyle.Triangle) ss |= StrokeType.DC_Triangle;

            ST = (byte)ss;
        }
        internal void SetStartCap(CapStyle v)
        {
            var ss = (StrokeType)ST & ~StrokeType.SC_Triangle;

            if (v == CapStyle.Round) ss |= StrokeType.SC_Round;
            else if (v == CapStyle.Square) ss |= StrokeType.SC_Square;
            else if (v == CapStyle.Triangle) ss |= StrokeType.SC_Triangle;

            ST = (byte)ss;
        }
        internal void SetStokeStyle(StrokeStyle v)
        {
            var ss = (StrokeType)ST & ~StrokeType.Filled;

            if (v == StrokeStyle.Filled) ss = StrokeType.Filled;
            else if (v == StrokeStyle.Dotted) ss |= StrokeType.Dotted;
            else if (v == StrokeStyle.Dashed) ss |= StrokeType.Dashed;

            ST = (byte)ss;
        }

        internal (ShapeType, StrokeType, byte) ShapeStrokeWidth() => (ShapeType, (StrokeType)ST, SW);
        protected (ShapeType, StrokeType, byte) ShapeStrokeWidth(float scale)
        {
            var sw = (byte)(SW * scale); // compensate for exagerated size
            if (sw < 1) sw = 1;
            return (ShapeType, (StrokeType)ST, sw);
        }
        internal (byte, byte, byte, byte) ShapeColor(Coloring c = Coloring.Normal) => c == Coloring.Normal ? (A, R, G, B) : c == Coloring.Light ? (_a, R, B, G) : (_a, _g, _g, _g);
        private const byte _g = 0x80;
        private const byte _a = 0x60;

        #region Color  ========================================================
        internal enum Coloring { Gray, Light, Normal };

        internal void SetColor((byte ,byte,byte,byte) color)
        {
            var (a, r, g, b) = color;
            A = a;
            R = r;
            G = g;
            B = b;
        }
        #endregion

        #region Radius  =======================================================
        public static byte ToByte(float v, float L = 0)
        {
            if (v < L) v = L;
            var b = v * 255;
            if (b > 255) b = 255;
            return (byte)b;
        }
        public static sbyte ToSByte(float v)
        {
            if (v < -1) v = -1;
            if (v >  1) v =  1;

            return (sbyte)(v * 127);
        }
        public static float ToFloat(byte b) => b / 255f;
        public static float ToFloat(sbyte s) => s / 127f;

        public static (float, float) ToFloat((sbyte dx, sbyte dy) p) => (ToFloat(p.dx), ToFloat(p.dy));
        public static Vector2 ToVector((sbyte dx, sbyte dy) p) => new Vector2(ToFloat(p.dx), ToFloat(p.dy));
        public static (sbyte dx, sbyte dy) ToSByte(Vector2 p) => (ToSByte(p.X), ToSByte(p.Y));

        protected float Radius1 { get { return ToFloat(R1); } set { R1 = ToByte(value, 0.004f); } }
        protected float Radius2 { get { return ToFloat(R2); } set { R2 = ToByte(value, 0.004f); } }
        protected float AuxFactor { get { return ToFloat(F1); } set { F1 = ToByte(value); } }

        protected Vector2 Radius => new Vector2(Radius1, Radius2);
        protected (float r1, float r2, float f1) GetRadius(float scale) => (Radius1 * scale, Radius2 * scale, AuxFactor * scale);
        protected (float r1, float r2, float f1) GetRadius() => (Radius1, Radius2, AuxFactor);
        #endregion

        #region Dimension  ====================================================
        internal int Dimension { get => PD; set => SetPD(value); }
        private void SetPD(int val)
        {
            var (min, max) = MinMaxDimension;
            PD = (byte)((val < min) ? min : (val > max) ? max : val);
            CreatePoints();
        }
        #endregion

        #region Radians  ======================================================
        protected static float FullRadians = (float)(2 * Math.PI);   //360 degrees
        protected static float DeltaRadians0 = (float)(Math.PI / 8); //22.5 degrees
        protected static float DeltaRadians1 = (float)(Math.PI / 6); //30 degrees
        protected float RotateLeftRadians0 => -DeltaRadians0;
        protected float RotateRightRadians0 => DeltaRadians0;
        protected float RotateLeftRadians1 => -DeltaRadians1;
        protected float RotateRightRadians1 => DeltaRadians1;
        protected float RadiansStart => (A0 % 16) * DeltaRadians0 + (A1 % 12) * DeltaRadians1;

        protected void RotateStartLeft0() { A0 = (byte)((A0 - 1) % 16); }
        protected void RotateStartLeft1() { A1 = (byte)((A1 - 1) % 12); }
        protected void RotateStartRight0() { A0 = (byte)((A0 + 1) % 16); }
        protected void RotateStartRight1() { A1 = (byte)((A1 + 1) % 12); }
        #endregion

        #region Axiss  ======================================================
        internal bool IsLocked { get { return PL != 0; } set { PL = (byte)(value ? 1 : 0); } }
        internal byte AuxAxis { get { return (byte)(100 * AuxFactor); } set { AuxFactor = (float)value / 100; CreatePoints(); } }
        internal byte MajorAxis { get { return (byte)(100 * Radius1); } set { Radius1 = (float)value / 100; CreatePoints(); } }
        internal byte MinorAxis { get { return (byte)(100 * Radius2); } set { Radius2 = (float)value / 100; CreatePoints(); } }
        #endregion

        #endregion

        #region SaveShapes  ===================================================
        internal static byte[] SaveShaptes(IEnumerable<Shape> shapes)
        {
            var (_, _, _, _, _, _, fw, fh) = GetExtent(shapes);
            var data = new List<byte>(shapes.Count() * 30);

            foreach (var shape in shapes)
            {
                data.Add((byte)shape.ShapeType);   // 0

                data.Add(shape.A);          // 1
                data.Add(shape.R);          // 2
                data.Add(shape.G);          // 3
                data.Add(shape.B);          // 4
                data.Add(shape.SW);         // 5
                data.Add(shape.ST);         // 6
                data.Add(shape.R1);         // 7
                data.Add(shape.R2);         // 8
                data.Add(shape.F1);         // 9
                data.Add(shape.PD);         //10
                data.Add(shape.PL);         //11
                data.Add(shape.A0);         //12
                data.Add(shape.A1);         //13

                var points = shape.DXY;
                var count = points.Count;
                if (count > byte.MaxValue) count = byte.MaxValue;

                data.Add((byte)count);      //19

                for (int i = 0; i < count; i++)
                {
                    var p = points[i];
                    data.Add((byte)ToSByte(p.X));
                    data.Add((byte)ToSByte(p.Y));
                }
            }

            return data.ToArray();
        }
        #endregion

        #region LoadShapes  ===================================================
        /// <summary>Load Shapes from Symbol data</summary>
        internal static List<Shape> LoadShapes(byte[] data)
        {
            var shapes = new List<Shape>(10);
            if (data is null || data.Length < 2 ) return shapes;

            var M = data.Length;
            var I = 0;

            while (IsMoreDataAvailable())
            {
                var st = (ShapeType)data[I++];

                switch (st)
                {
                    case ShapeType.Line:
                        ReadData(new Line(true));
                        break;

                    case ShapeType.Circle:
                        ReadData(new Circle(true));
                        break;

                    case ShapeType.Ellipse:
                        ReadData(new Ellipes(true));
                        break;

                    case ShapeType.PolySide:
                        ReadData(new PolySide(true));
                        break;

                    case ShapeType.PolyStar:
                        ReadData(new PolyStar(true));
                        break;

                    case ShapeType.PolyGear:
                        ReadData(new PolyGear(true));
                        break;

                    case ShapeType.PolyWave:
                        ReadData(new PolyWave(true));
                        break;

                    case ShapeType.Rectangle:
                        ReadData(new Rectangle(true));
                        break;

                    case ShapeType.PolySpike:
                        ReadData(new PolySpike(true));
                        break;

                    case ShapeType.PolyPulse:
                        ReadData(new PolyPulse(true));
                        break;

                    case ShapeType.PolySpring:
                        ReadData(new PolySpring(true));
                        break;

                    case ShapeType.RoundedRectangle:
                        ReadData(new RoundedRectangle(true));
                        break;
                    default:
                        return shapes; // stop and disregard invalid shape data
                }
            }
            return shapes;

            bool IsMoreDataAvailable()
            {
                var J = I + PointsOffset;
                if (M > J)
                {
                    var K = data[J];
                    return (K != 0 && M > J + K * 2);
                }
                return false;
            }
            void ReadData(Shape shape)
            {
                shapes.Add(shape);
                shape.A = data[I++];
                shape.R = data[I++];
                shape.G = data[I++];
                shape.B = data[I++];
                shape.SW = data[I++];
                shape.ST = data[I++];
                shape.R1 = data[I++];
                shape.R2 = data[I++];
                shape.F1 = data[I++];
                shape.PD = data[I++];
                shape.PL = data[I++];
                shape.A0 = data[I++];
                shape.A1 = data[I++];
                var pc = data[I++];
                if (pc > 0)
                {
                    shape.DXY = new List<Vector2>(pc);
                    for (int i = 0; i < pc; i++)
                    {
                        var dx = ((sbyte)data[I++]) / 127.0f; //use as much of the sbyte
                        var dy = ((sbyte)data[I++]) / 127.0f; //precision as posible
                        shape.DXY.Add(new Vector2(dx, dy));
                    }
                }
            }
        }
        #endregion

        #region CopyData  =====================================================
        protected void CopyData(Shape s)
        {
            A = s.A;
            R = s.R;
            G = s.G;
            B = s.B;
            SW = s.SW;
            ST = s.ST;
            R1 = s.R1;
            R2 = s.R2;
            F1 = s.F1;
            PD = s.PD;
            PL = s.PL;
            A0 = s.A0;
            A1 = s.A1;
            DXY = new List<Vector2>(s.DXY);
        }
        #endregion
    }
}
