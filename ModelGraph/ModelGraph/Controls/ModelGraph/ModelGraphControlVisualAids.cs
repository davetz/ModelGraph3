﻿using ModelGraph.Core;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class ModelGraphControl
    {
        #region Tooltip  ======================================================
        private bool _isToolTipVisible;

        private void ShowNodeTooltip(Node node)
        {
            if (node != null)
            {
                ItemToolTip.Text = node.Item.GetDoubleNameId();
                ShowTooltip();
            }
        }
        private void ShowEdgeTooltip(Edge edge)
        {
            ItemToolTip.Text = $"({edge.Node1.Item.GetDoubleNameId()})  -->  ({edge.Node2.Item.GetDoubleNameId()})";
            ShowTooltip();
        }
        private void ShowTooltip()
        {
            var ds = ItemToolTip.Text.Length * 4;
            var x = _rootRef.Point2.X - ds;
            var y = _rootRef.Point2.Y - 40;

            Canvas.SetTop(ToolTipBorder, y);
            Canvas.SetLeft(ToolTipBorder, x);
            ToolTipBorder.Visibility = Windows.UI.Xaml.Visibility.Visible;
            _isToolTipVisible = true;
        }
        private void HideTootlip()
        {
            if (_isToolTipVisible)
            {
                _isToolTipVisible = false;
                ToolTipBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //_model.GraphicController.HideLocator(this);
            }
        }
        #endregion

        #region RemoveSelectors  ==============================================
        private void RemoveSelectors()
        {
            _selector.Clear();
            EditorCanvas.Invalidate();
        }
        #endregion

    }
}
