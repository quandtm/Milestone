using System.Windows.Threading;
using Milestone.Model;

namespace Milestone.ViewModel
{
    public class ViewModelLocator
    {
        private MainViewModel _main;
        private IssuesViewModel _issues;
        private static GitHubModel _model;

        private static Dispatcher _dispatcher;
        public static Dispatcher Dispatcher
        {
            get { return _dispatcher; }
            set
            {
                if (_dispatcher == value)
                    return;

                _dispatcher = value;
                if (_model != null)
                    _model.Dispatcher = _dispatcher;
            }
        }

        public static GitHubModel Model
        {
            get
            {
                if (_model == null)
                {
                    _model = new GitHubModel(Dispatcher);
                    _model.Load();
                }
                return _model;
            }
        }

        public MainViewModel Main { get { return _main ?? (_main = new MainViewModel(Model)); }}
        public IssuesViewModel Issues { get { return _issues ?? ( _issues = new IssuesViewModel(Model)); } }
        public IssueDetailsViewModel IssueDetails { get { return new IssueDetailsViewModel(Model); } }
    }
}