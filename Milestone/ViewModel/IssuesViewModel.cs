﻿using System.ComponentModel;
using System.Linq;
using Milestone.Model;
using NGitHub.Models;

namespace Milestone.ViewModel
{
    public class IssuesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
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

        private int _numBusy = 0;
        public bool IsBusy
        {
            get;
            set;
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
                    Repo = Context.Repositories.FirstOrDefault(r => r.Repository.Name == RepoName);
                    RefreshIssues();
                }
            }
        }

        public void RefreshIssues()
        {
            _model.DownloadIssues(Context, Repo,
                () => // onStart
                {
                    ++_numBusy;
                    SetBusy();
                },
                () => // onComplete
                {
                    --_numBusy;
                    SetBusy();
                });
        }


        private void SetBusy()
        {
            IsBusy = _numBusy > 0;
        }

        private readonly GitHubModel _model;

        private Issue _selectedIssue;
        public Issue SelectedIssue
        {
            get { return _selectedIssue; }
            set
            {
                if (value != null)
                {
                    _selectedIssue = value;

                }
            }
        }

        public IssuesViewModel(GitHubModel model)
        {
            _model = model;
        }
    }
}
