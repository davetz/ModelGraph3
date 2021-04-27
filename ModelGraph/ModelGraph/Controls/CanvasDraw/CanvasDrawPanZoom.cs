using System.Numerics;
using System.Collections.Generic;
using Microsoft.Graphics.Canvas.UI.Xaml;
using ModelGraph.Core;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl
    {
        private const float maxScale = 10;
        private const float minZoomDiagonal = 8000;
        private readonly Dictionary<CanvasControl, (float, Vector2)> CanvasScaleOffset = new Dictionary<CanvasControl, (float, Vector2)>(4);

        private void Pan(Vector2 adder)
        {
        }
        private void Zoom(float changeFactor)
        {
        }
        private void ZoomToExtent()
        {
        }
        private void PanZoomReset()
        {
            if (EditCanvas.IsEnabled)
            {
                SetScaleOffset(EditCanvas, Model.EditorData);
                EditCanvas.Invalidate();
            }
            if (OverCanvas.IsEnabled)
            {
                SetScaleOffset(OverCanvas, Model.EditorData);
                OverCanvas.Invalidate();
            }
            if (Pick1Canvas.IsEnabled)
            {
                SetScaleOffset(Pick1Canvas, Model.Picker1Data);
                Pick1Canvas.Invalidate();
            }
            if (Pick2Canvas.IsEnabled)
            {
                SetScaleOffset(Pick2Canvas, Model.Picker2Data);
                Pick2Canvas.Invalidate();
            }
        }
        private void SetScaleOffset(CanvasControl canvas, IDrawData data)
        {
            if (!canvas.IsEnabled || data is null) return;

            var aw = (float)canvas.ActualWidth;
            var ah = (float)canvas.ActualHeight;

            var e = data.Extent;
            var ew = (float)e.Width;
            var eh = (float)e.Hieght;

            if (aw < 1) aw = 1;
            if (ah < 1) ah = 1;
            if (ew < 1) ew = 1;
            if (eh < 1) eh = 1;

            var zw = aw / ew;
            var zh = ah / eh;
            var z = (zw < zh) ? zw : zh;

            // zoom required to make the view extent fit the canvas
            if (z > maxScale) z = maxScale;

            var ec = new Vector2(e.CenterX, e.CenterY) * z; //center point of scaled view extent
            var ac = eh == 1 ? new Vector2(aw / 2, aw / 2) : new Vector2(aw / 2, ah / 2); //center point of the canvas

            CanvasScaleOffset[canvas] = (z, ac - ec);
        }
    }
}
