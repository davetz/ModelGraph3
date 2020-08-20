using ModelGraph.Controls;
using ModelGraph.Core;
using ModelGraph.Repository;
using ModelGraph.Views;
using System;
using System.Threading.Tasks;
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
        internal async Task CreateNewPageAsync(IDataModel model)
        {
            var viewLifetimeControl = await WindowManagerService.Current.TryShowAsStandaloneAsync(model.TitleName, typeof(ModelPage), model);
            viewLifetimeControl.Released += ViewLifetimeControl_Released;
        }

        private void ViewLifetimeControl_Released(object sender, EventArgs e)
        {
            if (sender is ViewLifetimeControl ctrl)
            {
                ctrl.Released -= ViewLifetimeControl_Released;
                var modelControl = ctrl.DataModel?.PageControl as IModelPageControl;
                modelControl?.Release();
            }
        }
        #endregion

        #region SaveModel  ====================================================
        public async Task<bool> SaveModel(IModelPageControl ctrl)
        {
            if (ctrl is null) return false;
            var root = ctrl.DataModel.GetRoot();
            if (root.Repository is StorageFileRepo repo)
                _ = await repo.SaveAsync(root);
            return false;
        }
        #endregion

        #region SaveModelAs  ==================================================
        public async Task<bool> SaveModelAs(IModelPageControl ctrl)
        {
            if (ctrl is null) return false;
            var root = ctrl.DataModel.GetRoot();
            if (root.Repository is StorageFileRepo repo)
                _ = await repo.SaveAsAsync(root);
            return false;
        }
        #endregion

        #region CloseModel  ==================================================
        public void CloseModel(IModelPageControl ctrl)
        {
            if (ctrl is null) return;
            var model = ctrl.DataModel;
            if (model.GetRoot().Repository is StorageFileRepo repo)
            {
                RemoveModelPage(model);
                model.Release();

                WindowManagerService.Current.CloseRelatedModels(model);
            }
        }
        #endregion

        #region ReloadModel  ==================================================
        public async Task<bool> ReloadModel(IModelPageControl ctrl)
        {
            if (ctrl is null) return false;
            var oldModel = ctrl.DataModel;
            if (oldModel.GetRoot().Repository is StorageFileRepo repo)
            {
                RemoveModelPage(oldModel);
                oldModel.Release();

                WindowManagerService.Current.CloseRelatedModels(oldModel);

                var rootModel = new RootModel();
                var newRoot = rootModel.GetRoot();

                _ = await repo.ReloadAsync(newRoot).ConfigureAwait(true);

                await ctrl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    InsertModelPage(rootModel);
                });
            }
            return false;
        }
        #endregion

        #region CreateNewModel  ===============================================
        public async Task<bool> CreateNewModelAsync(CoreDispatcher dispatcher)
        {
            if (dispatcher is null) return false;

            var rootModel = new RootModel();
            var repo = new StorageFileRepo();
            repo.New(rootModel.GetRoot());

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
            bool success = await repo.OpenAsync(rootModel.GetRoot()).ConfigureAwait(true);


            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                InsertModelPage(rootModel);
            });

            return success;
        }
        #endregion

        public Action<IDataModel> InsertModelPage { get; set; } //coordination with ShellPage NavigationView
        public Action<IDataModel> RemoveModelPage { get; set; } //coordination with ShellPage NavigationView
    }
}
