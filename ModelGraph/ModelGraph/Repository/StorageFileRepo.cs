using System;
using ModelGraph.Helpers;
using ModelGraph.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.Storage.Pickers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelGraph.Repository
{
    public class StorageFileRepo : IRepository
    {
        static int _instanceCount;

        int _instanceId;
        StorageFile _storageFile;

        public StorageFileRepo() { }

        #region Properties  ===================================================
        public bool HasNoStorage => _storageFile is null;
        public string FullName => (_storageFile is null) ? InstanceName : _storageFile.Path;
        public string Name
        {
            get
            {
                if (_storageFile is null) return InstanceName;

                var name = _storageFile.Name;
                var index = name.LastIndexOf('.');
                if (index < 0) return name;
                return name.Substring(0, index);
            }
        }
        private string InstanceName => string.Format("{0} {1}", "StorageFileRepo_New".GetLocalized(), _instanceId);
        #endregion

        #region New  ==========================================================
        public void New(Root chef)
        {
            if (chef is null) throw new ArgumentNullException(nameof(chef));
            chef.Repository = this;
            _instanceCount += 1;
            _instanceId = _instanceCount;
        }
        #endregion

        #region Open  =========================================================
        public async Task<bool> OpenAsync(Root chef)
        {
            if (chef is null) throw new ArgumentNullException(nameof(chef));
            chef.Repository = this;

            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".mgd");
            _storageFile = await openPicker.PickSingleFileAsync();

            if (_storageFile is null)
                return false;

            bool success = await ReadAsync(chef).ConfigureAwait(true);
            return success;
        }
        #endregion

        #region Save  =========================================================
        public async Task<bool> SaveAsync(Root chef)
        {
            if (chef is null) throw new ArgumentNullException(nameof(chef));
            if (_storageFile is null)
                return false;

            bool success = await WriteAsync(chef).ConfigureAwait(true); ;
            return success;
        }
        #endregion

        #region Reload  =======================================================
        public async Task<bool> ReloadAsync(Root newChef)
        {
            if (newChef is null) throw new ArgumentNullException(nameof(newChef));
            newChef.Repository = this;
            bool success = await ReadAsync(newChef).ConfigureAwait(true);
            return success;
        }
        #endregion

        #region SaveAs  =======================================================
        public void SaveAS(Root chef)
        {
            SaveAsAsync(chef).ConfigureAwait(false);
        }

        private async Task<bool> SaveAsAsync(Root chef)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = string.Empty
            };
            savePicker.FileTypeChoices.Add("DataFile", new List<string>() { ".mgd" });

            _storageFile = await savePicker.PickSaveFileAsync();

            if (_storageFile is null)
                return false ;

            bool success = await WriteAsync(chef).ConfigureAwait(true);
            return success;
        }
        #endregion

        #region Read  =========================================================
        private async Task<bool> ReadAsync(Root chef)
        {
            if (chef is null) throw new ArgumentNullException(nameof(chef));
            try
            {
                using (var stream = await _storageFile.OpenAsync(FileAccessMode.Read))
                {
                    using (DataReader r = new DataReader(stream))
                    {
                        r.ByteOrder = ByteOrder.LittleEndian;
                        UInt64 size = stream.Size;
                        if (size < UInt32.MaxValue)
                        {
                            var byteCount = await r.LoadAsync((UInt32)size);
                            chef.Deserialize(r);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                chef.AddRepositorReadError(ex.Message);
                return false;
            }
            return true;
        }
        #endregion

        #region Write  ========================================================
        private async Task<bool> WriteAsync(Root chef)
        {
            if (chef is null) throw new ArgumentNullException(nameof(chef));
            try
            {
                using (var tran = await _storageFile.OpenTransactedWriteAsync())
                {
                    using (var w = new DataWriter(tran.Stream))
                    {
                        w.ByteOrder = ByteOrder.LittleEndian;
                        chef.Serialize(w);
                        tran.Stream.Size = await w.StoreAsync(); // reset stream size to override the file
                        await tran.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                chef.AddRepositorWriteError(ex.Message);
                return false;
            }
            return true;
        }
        #endregion

    }
}
