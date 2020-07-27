using ModelGraph.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Services
{
    public delegate void ViewClosedHandler(ViewLifetimeControl viewControl, EventArgs e);

    // For instructions on using this service see https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/UWP/features/multiple-views.md
    // More details about showing multiple views at https://docs.microsoft.com/windows/uwp/design/layout/show-multiple-views
    public class WindowManagerService
    {
        private static WindowManagerService _current;

        public static WindowManagerService Current => _current ?? (_current = new WindowManagerService());

        // Contains all the opened secondary views.
        public ObservableCollection<ViewLifetimeControl> SecondaryViews { get; } = new ObservableCollection<ViewLifetimeControl>();

        public int MainViewId { get; private set; }

        public CoreDispatcher MainDispatcher { get; private set; }

        #region ModelPageService  =============================================
        public void CloseRelatedModels(IDataModel model)
        {
            var views = SecondaryViews.ToArray();
            foreach (var view in views)
            {
                var m = view.DataModel;

                if (m is null) continue;
                if (m.DataRoot != m.DataRoot) continue;

                view.CloseModel();
            }
        }
        public async Task<ViewLifetimeControl> TryShowAsStandaloneAsync(string windowTitle, Type pageType, IDataModel model)
        {
            ViewLifetimeControl viewControl = await CreateViewLifetimeControlAsync(windowTitle, pageType, model);
            SecondaryViews.Add(viewControl);
            viewControl.StartViewInUse();
            await ApplicationViewSwitcher.TryShowAsViewModeAsync(viewControl.Id, ApplicationViewMode.Default);
            viewControl.StopViewInUse();
            return viewControl;

            //SecondaryViews.Add(viewControl);
            //viewControl.StartViewInUse();
            //await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewControl.Id, ViewSizePreference.Default, ApplicationView.GetForCurrentView().Id, ViewSizePreference.Default);
            //viewControl.StopViewInUse();
            //return viewControl;
        }
        private async Task<ViewLifetimeControl> CreateViewLifetimeControlAsync(string windowTitle, Type pageType, IDataModel model = null)
        {
            ViewLifetimeControl viewControl = null;

            await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                viewControl = ViewLifetimeControl.CreateForCurrentView();
                viewControl.Title = windowTitle;
                viewControl.DataModel = model;
                viewControl.StartViewInUse();
                var frame = new Frame
                {
                    RequestedTheme = ThemeSelectorService.Theme
                };
                frame.Navigate(pageType, viewControl);
                Window.Current.Content = frame;
                Window.Current.Activate();
                ApplicationView.GetForCurrentView().Title = viewControl.Title;
            });

            return viewControl;
        }

        #endregion

        public async Task InitializeAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                MainViewId = ApplicationView.GetForCurrentView().Id;
                MainDispatcher = Window.Current.Dispatcher;
            });
        }

        // Displays a view as a standalone
        // You can use the resulting ViewLifeTileControl to interact with the new window.

        // Displays a view in the specified view mode
        public async Task<ViewLifetimeControl> TryShowAsViewModeAsync(string windowTitle, Type pageType, ApplicationViewMode viewMode = ApplicationViewMode.Default)
        {
            ViewLifetimeControl viewControl = await CreateViewLifetimeControlAsync(windowTitle, pageType);
            SecondaryViews.Add(viewControl);
            viewControl.StartViewInUse();
            await ApplicationViewSwitcher.TryShowAsViewModeAsync(viewControl.Id, viewMode);
            viewControl.StopViewInUse();
            return viewControl;
        }

        private async Task<ViewLifetimeControl> CreateViewLifetimeControlAsync(string windowTitle, Type pageType)
        {
            ViewLifetimeControl viewControl = null;

            await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                viewControl = ViewLifetimeControl.CreateForCurrentView();
                viewControl.Title = windowTitle;
                viewControl.StartViewInUse();
                var frame = new Frame();
                frame.RequestedTheme = ThemeSelectorService.Theme;
                frame.Navigate(pageType, viewControl);
                Window.Current.Content = frame;
                Window.Current.Activate();
                ApplicationView.GetForCurrentView().Title = viewControl.Title;
            });

            return viewControl;
        }

        public bool IsWindowOpen(string windowTitle) => SecondaryViews.Any(v => v.Title == windowTitle);
    }
}
