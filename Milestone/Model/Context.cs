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
}
