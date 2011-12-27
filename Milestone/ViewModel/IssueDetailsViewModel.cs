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
                _repoName = value;
                if (Context != null)
                {
                    var x = Context.MyRepositories.FirstOrDefault(r => r.Repository.Name == RepoName);
                    var y = Context.WatchedRepositories.FirstOrDefault(r => r.Repository.Name == RepoName);
                    Repo = x ?? y;

                }
            }
        }

        public int Id { get; set; }

        public Issue Issue
        {
            get
            {
                return Repo.Issues.FirstOrDefault(i => i.Number == Id);
            }
        }

        public IssueDetailsViewModel(GitHubModel model)
        {
            _model = model;
        }
    }
}
