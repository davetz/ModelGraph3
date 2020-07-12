using System;
using System.Threading.Tasks;

namespace ModelGraph.Core
{
    public partial class Root
    {
        #region PostUIRequest  ================================================
        // These methods are called from the ui thread and typically they invoke 
        // some change to the dataChefs objects (create, remove, update, link, unlink)
        internal void PostRefresh()
        {
            PostModelRequest(DoNothing);
            void DoNothing() { }
        }
        internal void PostCommand(LineCommand command)
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
            {
                PostSetTextValue(item, prop, enu.GetValue(index));
            }
        }
        #endregion

        #region ExecuteRequest ================================================
        private async void PostModelRequest(Action action)
        {
            await Task.Run(() => { ExecuteRequest(action); }); // runs on worker thread 
            //<=== control immediatley returns to the ui thread
            //(some time later the worker task completes and signals the ui thread)
            //===> the ui thread returns here and continues executing the following code
            foreach (var item in Items)
            {
                if (item is TreeModel tm) tm.Validate();
            }
        }
        private void ExecuteRequest(Action action)
        {
            // the action will likey modify objects, 
            // and we can't have multiple threads stepping on each other
            lock (this)
            {
                action();
                var cr = Get<ChangeRoot>();
                cr.RecordChanges();
            }
        }
        #endregion
    }
}
