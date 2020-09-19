using System.Collections.Generic;

namespace ModelGraph.Core
{
    public interface ITreeModel
    {
        Root GetRoot();
        ItemModel HeaderModel { get; }

        void RefreshViewList(int viewSize, ItemModel leading, ItemModel selected, ChangeType change = ChangeType.None);
        (List<ItemModel>, ItemModel, bool, bool) GetCurrentView(int viewSize, ItemModel leading, ItemModel selected);

        void SetUsage(ItemModel model, Usage usage);
        void SetFilter(ItemModel model, string text);
        void SetSorting(ItemModel model, Sorting sorting);
        (int, Sorting, Usage, string) GetFilterParms(ItemModel model);
    }
}
