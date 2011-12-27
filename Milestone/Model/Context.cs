using System.Collections.ObjectModel;
using System.ComponentModel;
using NGitHub.Models;

namespace Milestone.Model
{
    public class Context : INotifyPropertyChanged
    {
        public User User { get; set; }
        public ObservableCollection<Repo> Repositories { get; private set; }

        public Context()
        {
            Repositories = new ObservableCollection<Repo>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
