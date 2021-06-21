using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ModelGraph.Core
{
    internal abstract partial class Shape
    {
        private byte A = 0xFF;  // of color(A, R, G, B)
        private byte R = 0xFF;  // of color(A, R, G, B)
        private byte G = 0xFF;  // of color(A, R, G, B)
        private byte B = 0xFF;  // of color(A, R, G, B)
        protected byte SW = 1;  // stroke width
        private byte ST = 4;    // stroke type
        private byte R1;        // radius 1 for circles and squares
        private byte R2;        // radius 2 for rectangles and ellipses
        private byte F1;        // clipping factor
        private byte PD = 1;    // polyline dimension specific to the shape type
        private byte PL;        // the polyline control parameters are locked
        private byte AI;        // rotation angle index
        private sbyte OX;       // offset delta X
        private sbyte OY;       // offset delta Y
        protected List<Vector2> DXY;  // one or more defined points

        #region Properties  ===================================================

        #region Stroke  =======================================================
        internal void SetStrokeWidth(byte sw) => SW = sw;
        internal void SetStokeStyle(StrokeStyle v)
        {
            var ss = (StrokeType)ST & ~StrokeType.Filled;

            if (v == StrokeStyle.Filled) ss = StrokeType.Filled;
            else if (v == StrokeStyle.Dotted) ss |= StrokeType.Dotted;
            else if (v == StrokeStyle.Dashed) ss |= StrokeType.Dashed;

            ST = (byte)ss;
        }
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

        internal (ShapeType, StrokeType, byte) ShapeStrokeWidth() => (ShapeType, (StrokeType)ST, SW);
        protected (ShapeType, StrokeType, byte) ShapeStrokeWidth(float scale)
        {
            var sw = (byte)(SW * scale); // compensate for exagerated size
            if (sw < 1) sw = 1;
            return (ShapeType, (StrokeType)ST, sw);
        }
        protected ShapeProperty StrokePropertyFlags()
        {
            var ss = (StrokeType)ST & StrokeType.Filled;
            if (ss == StrokeType.Filled) return ShapeProperty.StrokeStyle;
            if (ss == StrokeType.Dashed || ss == StrokeType.Dotted) return (ShapeProperty.StrokeStyle | ShapeProperty.StartCap | ShapeProperty.DashCap | ShapeProperty.EndCap | ShapeProperty.StrokeWidth) & ValidLineProperty;
            return (ShapeProperty.StrokeStyle | ShapeProperty.StartCap | ShapeProperty.EndCap | ShapeProperty.StrokeWidth) & ValidLineProperty;
        }
        protected virtual ShapeProperty ValidLineProperty => ShapeProperty.StrokeStyle | ShapeProperty.StartCap | ShapeProperty.DashCap | ShapeProperty.EndCap | ShapeProperty.StrokeWidth;
        #endregion

        #region Color  ========================================================
        internal enum Coloring { Gray, Light, Normal };

        internal (byte, byte, byte, byte) ShapeColor(Coloring c = Coloring.Normal) => c == Coloring.Normal ? (A, R, G, B) : c == Coloring.Light ? (_a, R, B, G) : (_a, _g, _g, _g);
        private const byte _g = 0x80;
        private const byte _a = 0x60;

        internal void SetColor((byte, byte, byte, byte) color)
        {
            var (a, r, g, b) = color;
            A = a;
            R = r;
            G = g;
            B = b;
        }
        #endregion

        #region Radius  =======================================================
        protected float Factor1 { get { return ToFloat(F1); } set { F1 = ToByte(value); } }
        protected float Radius1 { get { return ToFloat(R1); } set { R1 = ToByte(value, 0.004f); } }
        protected float Radius2 { get { return ToFloat(R2); } set { R2 = ToByte(value, 0.004f); } }

        protected Vector2 Radius => new Vector2(Radius1, Radius2);
        #endregion

        #region Center  =======================================================
        internal float CenterX { get => ToFloat(OX); set => OX = ToSByte(value); }
        internal float CenterY { get => ToFloat(OY); set => OX = ToSByte(value); }
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

        #region Rotation  =====================================================
        private static int ValidAngle(byte v) => v % 192;
        private const double AngleIncrement = Math.PI / 96; // 1.875 degrees (common denominator for 15 and 22.5 degress)
        protected float Angle => (float)(AngleIncrement * ValidAngle(AI));

        // defunct legacy - TO BE REMOVED
        protected static float DeltaRadiansA0 = (float)(Math.PI / 8); //22.5 degrees
        protected static float DeltaRadiansA1 = (float)(Math.PI / 6); //30 degrees
        protected float RotateLeftRadians0 => -DeltaRadiansA0;
        protected float RotateRightRadians0 => DeltaRadiansA0;
        protected float RotateLeftRadians1 => -DeltaRadiansA1;
        protected float RotateRightRadians1 => DeltaRadiansA1;

        protected void RotateStartLeftA1() { AI = (byte)((AI - 1) % 12); }
        protected void RotateStartRightA1() { AI = (byte)((AI + 1) % 12); }
        #endregion

        #region ConvertDataValues  ============================================
        internal static byte ToByte(float v, float L = 0)
        {
            if (v < L) v = L;
            var b = v * 255;
            if (b > 255) b = 255;
            return (byte)b;
        }
        internal static sbyte ToSByte(float v)
        {
            if (v < -1) v = -1;
            if (v > 1) v = 1;

            return (sbyte)(v * 127);
        }
        internal static float ToFloat(byte b) => b / 255f;
        internal static float ToFloat(sbyte s) => s / 127f;

        // for converting shape point display values (-1 , 1) <=> (-128, 128)
        const short LNS = -128;
        const short LPS = 128;
        internal static float ToFloat(short d) => d < -128 ? -1f : d > 128f ? 1f : d / 128;
        internal static short ToShort(float f) => f < -1 ? LNS : f > 1 ? LPS : (short)Math.Round(f * 128);

        // for converting shape point display values (0 , 1) <=> (0, 256)
        const ushort LNUS = 0;
        const ushort LPUS = 256;
        internal static float ToFloat(ushort d) => d > 256f ? 1f : d / 256;
        internal static ushort ToUShort(float f) => f < -1 ? LNUS : f > 1 ? LPUS : (ushort)Math.Round(f * 256);
        #endregion
        #endregion

        #region SaveShapes  ===================================================
        internal static byte[] SaveShaptes(IEnumerable<Shape> shapes)
        {
            var data = new List<byte>(shapes.Count() * 30);

            foreach (var s in shapes)
            {
                var TY = s.ShapeType;
                var XY = s.DXY;
                var PC = XY.Count;
                if (PC > byte.MaxValue) PC = byte.MaxValue;

                data.Add((byte)TY);     // 0
                data.Add((byte)PC);     // 1
                data.Add(s.A);          // 2
                data.Add(s.R);          // 3
                data.Add(s.G);          // 4
                data.Add(s.B);          // 5
                data.Add(s.SW);         // 6
                data.Add(s.ST);         // 7
                data.Add(s.R1);         // 8
                data.Add(s.R2);         // 9

                if (TY > ShapeType.SimpleShapeMask)
                {
                    data.Add((byte)s.F1);   //10
                    data.Add(s.PD);         //12
                    data.Add(s.PL);         //13
                    data.Add(s.AI);         //14
                    data.Add((byte)s.OX);   //15
                    data.Add((byte)s.OY);   //16
                }

                foreach(var p in XY)
                {
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
            if (data is null || data.Length < 2) return shapes;

            var L = data.Length - 2;
            var P = L - 6;
            var Q = P - 6;
            var I = 0;
            Shape s = null;

            while (I < L)
            {
                var st = (ShapeType)data[I++];
                var pc = data[I++];
                var N = I + 2 * pc;

                // ensure we don't overrun the data array 
                if (st < ShapeType.SimpleShapeMask && N > P) break;
                else if (st > ShapeType.SimpleShapeMask && N > Q) break;

                switch (st)
                {
                    case ShapeType.Line: s = new Line(); break;
                }
                shapes.Add(s);

                // read stroke and simple shape parameters
                s.A = data[I++];
                s.R = data[I++];
                s.G = data[I++];
                s.B = data[I++];
                s.SW = data[I++];
                s.ST = data[I++];
                s.R1 = data[I++];
                s.R2 = data[I++];

                if (st > ShapeType.SimpleShapeMask)
                {
                    //read complex shape parameters
                    s.F1 = data[I++];
                    s.PD = data[I++];
                    s.PL = data[I++];
                    s.AI = data[I++];
                    s.OX = (sbyte)data[I++];
                    s.OY = (sbyte)data[I++];
                }

                if (pc > 0)
                {
                    // read point data, if any
                    s.DXY = new List<Vector2>(pc);
                    for (int j = 0; j < pc; j++)
                    {
                        var dx = ((sbyte)data[I++]) / 127.0f; //use as much of the sbyte
                        var dy = ((sbyte)data[I++]) / 127.0f; //precision as posible
                        s.DXY.Add(new Vector2(dx, dy));
                    }
                }
                else
                {
                    s.DXY = new List<Vector2>();
                    s.CreatePoints();
                }
            }
            return shapes;
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
            AI = s.AI;
            OX = s.OX;
            OY = s.OY;
            DXY = new List<Vector2>(s.DXY);
        }
        #endregion
    }
}
