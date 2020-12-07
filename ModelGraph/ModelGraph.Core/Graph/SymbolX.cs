﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;

namespace ModelGraph.Core
{
    public class SymbolX : ChildOf<SymbolXRoot>
    {
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        internal byte[] Data;
        internal List<(Target trg, TargetIndex tix, Contact con, (sbyte dx, sbyte dy) pnt, byte siz)> TargetContacts = new List<(Target, TargetIndex, Contact, (sbyte, sbyte), byte)>(4);
        internal Attach Attach;
        internal AutoFlip AutoFlip;

        #region Constructors  =================================================
        public SymbolX(SymbolXRoot owner, bool autoExpand = false)
        {
            Owner = owner;
            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion

        #region <Get,Save>Shapes  =============================================
        internal List<Shape> GetShapes()
        {
            if (_shapes is null) _shapes = Shape.LoadShapes(Data);
            return _shapes;
        }
        internal void SaveShapes()
        {
            Data = (_shapes is null || _shapes.Count == 0) ? null : Shape.SaveShaptes(_shapes);
        }
        internal void ReloadShapes()
        {
            _shapes = null;
            GetShapes();
        }
        private List<Shape> _shapes;
        internal byte AbsoluteSize => Owner.TryGetParent(this, out GraphX p) ? p.SymbolSize : (byte)16;
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.SymbolX;
        public override string GetNameId() => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetSummaryId() => Summary;
        public override string GetDescriptionId() => Description;
        public override Item GetParent() => Owner.TryGetParent(this, out GraphX p) ? p : GetOwner(); 
        #endregion

        #region Properties/Methods  ===========================================
        internal float Width => 1;
        internal float Height => 1;

        #region GetFlipTarget  ================================================
        internal (float sdx, float sdy, byte tsiz, byte tix, Direction tdir) GetFlipTarget(int ti, FlipState flip, float scale)
        {
            var e = TargetContacts[ti];

            var fix = (int)flip;
            var tix = (int)e.tix;
            var (tdx, tdy) = ToFloat(e.pnt);
            var (fdx, fdy) = _flipper[fix](tdx, tdy);
            var sdx = fdx * scale;
            var sdy = fdy * scale;
            var siz = (byte)(e.siz * scale / 255);
            var ftd = _flipDirection[fix][tix];

            return (sdx, sdy, siz, (byte)tix, ftd);
        }
        private static readonly Direction[] _flipNoneDirection =
        {
            Direction.E,   //EN
            Direction.E,   //E
            Direction.E,   //ES
            Direction.SEC, //SEC
            Direction.S,   //SE
            Direction.S,   //S
            Direction.S,   //SW
            Direction.SWC, //SWC
            Direction.W,   //WS
            Direction.W,   //W
            Direction.W,   //WN
            Direction.NWC, //NWC
            Direction.N,   //NW
            Direction.N,   //N
            Direction.N,   //NE
            Direction.NEC, //NEC
        };
        private static readonly Direction[] _vertFlipDirection =
        {
            Direction.E,
            Direction.E,
            Direction.E,
            Direction.NEC,
            Direction.N,
            Direction.N,
            Direction.N,
            Direction.NWC,
            Direction.W,
            Direction.W,
            Direction.W,
            Direction.SWC,
            Direction.S,
            Direction.S,
            Direction.S,
            Direction.SEC,
        };
        private static readonly Direction[] _horzFlipDirection =
        {
            Direction.W,
            Direction.W,
            Direction.W,
            Direction.SWC,
            Direction.S,
            Direction.S,
            Direction.S,
            Direction.SEC,
            Direction.E,
            Direction.E,
            Direction.E,
            Direction.NEC,
            Direction.N,
            Direction.N,
            Direction.N,
            Direction.NWC,
        };
        private static readonly Direction[] _bothFlipDirection =
        {
            Direction.W,
            Direction.W,
            Direction.W,
            Direction.NEC,
            Direction.N,
            Direction.N,
            Direction.N,
            Direction.NWC,
            Direction.E,
            Direction.E,
            Direction.E,
            Direction.SWC,
            Direction.S,
            Direction.S,
            Direction.S,
            Direction.SEC,
        };
        private static readonly Direction[] _rotateLeftDirection =
        {
            Direction.N,
            Direction.N,
            Direction.N,
            Direction.NEC,
            Direction.E,
            Direction.E,
            Direction.E,
            Direction.SEC,
            Direction.S,
            Direction.S,
            Direction.S,
            Direction.SWC,
            Direction.W,
            Direction.W,
            Direction.W,
            Direction.NWC,
        };
        private static readonly Direction[] _leftHorzDirection =
        {
            Direction.S,
            Direction.S,
            Direction.S,
            Direction.NWC,
            Direction.E,
            Direction.E,
            Direction.E,
            Direction.SWC,
            Direction.N,
            Direction.N,
            Direction.N,
            Direction.SEC,
            Direction.W,
            Direction.W,
            Direction.W,
            Direction.NEC,
        };
        private static readonly Direction[] _rotateRightDirection =
        {
            Direction.S,
            Direction.S,
            Direction.S,
            Direction.SWC,
            Direction.W,
            Direction.W,
            Direction.W,
            Direction.NWC,
            Direction.N,
            Direction.N,
            Direction.N,
            Direction.NEC,
            Direction.E,
            Direction.E,
            Direction.E,
            Direction.SEC,
        };
        private static readonly Direction[] _rightHorzDirection =
        {
            Direction.N,
            Direction.N,
            Direction.N,
            Direction.SEC,
            Direction.W,
            Direction.W,
            Direction.W,
            Direction.NEC,
            Direction.S,
            Direction.S,
            Direction.S,
            Direction.NWC,
            Direction.E,
            Direction.E,
            Direction.E,
            Direction.SWC,
        };
        private static readonly Direction[][] _flipDirection =
        {
            _flipNoneDirection,
            _vertFlipDirection,
            _horzFlipDirection,
            _bothFlipDirection,
            _rotateLeftDirection,
            _leftHorzDirection,
            _rotateRightDirection,
            _rightHorzDirection,
        };
        #endregion

        #region GetFlipTargetContacts  ========================================
        internal void GetFlipTargetContacts(FlipState flip, float cx, float cy, float scale, float tmLen, List<(Target trg, byte tix, Contact con, Vector2 pnt)> list)
        {
            list.Clear();
            foreach (var (trg, tix, con, pnt, siz) in TargetContacts)
            {
                var fix = (int)flip;
                var (tdx, tdy) = ToFloat(pnt);
                var (fdx, fdy) = _flipper[fix](tdx, tdy);

                var (tx, ty) = _targetSurface[(int)tix];
                var (dx, dy) = _flipper[fix](tx, ty);

                var x = fdx * scale + cx + dx * tmLen;
                var y = fdy * scale + cy + dy * tmLen;

                list.Add((trg, (byte)tix, con, new Vector2(x, y)));
            }           
        }
        private const float Q = 0.7071067811865f; // 1 / SQRT(2)
        private static readonly (float dx, float dy)[] _targetSurface =
        {
            (1, 0),  //EN
            (1, 0),  //E
            (1, 0),  //ES
            (Q, Q), //SEC
            (0, 1),  //SE
            (0, 1),  //S
            (0, 1),  //SW
            (-Q, Q),  //SWC
            (-1, 0),  //WS
            (-1, 0),  //W
            (-1, 0),  //WN
            (-Q, -Q), //NWC
            (0, -1),  //NW
            (0, -1),  //N
            (0, -1),  //NE
            (Q, -Q),  //NEC
        };

        public void GetTargetContacts(Dictionary<Target, (Contact contact, (sbyte dx, sbyte dy) point, byte size)> dict)
        {
            dict.Clear();
            foreach (var (trg, _, con, pnt, siz) in TargetContacts)
            {
                dict[trg] = (con, pnt, siz);
            }
        }
        public void SetTargetContacts(Dictionary<Target, (Contact contact, (sbyte dx, sbyte dy) point, byte size)> dict)
        {
            TargetContacts.Clear();
            foreach (var e in dict)
            {
                TargetContacts.Add((e.Key, GetTargetIndex(e.Key), e.Value.contact, e.Value.point, e.Value.size));
            }
            TargetContacts.Sort((u,v) => (u.tix < v.tix) ? -1 : (u.tix > v.tix) ? 1 : 0);
        }
        public static TargetIndex GetTargetIndex(Target tg)
        {
            if (tg == Target.E) return TargetIndex.E;
            if (tg == Target.W) return TargetIndex.W;
            if (tg == Target.N) return TargetIndex.N;
            if (tg == Target.S) return TargetIndex.S;

            if (tg == Target.EN) return TargetIndex.EN;
            if (tg == Target.ES) return TargetIndex.ES;
            if (tg == Target.WS) return TargetIndex.WS;
            if (tg == Target.WN) return TargetIndex.WN;

            if (tg == Target.NW) return TargetIndex.NW;
            if (tg == Target.NE) return TargetIndex.NE;
            if (tg == Target.SE) return TargetIndex.SE;
            if (tg == Target.SW) return TargetIndex.SW;

            if (tg == Target.SEC) return TargetIndex.SEC;
            if (tg == Target.SWC) return TargetIndex.SWC;
            if (tg == Target.NWC) return TargetIndex.NWC;
            if (tg == Target.NEC) return TargetIndex.NEC;
            return 0;
        }
        private static (float u, float v) ToFloat((sbyte dx, sbyte dy) p) => (p.dx / 127f, p.dy / 127f);
        private static (float, float) ToNone(float x, float y) => (x, y);
        private static (float, float) ToVertFlip(float x, float y) => (x, -y);
        private static (float, float) ToHorzFlip(float x, float y) => (-x, y);
        private static (float, float) ToVertHorzFlip(float x, float y) => (-x, -y);
        private static (float, float) ToLeftRotate(float x, float y) => (y, -x);
        private static (float, float) ToLeftHorzFlip(float x, float y) => (-y, -x);
        private static (float, float) ToRightRotate(float x, float y) => (-y, x);
        private static (float, float) ToRightHorzFlip(float x, float y) => (y, x);

        private static Func<float, float, (float x, float y)>[] _flipper = { ToNone, ToVertFlip, ToHorzFlip, ToVertHorzFlip, ToLeftRotate, ToLeftHorzFlip, ToRightRotate, ToRightHorzFlip };
        #endregion

        #region GetFlipTargetPenalty  =========================================
        internal byte[][] GetFlipTargetPenalty(FlipState flip) => _flipTargetPenalty[(int)flip];
        internal static byte MaxPenalty => 5;
        internal static float[] PenaltyFactor = { 1, 1.1f, 1.2f, 1.3f, 1.4f, 100f };
        //=== non-flipped normal penalty of each dx/dy directional sector to the symbol's target contact point
        //========== dx/dy directional sector  0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F ================
        private static readonly byte[] _ES  = { 0, 0, 1, 2, 3, 5, 5, 5, 5, 5, 5, 5, 5, 3, 2, 1 }; //ES
        private static readonly byte[] _SEC = { 1, 0, 0, 1, 2, 3, 4, 5, 5, 5, 5, 5, 5, 4, 3, 2 }; //SEC  
        private static readonly byte[] _SE  = { 2, 1, 0, 0, 1, 2, 3, 4, 5, 5, 5, 5, 5, 5, 5, 3 }; //SE
        private static readonly byte[] _S   = { 3, 2, 1, 0, 0, 1, 2, 3, 5, 5, 5, 5, 5, 5, 5, 5 }; //S
        private static readonly byte[] _SW  = { 4, 3, 2, 1, 0, 0, 1, 2, 3, 5, 5, 5, 5, 5, 5, 5 }; //SW
        private static readonly byte[] _SWC = { 5, 4, 3, 2, 1, 0, 0, 1, 2, 3, 4, 5, 5, 5, 5, 5 }; //SWC  
        private static readonly byte[] _WS  = { 5, 5, 5, 3, 2, 1, 0, 0, 1, 2, 3, 4, 5, 5, 5, 5 }; //WS
        private static readonly byte[] _W   = { 5, 5, 5, 5, 3, 2, 1, 0, 0, 1, 2, 3, 5, 5, 5, 5 }; //W
        private static readonly byte[] _WN  = { 5, 5, 5, 5, 5, 3, 2, 1, 0, 0, 1, 2, 3, 5, 5, 5 }; //WN
        private static readonly byte[] _NWC = { 5, 5, 5, 5, 5, 4, 3, 2, 1, 0, 0, 1, 2, 3, 4, 5 }; //NWC  
        private static readonly byte[] _NW  = { 5, 5, 5, 5, 5, 5, 5, 3, 2, 1, 0, 0, 1, 2, 3, 5 }; //NW
        private static readonly byte[] _N   = { 5, 5, 5, 5, 5, 5, 5, 5, 3, 2, 1, 0, 0, 1, 2, 3 }; //N
        private static readonly byte[] _NE  = { 3, 5, 5, 5, 5, 5, 5, 5, 5, 3, 2, 1, 0, 0, 1, 2 }; //NE 
        private static readonly byte[] _NEC = { 2, 3, 4, 5, 5, 5, 5, 5, 5, 4, 3, 2, 1, 0, 0, 1 }; //NEC 
        private static readonly byte[] _EN  = { 1, 2, 3, 5, 5, 5, 5, 5, 5, 5, 5, 3, 2, 1, 0, 0 }; //EN
        private static readonly byte[] _E   = { 0, 1, 2, 3, 5, 5, 5, 5, 5, 5, 5, 5, 3, 2, 1, 0 }; //E

        //==============================  EN   E   ES     SEC   SE   S   SW   SWC     WS   W   WN     NWC   NW   N   NE   NEC
        private static byte[][] _pNo = { _EN, _E, _ES,   _SEC, _SE, _S, _SW, _SWC,   _WS, _W, _WN,   _NWC, _NW, _N, _NE, _NEC };//
        private static byte[][] _pVF = { _ES, _E, _EN,   _NEC, _NE, _N, _NW, _NWC,   _WN, _W, _WS,   _SWC, _SW, _S, _SE, _SEC };//
        private static byte[][] _pHF = { _WN, _W, _WS,   _SWC, _SW, _S, _SE, _SEC,   _ES, _E, _EN,   _NEC, _NE, _N, _NW, _NWC };//
        private static byte[][] _pVH = { _WS, _W, _WN,   _NWC, _NW, _N, _NE, _NEC,   _EN, _E, _ES,   _SEC, _SE, _S, _SW, _SWC };//
        private static byte[][] _pLR = { _NW, _N, _NE,   _NEC, _EN, _E, _EN, _NEC,   _NE, _N, _NW,   _NWC, _WN, _W, _WS, _NWC };//
        private static byte[][] _pLH = { _SW, _S, _SE,   _SEC, _ES, _E, _EN, _NEC,   _NE, _N, _NW,   _NWC, _WN, _W, _WS, _SWC };//
        private static byte[][] _pRR = { _SE, _S, _SW,   _SWC, _WS, _S, _WN, _NWC,   _NW, _N, _NE,   _NEC, _EN, _E, _ES, _SEC };//
        private static byte[][] _pRH = { _NE, _N, _NW,   _NWC, _WN, _W, _WS, _SWC,   _SW, _S, _SE,   _SEC, _ES, _E, _EN, _NEC };//

        private static byte[][][] _flipTargetPenalty = { _pNo, _pVF, _pHF, _pVH, _pLR, _pLH, _pRR, _pRH };
        #endregion

        #endregion
    }
}
