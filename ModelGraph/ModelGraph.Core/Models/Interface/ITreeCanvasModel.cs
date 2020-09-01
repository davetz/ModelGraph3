using System.Collections.Generic;

namespace ModelGraph.Core
{
    public interface ITreeCanvasModel
    {
        string HeaderTitle { get; }

        void RefreshViewList(int viewSize, ItemModel leading, ItemModel selected, ChangeType change = ChangeType.None);
        (List<ItemModel>, ItemModel, bool, bool) GetCurrentView(int viewSize, ItemModel leading, ItemModel selected);

        void SetUsage(ItemModel model, Usage usage);
        void SetFilter(ItemModel model, string text);
        void SetSorting(ItemModel model, Sorting sorting);
        (int, Sorting, Usage, string) GetFilterParms(ItemModel model);

        int GetIndexValue(ItemModel model);
        bool GetBoolValue(ItemModel model);
        string GetTextValue(ItemModel model);
        string[] GetListValue(ItemModel model);

        void PostSetIndexValue(ItemModel model, int val);
        void PostSetBoolValue(ItemModel model, bool val);
        void PostSetTextValue(ItemModel model, string val);

        void DragDrop(ItemModel model);
        void DragStart(ItemModel model);
        DropAction DragEnter(ItemModel model);

        void GetButtonCommands(List<ItemCommand> buttonCommands);
        void GetMenuCommands(ItemModel model, List<ItemCommand> menuCommands);
        void GetButtonCommands(ItemModel model, List<ItemCommand> buttonCommands);
    }
}
