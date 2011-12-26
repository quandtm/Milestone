using GalaSoft.MvvmLight;
using Milestone.Model;
using System.Windows.Threading;

namespace Milestone.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public GitHubModel Model { get; set; }

        public MainViewModel()
        {
            Model = ViewModelLocator.Model;
        }

        public void RefreshAll()
        {
            RefreshRepositories();
        }

        public void RefreshRepositories()
        {
            Model.RefreshRepos();
        }
    }
}