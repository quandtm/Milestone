using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Milestone.Model;
using NGitHub.Models;

namespace Milestone.ViewModel
{
    public class IssueDetailsViewModel : ViewModelBase
    {
        public bool IsBusy { get; set; }
        private readonly GitHubModel _model;
        public Context Context { get; set; }
        public Repo Repo { get; set; }
        private int _contextIndex = -1;
        public int ContextIndex
        {
            get { return _contextIndex; }
            set
            {
                _contextIndex = value;
                Context = _model.Contexts[ContextIndex];
            }
        }

        private string _repoName;
        public string RepoName
        {
            get { return _repoName; }
            set
            {
                if (value == null) return;
                _repoName = value;
                if (Context == null) return;
                Repo = Context.Repositories.FirstOrDefault(r => r.Repository.Name == RepoName);
                RefreshComments();
            }
        }

        public int Id { get; set; }
        public ObservableCollection<Comment> Comments
        {
            get
            {
                return Repo.IssueComments.ContainsKey(Issue) ? Repo.IssueComments[Issue] : null;
            }
        }
        public Issue Issue
        {
            get
            {
                return Repo.Issues.FirstOrDefault(i => i.Number == Id);
            }
        }

        public void RefreshComments()
        {
            IsBusy = true;
            _model.DownloadIssueComments(Context, Repo, Issue, r =>
            {
                IsBusy = false;
                RaisePropertyChanged("Comments");
            });
        }

        public IssueDetailsViewModel(GitHubModel model)
        {
            _model = model;
        }
    }
}
