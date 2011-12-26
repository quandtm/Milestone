using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NGitHub.Models;

namespace Milestone.Model
{
    public class Context : INotifyPropertyChanged
    {
        public User User { get; set; }
        public ObservableCollection<Repo> MyRepositories { get; private set; }
        public ObservableCollection<Repo> WatchedRepositories { get; private set; }

        public Context()
        {
            MyRepositories = new ObservableCollection<Repo>();
            WatchedRepositories = new ObservableCollection<Repo>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Repo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Repository Repository { get; set; }
        public ObservableCollection<Issue> Issues { get; set; }

        public Repo()
        {
            
        }

        public Repo(Repository repository)
        {
            Repository = repository;
        }

        public Repo(Repository repository, IEnumerable<Issue> issues)
        {
            Repository = repository;
            Issues = new ObservableCollection<Issue>(issues);
        }
    }
}
