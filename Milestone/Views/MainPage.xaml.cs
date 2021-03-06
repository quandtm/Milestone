﻿using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using Milestone.Model;
using Milestone.ViewModel;

namespace Milestone.Views
{
    public partial class MainPage
    {
        private MainViewModel _vm;
        private bool _ignoreRepoSelection = false;
        private int _contextIndex;
        private Action _refreshAction;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _refreshAction = () =>
                                {
                                    Dispatcher.BeginInvoke(() =>
                                                               {
                                                                   var cvs =
                                                                       Resources["cvsOwned"] as
                                                                       CollectionViewSource;
                                                                   var cvsWatched = Resources["cvsWatched"] as CollectionViewSource;
                                                                   cvs.View.Refresh();
                                                                   cvsWatched.View.Refresh();
                                                               });
                                };
            Loaded += MainPageLoaded;
        }

        void MainPageLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //Inelegant solution to a stupid problem
            var cvs = Resources["cvsOwned"] as CollectionViewSource;
            if (cvs.View != null)
                cvs.View.MoveCurrentToPosition(-1);
            lstMine.ItemsSource = null;
            lstMine.ItemsSource = cvs.View;

            var cvsWatched = Resources["cvsWatched"] as CollectionViewSource;
            if (cvsWatched.View != null)
                cvsWatched.View.MoveCurrentToPosition(-1);

            lstWatched.ItemsSource = null;
            lstWatched.ItemsSource = cvsWatched.View;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _vm = (MainViewModel)DataContext;

            if (!_vm.Model.IsAuthenticated)
                NavigationService.Navigate(new Uri("/Views/Login.xaml", UriKind.Relative));
            else
            {
                if (NavigationContext.QueryString.ContainsKey("from") && NavigationContext.QueryString["from"].Equals("login"))
                    NavigationService.RemoveBackEntry();
            }

            base.OnNavigatedTo(e);
        }

        private void Logout(object sender, EventArgs e)
        {
            _vm.Logout();
            NavigationService.Navigate(new Uri("/Views/Login.xaml", UriKind.Relative));
        }

        private void SelectContext(object sender, SelectionChangedEventArgs e)
        {
            if (contextSelector.SelectedIndex == -1)
                return;

            _vm.SelectedContext = _vm.Model.Contexts[contextSelector.SelectedIndex];
            _contextIndex = contextSelector.SelectedIndex;
            _vm.Refresh(_refreshAction);
        }

        private void SelectRepo(object sender, SelectionChangedEventArgs e)
        {
            if (_ignoreRepoSelection)
                return;

            var listbox = sender as ListBox;
            if (listbox == null || listbox.SelectedItem == null)
                return;

            var repo = listbox.SelectedItem as Repo;
            if (repo == null)
                return;

            if (!repo.Repository.HasIssues)
                return;

            // Reset the selection
            _ignoreRepoSelection = true;
            listbox.SelectedIndex = -1;
            _ignoreRepoSelection = false;

            NavigationService.Navigate(new Uri("/Views/IssueList.xaml?context=" + _contextIndex + "&repo=" + repo.Repository.Name.Replace(" ", "%20"), UriKind.Relative));
        }

        private void About(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AboutView.xaml", UriKind.Relative));
        }

        private void CvsOwnedFilter(object sender, FilterEventArgs e)
        {
            e.Accepted = (((Repo)e.Item).Type & RepoType.Owned) == RepoType.Owned;
        }

        private void CvsWatchedFilter(object sender, FilterEventArgs e)
        {
            e.Accepted = (((Repo)e.Item).Type & RepoType.Watched) == RepoType.Watched;
        }

        private void Refresh(object sender, EventArgs e)
        {
            _vm.Refresh(_refreshAction);
        }
    }
}