using Microsoft.Graphics.Canvas.Geometry;
using ModelGraph.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class ModelSymbolControl : Page, IPageControl, IModelPageControl
    {
        public IPageModel PageModel { get; }
        IDrawModel DrawModel => PageModel.LeadModel as IDrawModel;
        public (int Width, int Height) PreferredSize => throw new System.NotImplementedException();

        #region Constructor  ==================================================
        public ModelSymbolControl(IPageModel model)
        {
            PageModel = model;
            InitializeComponent();
            SymbolCanvas.Initialize(DrawModel);
            SymbolCanvas.SetOverview(32, 32);
            SymbolCanvas.ShowPicker1(32, 32);
            SymbolCanvas.ShowPicker2(32);
            Loaded += ModelSymbolControl_Loaded;
        }

        private void ModelSymbolControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Loaded -= ModelSymbolControl_Loaded;
            SymbolCanvas.SetSideTreeSize(320, 500);
        }
        #endregion

        #region IPageControl  =================================================
        public void Reload()
        {
        }
        public void SaveAs()
        {
        }
        public void Save()
        {
        }
        public void Close()
        {
        }
        public void NewView(IPageModel model)
        {
        }
        #endregion

        #region IModelPageControl  ============================================
        public void Apply()
        {
        }

        public void Revert()
        {
        }

        public void Release()
        {
        }

        public async void Refresh()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, SetButtons);

            SymbolCanvas.Refresh();

            void SetButtons()
            {
                var enb1 = DrawModel.DrawState == DrawState.EditMode;
                var enb2 = DrawModel.DrawState == DrawState.ViewMode && DrawModel.IsPasteActionEnabled;
                CutButton.IsEnabled = enb1;
                CopyButton.IsEnabled = enb1;
                PasteButton.IsEnabled = enb2;
                RecenterButton.IsEnabled = enb1;
                RotateLeftButton.IsEnabled = enb1;
                RotateRightButton.IsEnabled = enb1;
                RotateAngleButton.IsEnabled = enb1;
                FlipVerticalButton.IsEnabled = enb1;
                FlipHorizontalButton.IsEnabled = enb1;
            }
        }

        public void SetSize(double width, double height)
        {
        }
        #endregion

        #region DrawingStyles  ================================================
        public enum Fill_Stroke { Stroke = 0, Filled = 1 }
        public enum Edit_Contact { Edit, Contacts, AutoFlip, }
        public static List<T> GetEnumAsList<T>() { return Enum.GetValues(typeof(T)).Cast<T>().ToList(); }
        public List<CanvasDashStyle> DashStyleList => GetEnumAsList<CanvasDashStyle>();
        public List<CanvasCapStyle> CapStyleList => GetEnumAsList<CanvasCapStyle>();
        public List<CanvasLineJoin> LineJoinList => GetEnumAsList<CanvasLineJoin>();
        public List<Fill_Stroke> FillStrokeList => GetEnumAsList<Fill_Stroke>();
        public List<Edit_Contact> EditContactList => GetEnumAsList<Edit_Contact>();
        public List<Contact> ContactList { get { return GetEnumAsList<Contact>(); } }
        #endregion

        #region Events  =======================================================
        private void RotateLeftButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.RotateLeft);
        private void RotateRightButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.RotateRight);
        private void RotateAngleButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_use30degreeDelta)
            {
                _use30degreeDelta = false;
                RotateAngleButton.Content = "22.5";
                PostEvent(DrawEvent.Angle22);
            }
            else
            {
                _use30degreeDelta = true;
                RotateAngleButton.Content = "30.0";
                PostEvent(DrawEvent.Angle30);
            }
        }
        private void FlipVerticalButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.FlipVertical);
        private void FlipHorizontalButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.FlipHorizontal);

        private void CutButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.Cut);

        private void CopyButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.Copy);

        private void PasteButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.Paste);

        private void ApplyButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.Apply);
        private void ReloadButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.Revert);

        private void RecenterButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => PostEvent(DrawEvent.Recenter);

        private void MovePins_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => DrawModel.TrySetState(DrawState.PinsMode);
        private void EditSelect_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => DrawModel.TrySetState(DrawState.ViewMode);
        private void TermSelect_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => DrawModel.TrySetState(DrawState.LinkMode);
        private void FlipSelect_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => DrawModel.TrySetState(DrawState.OperateMode);


        private List<(float dx, float dy)> _getList = new List<(float dx, float dy)>();
        private List<(float dx, float dy)> _setList = new List<(float dx, float dy)>();
        private bool _use30degreeDelta;

        internal async void PostEvent(DrawEvent evt)
        {
            if (DrawModel.TryGetDrawEventAction(evt, out Action action))
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { action(); });
        }

        #endregion
    }
}
