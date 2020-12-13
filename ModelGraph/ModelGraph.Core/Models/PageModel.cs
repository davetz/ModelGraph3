using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    /// <summary>Flat list of LineModel that emulates a UI tree view</summary>
    public class PageModel : ChildOf<Root>, IPageModel
    {
        public Root Root => Owner;
        public ILeadModel LeadModel { get; private set; }
        public IPageControl PageControl { get; set; } // reference the UI PageControl       
        public ControlType ControlType { get; private set; }
        public string TitleName { get; internal set; } = BlankName;
        public string TitleSummary { get; internal set; } = BlankName;

        internal void Add(ILeadModel lead) { if (LeadModel is null) LeadModel = lead; } //works for GraphModel and TreeModel, but doesn't accept FlyoutTreeModel and SideTreeModel

        internal PageModel() //========== invoked in the RootModel constructor
        {
            Owner = new Root();
            new TreeModel(this);
            ControlType = ControlType.PrimaryTree;
            Owner.Add(this);
        }

        internal PageModel(Root root, Action<PageModel> createLeadModel, ControlType controlType)
        {
            Owner = root;
            createLeadModel(this);
            ControlType = controlType;
            root.Add(this);
        }


        internal void Close()
        {
            IsClosed = true;
            PageControl?.Close();
        }
        public bool IsClosed { get; private set; }

        internal void Save()
        {
            PageControl?.Save();
        }

        internal void SaveAs()
        {
            PageControl?.SaveAs();
        }

        internal void Reload()
        {
            PageControl?.Reload();
        }

        internal void NewView(Action<PageModel> createLeadModel, ControlType controlType)
        {
            var model = new PageModel(Owner, createLeadModel, controlType);
            PageControl?.NewView(model);
        }

        public void Release()
        {
            if (Owner is null) return;

            Owner.Remove(this);
            Discard(); //discard myself and recursivly discard all my children

            if (ControlType == ControlType.PartialTree)
                Owner.Discard(); //kill off the dataChef

            Owner = null;
        }

        public void TriggerUIRefresh() => PageControl?.RefreshAsync();
    }
}
