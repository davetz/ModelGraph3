﻿
using ModelGraph.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public interface IModelPageControl
    {
        (int Width, int Height) PreferredSize { get; }
        void Apply();
        void Revert();
        void Release();
        void RefreshAsync();
        void SetSize(double width, double height);

        CoreDispatcher Dispatcher { get; }
        IPageModel PageModel {get;}
    }
}
