using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NGitHub.Models;

namespace Milestone.Model
{
    [Flags]
    public enum RepoType : byte
    {
        None = 0,
        Owned = 1,
        Watched = 2,
        Both = 3
    }

    public class Repo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Repository Repository { get; set; }
        public ObservableCollection<Issue> Issues { get; set; }
        public Dictionary<Issue, ObservableCollection<Comment>> IssueComments { get; set; }
        public RepoType Type { get; set; }

        public Repo()
        {
            Issues = new ObservableCollection<Issue>();
            IssueComments = new Dictionary<Issue, ObservableCollection<Comment>>();
            Type = RepoType.None;
        }

        public Repo(Repository repository)
        {
            Repository = repository;
            Issues = new ObservableCollection<Issue>();
            IssueComments = new Dictionary<Issue, ObservableCollection<Comment>>();
            Type = RepoType.None;
        }

        public Repo(Repository repository, IEnumerable<Issue> issues)
        {
            Repository = repository;
            IssueComments = new Dictionary<Issue, ObservableCollection<Comment>>();
            Issues = new ObservableCollection<Issue>(issues);
            Type = RepoType.None;
        }
    }
}