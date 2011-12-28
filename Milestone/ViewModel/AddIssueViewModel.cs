using GalaSoft.MvvmLight;
using Milestone.Model;

namespace Milestone.ViewModel
{
    public class AddIssueViewModel : ViewModelBase
    {
        private readonly GitHubModel _model;
        public string RepoName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public AddIssueViewModel(GitHubModel model)
        {
            _model = model;
        }

        public void Submit()
        {
            IsBusy = true;
            _model.UploadIssue();
        }

        protected bool IsBusy { get; set; }
    }
}
