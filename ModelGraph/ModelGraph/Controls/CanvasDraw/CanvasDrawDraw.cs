using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using ModelGraph.Core;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl
    {
        private CanvasStrokeStyle StrokeStyle(StrokeType s)
        {
            var ss = _strokeStyle;
            if (s != Core.StrokeType.Filled)
            {
                var sc = s & Core.StrokeType.SC_Triangle;
                var dc = s & Core.StrokeType.DC_Triangle;
                var ec = s & Core.StrokeType.EC_Triangle;
                var ds = s & Core.StrokeType.Filled;

                ss.EndCap = ec == Core.StrokeType.EC_Round ? CanvasCapStyle.Round : ec == Core.StrokeType.EC_Square ? CanvasCapStyle.Square : ec == Core.StrokeType.EC_Triangle ? CanvasCapStyle.Triangle : CanvasCapStyle.Flat;
                ss.DashCap = dc == Core.StrokeType.DC_Round ? CanvasCapStyle.Round : dc == Core.StrokeType.DC_Square ? CanvasCapStyle.Square : dc == Core.StrokeType.DC_Triangle ? CanvasCapStyle.Triangle : CanvasCapStyle.Flat;
                ss.StartCap = sc == Core.StrokeType.SC_Round ? CanvasCapStyle.Round : sc == Core.StrokeType.SC_Square ? CanvasCapStyle.Square : sc == Core.StrokeType.SC_Triangle ? CanvasCapStyle.Triangle : CanvasCapStyle.Flat;
                ss.DashStyle = ds == Core.StrokeType.Dotted ? CanvasDashStyle.Dot : ds == Core.StrokeType.Dashed ? CanvasDashStyle.Dash : CanvasDashStyle.Solid;
                ss.LineJoin = CanvasLineJoin.Round;
            }
            return ss;
        }
        private CanvasStrokeStyle _strokeStyle = new CanvasStrokeStyle();

        private void DrawCanvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (_isFirstCall) { _isFirstCall = false; PanZoomReset(); }

            if (sender == EditCanvas)
            {
                Draw(Model.HelperData);
                Draw(Model.EditorData);
                if (SelectorGrid.Visibility == Visibility.Visible)
                    UpdateSelectorGrid();
            }
            else if (sender == OverCanvas) Draw(Model.EditorData);
            else if (sender == Pick1Canvas) Draw(Model.Picker1Data);
            else if (sender == Pick2Canvas) Draw(Model.Picker2Data);

            void Draw(IDrawData data)
            {
                if (data is null) return;

                var (scale, offset) = CanvasScaleOffset[sender];
                var ds = args.DrawingSession;

                foreach (var (P, (K, S, W), (A, R, G, B)) in data.Parms)
                {
                    var isFilled = S == StrokeType.Filled;
                    var V = W * scale;
                    if (V < 1) V = 1;
                    if (K < ShapeType.SimpleShapeMask)
                    {
                        var color = Color.FromArgb(A, R, G, B);
                        var stroke = StrokeStyle(S);
                        switch (K)
                        {
                            case ShapeType.Line:
                                for (int i = 0; i < P.Length; i += 2)
                                {
                                    var a = P[i] * scale + offset;
                                    var b = P[i + 1] * scale;
                                    ds.DrawLine(a, b + offset, color, V, stroke);
                                }
                                break;
                            case ShapeType.Circle:
                                if (isFilled)
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        ds.FillCircle(a, b.X, color);
                                    }
                                else
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        ds.DrawCircle(a, b.X, color, V, stroke);
                                    }
                                break;
                            case ShapeType.Ellipse:
                                if (isFilled)
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        ds.FillEllipse(a, b.X, b.Y, color);
                                    }
                                else
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        ds.DrawEllipse(a, b.X, b.Y, color, V, stroke);
                                    }
                                break;
                            case ShapeType.EqualRect:
                                var d = P[0] * scale;
                                if (isFilled)
                                    for (int i = 1; i < P.Length; i++)
                                    {
                                        var a = P[i] * scale + offset;
                                        ds.FillRectangle(a.X, a.Y, d.X, d.Y, color);
                                    }
                                else
                                    for (int i = 1; i < P.Length; i++)
                                    {
                                        var a = P[i] * scale + offset;
                                        ds.DrawRectangle(a.X, a.Y, d.X, d.Y, color, V, stroke);
                                    }
                                break;
                            case ShapeType.CornerRect:
                                if (isFilled)
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        ds.FillRectangle(a.X, a.Y, b.X, b.Y, color);
                                    }
                                else
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        ds.DrawRectangle(a.X, a.Y, b.X, b.Y, color, V, stroke);
                                    }
                                break;
                            case ShapeType.CenterRect:
                                if (isFilled)
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        var e = a - b;
                                        var f = 2 * b;
                                        ds.FillRectangle(e.X, e.Y, f.X, f.Y, color);
                                    }
                                else
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        var e = a - b;
                                        var f = 2 * b;
                                        ds.DrawRectangle(e.X, e.Y, f.X, f.Y, color, V, stroke);
                                    }
                                break;
                            case ShapeType.RoundedRect:
                                var r = 8 * scale;
                                if (isFilled)
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        var e = a - b;
                                        var f = 2 * b;
                                        ds.FillRoundedRectangle(e.X, e.Y, f.X, f.Y, r, r, color);
                                    }
                                else
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        var e = a - b;
                                        var f = 2 * b;
                                        ds.DrawRoundedRectangle(e.X, e.Y, f.X, f.Y, r, r, color, V, stroke);
                                    }
                                break;
                            case ShapeType.Pin2:
                                {
                                    var b = 2 * scale;
                                    if (isFilled)
                                        for (int i = 0; i < P.Length; i++)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.FillCircle(a, b, color);
                                            ds.DrawCircle(a, b + V, Colors.Black, V, stroke);
                                        }
                                    else
                                        for (int i = 0; i < P.Length; i++)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.DrawCircle(a, b - V, Colors.Black, V, stroke);
                                            ds.DrawCircle(a, b, color, V, stroke);
                                            ds.DrawCircle(a, b + V, Colors.Black, V, stroke);
                                        }
                                }
                                break;
                            case ShapeType.Pin4:
                                {
                                    var b = 4 * scale;
                                    if (isFilled)
                                        for (int i = 0; i < P.Length; i++)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.FillCircle(a, b, color);
                                            ds.DrawCircle(a, b + V, Colors.Black, V, stroke);
                                        }
                                    else
                                        for (int i = 0; i < P.Length; i++)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.DrawCircle(a, b - V, Colors.Black, V, stroke);
                                            ds.DrawCircle(a, b, color, V, stroke);
                                            ds.DrawCircle(a, b + V, Colors.Black, V, stroke);
                                        }
                                }
                                break;
                            case ShapeType.Grip2:
                                {
                                    var e = 2 * scale;
                                    var f = 2 * e;
                                    if (isFilled)
                                        for (int i = 0; i < P.Length; i++)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.FillRectangle(a.X - e, a.Y - e, f, f, color);
                                        }
                                    else
                                        for (int i = 0; i < P.Length; i += 2)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.DrawRectangle(a.X - e, a.Y - e, f, f, color, V, stroke);
                                        }
                                }
                                break;
                            case ShapeType.Grip4:
                                {
                                    var e = 4 * scale;
                                    var f = 2 * e;
                                    if (isFilled)
                                        for (int i = 0; i < P.Length; i++)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.FillRectangle(a.X - e, a.Y - e, f, f, color);
                                        }
                                    else
                                        for (int i = 0; i < P.Length; i += 2)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.DrawRectangle(a.X - e, a.Y - e, f, f, color, V, stroke);
                                        }
                                }
                                break;
                        }
                    }
                    else
                    {
                        using (var pb = new CanvasPathBuilder(ds))
                        {
                            pb.BeginFigure(P[0] * scale + offset);
                            if ((K & ShapeType.JointedLines) != 0 || (K & ShapeType.ClosedLines) != 0)
                            {
                                for (int i = 1; i < P.Length; i++)
                                {
                                    pb.AddLine(P[i] * scale + offset);
                                }
                            }
                            else
                            {
                                var N = P.Length;
                                for (var i = 0; i < N - 2;)
                                {
                                    pb.AddCubicBezier(P[i] * scale + offset, P[++i] * scale + offset, P[++i] * scale + offset);
                                }
                            }
                            if ((K & ShapeType.ClosedLines) != 0)
                                pb.EndFigure(CanvasFigureLoop.Closed);
                            else
                                pb.EndFigure(CanvasFigureLoop.Open);

                            using (var geo = CanvasGeometry.CreatePath(pb))
                            {
                                if (isFilled)
                                    ds.FillGeometry(geo, Color.FromArgb(A, R, G, B));
                                else
                                    ds.DrawGeometry(geo, Color.FromArgb(A, R, G, B), V, StrokeStyle(S));
                            }
                        }
                    }
                }

                foreach (var ((P, T), (A, R, G, B)) in data.Text)
                {
                    var p = P * scale + offset;
                    ds.DrawText(T, p, Color.FromArgb(A, R, G, B));
                }
            }
        }
        private bool _isFirstCall = true;
    }
}
