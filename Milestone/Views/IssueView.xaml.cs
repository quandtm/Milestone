using System;
using System.Windows.Navigation;
using Milestone.Extensions;
using Milestone.ViewModel;

namespace Milestone.Views
{
    public partial class IssueDetailsView
    {
        public IssueDetailsView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var contextIndex = NavigationContext.TryGetKey("context");
            var repoName = NavigationContext.TryGetStringKey("repo");
            var issueId = NavigationContext.TryGetKey("id");


            var vm = (DataContext as IssueDetailsViewModel);
            if (vm != null && contextIndex.HasValue && issueId.HasValue)
            {
                vm.Id = issueId.Value;
                vm.ContextIndex = contextIndex.Value;
                vm.RepoName = repoName;
                //HACK: should we have an 'initialize' method rather than triggering on properties?
            }
            base.OnNavigatedTo(e);
        }

        private void PostComment(object sender, System.EventArgs e)
        {
            var vm = (DataContext as IssueDetailsViewModel);

            if (vm != null)
            {
                var repoName = vm.RepoName;
                var issueNum = vm.Issue.Number.ToString();
                NavigationService.Navigate(new Uri("/Views/NewCommentView.xaml?repoOwner=" + vm.Repo.Repository.Owner + "&repo=" + repoName + "&issue=" + issueNum, UriKind.Relative));
            }
        }
    }
}