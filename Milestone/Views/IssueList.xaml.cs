using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using Milestone.ViewModel;
using NGitHub.Models;

namespace Milestone.Views
{
    public partial class IssueList
    {
        private bool _ignoreSelection = false;

        public IssueList()
        {
            InitializeComponent();
            Loaded += MainPageLoaded;
        }

        void MainPageLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //Inelegant solution to a stupid problem
            var cvsOpen = Resources["cvsOpen"] as CollectionViewSource;
            cvsOpen.View.MoveCurrentToPosition(-1);
            lstOpen.ItemsSource = cvsOpen.View;

            var cvsClosed = Resources["cvsClosed"] as CollectionViewSource;
            cvsClosed.View.MoveCurrentToPosition(-1);
            lstClosed.ItemsSource = cvsClosed.View;
        }

        public IssuesViewModel ViewModel
        {
            get
            {
                return (DataContext as IssuesViewModel);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var contextIndex = int.Parse(NavigationContext.QueryString["context"]);
            var repoName = NavigationContext.QueryString["repo"];

            if (ViewModel != null)
            {
                ViewModel.ContextIndex = contextIndex;
                ViewModel.RepoName = repoName;
            }
            base.OnNavigatedTo(e);
        }

        private void OpenIssue(object sender, SelectionChangedEventArgs e)
        {
            if (_ignoreSelection)
                return;

            var list = sender as ListBox;
            if (list == null)
                return;

            if (list.SelectedItem == null)
                return;

            var issue = (Issue)list.SelectedItem;
            _ignoreSelection = true;
            list.SelectedIndex = -1;
            _ignoreSelection = false;
            NavigationService.Navigate(new Uri("/Views/IssueView.xaml?id=" + issue.Number + "&context=" + ViewModel.ContextIndex + "&repo=" + ViewModel.RepoName, UriKind.Relative));
        }

        private void CvsOpenFilter(object sender, System.Windows.Data.FilterEventArgs e)
        {
            e.Accepted = ((Issue)e.Item).State == "open";
        }
        private void CvsClosedFilter(object sender, System.Windows.Data.FilterEventArgs e)
        {
            e.Accepted = ((Issue)e.Item).State == "closed";
        }

        private void AddIssue(object sender, EventArgs e)
        {
            
        }
    }
}