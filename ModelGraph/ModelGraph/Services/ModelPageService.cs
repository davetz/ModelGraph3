using ModelGraph.Controls;
using ModelGraph.Core;
using ModelGraph.Repository;
using ModelGraph.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace ModelGraph.Services
{
    public class ModelPageService
    {
        public static ModelPageService Current => _current ?? (_current = new ModelPageService());
        private static ModelPageService _current;

        #region Constructor  ==================================================
        private ModelPageService()
        {
            ApplicationView.GetForCurrentView().Consolidated += ModelPageService_Consolidated;
        }

        private void ModelPageService_Consolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            var app = (App)Application.Current;
            app.Exit(); // all ModelPage will close
        }
        #endregion

        #region Dispatch  =====================================================
        internal async Task CreateNewPageAsync(IRootModel model, ControlType ctlType)
        {
            var viewLifetimeControl = await WindowManagerService.Current.TryShowAsStandaloneAsync(model.TitleName, typeof(ModelPage), model).ConfigureAwait(true);
            viewLifetimeControl.Released += ViewLifetimeControl_Released;
        }

        private void ViewLifetimeControl_Released(object sender, EventArgs e)
        {
            if (sender is ViewLifetimeControl ctrl)
            {
                ctrl.Released -= ViewLifetimeControl_Released;
                var modelControl = ctrl.IModel?.PageControl as IModelPageControl;
                modelControl?.Release();
            }
        }
        #endregion

        #region ReloadModel  ==================================================
        public async Task<bool> ReloadModelAsync(IModelPageControl ctrl)
        {
            if (ctrl is null) return false;

            var oldRootModel = ctrl.IModel;
            var oldChef = oldRootModel.DataRoot;
            var repo = oldChef.Repository;

            RemoveModelPage(oldRootModel);
            oldRootModel.Release();

            WindowManagerService.Current.CloseRelatedModels(oldRootModel);

            var rootModel = new RootModel();
            var newChef = rootModel.DataRoot;

            _ = await repo.ReloadAsync(newChef).ConfigureAwait(true);

            await ctrl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                InsertModelPage(rootModel);
            });

            return true;
        }
        #endregion

        #region CreateNewModel  ===============================================
        public async Task<bool> CreateNewModelAsync(CoreDispatcher dispatcher)
        {
            if (dispatcher is null) return false;

            var rootModel = new RootModel();
            var repo = new StorageFileRepo();
            repo.New(rootModel.DataRoot);

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                InsertModelPage(rootModel);
            });

            return true;
        }
        #endregion

        #region OpenModelDataFile  ============================================\
        public async Task<bool> OpenModelDataFileAsync(CoreDispatcher dispatcher)
        {
            if (dispatcher is null) return false;

            var rootModel = new RootModel();
            var repo = new StorageFileRepo();
            bool success = await repo.OpenAsync(rootModel.DataRoot).ConfigureAwait(true);


            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                InsertModelPage(rootModel);
            });

            return success;
        }
        #endregion

        public Action<IRootModel> InsertModelPage { get; set; } //coordination with ShellPage NavigationView
        public Action<IRootModel> RemoveModelPage { get; set; } //coordination with ShellPage NavigationView
    }
}
