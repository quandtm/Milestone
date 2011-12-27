using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NGitHub.Models;

namespace Milestone.Model
{
    public class Repo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Repository Repository { get; set; }
        public ObservableCollection<Issue> Issues { get; set; }

        public Repo()
        {
            Issues = new ObservableCollection<Issue>();
        }

        public Repo(Repository repository)
        {
            Repository = repository;
            Issues = new ObservableCollection<Issue>();
        }

        public Repo(Repository repository, IEnumerable<Issue> issues)
        {
            Repository = repository;
            issues = new ObservableCollection<Issue>(issues);
        }
    }
}