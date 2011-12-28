﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using Microsoft.Phone.Controls;
using Milestone.Model;

namespace Milestone.ViewModel
{
    public class AddIssueViewModel : ViewModelBase
    {
        private readonly GitHubModel _model;
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
        public AddIssueViewModel(GitHubModel model)
        {
            _model = model;
        }

        public void Submit()
        {
            IsBusy = true;
            _model.UploadIssue(Repo, Title, Description, i =>
                                                             {
                                                                 IsBusy = false;
                                                                 (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/IssueList.xaml?context=" + _contextIndex + "&repo=" + Repo.Repository.Name.Replace(" ", "%20"), UriKind.Relative));
                                                             });
        }

        protected bool IsBusy { get; set; }
    }
}