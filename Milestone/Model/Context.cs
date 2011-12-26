using System.Collections.ObjectModel;
using System.ComponentModel;
using NGitHub.Models;

namespace Milestone.Model
{
    public class Context : INotifyPropertyChanged
    {
        public User User { get; set; }
        public ObservableCollection<Repository> MyRepositories { get; private set; }
        public ObservableCollection<Repository> WatchedRepositories { get; private set; }

        public Context()
        {
            MyRepositories = new ObservableCollection<Repository>();
            WatchedRepositories = new ObservableCollection<Repository>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
