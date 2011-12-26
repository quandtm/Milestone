using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;
using Milestone.Model;

namespace Milestone.ViewModel
{
    public class IssuesViewModel : INotifyPropertyChanged
    {
        public bool IsBusy { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public Context Context { get; set; }
        public Repo Repo { get; set; }
        private int _contextIndex =-1;
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

                    _model.LoadIssues(Context, Repo);                    
                }
            }
        }

        private readonly Dispatcher _dispatcher;
        private readonly GitHubModel _model;

        public IssuesViewModel(Dispatcher dispatcher, GitHubModel model)
        {
            _dispatcher = dispatcher;
            _model = model;
        }
    }
}
