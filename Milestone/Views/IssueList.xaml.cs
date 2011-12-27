using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using Milestone.ViewModel;
using NGitHub.Models;

namespace Milestone.Views
{
    public partial class IssueList
    {
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

        private void LstOpenOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstOpen.SelectedItem == null)
                return;

            var issue = (Issue)lstOpen.SelectedItem;
            NavigationService.Navigate(new Uri("/Views/IssueView.xaml?id=" + issue.Number + "&context=" + ViewModel.ContextIndex + "&repo=" + ViewModel.RepoName, UriKind.Relative));
        }
    }
}