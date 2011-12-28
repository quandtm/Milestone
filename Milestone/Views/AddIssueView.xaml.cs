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
            var contextIndex = int.Parse(NavigationContext.QueryString["context"]);
            var repoName = NavigationContext.TryGetStringKey("repo");
            ViewModel.ContextIndex = contextIndex;
            ViewModel.RepoName = repoName;
        }

        private void SubmitIssueclick(object sender, System.EventArgs e)
        {
            ViewModel.Title = txtTitle.Text;
            ViewModel.Description = txtBody.Text;
            ViewModel.Submit();
        }
    }
}
