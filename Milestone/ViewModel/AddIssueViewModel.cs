using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using Microsoft.Phone.Controls;
using Milestone.Model;

namespace Milestone.ViewModel
{
    public class AddIssueViewModel : ViewModelBase
    {
        private readonly GitHubModel _model;
        private readonly Dispatcher _dispatcher;
        public string Title { get; set; }
        public string Description { get; set; }
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
        public Repo Repo { get; set; }
        public Context Context { get; set; }
        private string _repoName;
        public string RepoName
        {
            get { return _repoName; }
            set
            {
                _repoName = value;
                if (Context != null)
                {
                    Repo = Context.Repositories.FirstOrDefault(r => r.Repository.Name == RepoName);
                }
            }
        }

        public bool IsBusy { get; set; }

        public AddIssueViewModel(GitHubModel model, Dispatcher dispatcher)
        {
            _model = model;
            _dispatcher = dispatcher;
        }

        public void Submit()
        {
            IsBusy = true;
            _model.UploadIssue(Repo, Title, Description,
                i =>
                {
                    _dispatcher.BeginInvoke(
                        () =>
                        {
                            IsBusy = false;
                            Repo.Issues.Add(i);
                            (Application.Current.RootVisual as PhoneApplicationFrame).GoBack();
                            //(Application.Current.RootVisual as PhoneApplicationFrame).Navigate(
                            //        new Uri("/Views/IssueList.xaml?context=" +_contextIndex +"&repo=" +Repo.Repository.Name.Replace(" ","%20"),UriKind.Relative));
                        });
                });
        }
    }
}
