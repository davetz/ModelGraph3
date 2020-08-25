﻿using ModelGraph.Controls;
using ModelGraph.Helpers;
using ModelGraph.Services;
using ModelGraph.Core;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ModelGraph.Views
{
    public sealed partial class ModelPage : Page
    {
        IModelPageControl PageControl;

        #region Constructor  ==================================================
        public ModelPage()
        {
            InitializeComponent();
            SizeChanged += ModelPage_SizeChanged;
        }

        private void ModelPage_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            PageControl?.SetSize(ActualWidth, ActualHeight); // sets number of lines per page
        }
        #endregion

        #region NavigatedTo/From  =============================================
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigatedToAsync(e.Parameter);
        }

        internal async System.Threading.Tasks.Task NavigatedToAsync(object parm)
        {
            if (parm is RootModel m1)
            {
                GetModelControl(m1);
                NavigationService.ActiveModelPage = this;
            }
            else if (parm is ViewLifetimeControl viewControl && viewControl.DataModel is IPageModel m2)
            {
                GetModelControl(m2);
            }
            ControlGrid.Children.Add(PageControl as UIElement);


            void GetModelControl(IPageModel m)
            {
                if (m.PageControl is null)
                {
                    Root.SetLocalizer(ResourceExtensions.CoreLocalizer());

                    switch (m.ControlType)
                    {
                        case ControlType.PrimaryTree:
                        case ControlType.PartialTree:
                            var treeControl = new ModelTreeControl(m);
                            treeControl.Loaded += TreeControll_Loaded;
                            m.PageControl = treeControl;
                            break;

                        case ControlType.SymbolEditor: m.PageControl = new SymbolEditControl(m); break;

                        case ControlType.GraphDisplay: m.PageControl = new ModelGraphControl(m); break;

                        default:
                            throw new ArgumentException("Unknown ControlType");
                    }
                }
                PageControl = m.PageControl as IModelPageControl;
            }
        }

        private void TreeControll_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ModelTreeControl treeControl && sender is IModelPageControl pageControl)
            {
                treeControl.Loaded -= TreeControll_Loaded;
                pageControl.SetSize(ActualWidth, ActualHeight); // initializes number of lines per page
            }
        }

        internal void NavigatedFrom()
        {
            ControlGrid.Children.Clear();
            PageControl = null;
        }
        #endregion
    }
}
