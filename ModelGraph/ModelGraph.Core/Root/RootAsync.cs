using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace ModelGraph.Core
{
    public partial class Root
    {
        #region ItemTriggeredRefresh  =========================================
        private readonly HashSet<Item> RefreshTriggerItems = new HashSet<Item>();
        private readonly Dictionary<Item, HashSet<Item>> ItemTriggeredRefresh = new Dictionary<Item, HashSet<Item>>();

        internal void ClearItemTriggeredRefresh()
        {
            foreach (var item in ItemTriggeredRefresh.Keys)
            {
                item.IsRefreshTriggerItem = false;
            }
            ItemTriggeredRefresh.Clear();
        }
        
        internal void AddRefreshTriggerItem(Item item) => RefreshTriggerItems.Add(item);
        internal void AddItemTriggeredRefresh(Relation r, Item ti) => GetRefreshHashSet(r).Add(ti);
        internal void AddItemTriggeredRefresh(Property p, Item ti) => GetRefreshHashSet(p).Add(ti);
        private HashSet<Item> GetRefreshHashSet(Item item)
        {
            if (!ItemTriggeredRefresh.TryGetValue(item, out HashSet<Item> hs))
            {
                hs = new HashSet<Item>();
                ItemTriggeredRefresh.Add(item, hs);
            }
            return hs;
        }

        private void ExecuteItemTriggeredRefresh()
        {
            if (RefreshTriggerItems.Count > 0)
            {
                var items = RefreshTriggerItems.ToArray();
                RefreshTriggerItems.Clear();

                var doneItems = new HashSet<Item>();

                foreach (var item in items)
                {
                    if (ItemTriggeredRefresh.TryGetValue(item, out HashSet<Item> hs))
                    {
                        foreach (var refreshItem in hs)
                        {
                            if (doneItems.Contains(refreshItem)) continue;

                            if (refreshItem is GraphX gx) gx.Refresh(); ;
                            if (refreshItem is ComputeX cx) cx.Value.Clear();

                            doneItems.Add(refreshItem);
                        }
                    }
                }
            }
        }
        #endregion

        #region PostUIRequest  ================================================
        // These methods are called from the ui thread and typically they invoke 
        // some change to the dataChefs objects (create, remove, update, link, unlink)
        internal void PostRefresh()
        {
            PostModelRequest(DoNothing);
            void DoNothing() { }
        }
        internal void PostCommand(ItemCommand command)
        {
            PostModelRequest(command.Action);
        }
        internal void PostSetBoolValue(Item item, Property prop, bool value)
        {
            PostSetTextValue(item, prop, value.ToString());
        }
        internal void PostSetTextValue(Item item, Property prop, string value)
        {
            if (ItemUpdated.IsNotRequired(item, prop, value)) return;

            PostModelRequest(() => { ItemUpdated.Record(this, item, prop, value); });
        }
        internal void PostSetIndexValue(Item item, Property prop, int index)
        {
            if (prop is IEnumProperty enu)
                PostSetTextValue(item, prop, enu.GetValue(index));
            else if (prop is ColumnX cx)
                PostSetTextValue(item, prop, cx.Owner.GetActualValueAt(cx, index)); 
        }
        #endregion

        #region ExecuteRequest ================================================
        private async void PostModelRequest(Action action)
        {
            await Task.Run(() => { ExecuteRequest(action); }); // runs on worker thread 
            //<=== control immediatley returns to the ui thread
            //(some time later the worker task completes and signals the ui thread)
            //===> the ui thread returns here and continues executing the following code            
            foreach (var pm in Items) { pm.TriggerUIRefresh(); }
        }
        private void ExecuteRequest(Action action)
        {
            // the action will likey modify objects, 
            // and we can't have multiple threads stepping on each other
            lock (this)
            {
                action();
                var cr = Get<ChangeManager>();
                cr.RecordChanges();

                ExecuteItemTriggeredRefresh();

                foreach (var pm in Items) 
                {
                    var lm = pm.LeadModel;
                    if (lm is TreeModel tm) tm.Validate();
                    if (lm is DrawModel dm)
                    {
                        if (dm.FlyOutTreeModel is TreeModel fm) fm.Validate();
                        if (dm.SideTreeModel is TreeModel sm) sm.Validate();
                    }
                }
            }
        }
        #endregion
    }
}
