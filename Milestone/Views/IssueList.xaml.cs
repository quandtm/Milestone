using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using Milestone.ViewModel;
using NGitHub.Models;

namespace Milestone.Views
{
    public partial class IssueList
    {
        private bool _ignoreSelectionChanged = false;

        public IssueList()
        {
            InitializeComponent();
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
            var list = sender as ListBox;
            if (list == null)
                return;

            if (list.SelectedItem == null || _ignoreSelectionChanged)
                return;

            var issue = (Issue)list.SelectedItem;
            NavigationService.Navigate(new Uri("/Views/IssueView.xaml?id=" + issue.Number + "&context=" + ViewModel.ContextIndex + "&repo=" + ViewModel.RepoName, UriKind.Relative));
            _ignoreSelectionChanged = true;
            list.SelectedIndex = -1;
            _ignoreSelectionChanged = false;
        }

        private void CvsOpenFilter(object sender, System.Windows.Data.FilterEventArgs e)
        {
            e.Accepted = ((Issue) e.Item).State == "open";
        }
        private void CvsClosedFilter(object sender, System.Windows.Data.FilterEventArgs e)
        {
            e.Accepted = ((Issue) e.Item).State == "closed";
        }
        
    }
}