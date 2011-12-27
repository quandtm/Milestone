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
        public RepoType Type { get; set; }

        public Repo()
        {
            Issues = new ObservableCollection<Issue>();
            Type = RepoType.None;
        }

        public Repo(Repository repository)
        {
            Repository = repository;
            Issues = new ObservableCollection<Issue>();
            Type = RepoType.None;
        }

        public Repo(Repository repository, IEnumerable<Issue> issues)
        {
            Repository = repository;
            issues = new ObservableCollection<Issue>(issues);
            Type = RepoType.None;
        }
    }
}