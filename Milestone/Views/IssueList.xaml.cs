using System.Windows.Navigation;
using Milestone.ViewModel;

namespace Milestone.Views
{
    public partial class IssueList
    {
        public IssueList()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var contextIndex = int.Parse(NavigationContext.QueryString["context"]);
            var repoName = NavigationContext.QueryString["repo"];

            var vm = (DataContext as IssuesViewModel);
            if (vm != null)
            {
                vm.ContextIndex = contextIndex;
                vm.RepoName = repoName;
            }
            base.OnNavigatedTo(e);
        }
    }
}