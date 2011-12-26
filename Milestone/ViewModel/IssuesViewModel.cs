using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using Milestone.Model;
using NGitHub.Models;

namespace Milestone.ViewModel
{
    public class IssuesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int ContextIndex { get; set; }
        public string RepoName { get; set; }

        private readonly Dispatcher _dispatcher;
        private readonly GitHubModel _model;

        public IssuesViewModel(Dispatcher dispatcher, GitHubModel model)
        {
            _dispatcher = dispatcher;
            _model = model;

            
        }
    }
}
