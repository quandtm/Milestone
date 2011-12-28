using System.Windows.Navigation;
using Milestone.Extensions;
using Milestone.ViewModel;

namespace Milestone.Views
{
    public partial class AddIssueView
    {
        public AddIssueView()
        {
            InitializeComponent();
        }

        public AddIssueViewModel ViewModel
        {
            get
            {
                return (DataContext as AddIssueViewModel);
            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var repoName = NavigationContext.TryGetStringKey("repo");
            ViewModel.RepoName = repoName;
        }

        private void SubmitIssueclick(object sender, System.EventArgs e)
        {
            ViewModel.Submit();
        }
    }
}
