using System.Windows.Navigation;
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
            var contextIndex = int.Parse(NavigationContext.QueryString["context"]);
            var repoName = NavigationContext.QueryString["repo"];
            var issueId = int.Parse(NavigationContext.QueryString["id"]);

            var vm = (DataContext as IssueDetailsViewModel);
            if (vm != null)
            {
                vm.ContextIndex = contextIndex;
                vm.RepoName = repoName;
                vm.Id = issueId;
            }
            base.OnNavigatedTo(e);
        }
    }
}